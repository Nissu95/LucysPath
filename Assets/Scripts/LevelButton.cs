using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] Toggle lockedToggle;
    [SerializeField] Toggle starToggle;

    int itemIndex = 0;
    Button button;
    Vector2Int position;
    Image buttonImage;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

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
        return lockedToggle.isOn;
    }

    public void SetLocked(bool locked)
    {
        lockedToggle.isOn = locked;
    }

    public bool HasStar()
    {
        return starToggle.isOn;
    }

    public void SetStar(bool value)
    {
        starToggle.isOn = value;
    }
}
