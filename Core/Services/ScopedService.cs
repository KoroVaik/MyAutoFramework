using Microsoft.Extensions.DependencyInjection;

namespace Core.Services
{
    public enum ServiceLifetime
    {
        TestScoped,
        //FixtureScoped,
        TestRunScoped
    }

    public static class ScopedService
    {
        private static readonly ServiceCollection _collection = new ServiceCollection();
        private static readonly ThreadLocal<IServiceProvider> _scopedProvider = new(BuildProvider);
        private static readonly object _lock = new object();

        public static void Register<TImplementation>(ServiceLifetime serviceLifetime = ServiceLifetime.TestScoped)
            where TImplementation : class
        {
            lock (_lock)
            {
                switch (serviceLifetime)
                {
                    case ServiceLifetime.TestScoped:
                        _collection.AddScoped<TImplementation>();
                        break;
                    //case ServiceLifetime.FixtureScoped:
                    //    _collection.AddScoped<TImplementation>();
                    //    break;
                    case ServiceLifetime.TestRunScoped:
                        _collection.AddSingleton<TImplementation>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
                }

                ResetProvider();
            }
        }

        public static void Register<TType, TImplementation>(ServiceLifetime serviceLifetime = ServiceLifetime.TestScoped)
            where TType : class
            where TImplementation : class, TType
        {
            lock (_lock)
            {
                switch (serviceLifetime)
                {
                    case ServiceLifetime.TestScoped:
                        _collection.AddScoped<TType, TImplementation>();
                        break;
                    //case ServiceLifetime.FixtureScoped:
                    //    _collection.AddScoped<TType, TImplementation>();
                    //    break;
                    case ServiceLifetime.TestRunScoped:
                        _collection.AddSingleton<TType, TImplementation>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
                }

                ResetProvider();
            }
        }

        public static void Register<TType>(TType instance, ServiceLifetime serviceLifetime = ServiceLifetime.TestScoped)
            where TType : class
        {
            Register(_ => instance, serviceLifetime);
        }

        public static void Register<TType>(Func<IServiceProvider, TType> instanceFactory, ServiceLifetime serviceLifetime = ServiceLifetime.TestScoped)
            where TType : class
        {
            lock (_lock)
            {
                switch (serviceLifetime)
                {
                    case ServiceLifetime.TestScoped:
                        _collection.AddScoped(instanceFactory);
                        break;
                    //case ServiceLifetime.FixtureScoped:
                    //    _collection.AddScoped(instanceFactory);
                    //    break;
                    case ServiceLifetime.TestRunScoped:
                        _collection.AddSingleton(instanceFactory);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
                }

                ResetProvider();
            }
        }

        public static TType Get<TType>() where TType : class
        {
            var service = GetNullable<TType>();

            if (service == null)
            {
                throw new InvalidOperationException($"Service of type {typeof(TType).Name} is not registered.");
            }

            return service;
        }

        public static TType? GetNullable<TType>() where TType : class
        {
            try
            {
                var service = _scopedProvider.Value?.GetService<TType>();

                return service;
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                throw new Exception($"An error occurred while resolving {typeof(TType).Name}.", ex);
            }
        }

        internal static void ResetProvider()
        {
            if (_scopedProvider.IsValueCreated)
            {
                _scopedProvider.Value = BuildProvider();
            }
        }

        private static IServiceProvider BuildProvider()
        {
            try
            {
                return _collection.BuildServiceProvider();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to build service provider.", ex);
            }
        }
    }
}
