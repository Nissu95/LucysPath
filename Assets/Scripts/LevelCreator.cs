using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] Path[,] paths;
    [SerializeField] GameObject[] pathPrefabs;

    Pathfinding playerPathfinding;

    Vector2Int firstPathPosition;
    Vector2Int lastPathPosition;

    [SerializeField] float itemsSpacing;

    public Color firstPathColor;
    public Color lastPathColor;
    public Color intermidiatePathColor;

    const float nodeSpacing = 0.5f;
    const float nodeHeight = 0.1f;

    List<GameObject> items = new List<GameObject>();

    readonly Vector2Int nullPosition = new Vector2Int(100, 100);

    List<Node> nodes = new List<Node>();
    List<Vector3> nodesPosition = new List<Vector3>();


    public static LevelCreator singleton;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (singleton != null)
            Destroy(gameObject);
        else
            singleton = this;
    }

    public void FindPath()
    {
        Vector2Int nextPathPosition = firstPathPosition;

        playerPathfinding = GetPath(firstPathPosition).GetComponentInChildren<Pathfinding>();

        while (true)
        {
            nextPathPosition = FindNextPath(nextPathPosition);

            if (nextPathPosition == lastPathPosition || nextPathPosition == nullPosition)
            {
                playerPathfinding.StartWalking(nodesPosition);
                return;
            }

            Path nextPath = GetPath(nextPathPosition);

            nextPath.GetComponent<MeshRenderer>().material.color = intermidiatePathColor;
            nextPath.Lock();
        }
    }

    Vector2Int FindNextPath(Vector2Int pathGridPosition)
    {
        uint[] nodes = GetPath(pathGridPosition).GetNodes();

        Vector2Int currentPathGridPosition = pathGridPosition;
        Vector2Int nextPathGridPosition;

        uint connectionNodeIndex;

        for (int i = 0; i < nodes.Length; i++)
        {
            uint bias = Constants.nodesCount / 2;

            if (nodes[i] == 1)
            {
                if (i < bias) connectionNodeIndex = (uint)i + bias; else connectionNodeIndex = (uint)i - bias;
                //Debug.Log("Nodo actual " + i + " Nodo de conexion " + connectionNodeIndex);

                nextPathGridPosition = currentPathGridPosition + Constants.pathsOrder[i];

                Path nextPath = paths[nextPathGridPosition.x, nextPathGridPosition.y];

                if (!nextPath.IsLocked() && nextPathGridPosition.x >= 0 && nextPathGridPosition.x < paths.GetLength(0)
                                         && nextPathGridPosition.y >= 0 && nextPathGridPosition.y < paths.GetLength(1))
                {

                    uint[] nextPathNodes = nextPath.GetNodes();

                    if (nextPathNodes[connectionNodeIndex] == 1)
                    {
                        Vector3 pathPosition = nextPath.transform.position;
                        Vector3 nodePosition = new Vector3(pathPosition.x + (Constants.pathsOrder[connectionNodeIndex].x) * nodeSpacing,
                                                           pathPosition.y + nodeHeight,
                                                           pathPosition.z + (Constants.pathsOrder[connectionNodeIndex].y) * nodeSpacing);

                        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphere.transform.localScale *= 0.2f;
                        sphere.transform.position = nodePosition;

                        nodesPosition.Add(nodePosition);

                        return nextPathGridPosition;
                    }
                }
            }
        }
        return nullPosition;
    }

    Path GetPath(Vector2Int pathPosition)
    {
        if (pathPosition != nullPosition)
            return paths[pathPosition.x, pathPosition.y];
        else
            return null;
    }


    public void CreateLevel(Level level)
    {
        int rows = level.GetRows();
        int columns = level.GetColumns();

        foreach (var obstacle in items)
            Destroy(obstacle);

        items.Clear();

        paths = new Path[rows, columns];

        for (int i = 0; i < paths.GetLength(0); i++)
        {
            for (int j = 0; j < paths.GetLength(1); j++)
            {
                int index = level.GetGrid()[j, i];

                GameObject prefab = pathPrefabs[index - 1];

                if (prefab.CompareTag(Constants.firstPathTag))
                    firstPathPosition = new Vector2Int(i, j);
                else if (prefab.CompareTag(Constants.lastPathTag))
                    lastPathPosition = new Vector2Int(i, j);

                paths[i, j] = Instantiate<GameObject>(prefab, new Vector3(i, 1, j), Quaternion.identity).GetComponent<Path>();
            }
        }

        Path firstPathSquare = GetPath(firstPathPosition);
        firstPathSquare.GetComponent<MeshRenderer>().material.color = firstPathColor;
        firstPathSquare.Lock();

        Path lastPathSquare = GetPath(lastPathPosition);
        lastPathSquare.GetComponent<MeshRenderer>().material.color = lastPathColor;
    }

    public void DestroyLevel()
    {
        foreach (var obstacle in items)
            Destroy(obstacle);

        items.Clear();
    }

    public GameObject[] GetObstacles()
    {
        return pathPrefabs;
    }
}



public static class Constants
{
    public static readonly Vector2Int[] pathsOrder =
    {
        new Vector2Int(+1, -1),
        new Vector2Int( 0, -1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1,  0),
        new Vector2Int(-1, +1),
        new Vector2Int( 0, +1),
        new Vector2Int(+1, +1),
        new Vector2Int(+1,  0),
    };

    public const uint nodesCount = 8;

    public const string firstPathTag = "FirstPath";
    public const string lastPathTag = "LastPath";
    public const string PathTag = "Path";
}


