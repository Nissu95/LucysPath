﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField] GameObject levelSelectionButtonPrefab;
    [SerializeField] Transform selectionPanel;

    public static LevelSelectionManager singleton;

    int currentLevel = 0;

    void Awake()
    {
        if (singleton != null)
            Destroy(gameObject);
        else
            singleton = this;
    }

    List<Level> levels;

    bool lastWon = false;
    void Start()
    {
        levels = LevelsLoader.GetLevels();

        for (int i = 0; i < levels.Count; i++)
        {
            GameObject buttonInstance = Instantiate<GameObject>(levelSelectionButtonPrefab, selectionPanel);
            LevelSelectionButton button = buttonInstance.GetComponent<LevelSelectionButton>();
            button.SetNumber(i);

            Button buttonScript = buttonInstance.GetComponent<Button>();

            bool won = levels[i].GetWon();

            if (i > 0)
                buttonScript.interactable = won;

            if (lastWon == true)
                buttonScript.interactable = true;

            lastWon = won;
        }
    }

    public void PlayLevel(int index)
    {
        selectionPanel.gameObject.SetActive(false);
        LevelCreator.singleton.CreateLevel(LevelsLoader.GetLevel(index));

        currentLevel = index;
    }

    public void LevelWon()
    {
        Level level = LevelsLoader.GetLevel(currentLevel);
        level.SetWon(true);
    }
}