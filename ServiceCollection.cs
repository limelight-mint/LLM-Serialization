using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LLM.Serialization.Base;

namespace LLM.Serialization
{
    public class ServiceCollection
    {
        protected Dictionary<Type, IService> Services;

        public bool IsInitialized { get; protected set; }

        public ServiceCollection()
        {
            Services = new Dictionary<Type, IService>();
        }

        public virtual void Initialize()
        {
            foreach (var item in Services.Values)
            {
                if(item.IsInitialized) continue;
                item.Initialize(this).Forget();
            }
            IsInitialized = true;
        }

        public virtual async UniTask InitializeAsync()
        {
            foreach (var item in Services.Values)
            {
                if(item.IsInitialized) continue;
                await item.Initialize(this);
            }
            IsInitialized = true;
        }

        public virtual bool Has(Type service) => Services.ContainsKey(service);
        public virtual bool Has(IService service) => Services.ContainsKey(service.GetType());

        public bool IsServiceInitialized<TService>() where TService : IService => Has(typeof(TService)) ? Get<TService>().IsInitialized : false;

        public virtual ServiceCollection Add(IService service)
        {
            IsInitialized = false; //just in case so developer will use Build()
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

        public void Get<T1>(out T1 service1)
            where T1 : IService
        {
            service1 = Get<T1>();
        }

        public void Get<T1, T2>(out T1 service1, out T2 service2)
            where T1 : IService
            where T2 : IService
        {
            Get<T1>(out service1);
            service2 = Get<T2>();
        }

        public void Get<T1, T2, T3>(out T1 service1, out T2 service2, out T3 service3)
            where T1 : IService
            where T2 : IService
            where T3 : IService
        {
            Get<T1, T2>(out service1, out service2);
            service3 = Get<T3>();
        }

        public void Get<T1, T2, T3, T4>(out T1 service1, out T2 service2, out T3 service3, out T4 service4)
            where T1 : IService
            where T2 : IService
            where T3 : IService
            where T4 : IService
        {
            Get<T1, T2, T3>(out service1, out service2, out service3);
            service4 = Get<T4>();
        }

        public void Get<T1, T2, T3, T4, T5>(out T1 service1, out T2 service2, out T3 service3, out T4 service4, out T5 service5)
            where T1 : IService
            where T2 : IService
            where T3 : IService
            where T4 : IService
            where T5 : IService
        {
            Get<T1, T2, T3, T4>(out service1, out service2, out service3, out service4);
            service5 = Get<T5>();
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
