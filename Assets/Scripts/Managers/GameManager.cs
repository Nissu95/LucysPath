﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { Play, MainMenu, Options, Pause, SelectionLevel, QuitWarning }

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

#pragma warning disable 649
    [SerializeField] string mainMenuName;
    [SerializeField] string privacyPoliciesLink;

    [SerializeField] GameObject pauseGO;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject quitWarning;
    [SerializeField] GameObject optionsGO;
    [SerializeField] GameObject levelSelectionMenu;

    [SerializeField] Button nextLevelButton;
    [SerializeField] Button previousLevelButton;

    [SerializeField] Color[] portalColors;

    [SerializeField] float objectGrabHeight;
    [SerializeField] float mouseDragTime = 0.05f;

    [SerializeField] Transform levelSelectionContainer;
    [SerializeField] UIStars uIStars;

    [SerializeField] int multipleOf;
    [SerializeField] int starsPercentage;

    [SerializeField] ImageSequencePlayer tutorialPlayer;
    [SerializeField] TutorialContainer[] tutorials;


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

    //Win
    [SerializeField] Text winText;

    //Pause
    [SerializeField] Text pauseTitle;

    //Quit Warning
    [SerializeField] Text quitWarningTitle;

    //Options Menu
    [SerializeField] Text optionsTitle;
    [SerializeField] Text privacyPoliciesText;
    [SerializeField] Dropdown languagesDropdown;

    SystemLanguage language;
    int dropdownValue = 0;

    string needStarsString;
    string haveStarsString;

    //----------------------------------------------------------------------------
    //Level Selection Variables

    [SerializeField] GameObject levelSelectionButtonPrefab;

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
    }

    private void Start()
    {
        if (mainMenu)
            mainMenu.SetActive(true);

        LevelSelectionStart();

        if (levelSelectionMenu)
            levelSelectionMenu.SetActive(false);

        if (pauseButton)
            pauseButton.SetActive(false);

        if (pauseGO)
            pauseGO.SetActive(false);

        StartLanguage();
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
                    levelSelectionMenu.SetActive(false);
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

    void StartLanguage()
    {
        string languageSave = PlayerPrefs.GetString("Language");

        if (!string.IsNullOrEmpty(languageSave))
            language = (SystemLanguage)System.Enum.Parse(typeof(SystemLanguage), languageSave);
        else
            language = Application.systemLanguage;

        switch (language)
        {
            case SystemLanguage.Spanish:
                dropdownValue = 0;
                break;
            case SystemLanguage.English:
                dropdownValue = 1;
                break;
            default:
                language = SystemLanguage.English;
                dropdownValue = 1;
                break;
        }

        languagesDropdown.value = dropdownValue;
        ChangeLanguge();
    }

    void ChangeLanguge()
    {
        switch (language)
        {
            case SystemLanguage.English:

                winText.text = english.GetWinText();

                pauseTitle.text = english.GetPauseTitle();

                quitWarningTitle.text = english.GetQuitWarningTitle();

                optionsTitle.text = english.GetOptionsText();
                privacyPoliciesText.text = english.GetPrivacyPoliciesText();

                for (int i = 0; i < languagesDropdown.options.Count; i++)
                    languagesDropdown.options[i].text = english.GetLanguagesDropdownTxt()[i];

                languagesDropdown.captionText.text = english.GetLanguagesDropdownTxt()[1];

                needStarsString = english.GetNeedStarsString();
                haveStarsString = english.GetHaveStarsString();
                break;

            case SystemLanguage.Spanish:

                winText.text = spanish.GetWinText();

                pauseTitle.text = spanish.GetPauseTitle();

                quitWarningTitle.text = spanish.GetQuitWarningTitle();

                optionsTitle.text = spanish.GetOptionsText();
                privacyPoliciesText.text = spanish.GetPrivacyPoliciesText();

                for (int i = 0; i < languagesDropdown.options.Count; i++)
                    languagesDropdown.options[i].text = spanish.GetLanguagesDropdownTxt()[i];

                languagesDropdown.captionText.text = spanish.GetLanguagesDropdownTxt()[0];

                needStarsString = spanish.GetNeedStarsString();
                haveStarsString = spanish.GetHaveStarsString();
                break;
        }
        
        PlayerPrefs.SetString("Language", language.ToString());
        PlayerPrefs.Save();
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
        uIStars.UpdateUI(stars);

        if (stars > recordStars)
            recordStars = stars;

        pauseButton.SetActive(false);

        LevelsLoader.SaveLevelWon(recordStars, currentLevel);
        Level level = LevelsLoader.GetLevel(currentLevel);

        if (winObj)
            winObj.SetActive(true);

        nextLevelButton.interactable = IsNextLevel();
        previousLevelButton.interactable = IsPreviousLevel();

        UpdateLevelSelection();

        AdmobScript.singleton.ShowInterstitialAd();
    }

    bool IsNextLevel()
    {
        int aux = currentLevel + 1;

        if (LevelsLoader.GetLevel(aux) == null)
        {
            return false;
        }
        else
        {
            if ((aux + 1) % multipleOf == 0)
            {
                return GetHaveStarsToPlay();
            }
        }

        return true;
    }

    bool IsPreviousLevel()
    {
        int aux = currentLevel - 1;

        if (LevelsLoader.GetLevel(aux) == null)
            return false;
        else
            return true;
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

        for (int i = 0; i < levels.Count; i++)
        {
            GameObject buttonInstance = Instantiate<GameObject>(levelSelectionButtonPrefab, levelSelectionContainer);
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
                buttons[i].SetStars(levelsWon[i].GetStars());

            if (i > 0)
                buttonScript.interactable = won;

            if (lastWon == true)
                buttonScript.interactable = true;

            lastWon = won;
        }
    }

    public bool GetIsButtonInteractable(int index)
    {
        return buttons[index].GetComponent<Button>().interactable;
    }

    //----------------------------------------------------------------------------
    //Buttons

    public void PlayButton()
    {
        mainMenu.SetActive(false);
        levelSelectionMenu.SetActive(true);
        portalsActive.Clear();
        SoundManager.singleton.Nyan();
        gs = GameState.SelectionLevel;
    }

    public void PlayLevel(int index)
    {
        levelSelectionMenu.SetActive(false);
        LevelCreator.singleton.CreateLevel(LevelsLoader.GetLevel(index));

        currentLevel = index;

        for (int i = 0; i < tutorials.Length; i++)
            if (currentLevel == tutorials[i].GetLevel() && LevelsLoader.GetLevelsWon().Count < currentLevel+1)
                PlayTutorial(tutorials[i]);

        playerPath = FindObjectOfType<Pathfinding>();

        stars = 0;

        LevelWon levelWon = LevelsLoader.GetLevelWon(index);

        pauseButton.SetActive(true);

        if (levelWon != null)
            recordStars = levelWon.GetStars();
        else
            recordStars = 0;

        SoundManager.singleton.ChangeToGame();
        gs = GameState.Play;
    }

    void PlayTutorial(TutorialContainer tutorial)
    {
        tutorialPlayer.gameObject.SetActive(true);
        tutorialPlayer.SetTutorial(tutorial);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        pauseGO.SetActive(false);
        gs = GameState.Play;

        winObj.SetActive(false);
        portalsActive.Clear();
        PlayLevel(currentLevel);
    }

    public void NextLevelButton()
    {
        portalsActive.Clear();
        currentLevel++;
        winObj.SetActive(false);
        PlayLevel(currentLevel);
    }

    public void PreviousLevelButton()
    {
        portalsActive.Clear();
        currentLevel--;
        winObj.SetActive(false);
        PlayLevel(currentLevel);
    }

    public void BackToMenuButton()
    {
        portalsActive.Clear();

        Time.timeScale = 1;
        pauseGO.SetActive(false);
        gs = GameState.Play;

        winObj.SetActive(false);
        mainMenu.SetActive(true);
        pauseButton.SetActive(false);
        optionsGO.SetActive(false);

        SoundManager.singleton.Nyan();
        LevelCreator.singleton.DestroyLevel();
        SoundManager.singleton.ChangeToMenu();
        gs = GameState.MainMenu;
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void QuitWarningSetOff()
    {
        //SoundManager.singleton.Nyan();
        quitWarning.SetActive(false);
        gs = GameState.MainMenu;
    }

    public void OptionsButton()
    {
        optionsGO.SetActive(true);
        mainMenu.SetActive(false);
        SoundManager.singleton.Nyan();
        gs = GameState.Options;
    }

    public void MuteButton()
    {
        SoundManager.singleton.Mute();
    }

    public void PauseButton()
    {
        Time.timeScale = 0;
        pauseGO.SetActive(true);
        pauseButton.SetActive(false);
        SoundManager.singleton.Nyan();
        gs = GameState.Pause;
    }

    public void ContinueButton()
    {
        Time.timeScale = 1;
        pauseGO.SetActive(false);
        pauseButton.SetActive(true);
        SoundManager.singleton.Nyan();
        gs = GameState.Play;
    }

    public void PrivacyPlicies()
    {
        Application.OpenURL(privacyPoliciesLink);
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

    public float GetGrabHeight()
    {
        return objectGrabHeight;
    }

    public float GetMouseDragTime()
    {
        return mouseDragTime;
    }

    public bool IsPause()
    {
        if (pauseGO.activeInHierarchy)
            return true;
        else
            return false;
    }

    public int GetMultipleOf()
    {
        return multipleOf;
    }

    public bool GetHaveStarsToPlay()
    {
        int totalStarsCollected = 0;
        int totalStars = 0;
        int needStars = 0;

        for (int i = 0; i < levelsWon.Count; i++)
            totalStarsCollected += levelsWon[i].GetStars();

        totalStars = (currentLevel + 1) * 3;
        needStars = starsPercentage * totalStars / 100;

        if (totalStarsCollected < needStars)
            return false;
        else
            return true;
    }

    public bool GetHaveStarsToPlay(Text needStarsTxt, Text haveStarsTxt, int index)
    {
        int totalStarsCollected = 0;
        int totalStars = 0;
        int needStars = 0;

        for (int i = 0; i < levelsWon.Count; i++)
            totalStarsCollected += levelsWon[i].GetStars();

        totalStars = index * 3;
        needStars = starsPercentage * totalStars / 100;
        
        needStarsTxt.text = needStarsString + needStars;
        haveStarsTxt.text = haveStarsString + totalStarsCollected;

        if (totalStarsCollected < needStars)
            return false;
        else
            return true;
    }
}