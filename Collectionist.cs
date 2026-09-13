using Cysharp.Threading.Tasks;
using LLM.Serialization.Base;

namespace LLM.Serialization
{
    /// <summary>
    /// Service Collection (Service Locator / Low effort DI). Store your services here, either static or dynamic, and get them from any other place via ScriptableObject Link.
    /// <br /> <br />
    /// Examples and How To: https://github.com/limelight-mint/LLM-Serialization
    /// </summary>
    public class Collectionist
    {
        public ServiceCollection StaticServices;
        public ServiceCollection DynamicServices;

        public bool IsInitialized => StaticServices.IsInitialized;


        /// <summary>
        /// Initializer for Static Services that are gonna be here forever.
        /// <br /> <b>DONT FORGET</b> TO RUN <see cref="Initialize"/> AFTERWARDS!
        /// </summary>
        /// <param name="staticGlobalServices">List of service to cache for entire lifespan (singleton)</param>
        public Collectionist(ServiceCollection staticGlobalServices)
        {
            StaticServices = staticGlobalServices;
            DynamicServices = new ServiceCollection();
        }

        /// <summary>
        /// Initializes static services (global)
        /// </summary>
        public async UniTask Initialize() => await StaticServices.InitializeAsync();

        /// <summary>
        /// Build new local services and initialize them 
        /// <br> if you used .Add(IService).Add(IService) pattern just put .Build() in the end</br>
        /// </summary>
        /// <returns></returns>
        public async UniTask Build() => await DynamicServices.InitializeAsync();

        /// <summary>
        /// Adds local service
        /// </summary>
        public Collectionist Add(IService service)
        {
            DynamicServices.Add(service);
            return this;
        }

        /// <summary>
        /// Removes local service
        /// </summary>
        public Collectionist Remove(IService service)
        {
            DynamicServices.Remove(service);
            return this;
        }

        /// <summary>
        /// Removes local service
        /// </summary>
        public Collectionist Remove<TService>() where TService : IService
        {
            DynamicServices.Remove<TService>();
            return this;
        }

        /// <summary>
        /// Get the service you want from a service collector <br />
        /// <b>NOTE:</b> Should be registered beforehand either via static registrator on new Collectionist(here goes services u want) and initialized via <see cref="Initialize"/> <br />
        /// <b>OR</b> added in runtime to the current services via <see cref="Add(IService)"/> and initialized via <see cref="Build"/>
        /// </summary>
        public TService Get<TService>()
            where TService : IService
        {
            return StaticServices.Has(typeof(TService)) ? StaticServices.Get<TService>() : DynamicServices.Get<TService>();
        }

        public void Get<T1>(out T1 param1)
            where T1 : IService
        {
            param1 = StaticServices.Has(typeof(T1)) ? StaticServices.Get<T1>() : DynamicServices.Get<T1>();
        }

        public void Get<T1, T2>(out T1 param1, out T2 param2)
            where T1 : IService
            where T2 : IService
        {
            param1 = StaticServices.Has(typeof(T1)) ? StaticServices.Get<T1>() : DynamicServices.Get<T1>();
            param2 = StaticServices.Has(typeof(T2)) ? StaticServices.Get<T2>() : DynamicServices.Get<T2>();
        }

        public void Get<T1, T2, T3>(out T1 param1, out T2 param2, out T3 param3)
            where T1 : IService
            where T2 : IService
            where T3 : IService
        {
            param1 = StaticServices.Has(typeof(T1)) ? StaticServices.Get<T1>() : DynamicServices.Get<T1>();
            param2 = StaticServices.Has(typeof(T2)) ? StaticServices.Get<T2>() : DynamicServices.Get<T2>();
            param3 = StaticServices.Has(typeof(T3)) ? StaticServices.Get<T3>() : DynamicServices.Get<T3>();
        }

        public void Get<T1, T2, T3, T4>(out T1 param1, out T2 param2, out T3 param3, out T4 param4)
            where T1 : IService
            where T2 : IService
            where T3 : IService
            where T4 : IService
        {
            param1 = StaticServices.Has(typeof(T1)) ? StaticServices.Get<T1>() : DynamicServices.Get<T1>();
            param2 = StaticServices.Has(typeof(T2)) ? StaticServices.Get<T2>() : DynamicServices.Get<T2>();
            param3 = StaticServices.Has(typeof(T3)) ? StaticServices.Get<T3>() : DynamicServices.Get<T3>();
            param4 = StaticServices.Has(typeof(T4)) ? StaticServices.Get<T4>() : DynamicServices.Get<T4>();
        }

        public void Get<T1, T2, T3, T4, T5>(out T1 param1, out T2 param2, out T3 param3, out T4 param4, out T5 param5)
            where T1 : IService
            where T2 : IService
            where T3 : IService
            where T4 : IService
            where T5 : IService
        {
            param1 = StaticServices.Has(typeof(T1)) ? StaticServices.Get<T1>() : DynamicServices.Get<T1>();
            param2 = StaticServices.Has(typeof(T2)) ? StaticServices.Get<T2>() : DynamicServices.Get<T2>();
            param3 = StaticServices.Has(typeof(T3)) ? StaticServices.Get<T3>() : DynamicServices.Get<T3>();
            param4 = StaticServices.Has(typeof(T4)) ? StaticServices.Get<T4>() : DynamicServices.Get<T4>();
            param5 = StaticServices.Has(typeof(T5)) ? StaticServices.Get<T5>() : DynamicServices.Get<T5>();
        }

        public void OnServiceDispose()
        {
            DynamicServices?.Dispose();
            StaticServices?.Dispose();
        }
    }
}
