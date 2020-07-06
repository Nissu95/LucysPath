using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    [SerializeField] string mainMenuName;

    GameObject winObj;
    int stars;
    int maxStars = 0;

    GameObject mainMenu;

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
        selectionPanel.gameObject.SetActive(false);
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

        LevelsLoader.SaveLevelWon(stars, currentLevel);
        Level level = LevelsLoader.GetLevel(currentLevel);
        level.SetWon(true);
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
        LevelSelectionStart();
    }

    public void PlayLevel(int index)
    {
        selectionPanel.gameObject.SetActive(false);
        LevelCreator.singleton.CreateLevel(LevelsLoader.GetLevel(index));

        currentLevel = index;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void OptionsButton()
    {

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