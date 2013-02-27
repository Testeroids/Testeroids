// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestPlatformEnlightenmentProvider.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System.Reactive.Concurrency;
    using System.Reactive.PlatformServices;

    using Microsoft.Reactive.Testing;

    /// <summary>
    /// Special implementation of <see cref="IPlatformEnlightenmentProvider"/> meant for test environments. It replaces the <see cref="IConcurrencyAbstractionLayer"/> for an instance of <see cref="TestConcurrencyAbstractionLayer"/>.
    /// </summary>
    public class TestPlatformEnlightenmentProvider : IPlatformEnlightenmentProvider
    {
        #region Fields

        /// <summary>
        /// The fallback <see cref="IPlatformEnlightenmentProvider"/> instance.
        /// </summary>
        private readonly CurrentPlatformEnlightenmentProvider currentPlatformEnlightenmentProvider = new CurrentPlatformEnlightenmentProvider();

        /// <summary>
        /// The concurrency abstraction layer specializer for testing purposes.
        /// </summary>
        private readonly TestConcurrencyAbstractionLayer testConcurrencyAbstractionLayer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestPlatformEnlightenmentProvider"/> class.
        /// </summary>
        public TestPlatformEnlightenmentProvider()
        {
            this.testConcurrencyAbstractionLayer = new TestConcurrencyAbstractionLayer(this.currentPlatformEnlightenmentProvider.GetService<IConcurrencyAbstractionLayer>(new object[0]));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="Microsoft.Reactive.Testing.TestScheduler"/> instance used in the tests.
        /// </summary>
        public TestScheduler TestScheduler
        {
            get
            {
                return this.testConcurrencyAbstractionLayer.TestScheduler;
            }

            set
            {
                this.testConcurrencyAbstractionLayer.TestScheduler = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// (Infrastructure) Tries to gets the specified service.
        /// </summary>
        /// <remarks>Uses <see cref="CurrentPlatformEnlightenmentProvider"/> to retrieve services other than <see cref="IConcurrencyAbstractionLayer"/>.</remarks>
        /// <typeparam name="T">Service type.</typeparam><param name="args">Optional set of arguments.</param>
        /// <returns>
        /// Service instance or null if not found.
        /// </returns>
        public virtual T GetService<T>(object[] args) where T : class
        {
            var type = typeof(T);

            if (type == typeof(IConcurrencyAbstractionLayer))
            {
                return (T)(object)this.testConcurrencyAbstractionLayer;
            }

            if (type == typeof(IScheduler) && args != null)
            {
                switch ((string)args[0])
                {
                    case "ThreadPool":
                        return (T)(this.TestScheduler ?? (IScheduler)ThreadPoolScheduler.Instance);
                    case "TaskPool":
                        return (T)(this.TestScheduler ?? (IScheduler)TaskPoolScheduler.Default);
                    case "NewThread":
                        return (T)(this.TestScheduler ?? (IScheduler)NewThreadScheduler.Default);
                }
            }

            return this.currentPlatformEnlightenmentProvider.GetService<T>(args);
        }

        #endregion
    }
}