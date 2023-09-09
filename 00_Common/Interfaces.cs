using UnityEngine.UI;

public interface IGameInitializer
{
    public void GameInitialize();
}

public interface IInventoryOpenOption
{
    public void Initialize(Button _selectButton, Button _decompositionButton);
    public void Set();
}
