using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace LLM.Serialization.Base
{

    public interface IService
    {
        public UniTask Initialize();

        public bool IsInitialized { get; }
        public List<Type> InjectionQueue { get { return new List<Type>(0); } }

        public void Inject(params IService[] servicesToInject);

        public void OnServiceDispose();
    }
}