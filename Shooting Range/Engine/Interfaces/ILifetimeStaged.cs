namespace Engine {
internal interface ILifetimeStaged {
    void Start();
    void PreDestroy();
    void Destroy();
}
}
