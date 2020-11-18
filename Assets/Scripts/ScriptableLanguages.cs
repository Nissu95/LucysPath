using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Language")]
public class ScriptableLanguages : ScriptableObject
{
    [SerializeField] string optionsText;

    [SerializeField] string winText;

    [SerializeField] string pauseTitle;

    [SerializeField] string quitWarningTitle;
    
    [SerializeField] string privacyPoliciesText;
    [SerializeField] string[] languagesDropdownTxt;

    [SerializeField] string needStarsString;
    [SerializeField] string haveStarsString;

    public string[] GetLanguagesDropdownTxt()
    {
        return languagesDropdownTxt;
    }

    public string GetPrivacyPoliciesText()
    {
        return privacyPoliciesText;
    }

    public string GetQuitWarningTitle()
    {
        return quitWarningTitle;
    }

    public string GetPauseTitle()
    {
        return pauseTitle;
    }

    public string GetOptionsText()
    {
        return optionsText;
    }

    public string GetWinText()
    {
        return winText;
    }

    public string GetNeedStarsString()
    {
        return needStarsString;
    }

    public string GetHaveStarsString()
    {
        return haveStarsString;
    }
}
