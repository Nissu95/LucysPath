using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Language")]
public class ScriptableLanguages : ScriptableObject
{
    [SerializeField] string playText;
    [SerializeField] string optionsText;
    [SerializeField] string exitText;

    [SerializeField] string winText;
    [SerializeField] string nextLevel;
    [SerializeField] string playAgain;
    [SerializeField] string backToMenu;
    [SerializeField] string starText;
    [SerializeField] string maxStarsText;

    [SerializeField] string pauseTitle;
    [SerializeField] string continueText;
    [SerializeField] string restartText;

    [SerializeField] string quitWarningTitle;
    [SerializeField] string yesText;
    [SerializeField] string noText;
    
    [SerializeField] string privacyPoliciesText;
    [SerializeField] string muteText;

    public string GetPrivacyPoliciesText()
    {
        return privacyPoliciesText;
    }

    public string GetMuteText()
    {
        return muteText;
    }

    public string GetQuitWarningTitle()
    {
        return quitWarningTitle;
    }

    public string GetYes()
    {
        return yesText;
    }

    public string GetNo()
    {
        return noText;
    }

    public string GetPauseTitle()
    {
        return pauseTitle;
    }

    public string GetContinueText()
    {
        return continueText;
    }

    public string GetRestartText()
    {
        return restartText;
    }

    public string GetPlayText()
    {
        return playText;
    }

    public string GetOptionsText()
    {
        return optionsText;
    }

    public string GetExitText()
    {
        return exitText;
    }

    public string GetWinText()
    {
        return winText;
    }

    public string GetNextLevelText()
    {
        return nextLevel;
    }

    public string GetPlayAgainText()
    {
        return playAgain;
    }

    public string GetBackToMenuText()
    {
        return backToMenu;
    }

    public string GetStarsText()
    {
        return starText;
    }

    public string GetMaxStarsText()
    {
        return maxStarsText;
    }
}
