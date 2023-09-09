using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryOpenOptionDefault : IInventoryOpenOption
{
    Button selectButton;
    Text selectText;
    Button decompositionButton;

    public InventoryOpenOptionDefault(Button _selectButton, Button _decompositionButton)
    {
        Initialize(_selectButton, _decompositionButton);
    }

    public void Initialize(Button _selectButton, Button _decompositionButton)
    {
        selectButton = _selectButton;
        selectText = selectButton.transform.GetComponentInChildren<Text>();
        decompositionButton = _decompositionButton;
    }

    public void Set()
    {
        // selectButton.onClick.AddListener();
        selectText.text = "강화하기";
        // decompositionButton.onClick.AddListener();
        decompositionButton.gameObject.SetActive(true);
    }
}

public class InventoryOpenOptionMine : IInventoryOpenOption
{
    Button selectButton;
    Text selectText;
    Button decompositionButton;

    public InventoryOpenOptionMine(Button _selectButton, Button _decompositionButton)
    {
        Initialize(_selectButton, _decompositionButton);
    }

    public void Initialize(Button _selectButton, Button _decompositionButton)
    {
        selectButton = _selectButton;
        selectText = selectButton.transform.GetComponentInChildren<Text>();
        decompositionButton = _decompositionButton;
    }

    public void Set()
    {
        // selectButton.onClick.AddListener();
        selectText.text = "빌려주기";
        // decompositionButton.onClick.AddListener();
        decompositionButton.gameObject.SetActive(false);
    }
}

public class InventoryOpenOptionReinforce : IInventoryOpenOption
{
    Button selectButton;
    Text selectText;
    Button decompositionButton;

    public InventoryOpenOptionReinforce(Button _selectButton, Button _decompositionButton)
    {
        Initialize(_selectButton, _decompositionButton);
    }

    public void Initialize(Button _selectButton, Button _decompositionButton)
    {
        selectButton = _selectButton;
        selectText = selectButton.transform.GetComponentInChildren<Text>();
        decompositionButton = _decompositionButton;
    }

    public void Set()
    {
        // selectButton.onClick.AddListener();
        selectText.text = "강화하기";
        // decompositionButton.onClick.AddListener();
        decompositionButton.gameObject.SetActive(false);
    }
}

public class InventoryOpenOptionReinforceMaterial : IInventoryOpenOption
{
    Button selectButton;
    Text selectText;
    Button decompositionButton;

    public InventoryOpenOptionReinforceMaterial(Button _selectButton, Button _decompositionButton)
    {
        Initialize(_selectButton, _decompositionButton);
    }

    public void Initialize(Button _selectButton, Button _decompositionButton)
    {
        selectButton = _selectButton;
        selectText = selectButton.transform.GetComponentInChildren<Text>();
        decompositionButton = _decompositionButton;
    }

    public void Set()
    {
        // decompositionButton.onClick.AddListener();
        decompositionButton.gameObject.SetActive(false);
    }
}

public class InventoryOpenOptionMiniGame : IInventoryOpenOption
{
    Button selectButton;
    Text selectText;
    Button decompositionButton;

    public InventoryOpenOptionMiniGame(Button _selectButton, Button _decompositionButton)
    {
        Initialize(_selectButton, _decompositionButton);
    }

    public void Initialize(Button _selectButton, Button _decompositionButton)
    {
        selectButton = _selectButton;
        selectText = selectButton.transform.GetComponentInChildren<Text>();
        decompositionButton = _decompositionButton;
    }

    public void Set()
    {
        // selectButton.onClick.AddListener();
        selectText.text = "선택하기";
        // decompositionButton.onClick.AddListener();
        decompositionButton.gameObject.SetActive(false);
    }
}