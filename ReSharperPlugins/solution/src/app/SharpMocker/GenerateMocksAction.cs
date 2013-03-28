namespace SharpMocker
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using JetBrains.ActionManagement;
    using JetBrains.Application;
    using JetBrains.Application.Progress;
    using JetBrains.DataFlow;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Feature.Services.Bulbs;
    using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;    
    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.Psi.CSharp;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
    using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
    using JetBrains.ReSharper.Psi.Util;
    using JetBrains.ReSharper.Psi.Tree;    
    using JetBrains.TextControl;
    using JetBrains.TextControl.Impl;
    using JetBrains.UI.PopupMenu;
    using JetBrains.UI.RichText;
    using JetBrains.UI.StatusBar;
    using JetBrains.Util;
    using JetBrains.VsIntegration.CodeEditorEmbedding;

    using Testeroids;

    /// <summary>
    /// This is an example context action. The test project demonstrates tests for
    /// availability and execution of this action.
    /// </summary>
    [ContextAction(Name = "GenerateMocks", Description = "Generate and inject mocks", Group = "C#")]
    public class GenerateMocksAction : BulbItemImpl, 
                                       ISharpMockerContextAction
    {
        #region Fields

        /// <summary>
        /// The provider gives information about the context.
        /// </summary>
        private readonly ICSharpContextActionDataProvider provider;

        /// <summary>
        /// Represents a function-like construct that can be invoked.
        /// </summary>
        /// <remarks>
        /// Will store either a constructor or a method
        /// </remarks>
        private IParametersOwner selectedParametersOwner;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateMocksAction"/> class.
        /// </summary>
        /// <param name="provider">
        /// The provider gives information about the context.
        /// </param>
        public GenerateMocksAction(ICSharpContextActionDataProvider provider)
        {
            this.provider = provider;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets an array of bulb items that this bulb action supports.
        /// </summary>
        public new IBulbItem[] Items
        {
            get
            {
                return new IBulbItem[] { this };
            }
        }

        /// <summary>
        /// Gets Popup menu item text.
        /// </summary>
        public override string Text
        {
            get
            {
                return "Generate and inject mocks";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Check if this action is available at the constructed context.  Actions could
        /// store precalculated info in cache to share it between different actions
        /// </summary>
        /// <param name="cache">
        /// Stores some precalculated info 
        /// </param>
        /// <returns>
        /// true if this bulb action is available, false otherwise.
        /// </returns>
        public bool IsAvailable(IUserDataHolder cache)
        {
            var parametersOwner = this.provider.GetSelectedElement<IParametersOwnerDeclaration>(true, true);
            if (parametersOwner != null)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called to prepare QuickFix or ContextAction execution.
        /// Check if the cursor is located on a constructor or a method, and then set the selectedParametersOwner with it.
        /// </summary>
        /// <param name="solution">
        /// The solution is a group of related projects.
        /// </param>
        protected override void ExecuteBeforeProgressAndTransaction(ISolution solution)
        {
            base.ExecuteBeforeProgressAndTransaction(solution);

            var creationExpression = this.provider.GetSelectedElement<IObjectCreationExpression>(true, true);
            if (creationExpression != null)
            {
                this.selectedParametersOwner = this.SelectConstructorAsParameterOwner(creationExpression);
            }
            else
            {
                var invocationExpression = this.provider.GetSelectedElement<IInvocationExpression>(true, true);
                if (invocationExpression != null)
                {
                    this.selectedParametersOwner = this.SelectMethodAsParameterOwner(invocationExpression);
                }
            }
        }

        /// <summary>
        /// Executes QuickFix or ContextAction. Returns post-execute method.
        /// </summary>
        /// <param name="solution">
        /// The solution is a group of related projects.
        /// </param>
        /// <param name="progress">
        /// Given to a lengthy task that knows its progress, used to enable the UI indications of the task progress.
        /// </param>
        /// <returns>
        /// Action to execute after document and PSI transaction finish. Use to open TextControls, navigate caret, etc.
        /// </returns>
        protected override Action<ITextControl> ExecutePsiTransaction(
            ISolution solution, 
            IProgressIndicator progress)
        {
            var methodInvocation = this.provider.GetSelectedElement<ICSharpArgumentsOwner>(false, true);
            var methodCallArgumentsCount = methodInvocation.Arguments.Count;
            if (methodCallArgumentsCount != 0 && this.selectedParametersOwner.Parameters.Count > methodCallArgumentsCount)
            {
                JetBrains.Util.MessageBox.ShowError("Please add commas to the method call in order to indicate the empty parameters that SharpMocker needs to fill.");
                return null;
            }

            if (this.selectedParametersOwner.Parameters.Count < methodCallArgumentsCount)
            {
                JetBrains.Util.MessageBox.ShowError("The method call has more parameters than the actual method declaration. Please make sure the method is called with the right arguments count.");
                return null;
            }

            if (this.selectedParametersOwner as IConstructor != null)
            {
                this.BuildMocksForCtor(this.selectedParametersOwner);
            }

            // Shouldn't we set the ctor info in the BuildMocksForCtor and do the actual injection of mocks in here ?
            if (this.selectedParametersOwner as IMethod != null)
            {
                this.BuildSpecifiedArgumentsForMethod(this.selectedParametersOwner);
            }

            return null;
        }

        private void AppendGeneratedPropertyEstablishmentToMethod(
            IPropertyDeclaration propertyDeclaration, 
            string propertyEstablishmentMethodName, 
            IMethodDeclaration establishContextMethod)
        {
            var factory = CSharpElementFactory.GetInstance(this.provider.PsiModule);
            var initializationStatement = factory.CreateStatement(string.Format("this.{0} = this.{1}();", propertyDeclaration.NameIdentifier.Name, propertyEstablishmentMethodName));
            var lastStatement = establishContextMethod.Body.Statements.Last();

            ModificationUtil.AddChildAfter(lastStatement, initializationStatement);
        }

        // private void RemoveExistingInitializationsInMethod(IPropertyDeclaration propertyDeclaration, IMethodDeclaration establishContextMethod)
        // {
        // establishContextMethod.Body.Statements.Where(s => s as IObjectCreationExpression != null && ((IObjectCreationExpression)s).TypeName.ShortName == propertyDeclaration.Type.GetPresentableName(provider.PsiFile.Language));
        // }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="parameter"></param>
        /// <param name="parametersOwner"></param>
        /// <param name="suffixForPassedArgument"></param>
        /// <returns><c>true</c> if the the parameter has been added to the method invocation; <c>false</c> otherewise.</returns>
        private bool AppendGeneratedPropertyToPassedArguments<TElement>(
            string propertyName, 
            string parameter, 
            IParametersOwner parametersOwner, 
            string suffixForPassedArgument) where TElement : class, ICSharpArgumentsOwner
        {
            var callWasModified = false;
            var param = parametersOwner.Parameters.SingleOrDefault(o => o.ShortName == parameter);
            var index = parametersOwner.Parameters.IndexOf(param);

            // the "this" parameter (the first) will never be in the invocation's passed arguments.
            if (parametersOwner.IsExtensionMethod())
            {
                index--;
            }
            var methodInvocation = this.provider.GetSelectedElement<TElement>(false, true);
            var expressionToAdd = this.provider.ElementFactory.CreateExpression(string.Format("this.{0}{1}", propertyName, suffixForPassedArgument));
            var newArgument = this.provider.ElementFactory.CreateArgument(ParameterKind.UNKNOWN, null, expressionToAdd);

            ICSharpArgument anchor = null;
            if (index > 0)
            {
                anchor = methodInvocation.Arguments[index - 1];
            }

            ICSharpArgument specifiedParameter = null;
            var parameterIsSpecified = methodInvocation.Arguments.Count > index;
            if (parameterIsSpecified)
            {
                specifiedParameter = methodInvocation.Arguments[index];                
            }

            var parameterIsNotFilled = parameterIsSpecified && specifiedParameter.Value == null;
            if (parameterIsNotFilled)
            {
                methodInvocation.RemoveArgument(specifiedParameter);
                methodInvocation.AddArgumentAfter(newArgument, anchor);
                callWasModified = true;
            }
            // write argument if : no specified parameter yet, last specified parameter has been reached,
            // specified string differs from the one to insert.
            if ((methodInvocation.Arguments.Count == 0) || 
                (index == methodInvocation.Arguments.Count))
            {
                methodInvocation.AddArgumentAfter(newArgument, anchor);
                callWasModified = true;
            }
            return callWasModified;
        }

        private void AppendMockInitializationToMethod(IPropertyDeclaration propertyDeclaration,
                                                      IMethodDeclaration establishContextMethod,
                                                      IType parameterType)
        {
            var factory = CSharpElementFactory.GetInstance(this.provider.PsiModule);
            var language = this.provider.PsiFile.Language;
            var typeName = parameterType.GetPresentableName(language);// propertyDeclaration.Type.GetPresentableName(language);

            var bodyStatements = establishContextMethod.Body.Statements;

            var statement = string.Format("this.{0} = this.MockRepository.CreateMock<{1}>();", propertyDeclaration.NameIdentifier.Name, typeName);
            var isAssignationAlreadyPresent = bodyStatements.Any(o =>
                                                                     {
                                                                         var expressionStatement = o as IExpressionStatement;
                                                                         if (expressionStatement != null)
                                                                         {
                                                                             var assignmentExpression = expressionStatement.Expression as IAssignmentExpression;
                                                                             if (assignmentExpression != null)
                                                                             {
                                                                                 var referenceExpression = assignmentExpression.Dest as IReferenceExpression;
                                                                                 return referenceExpression != null && referenceExpression.NameIdentifier.Name == propertyDeclaration.NameIdentifier.Name;
                                                                             }
                                                                         }

                                                                         // if not correctly casted : it's not an assignation.
                                                                         return false;
                                                                     });

            var initializationStatement = factory.CreateStatement(statement);

            if (bodyStatements.All(s => s.GetText() != statement))
            {
                // The statement is not perfectly exactly the same, but the property is assigned to something : maybe we shouldn't add anything in the establish context.
                if (isAssignationAlreadyPresent)
                {
                    var warningMessage = string.Format("The {0} property was already assigned. This could mean that some context establishment were still underway, therefore, it was left untouched. Please double check your EstablishContext method.", propertyDeclaration.NameIdentifier.Name);
                    Shell.Instance.GetComponent<JetBrains.UI.StatusBar.IStatusBar>().SetText(warningMessage, true);
                    return;
                }
                var lastStatement = bodyStatements.Last();
                ModificationUtil.AddChildAfter(lastStatement, initializationStatement);
            }
        }

        /// <summary>
        /// Build a mock for each interface parameter in a constructor.
        /// </summary>
        /// <param name="ctor">
        /// Represents type constructor.
        /// </param>
        private void BuildMocksForCtor(IParametersOwner ctor)
        {
            var classDeclaration = this.provider.GetSelectedElement<IClassDeclaration>(true, true); // .GetContainingTypeDeclaration();
            var establishContextMethod = this.EstablishContextMethod(classDeclaration);
            foreach (var parameter in ctor.Parameters)
            {
                var shortName = parameter.ShortName;
                var capitalizedLeadingChar = shortName.Substring(0, 1).Capitalize();
                var pascalCasedName = capitalizedLeadingChar + shortName.Substring(1);

                IType argumentType;
                string propertyName;
                string suffixForPassedArgument;
                IPropertyDeclaration propertyDeclaration;
                var propertyExists = true;
                if (parameter.Type.IsInterfaceType())
                {
                    propertyName = string.Format("Injected{0}Mock", pascalCasedName);
                    suffixForPassedArgument = ".Object";                        

                    propertyDeclaration = classDeclaration.PropertyDeclarations.SingleOrDefault(m => m.NameIdentifier.Name == propertyName);
                    if (propertyDeclaration == null)
                    {
                        var genericType = typeof(IMock<>);
                        var clrTypeName = new ClrTypeName(genericType.FullName);

                        var argumentTypeElem = TypeElementUtil.GetTypeElementByClrName(clrTypeName, this.provider.PsiModule);

                        // ... i.e. in the current project in which the plugin is used.
                        var message = string.Format("The type or assembly {0} was not found in project {1}. Are you missing a reference ?", genericType.AssemblyQualifiedName, this.provider.PsiModule.DisplayName);
                        if (argumentTypeElem == null)
                        {
                            throw new InvalidOperationException(message);
                        }

                        try
                        {   
                            argumentType = TypeFactory.CreateType(argumentTypeElem, parameter.Type);
                        }
                        catch (InvalidOperationException e)
                        {
                            throw new InvalidOperationException(message, e);
                        }
                        
                        propertyDeclaration = this.provider.ElementFactory.CreatePropertyDeclaration(argumentType, propertyName);

                        propertyExists = false;
                    }

                    this.AppendMockInitializationToMethod(propertyDeclaration, establishContextMethod, parameter.Type);
                }
                else
                {
                    propertyName = string.Format("Injected{0}", pascalCasedName);
                    suffixForPassedArgument = string.Empty;
                    argumentType = parameter.Type; 
                    
                    propertyDeclaration = classDeclaration.PropertyDeclarations.SingleOrDefault(m => m.NameIdentifier.Name == propertyName);
                    if (propertyDeclaration == null)
                    {
                        propertyExists = false;
                        propertyDeclaration = this.provider.ElementFactory.CreatePropertyDeclaration(argumentType, propertyName);

                        // generate the abstract method declaration for EstablishXXX(). We need to do it after adding the establishMethod (in the case where it was not found in the current class)
                        var propertyEstablishmentMethodName = string.Format("Establish{0}", propertyName);
                        var propertyEstablishmentMethodExists = classDeclaration.MethodDeclarations.Any(o => o.DeclaredName == propertyEstablishmentMethodName);

                        this.AppendGeneratedPropertyEstablishmentToMethod(propertyDeclaration, propertyEstablishmentMethodName, establishContextMethod);

                        if (!propertyEstablishmentMethodExists)
                        {
                            var establishmentMethodDeclaration = this.CreateDeclarationForAbstractPropertyEstablishmentMethod(propertyDeclaration, propertyEstablishmentMethodName, establishContextMethod);
                            ModificationUtil.AddChildBefore(establishContextMethod, establishmentMethodDeclaration);                          
                        }
                        else
                        {
                            var warningMessage = string.Format("{0} was already existing. make sure it is only used where it is supposed to be, and that no dead legacy code use it unintentionnaly.", propertyEstablishmentMethodName);
                            Shell.Instance.GetComponent<JetBrains.UI.StatusBar.IStatusBar>().SetText(warningMessage, true);
                        }
                    }
                }

                if (!propertyExists)
                {
                    var getter = this.provider.ElementFactory.CreateAccessorDeclaration(AccessorKind.GETTER, false);
                    var setter = this.provider.ElementFactory.CreateAccessorDeclaration(AccessorKind.SETTER, false);
                    propertyDeclaration.SetAccessRights(AccessRights.PROTECTED);
                    setter.SetAccessRights(AccessRights.PRIVATE);
                    propertyDeclaration.AddAccessorDeclarationAfter(setter, null);
                    propertyDeclaration.AddAccessorDeclarationAfter(getter, null);
                    ModificationUtil.AddChildBefore(establishContextMethod, propertyDeclaration);
                }
                
                var callWasModified = this.AppendGeneratedPropertyToPassedArguments<IObjectCreationExpression>(propertyName, parameter.ShortName, ctor, suffixForPassedArgument);
                if (callWasModified && propertyExists)
                {
                    var warningMessage = string.Format("The {0} property was already existing. This could mean that some context establishment were still underway, therefore, it was left untouched. Please double check your EstablishContext method.", propertyName);
                    Shell.Instance.GetComponent<JetBrains.UI.StatusBar.IStatusBar>().SetText(warningMessage, true);       
                }
            }
        }

        private void BuildSpecifiedArgumentsForMethod(IParametersOwner method)
        {
            var classDeclaration = this.provider.GetSelectedElement<IClassDeclaration>(true, true); // .GetContainingTypeDeclaration();
            var establishContextMethod = classDeclaration.MethodDeclarations.FirstOrDefault(m => m.NameIdentifier.Name == "EstablishContext");

            if (establishContextMethod == null)
            {
                // RemoveExistingInitializationsInMethod(propertyDeclaration, establishContextMethod);
                establishContextMethod = this.CreateEstablishContextMethod();
                var methodDeclaration = this.provider.GetSelectedElement<IMethodDeclaration>(true, true); // treeNode.GetContainingTypeMemberDeclaration() as IMethodDeclaration; // .MemberDeclarations.Add(EternalLifetime.Instance, propertyDeclaration);
                ModificationUtil.AddChildBefore(methodDeclaration, establishContextMethod);

                // make sure we re-discover the method in order to have its surrounding context AFTER its injection in the code. otherwise, we'll still have the context as an in-memory representation. example, the parent property is of type Sandbox and not IClassBody !
                establishContextMethod = classDeclaration.MethodDeclarations.FirstOrDefault(m => m.NameIdentifier.Name == "EstablishContext");
            }

            // skip the first parameter if it's an extension method.
            IEnumerable<IParameter> parameters = method.IsExtensionMethod()
                             ? method.Parameters.Skip(1)
                             : method.Parameters;

            foreach (var parameter in parameters)
            {
                var shortName = parameter.ShortName;
                var capitalizedLeadingChar = shortName.Substring(0, 1).Capitalize();
                var pascalCasedName = capitalizedLeadingChar + shortName.Substring(1);
                var propertyName = string.Format("Specified{0}", pascalCasedName);

                // this.RemoveExistingPropertyDeclaration(propertyName);
                var propertyDeclaration = this.provider.ElementFactory.CreatePropertyDeclaration(parameter.Type, propertyName);

                var getter = this.provider.ElementFactory.CreateAccessorDeclaration(AccessorKind.GETTER, false);
                var setter = this.provider.ElementFactory.CreateAccessorDeclaration(AccessorKind.SETTER, false);
                propertyDeclaration.SetAccessRights(AccessRights.PROTECTED);
                setter.SetAccessRights(AccessRights.PRIVATE);
                propertyDeclaration.AddAccessorDeclarationAfter(setter, null);
                propertyDeclaration.AddAccessorDeclarationAfter(getter, null);

                this.AppendGeneratedPropertyToPassedArguments<IInvocationExpression>(propertyName, parameter.ShortName, method, string.Empty);

                ModificationUtil.AddChildBefore(establishContextMethod, propertyDeclaration);

                // TODO: [lmbsaf1 14.02.13 11:46] Ask the user for each param if he wants to generate an establishment method.               

                // generate the abstract method declaration for EstablishXXX(). We need to do it after adding the establishMethod (in the case where it was not found in the current class)
                var propertyEstablishmentMethodName = string.Format("Establish{0}", propertyName);

                this.AppendGeneratedPropertyEstablishmentToMethod(propertyDeclaration, propertyEstablishmentMethodName, establishContextMethod);
                var establishmentMethodDeclaration = this.CreateDeclarationForAbstractPropertyEstablishmentMethod(propertyDeclaration, propertyEstablishmentMethodName, establishContextMethod);
                ModificationUtil.AddChildBefore(establishContextMethod, establishmentMethodDeclaration);
            }
        }

        private IMethodDeclaration CreateDeclarationForAbstractPropertyEstablishmentMethod(IPropertyDeclaration propertyDeclaration, 
                                                                                           string propertyEstablishmentMethodName, 
                                                                                           IMethodDeclaration establishContextMethod)
        {
            var methodDeclaration = (IMethodDeclaration)this.provider.ElementFactory.CreateTypeMemberDeclaration(string.Format("protected abstract {0} {1}();", propertyDeclaration.Type.GetPresentableName(this.provider.PsiFile.Language), propertyEstablishmentMethodName));
            return methodDeclaration;
        }

        /// <summary>
        /// Create a method EstablishContextMethod.
        /// </summary>
        /// <returns>
        /// Retruns the interface to <see cref="JetBrains.ReSharper.Psi.CSharp.Tree.IMethodDeclaration"/>
        /// </returns>
        private IMethodDeclaration CreateEstablishContextMethod()
        {
            var methodDeclaration = (IMethodDeclaration)this.provider.ElementFactory.CreateTypeMemberDeclaration("protected override void EstablishContext() {\r\nbase.EstablishContext();\r\n\r\n}");
            return methodDeclaration;
        }

        /// <summary>
        /// Build a text to display in menu item according to the given constructor type.
        /// </summary>
        /// <param name="ctor">
        /// Represents type constructor.
        /// </param>
        /// <returns>
        /// The string to add in a menu item.
        /// </returns>
        private string GetSimpleMenuItemTextForCtor(IConstructor ctor)
        {
            return ctor.GetDeclarations().First().DeclaredName +
                   ctor.Parameters.AggregateString("(", ", ", (builder, 
                                                               parameter) => builder.Append(parameter.Type.GetPresentableName(this.provider.PsiFile.Language) + " " + parameter.ShortName)) + ")";
        }

        /// <summary>
        /// Build a text to display in menu item according to the given method.
        /// </summary>
        /// <param name="method">
        /// Represents the method.
        /// </param>
        /// <returns>
        /// The string to add in a menu item.
        /// </returns>
        private RichText GetSimpleMenuItemTextForMethod(IMethod method)
        {
            return method.ShortName +
                   method.Parameters.AggregateString("(", ", ", (builder, 
                                                                 parameter) => builder.Append(parameter.Type.GetPresentableName(this.provider.PsiFile.Language) + " " + parameter.ShortName)) + ")";
        }

        /// <summary>
        /// Execute this code when the plugin is started with the cursor located on a constructor.
        /// </summary>
        /// <param name="creationExpression">
        /// The selected element on which the plugin is executed.
        /// </param>
        /// <returns>
        /// The method signature to use for filling parameters.
        /// </returns>        
        private IParametersOwner SelectConstructorAsParameterOwner(IObjectCreationExpression creationExpression)
        {
            var typeReference = creationExpression.TypeReference;
            IParametersOwner selectedConstructorParametersOwner = null;

            if ((typeReference != null) && (typeReference.CurrentResolveResult != null) && (typeReference.CurrentResolveResult.DeclaredElement != null))
            {
                var constructedType = (Class)typeReference.CurrentResolveResult.DeclaredElement;

                // If only one available ctor, select it directly. If more, display the ctors available in a menu.
                if (constructedType.Constructors.Count() == 1)
                {
                    selectedConstructorParametersOwner = constructedType.Constructors.First();
                }
                else
                {
                    var jpmComponent = Shell.Instance.GetComponent<JetPopupMenus>();
                    var jpm = jpmComponent.Create();
                    var menuitems = new List<SimpleMenuItem>();

                    constructedType
                        .Constructors
                        .ForEach(ctor => menuitems.Add(
                            new SimpleMenuItem(
                                             this.GetSimpleMenuItemTextForCtor(ctor), 
                                             null, 
                                             () => selectedConstructorParametersOwner = ctor)));

                    jpm.SetItems(menuitems.ToArray());
                    jpm.ShowModal(JetPopupMenu.ShowWhen.NoItemsBannerIfNoItems);
                }
            }

            return selectedConstructorParametersOwner;
        }

        /// <summary>
        /// Execute this code when the plugin is started with the cursor located on a method.
        /// </summary>
        /// <param name="invocationExpression">
        /// The selected element on which the plugin is executed.
        /// </param>
        /// <returns>
        /// The method signature to use for filling parameters.
        /// </returns>
        private IParametersOwner SelectMethodAsParameterOwner(IInvocationExpression invocationExpression)
        {
            IParametersOwner selectedMethodParametersOwner = null;

            var firstClassReference = invocationExpression.GetFirstClassReferences().First();
            if ((firstClassReference != null) && (firstClassReference.CurrentResolveResult != null))
            {
                var resolveResultWithInfo = invocationExpression.GetFirstClassReferences().First().CurrentResolveResult;
                
                // If only one available ctor, select it directly. If more, display the ctors available in a menu.
                if (resolveResultWithInfo.DeclaredElement != null)
                {
                    selectedMethodParametersOwner = (IMethod)resolveResultWithInfo.DeclaredElement;
                }
                else
                {
                    var availableSignatures = firstClassReference.CurrentResolveResult.Result.Candidates;

                    var jpmComponent = Shell.Instance.GetComponent<JetPopupMenus>();
                    var jpm = jpmComponent.Create();
                    var menuitems = new List<SimpleMenuItem>();

                    availableSignatures
                        .OfType<IMethod>()
                        .ForEach(method => menuitems.Add(
                            new SimpleMenuItem(
                                               this.GetSimpleMenuItemTextForMethod(method), 
                                               null, 
                                               () => selectedMethodParametersOwner = method)));

                    jpm.SetItems(menuitems.ToArray());
                    jpm.ShowModal(JetPopupMenu.ShowWhen.NoItemsBannerIfNoItems);
                }
            }

            return selectedMethodParametersOwner;
        }

        private IMethodDeclaration EstablishContextMethod(IClassDeclaration classDeclaration)
        {
            var establishContextMethod = classDeclaration.MethodDeclarations.FirstOrDefault(m => m.NameIdentifier.Name == "EstablishContext");

            if (establishContextMethod == null)
            {
                establishContextMethod = this.CreateEstablishContextMethod();
                var methodDeclaration = this.provider.GetSelectedElement<IMethodDeclaration>(true, true); // treeNode.GetContainingTypeMemberDeclaration() as IMethodDeclaration; // .MemberDeclarations.Add(EternalLifetime.Instance, propertyDeclaration);
                ModificationUtil.AddChildBefore(methodDeclaration, establishContextMethod);

                // make sure we re-discover the method in order to have its surrounding context AFTER its injection in the code. otherwise, we'll still have the context as an in-memory representation. example, the parent property is of type Sandbox and not IClassBody !
                establishContextMethod = classDeclaration.MethodDeclarations.FirstOrDefault(m => m.NameIdentifier.Name == "EstablishContext");
            }
            return establishContextMethod;
        }

        #endregion
    }
}