using Cysharp.Threading.Tasks;

namespace LLM.Serialization.Base
{

    public interface IService
    {
        /// <summary>
        /// Initialization for the service itself
        /// </summary>
        /// <param name="services">Service collector with previous order items initialized (order matters, all services registered <b>BEFORE THIS SERVICE</b> will be available to <see cref="ServiceCollection.Get"/>>)</param>
        public UniTask Initialize(ServiceCollection services);

        public bool IsInitialized { get; }

        public void OnServiceDispose();
    }
}