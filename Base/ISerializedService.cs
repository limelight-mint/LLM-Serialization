using Cysharp.Threading.Tasks;

namespace LLM.Serialization.Base
{

    public interface ISerializedService 
    {
        public UniTask Initialize();
    }
}