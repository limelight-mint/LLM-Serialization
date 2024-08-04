using Cysharp.Threading.Tasks;

namespace LLM.Serialization
{

    public interface ISerializedService 
    {
        public UniTask Initialize();
    }
}