using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] Text levelText;
    [SerializeField] Text starsText;

    int index = 0;
    int stars = 0;
    public void SetNumber(int _index)
    {
        levelText.text = (_index + 1).ToString();
        index = _index;
    }

    public void SetStars(int _stars)
    {
        stars = _stars;
        switch (GameManager.singleton.GetLanguage())
        {
            case Languages.English:
                starsText.text = "Stars: " + stars;
                break;
            case Languages.Spanish:
                starsText.text = "Estrellas: " + stars;
                break;
            default:
                break;
        }
    }
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PlayLevel);
    }

    void PlayLevel()
    {
        GameManager.singleton.PlayLevel(index);
        GameManager.singleton.GetPauseButton().SetActive(true);
    }
}
