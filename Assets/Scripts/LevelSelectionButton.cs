using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] Text levelText;
    [SerializeField] Text needStarsText;
    [SerializeField] Text haveStarsText;
    [SerializeField] Image[] starsUI;
    [SerializeField] Color unlockedStar;
    [SerializeField] Color lockedStar;
    [SerializeField] GameObject starsToPlay;

    Button button;

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

        for (int i = 0; i < starsUI.Length; i++)
            if (i < stars)
                starsUI[i].color = unlockedStar;
            else
                starsUI[i].color = lockedStar;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayLevel);
    }

    private void OnEnable()
    {
        bool isButtonOn = GameManager.singleton.GetHaveStarsToPlay(needStarsText, haveStarsText);

        if ((index + 1) % GameManager.singleton.GetMultipleOf() == 0 && GameManager.singleton.GetIsButtonInteractable(index))
        {
            button.interactable = isButtonOn;
            starsToPlay.SetActive(!button.interactable);
        }
    }

    void PlayLevel()
    {
        SoundManager.singleton.Nyan();
        GameManager.singleton.GetPauseButton().SetActive(true);
        GameManager.singleton.PlayLevel(index);
    }

}
