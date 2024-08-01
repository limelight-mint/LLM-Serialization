
using UnityEngine;

namespace MentallyStable.Serialization.Links
{

    [CreateAssetMenu(fileName = "CollectionistLink", menuName = "Services/Links/CollectionistLink", order = 1)]
    public class CollectionistLink : ScriptableObject, ISerializedService
    {
        /// <summary>
        /// use Service = new Collectionist(new ServiceCollection().Add(services)); or Create(services) before init
        /// </summary>
        public Collectionist Service { get; private set; }

        public void Create(ServiceCollection servicesList)
        {
            Service = new Collectionist(servicesList);
        }

        public async void Initialize() => await Service.Initialize();
    }
}