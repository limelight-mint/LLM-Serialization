# LLM-Serialization
<sub>Extension for all the services in your app/game and basic initialization thru ScriptableObject links, no need to have a mess with 9999 serialized fields, use just one and share the date across .NET or Unity Objects!</sub>

### Ok but how to actually use it?
> Required <a href="https://github.com/Cysharp/UniTask">UniTask</a> itself, so first-thing-first install a .unitypackage <a href="https://github.com/Cysharp/UniTask/releases">from this link</a>.
> Can be used for default .NET classes if you install UniTask as NuGet package. Also feel free to change whatever you want or collab.

- <a href=#static-services>StaticServices (Global Services)</a>
- <a href=#dynamic-services>DynamicServices (Local Services)</a>
- <a href=#injections>Injections service-to-service<sub>(aka arrays from aliexpress)</sub></a>

<sub>First one get initialized on `Link` creation (e.g database). Second one is a flexible and can be added and removed whenever you want at any point of `Link` or `Collectionist` lifespan. Usually you dont need `DynamicServices` if its not a huge project. 
(unless you are a resource handling maniac who needs to free resources 24/7 or your service is a miner)</sub>

# Static Services
> There is a CollectionistLink for serialization purposes if you want to get services in `MonoBehaviour` objects

![Scriptable Object Link Picture](https://bunbun.cloud/assets/images/git/so.png)

> Drag and drop it to any `[SerializedField] private CollectionistLink _link;` field and initialize it (usually on startup of your app)

![Scriptable Object Link Picture](https://bunbun.cloud/assets/images/git/so1.png)

> Register service you wanna use in this collectionist (`_link.Service` to get actual service from the link whenever you want)
```
public class Initializer : MonoBehaviour
{
    [SerializeField] private CollectionistLink _link;

    private void Awake()
    {
        ServiceCollection globalServices = new ServiceCollection()
        .Add(new SaveService())
        .Add(new SettingsService())
        .Add(new AudioService())
        .Add(new LocalizationService())
        .Add(new PopupService(popupRoot))
        ;

        _link.Create(globalServices).Initialize();
    }
}
```

> [!TIP]
> `_link.Initialize()` is an awaitable operation in case your services do something on startup (like GET request from database, or load Audio). If so, put `await` before creation. (like so: `await _link.Create(globalServices).Initialize();`)

> Now you can use it whenever you want by getting that `ServiceCollection` like `_link.Service` to get actual service from the collection:
```
    [SerializeField] private CollectionistLink _globalCollection;

    ...
    Collectionist collection = _globalCollection.Service;

    collection.Get<AudioService, PopupService>(out var audioService, out var popupService);

    audioService.PlayMusic("HOYOMiX - Shade of Flowers");
    popupService.ShowPopup<CryingFurina>();
    ...
```

<br></br><br></br>
<hr></hr>

# Dynamic Services

Basically, they are the same but without creation process.
> Add services whenever you want:
```
    [SerializeField] private CollectionistLink _globalCollection;

    ...
    _globalCollection.Service
        .Add(new HoyoClientWrapper())
        .Add(new HoyoFpsUnlocker())

        .Build();
    ...
```
> [!TIP]
> `Collectionist.Build()` is an awaitable operation in case your services do something on startup (like GET request from database, or load Audio). If so, put `await` before creation.


> Using them in same pattern, just use `_globalCollection.Service.Get<TService>()`:
```
_globalCollection.Service.Get<HoyoFpsUnlocker>().UnlockFps();
```
> Or if you need service multiple services:
```
_globalCollection.Service.Get<HoyoFpsUnlocker, HoyoClientWrapper>(out var fpsUnlock, out var wrapper);
```

> [!WARNING] 
> Dont forget to `_link.Service.Remove(service)` or `_link.Service.Remove<TService>()` if you dont need it anymore or if service was destroyed!

<br></br><br></br>
<hr></hr>


### Injections
<sub>TODO: Change array injections to the more proper way (class or auto-inject reflection)</sub>
> [!CAUTION]
> If you gonna use Injections (if you need some services to be inside other services, for example our SaveService neeeded LoginService to get GET request from server with all user inventory), then you NEED to .Add(service) SERVICES IN ORDER! so the first services initialized first, and those who depends on them are the last one. Here is example of service used injection:

![Example Code Picture](https://bunbun.cloud/assets/images/git/injections.png)

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
