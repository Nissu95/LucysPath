using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
    int index = 0;
    public void SetNumber(int _index)
    {
        GetComponentInChildren<Text>().text = (_index + 1).ToString();
        index = _index;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PlayLevel);
    }

    void PlayLevel()
    {
        LevelSelectionManager.singleton.PlayLevel(index);
    }

    public void SetStars()
    {

    }
}
