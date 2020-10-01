using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { Play, MainMenu, Options, Pause, SelectionLevel, QuitWarning }
//public enum Languages { Spanish, English }

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

#pragma warning disable 649
    [SerializeField] string mainMenuName;
    [SerializeField] GameObject pauseGO;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject quitWarning;
    [SerializeField] GameObject optionsGO;
    [SerializeField] Color[] portalColors;

    GameObject winObj;
    int stars;
    int recordStars = 0;
    GameObject mainMenu;
    Pathfinding playerPath;

    List<Portal> portalsActive = new List<Portal>();
    Portal[] portals;

    GameState gs;

    //----------------------------------------------------------------------------
    //Language Variables

    [SerializeField] ScriptableLanguages english;
    [SerializeField] ScriptableLanguages spanish;

    //Main Menu
    [SerializeField] Text playText;
    [SerializeField] Text optionsText;
    [SerializeField] Text exitTest;

    //Win
    [SerializeField] Text winText;
    [SerializeField] Text nextLevel;
    [SerializeField] Text playAgain;
    [SerializeField] Text backToMenu;
    [SerializeField] Text starText;
    [SerializeField] Text maxStarsText;

    //Level Selection Panel
    string starString;

    //Pause
    [SerializeField] Text pauseTitle;
    [SerializeField] Text continueText;
    [SerializeField] Text restartText;
    [SerializeField] Text backToMenuPause;
    [SerializeField] Text pauseButtonText;

    //Quit Warning
    [SerializeField] Text quitWarningTitle;
    [SerializeField] Text yesText;
    [SerializeField] Text noText;

    //Options Menu
    [SerializeField] Text optionsTitle;
    [SerializeField] Text privacyPoliciesText;
    [SerializeField] Text muteText;
    [SerializeField] Text backToMenuOM;
    [SerializeField] Dropdown languagesDropdown;

    SystemLanguage language;
    int dropdownValue = 0;

    //----------------------------------------------------------------------------
    //Level Selection Variables

    [SerializeField] GameObject levelSelectionButtonPrefab;
#pragma warning disable 649
    [SerializeField] private Transform selectionPanel;

    List<Level> levels;
    List<LevelWon> levelsWon;
    List<LevelSelectionButton> buttons = new List<LevelSelectionButton>();
    int currentLevel = 0;
    bool lastWon = false;

    //----------------------------------------------------------------------------

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (singleton != null)
            Destroy(gameObject);
        else
            singleton = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
        mainMenu = GameObject.Find(mainMenuName);
        gs = GameState.MainMenu;

        switch (Application.systemLanguage)
        {
            case SystemLanguage.Spanish:
                language = SystemLanguage.Spanish;
                dropdownValue = 0;
                break;
            case SystemLanguage.English:
                language = SystemLanguage.English;
                dropdownValue = 1;
                break;
            default:
                language = SystemLanguage.English;
                dropdownValue = 1;
                break;
        }
    }

    private void Start()
    {
        if (mainMenu)
            mainMenu.SetActive(true);

        LevelSelectionStart();

        if (selectionPanel)
            selectionPanel.gameObject.SetActive(false);

        if (pauseButton)
            pauseButton.SetActive(false);

        if (pauseGO)
            pauseGO.SetActive(false);

        languagesDropdown.value = dropdownValue;
        ChangeLanguge();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (gs)
            {
                case GameState.Play:
                    PauseButton();
                    break;
                case GameState.MainMenu:
                    quitWarning.SetActive(true);
                    gs = GameState.QuitWarning;
                    break;
                case GameState.Options:
                    optionsGO.SetActive(false);
                    mainMenu.SetActive(true);
                    gs = GameState.MainMenu;
                    break;
                case GameState.Pause:
                    ContinueButton();
                    break;
                case GameState.SelectionLevel:
                    selectionPanel.gameObject.SetActive(false);
                    mainMenu.SetActive(true);
                    gs = GameState.MainMenu;
                    break;
                case GameState.QuitWarning:
                    quitWarning.SetActive(false);
                    gs = GameState.MainMenu;
                    break;
            }
        }
    }

    public void ChangeLanguge()
    {
        switch (language)
        {
            case SystemLanguage.English:
                playText.text = english.GetPlayText();
                optionsText.text = english.GetOptionsText();
                exitTest.text = english.GetExitText();

                winText.text = english.GetWinText();
                nextLevel.text = english.GetNextLevelText();
                playAgain.text = english.GetPlayAgainText();
                backToMenu.text = english.GetBackToMenuText();
                starText.text = english.GetStarsText() + stars;
                maxStarsText.text = english.GetMaxStarsText() + recordStars;

                starString = english.GetStarsText();
                for (int i = 0; i < buttons.Count; i++)
                    buttons[i].SetText(starString);

                pauseTitle.text = english.GetPauseTitle();
                continueText.text = english.GetContinueText();
                restartText.text = english.GetRestartText();
                backToMenuPause.text = english.GetBackToMenuText();
                pauseButtonText.text = english.GetPauseTitle();

                quitWarningTitle.text = english.GetQuitWarningTitle();
                yesText.text = english.GetYes();
                noText.text = english.GetNo();

                optionsTitle.text = english.GetOptionsText();
                privacyPoliciesText.text = english.GetPrivacyPoliciesText();
                muteText.text = english.GetMuteText();
                backToMenuOM.text = english.GetBackToMenuText();

                for (int i = 0; i < languagesDropdown.options.Count; i++)
                    languagesDropdown.options[i].text = english.GetLanguagesDropdownTxt()[i];

                languagesDropdown.captionText.text = english.GetLanguagesDropdownTxt()[1];

                break;

            case SystemLanguage.Spanish:
                playText.text = spanish.GetPlayText();
                optionsText.text = spanish.GetOptionsText();
                exitTest.text = spanish.GetExitText();

                winText.text = spanish.GetWinText();
                nextLevel.text = spanish.GetNextLevelText();
                playAgain.text = spanish.GetPlayAgainText();
                backToMenu.text = spanish.GetBackToMenuText();
                starText.text = spanish.GetStarsText() + stars;
                maxStarsText.text = spanish.GetMaxStarsText() + recordStars;

                starString = spanish.GetStarsText();
                for (int i = 0; i < buttons.Count; i++)
                    buttons[i].SetText(starString);

                pauseTitle.text = spanish.GetPauseTitle();
                continueText.text = spanish.GetContinueText();
                restartText.text = spanish.GetRestartText();
                backToMenuPause.text = spanish.GetBackToMenuText();
                pauseButtonText.text = spanish.GetPauseTitle();

                quitWarningTitle.text = spanish.GetQuitWarningTitle();
                yesText.text = spanish.GetYes();
                noText.text = spanish.GetNo();

                optionsTitle.text = spanish.GetOptionsText();
                privacyPoliciesText.text = spanish.GetPrivacyPoliciesText();
                muteText.text = spanish.GetMuteText();
                backToMenuOM.text = spanish.GetBackToMenuText();

                for (int i = 0; i < languagesDropdown.options.Count; i++)
                    languagesDropdown.options[i].text = spanish.GetLanguagesDropdownTxt()[i];

                languagesDropdown.captionText.text = spanish.GetLanguagesDropdownTxt()[0];

                break;
        }
    }

    public void HandleInputDataDropdown()
    {
        switch (languagesDropdown.value)
        {
            case 0:
                language = SystemLanguage.Spanish;
                break;
            case 1:
                language = SystemLanguage.English;
                break;
        }
        ChangeLanguge();
    }

    public void ConnectPortals()
    {
        for (int i = 0; i < portalsActive.Count; i++)
        {

            for (int j = 0; j < portalsActive.Count; j++)
            {
                if (portalsActive[i] != portalsActive[j] && !portalsActive[i].GetConnection() && !portalsActive[j].GetConnection())
                {
                    portalsActive[i].SetConnection(portalsActive[j]);
                    portalsActive[j].SetConnection(portalsActive[i]);
                    portalsActive[j].SetMeshMaterialColor(portalsActive[i].GetActiveColor());
                    portalsActive.Clear();
                    return;
                }
            }
        }
    }

    public void AddPortalActive(Portal portal)
    {
        portalsActive.Add(portal);
    }

    public bool IsPortalConnect()
    {
        if (portalsActive.Count >= 1)
            return true;
        else
            return false;
    }

    public void RemovePortalActive(Portal portal)
    {
        portalsActive.Remove(portal);
    }

    public void ResetPortals()
    {
        portals = FindObjectsOfType<Portal>();

        for (int i = 0; i < portals.Length; i++)
            portals[i].SetActiveColor(portalColors[i]);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        winObj = FindInactiveObjectByName("Win");
        stars = 0;
    }

    public void LevelWin()
    {
        if (stars > recordStars)
            recordStars = stars;

        if (winObj)
            winObj.SetActive(true);

        pauseButton.SetActive(false);

        LevelsLoader.SaveLevelWon(recordStars, currentLevel);
        Level level = LevelsLoader.GetLevel(currentLevel);
        level.SetWon(true);

        playerPath.GetFSM().SetEvent(Event.ToIdle);

        UpdateLevelSelection();

        //AdmobScript.singleton.RequestInterstitial();
        AdmobScript.singleton.ShowInterstitialAd();
    }

    public void StarsCount()
    {
        stars++;
    }

    public GameObject GetPauseButton()
    {
        return pauseButton;
    }

    //----------------------------------------------------------------------------
    //Level Selection Functions

    void LevelSelectionStart()
    {
        levels = LevelsLoader.GetLevels();
        levelsWon = LevelsLoader.GetLevelsWon();

        for (int i = 0; i < levels.Count; i++)
        {
            GameObject buttonInstance = Instantiate<GameObject>(levelSelectionButtonPrefab, selectionPanel);
            LevelSelectionButton button = buttonInstance.GetComponent<LevelSelectionButton>();
            buttons.Add(button);

            button.SetNumber(i);
        }

        UpdateLevelSelection();
    }

    public void UpdateLevelSelection()
    {
        levelsWon = LevelsLoader.GetLevelsWon();

        for (int i = 0; i < buttons.Count; i++)
        {
            Button buttonScript = buttons[i].GetComponent<Button>();

            bool won = (levelsWon != null && i < levelsWon.Count);

            if (won)
            {
                buttons[i].SetStars(levelsWon[i].GetStars());
                buttons[i].SetText(starString);
            }

            if (i > 0)
                buttonScript.interactable = won;

            if (lastWon == true)
                buttonScript.interactable = true;

            lastWon = won;
        }
    }

    //----------------------------------------------------------------------------
    //Buttons

    public void PlayButton()
    {
        mainMenu.SetActive(false);
        selectionPanel.gameObject.SetActive(true);
        portalsActive.Clear();
        gs = GameState.SelectionLevel;
    }

    public void PlayLevel(int index)
    {
        selectionPanel.gameObject.SetActive(false);
        LevelCreator.singleton.CreateLevel(LevelsLoader.GetLevel(index));

        currentLevel = index;

        playerPath = FindObjectOfType<Pathfinding>();

        stars = 0;

        LevelWon levelWon = LevelsLoader.GetLevelWon(index);

        if (levelWon != null)
            recordStars = levelWon.GetStars();
        else
            recordStars = 0;

        gs = GameState.Play;
    }

    public void PlayAgain()
    {
        ContinueButton();
        winObj.SetActive(false);
        pauseButton.SetActive(true);
        portalsActive.Clear();
        PlayLevel(currentLevel);
    }

    public void NextLevelButton()
    {
        winObj.SetActive(false);
        portalsActive.Clear();
        currentLevel++;
        PlayLevel(currentLevel);
    }

    public void BackToMenuButton()
    {
        portalsActive.Clear();
        ContinueButton();
        winObj.SetActive(false);
        mainMenu.SetActive(true);
        pauseButton.SetActive(false);
        optionsGO.SetActive(false);

        LevelCreator.singleton.DestroyLevel();
        gs = GameState.MainMenu;
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void QuitWarningSetOff()
    {
        quitWarning.SetActive(false);
        gs = GameState.MainMenu;
    }

    public void OptionsButton()
    {
        optionsGO.SetActive(true);
        mainMenu.SetActive(false);
        gs = GameState.Options;
    }

    public void MuteButton()
    {
        AudioListener.pause = !AudioListener.pause;
        Debug.Log(AudioListener.pause);
    }

    public void PauseButton()
    {
        Time.timeScale = 0;
        pauseGO.SetActive(true);
        gs = GameState.Pause;
    }

    public void ContinueButton()
    {
        Time.timeScale = 1;
        pauseGO.SetActive(false);
        gs = GameState.Play;
    }

    public void PrivacyPlicies()
    {
        Application.OpenURL("https://www.google.com/");
    }

    //----------------------------------------------------------------------------
    //Finds inactive GameObjects by name.

    public GameObject FindInactiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
    //----------------------------------------------------------------------------

    public bool PathFound { get; set; } = false;
}