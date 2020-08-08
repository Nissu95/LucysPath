using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public LevelEditor levelEditor;

    int itemIndex = 0;
    Button button;
    Vector2Int position;
    Image buttonImage;
    Toggle toggle;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        toggle = GetComponentInChildren<Toggle>();

        button.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        UpdateText();
    }

    void OnClick()
    {
        itemIndex++;

        if (itemIndex > LevelCreator.singleton.GetObstacles().Length)
            itemIndex = 0;

        UpdateText();
    }

    void UpdateText()
    {
        if (itemIndex == 0)
            buttonImage.sprite = null;
        else
            buttonImage.sprite = LevelCreator.singleton.GetObstacles()[itemIndex - 1].texture;
    }

    public void SetPosition(Vector2Int _position)
    {
        position = _position;
    }

    public int GetIndex()
    {
        return itemIndex;
    }

    public void SetIndex(int _index)
    {
        itemIndex = _index;
    }

    public bool IsLocked()
    {
        return toggle.isOn;
    }

    public void SetLocked(bool locked)
    {
        toggle.isOn = locked;
    }
}
