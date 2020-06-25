using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    public GameObject[] items;
    [SerializeField] int columns;
    [SerializeField] int rows;
    [SerializeField] float verticalSpacing;
    [SerializeField] float horizontalSpacing;
    [SerializeField] Vector2 UIposition;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] float itemsSpacing;
    [SerializeField] Transform canvas;
    [SerializeField] Text levelText;
    [SerializeField] Text totalLevels;

    LevelButton[,] buttons;

    int LevelIndex = 0;
    List<GameObject> levelObstacles = new List<GameObject>();

    private void Start()
    {
        ShowEditorUI();
        HandleTextFile.ReadLevels();
        LoadLevel();
        UpdateTotalLevels();
    }

    void UpdateLevelText()
    {
        levelText.text = (LevelIndex + 1).ToString();
    }

    void UpdateTotalLevels()
    {
        totalLevels.text = "Niveles: " + HandleTextFile.GetLevelsCount();
    }

    public void LeftLevel()
    {
        if (LevelIndex > 0)
            LevelIndex--;

        UpdateLevelText();
    }

    public void RightLevel()
    {
        if (LevelIndex < HandleTextFile.GetLevelsCount())
            LevelIndex++;

        UpdateLevelText();
    }

    void ShowEditorUI()
    {
        ClearButtons();

        buttons = new LevelButton[columns, rows];

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
            {
                GameObject button = Instantiate<GameObject>(buttonPrefab, canvas);
                Vector2Int position = new Vector2Int(j, i);
                button.transform.position = new Vector2(position.x * horizontalSpacing + UIposition.x,
                                                        position.y * verticalSpacing + UIposition.y);

                LevelButton levelButton = button.GetComponent<LevelButton>();
                levelButton.SetPosition(position);

                levelButton.levelEditor = this;

                buttons[j, i] = levelButton;
            }
    }

    void ShowEditorUI(Level level)
    {
        ClearButtons();

        columns = level.GetItems().GetLength(0);
        rows = level.GetItems().GetLength(1);

        buttons = new LevelButton[columns, rows];

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
            {
                GameObject button = Instantiate<GameObject>(buttonPrefab, canvas);
                Vector2Int position = new Vector2Int(j, i);
                button.transform.position = new Vector2(position.x * horizontalSpacing + UIposition.x,
                                                        position.y * verticalSpacing + UIposition.y);

                LevelButton levelButton = button.GetComponent<LevelButton>();
                levelButton.SetPosition(position);
                levelButton.SetIndex(level.GetItems()[j, i]);

                levelButton.levelEditor = this;

                buttons[j, i] = levelButton;
            }
    }

    void ClearButtons()
    {
        if (buttons == null)
            return;

        for (int i = 0; i < buttons.GetLength(0); i++)
            for (int j = 0; j < buttons.GetLength(1); j++)
                Destroy(buttons[i, j].gameObject);
    }

    public void TestLevel()
    {
        foreach (var obstacle in levelObstacles)
            Destroy(obstacle);

        levelObstacles.Clear();

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
            {
                int index = buttons[j, i].GetIndex();

                if (index > 0)
                {
                    GameObject item = Instantiate<GameObject>(items[index - 1], 
                    new Vector3(itemsSpacing * i, 0,
                    -itemsSpacing * j), Quaternion.identity);

                    levelObstacles.Add(item);
                }
            }
    }

    public void LoadLevel()
    {
        Level level = HandleTextFile.GetLevel(LevelIndex);

        if (level == null)
            ShowEditorUI();
        else
            ShowEditorUI(level);
    }

    public void SaveLevel()
    {
        Level level = new Level(buttons, columns, rows);
        HandleTextFile.SaveLevels(level,ref LevelIndex);
        UpdateLevelText();
        UpdateTotalLevels();
    }
}

[Serializable]
public class Level
{
    int[,] items;
    public Level(LevelButton[,] levelButtons, int colums, int rows)
    {
        items = new int[colums, rows];

        for (int i = 0; i < colums; i++)
            for (int j = 0; j < rows; j++)
            {
                items[i, j] = levelButtons[i, j].GetIndex();
            }
    }

    public int [,] GetItems()
    {
        return items;
    }

}
