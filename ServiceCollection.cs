using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MentallyStable.Serialization.Base;

namespace MentallyStable.Serialization
{
    public class ServiceCollection
    {
        protected Dictionary<Type, IService> Services;

        public bool IsInitialized { get; private set; }

        public ServiceCollection()
        {
            Services = new Dictionary<Type, IService>();
        }

        public virtual void Initialize()
        {
            foreach (var item in Services.Values)
            {
                if(item.IsInitialized) continue;
                item.Initialize().Forget();
            }
            IsInitialized = true;
        }

        public virtual async UniTask InitializeAsync()
        {
            foreach (var item in Services.Values)
            {
                if(item.IsInitialized) continue;
                await item.Initialize();
            }
            IsInitialized = true;
        }

        public virtual bool Has(Type service) => Services.ContainsKey(service);
        public virtual bool Has(IService service) => Services.ContainsKey(service.GetType());

        public bool IsServiceInitialized<TService>() where TService : IService => Has(typeof(TService)) ? Get<TService>().IsInitialized : false;

        public virtual ServiceCollection Add(IService service)
        {
            IsInitialized = false; //just in case so developer will use Build()
            InjectNeededServices(service);
            Services.Add(service.GetType(), service);
            return this;
        }

        public virtual ServiceCollection Remove(IService service)
        {
            if(!Has(service)) return this;
            service.OnServiceDispose();
            Services.Remove(service.GetType());
            return this;
        }

        public virtual ServiceCollection Remove<TService>()
            where TService : IService
        {
            if(!Has(typeof(TService))) return this;
            Services[typeof(TService)].OnServiceDispose();
            Services.Remove(typeof(TService));
            return this;
        }

        public T Get<T>()
            where T : IService
        {
            if(!Services.TryGetValue(typeof(T), out var service)) throw new FormatException($"[ServiceCollection] <color=yellow>{typeof(T)}</color> <color=red>NOT</color> registered.");
            return (T)service;
        }

        public void Get<T1>(out T1 param1)
            where T1 : IService
        {
            param1 = Get<T1>();
        }

        public void Get<T1, T2>(out T1 param1, out T2 param2)
            where T1 : IService
            where T2 : IService
        {
            Get<T1>(out param1);
            param2 = Get<T2>();
        }

        public void Get<T1, T2, T3>(out T1 param1, out T2 param2, out T3 param3)
            where T1 : IService
            where T2 : IService
            where T3 : IService
        {
            Get<T1, T2>(out param1, out param2);
            param3 = Get<T3>();
        }

        public void Get<T1, T2, T3, T4>(out T1 param1, out T2 param2, out T3 param3, out T4 param4)
            where T1 : IService
            where T2 : IService
            where T3 : IService
            where T4 : IService
        {
            Get<T1, T2, T3>(out param1, out param2, out param3);
            param4 = Get<T4>();
        }

        private void InjectNeededServices(IService service)
        {
            var injectionQueue = service.InjectionQueue;

            if(injectionQueue.Count <= 0) return;
            List<IService> servicesReadyForInjection = new List<IService>();
            foreach (var serviceToInject in Services)
            {
                if(!injectionQueue.Contains(serviceToInject.Key)) continue;
                servicesReadyForInjection.Add(serviceToInject.Value);
                injectionQueue.Remove(serviceToInject.Key);
            }

            service.Inject(servicesReadyForInjection.ToArray());
        }

        public void Dispose()
        {
            foreach (var service in Services.Values)
            {
                service?.OnServiceDispose();
            }
        }
    }
}
