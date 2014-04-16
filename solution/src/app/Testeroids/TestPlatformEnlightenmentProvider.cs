namespace Testeroids
{
    using System;
    using System.Reactive.Concurrency;
    using System.Reactive.PlatformServices;

    using Microsoft.Reactive.Testing;

    /// <summary>
    /// Special implementation of <see cref="IPlatformEnlightenmentProvider"/> meant for test environments. It replaces the <see cref="IConcurrencyAbstractionLayer"/> for an instance of <see cref="TestConcurrencyAbstractionLayer"/>.
    /// </summary>
    internal class TestPlatformEnlightenmentProvider : IPlatformEnlightenmentProvider
    {
        #region Fields

        /// <summary>
        /// The fallback <see cref="IPlatformEnlightenmentProvider"/> instance.
        /// </summary>
        private readonly CurrentPlatformEnlightenmentProvider currentPlatformEnlightenmentProvider = new CurrentPlatformEnlightenmentProvider();

        /// <summary>
        /// The concurrency abstraction layer specializer for testing purposes.
        /// </summary>
        private readonly Lazy<TestConcurrencyAbstractionLayer> testConcurrencyAbstractionLayer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestPlatformEnlightenmentProvider"/> class.
        /// </summary>
        public TestPlatformEnlightenmentProvider()
        {
            this.testConcurrencyAbstractionLayer = new Lazy<TestConcurrencyAbstractionLayer>(() => new TestConcurrencyAbstractionLayer(this.currentPlatformEnlightenmentProvider.GetService<IConcurrencyAbstractionLayer>(new object[0])));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the function which can return the <see cref="Microsoft.Reactive.Testing.TestScheduler"/> instance used in the tests.
        /// </summary>
        public Func<TestScheduler> GetTestScheduler
        {
            get
            {
                return this.testConcurrencyAbstractionLayer.Value.GetTestScheduler;
            }

            set
            {
                this.testConcurrencyAbstractionLayer.Value.GetTestScheduler = value;
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
                return (T)(object)this.testConcurrencyAbstractionLayer.Value;
            }

            if (type == typeof(IScheduler) && args != null)
            {
                switch ((string)args[0])
                {
                    case "ThreadPool":
                        return (T)(this.testConcurrencyAbstractionLayer.Value.UseDefaultScheduler
                                       ? (IScheduler)ThreadPoolScheduler.Instance
                                       : this.GetTestScheduler());
                    case "TaskPool":
                        return (T)(this.testConcurrencyAbstractionLayer.Value.UseDefaultScheduler
                                       ? (IScheduler)TaskPoolScheduler.Default
                                       : this.GetTestScheduler());
                    case "NewThread":
                        return (T)(this.testConcurrencyAbstractionLayer.Value.UseDefaultScheduler
                                       ? (IScheduler)NewThreadScheduler.Default
                                       : this.GetTestScheduler());
                }
            }

            return this.currentPlatformEnlightenmentProvider.GetService<T>(args);
        }

        #endregion
    }
}