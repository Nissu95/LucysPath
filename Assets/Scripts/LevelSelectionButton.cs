using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
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
        starsText.text = "Estrellas: " + stars;
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
