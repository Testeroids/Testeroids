using System.Reflection;
using JetBrains.ActionManagement;
using JetBrains.Application.PluginSupport;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Testeroids.PrivateWatchDog")]
[assembly: AssemblyDescription("Complains whenever a private field or property is being accessed in a nested Context test class")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Testeroids")]
[assembly: AssemblyProduct("Testeroids.PrivateWatchDog")]
[assembly: AssemblyCopyright("Copyright © Testeroids, 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: ActionsXml("Testeroids.PrivateWatchDog.Actions.xml")]

// The following information is displayed by ReSharper in the Plugins dialog
[assembly: PluginTitle("Private WatchDog")]
[assembly: PluginDescription("Complains whenever a private field or property is being accessed in a nested Context test class")]
[assembly: PluginVendor("Testeroids")]
