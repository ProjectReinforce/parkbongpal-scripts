using UnityEngine.UI;

public interface IGameInitializer
{
    public void GameInitialize();
}

public interface IInventoryOpenOption
{
    public void Set();
    public void Reset();
}
