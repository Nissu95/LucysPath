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
    Text text;
    private void Awake()
    {
        button = GetComponent<Button>();
        text = button.GetComponentInChildren<Text>();

        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        itemIndex++;

        if (itemIndex > levelEditor.items.Length)
            itemIndex = 0;

        UpdateText();
    }

    void UpdateText()
    {
        if (itemIndex == 0)
            text.text = "";
        else
            text.text = levelEditor.items[itemIndex - 1].name;
    }

    public void SetPosition(Vector2Int _position)
    {
        position = _position;
    }

    public int GetIndex()
    {
        return itemIndex;
    }
}
