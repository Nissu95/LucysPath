using System.Collections;
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
    [SerializeField] GameObject pauseGO;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject quitWarning;
    [SerializeField] GameObject optionsGO;

    GameObject winObj;
    int stars;
    int recordStars = 0;

    GameObject mainMenu;
    Pathfinding playerPath;

    GameState gs;

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

    public int GetStars()
    {
        return stars;
    }

    public int GetMaxStars()
    {
        return recordStars;
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
                buttons[i].SetStars(levelsWon[i].GetStars());

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
        PlayLevel(currentLevel);
    }

    public void NextLevelButton()
    {
        winObj.SetActive(false);
        currentLevel++;
        PlayLevel(currentLevel);
    }

    public void BackToMenuButton()
    {
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
}