
namespace LLM.Serialization.Base
{

    /// <summary>
    /// This is an example of how you can change or expand functionality for your needs
    /// <br></br>This could be used if you have LoadableServiceCollection : ServiceCollection or smth like that
    /// <br></br>where you override InitializeAsync() method for example and make it increase LoadingBar or smth
    /// </summary>
    public interface ILoadableService : IService
    {
        public virtual string LoadingName { get { return GetType().Name; } }
    }
}