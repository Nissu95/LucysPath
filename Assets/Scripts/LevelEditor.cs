using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    LevelButton[,] buttons;

    private void Start()
    {
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

    public void TestLevel()
    {
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
            {
                int index = buttons[j, i].GetIndex();

                if (index > 0)
                    Instantiate<GameObject>(items[index - 1], 
                    new Vector3(itemsSpacing * i, 0,
                    -itemsSpacing * j), Quaternion.identity);
            }
    }
}
