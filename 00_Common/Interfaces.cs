public interface IGameInitializer
{
    public void GameInitialize();
}

public interface IInventoryOpenOption
{
    public void Set();
    public void Reset();
}

public interface IVisibleNew
{
    public bool IsNew { get; set; }
}

public interface IAnimation
{
    public void Show(bool _ignorAnimation = false);
    public void Hide(bool _ignorAnimation = false);
}