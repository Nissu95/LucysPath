using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    [SerializeField] string mainMenuName;
    [SerializeField] GameObject pauseGO;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject quitWarning;

    GameObject winObj;
    int stars;
    int maxStars = 0;

    GameObject mainMenu;

    Pathfinding playerPath;

    //----------------------------------------------------------------------------
    //Level Selection Variables

    [SerializeField] GameObject levelSelectionButtonPrefab;
    [SerializeField] Transform selectionPanel;

    List<Level> levels;
    List<LevelWon> levelsWon;
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
    }

    private void Start()
    {
        mainMenu.SetActive(true);
        LevelSelectionStart();
        selectionPanel.gameObject.SetActive(false);
        pauseButton.SetActive(false);
        pauseGO.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mainMenu.activeInHierarchy)
                quitWarning.SetActive(true);
            else
            {
                if (selectionPanel.gameObject.activeInHierarchy)
                {
                    mainMenu.SetActive(true);
                    selectionPanel.gameObject.SetActive(false);
                }
                else
                {
                    if (pauseButton.activeInHierarchy)
                        PauseButton();
                }
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
        if (stars > maxStars)
            maxStars = stars;

        if (winObj)
            winObj.SetActive(true);

        pauseButton.SetActive(false);

        LevelsLoader.SaveLevelWon(stars, currentLevel);
        Level level = LevelsLoader.GetLevel(currentLevel);
        level.SetWon(true);

        playerPath.GetFSM().SetEvent(Event.ToIdle);
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
        return maxStars;
    }

    public GameObject GetPauseButton()
    {
        return pauseButton;
    }

    public GameObject GetQuitWarning()
    {
        return quitWarning;
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
            button.SetNumber(i);

            Button buttonScript = buttonInstance.GetComponent<Button>();

            bool won = (levelsWon != null && i < levelsWon.Count);

            if (won)
                button.SetStars(levelsWon[i].GetStars());

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
    }

    public void PlayLevel(int index)
    {
        selectionPanel.gameObject.SetActive(false);
        LevelCreator.singleton.CreateLevel(LevelsLoader.GetLevel(index));

        currentLevel = index;

        playerPath = FindObjectOfType<Pathfinding>();
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
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void QuitWarningSetOff()
    {
        quitWarning.SetActive(false);
    }

    public void OptionsButton()
    {

    }

    public void PauseButton()
    {
        Time.timeScale = 0;
        pauseGO.SetActive(true);
    }

    public void ContinueButton()
    {
        Time.timeScale = 1;
        pauseGO.SetActive(false);
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