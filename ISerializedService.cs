using Cysharp.Threading.Tasks;

namespace MentallyStable.Serialization
{

    public interface ISerializedService 
    {
        public UniTask Initialize();
    }
}