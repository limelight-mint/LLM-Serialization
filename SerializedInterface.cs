using System;
using System.Collections.Generic;

namespace LLM.Serialization
{
    [Serializable]
    public class SerializedInterface<T>
        where T : ISerializedService
    {
        public Dictionary<Type, UnityEngine.Object> Values;

        public Y Get<Y>()
            where Y : class, ISerializedService
        {
            if(Values[typeof(Y)].GetType() == typeof(Y)) return Values[typeof(Y)] as Y;
            else throw new FormatException($"You tried to convert {Values[typeof(Y)].GetType()} to {typeof(Y)} but requested {typeof(T)}");
        }

    }
}
