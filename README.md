# LLM-Serialization
[Unity] [Extension] Extension for all the services in your app/game and basic initialization thru ScriptableObject links, no need to have a mess with 9999 serialized fields, use just one and share the date across .NET or Unity Objects!

> There is a CollectionistLink for serialization purposes
![Scriptable Object Link Picture](https://mentallystable4sure.dev/assets/images/git/so.png)


> Drag and drop it to any `[SerializedField] private CollectionistLink _link;` field and initialize it (usually on startup of your app)
![Scriptable Object Link Picture](https://mentallystable4sure.dev/assets/images/git/so1.png)


```
public class Initializer : MonoBehaviour
{
    [SerializeField] private CollectionistLink _link;


    private void Awake()
    {
        _link.Initialize();

        Collectionist service = _link.Service;
        ....
```

> Register service you wanna use in this collectionist (`_link.Service` to get actual service from the link whenever you want)
```
    ...
    _link.Initialize();

    Collectionist service = _link.Service;

    service
        .Add(new CloudSaveService())
        .Add(new FirebaseService())
        .Add(new LoginService())
        .Add(new RegisterService())
        
        .Build() 
        //Do NOT forget to build up your services
        // this is async operation, you can use await service.Build();

        ;
    ...
}
```

> [!WARNING] 
> Dont forget to `_link.Service.Remove(service)` or `_link.Service.Remove<TService>()` if you dont need it anymore if service was destroyed!


> Use it whenever you need some service from the collectionist (in this example we use login)
```
public void TryLogin(string username, string password)
{
    CloudSaveService saveService = null;
    LoginService loginService = null;

    _link.Service.Get<CloudSaveService, LoginService>(out saveService, out loginService);

    var result = await loginService.Login(username, password);
    if(result.IsError) saveService.GetLocalAccount(username, password);
    ...
```


<hr></hr>


> [!CAUTION]
> If you gonna use Injections (if you need some services to be inside other services, for example our SaveService neeeded LoginService to get GET request from server with all user inventory), then you NEED to .Add(service) SERVICES IN ORDER! so the first services initialized first, and those who depends on them are the last one. Here is example of service used injection:
<img src="https://mentallystable4sure.dev/assets/images/git/injections.png"></img>

> [!NOTE] 
> You can use/create new links for any service if you want, but thats not that efficient since collectionist already contains everything inside. But we are not stopping u...
```
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
```