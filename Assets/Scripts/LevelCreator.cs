using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] float itemsSpacing;
    [SerializeField] GameObject[] levelObstacles;

    List<GameObject> items = new List<GameObject>();

    public static LevelCreator singleton;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (singleton != null)
            Destroy(gameObject);
        else
            singleton = this;
    }

    public GameObject[] GetObstacles()
    {
        return levelObstacles;
    }

    public void CreateLevel(Level level)
    {
        if (level == null)
            Debug.LogError("hola");

        int rows = level.GetRows();
        int columns = level.GetColumns();

        foreach (var obstacle in items)
            Destroy(obstacle);

        items.Clear();

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
            {
                int index = level.GetGrid()[j, i];

                if (index > 0)
                {
                    GameObject item = Instantiate<GameObject>(levelObstacles[index - 1],
                    new Vector3(itemsSpacing * i, 0,
                    -itemsSpacing * j), Quaternion.identity);

                    items.Add(item);
                }
            }
    }

    public void DestroyLevel()
    {
        foreach (var obstacle in items)
            Destroy(obstacle);

        items.Clear();
    }
}
