using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] Path[,] paths;
    [SerializeField] ScriptablePaths[] pathPrefabs;

    Pathfinding playerPathfinding;

    public Vector2Int firstPathPosition;
    Vector2Int lastPathPosition;

    [SerializeField] float itemsSpacing;

    public Color firstPathColor;
    public Color lastPathColor;
    public Color intermidiatePathColor;

    const float nodeSpacing = 0.5f;
    const float nodeHeight = 0.1f;

    List<GameObject> items = new List<GameObject>();

    public readonly Vector2Int nullPosition = new Vector2Int(100, 100);

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
        nodesPosition.Clear();

        Vector2Int nextPathPosition = firstPathPosition;


        for (int i = 0; i < items.Count; i++)
            items[i].GetComponent<Path>().Unlock();

        Path firstPathSquare = GetPath(firstPathPosition);
        firstPathSquare.Lock();

        playerPathfinding = GetPath(firstPathPosition).GetComponentInChildren<Pathfinding>();


        while (true)
        {

            nextPathPosition = FindNextPath(nextPathPosition);

            if (nextPathPosition == nullPosition)
                return;

            Path nextPath = GetPath(nextPathPosition);
            nodesPosition.Add(nextPath.transform.position + (Vector3.up * nodeHeight));

            if (nextPathPosition == lastPathPosition)
            {
                /*for (int i = 0; i < nodesPosition.Count; i++)
                {
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.localScale *= 0.2f;
                    sphere.transform.position = nodesPosition[i];
                }*/

                playerPathfinding.StartWalking(nodesPosition);
                return;
            }

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

                if (nextPathGridPosition.x >= 0 && nextPathGridPosition.x < paths.GetLength(0)
                    && nextPathGridPosition.y >= 0 && nextPathGridPosition.y < paths.GetLength(1))
                {
                    Path nextPath = paths[nextPathGridPosition.x, nextPathGridPosition.y];

                    if (nextPath && !nextPath.IsLocked())
                    {
                        uint[] nextPathNodes = nextPath.GetNodes();

                        if (nextPathNodes[connectionNodeIndex] == 1)
                        {

                            Vector3 pathPosition = nextPath.transform.position;
                            Vector3 nodePosition = new Vector3(pathPosition.x + (Constants.pathsOrder[connectionNodeIndex].x) * nodeSpacing,
                                                               pathPosition.y + nodeHeight,
                                                               pathPosition.z + (Constants.pathsOrder[connectionNodeIndex].y) * nodeSpacing);

                            nodesPosition.Add(nodePosition);

                            if (nextPath.gameObject.CompareTag(Constants.PortalTag))
                            {
                                Debug.Log("Es portal");

                                Transform connectionTransform = nextPath.GetComponent<Portal>().GetConnectionTransform();

                                if (connectionTransform)
                                {
                                    Debug.Log("Portal conectado");

                                    nodePosition = nextPath.transform.position;
                                    nodesPosition.Add(nodePosition);

                                    Path connectionPath = connectionTransform.GetComponent<Path>();
                                        return GetPath(connectionPath);
                                }
                                else
                                    return nullPosition;
                            }

                            return nextPathGridPosition;
                        }
                    }

                }
                else return nullPosition;
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

    public Vector2Int GetPath(Path path)
    {
        for (int i = 0; i < paths.GetLength(0); i++)
        {
            for (int j = 0; j < paths.GetLength(1); j++)
            {
                if (path == paths[i,j])
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return nullPosition;
    }

    public Vector2Int GetMaxPosition()
    {
        return new Vector2Int(paths.GetLength(0), paths.GetLength(1));
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
                int index = level.GetGrid()[j, i].Index;
                int rotation = level.GetGrid()[j, i].Rotation;
                bool locked = level.GetGrid()[j, i].Locked;
                bool hasStar = level.GetGrid()[j, i].Star;

                if (index > 0)
                {
                    GameObject prefab = pathPrefabs[index - 1].prefab;

                    if (prefab.CompareTag(Constants.firstPathTag))
                        firstPathPosition = new Vector2Int(i, j);
                    else if (prefab.CompareTag(Constants.lastPathTag))
                        lastPathPosition = new Vector2Int(i, j);

                    GameObject pathItem = Instantiate<GameObject>(prefab, new Vector3(i, 1, j), Quaternion.identity);
                    items.Add(pathItem);

                    Path pathScript = pathItem.GetComponent<Path>();
                    pathScript.SetRotation(rotation);

                    paths[i, j] = pathScript;

                    if (locked)
                        pathItem.GetComponent<MouseDrag>().Lock();

                    if (hasStar)
                        pathScript.HasStar();
                }
            }
        }

        Path firstPathSquare = GetPath(firstPathPosition);
        firstPathSquare.GetComponent<MeshRenderer>().material.color = firstPathColor;

        Path lastPathSquare = GetPath(lastPathPosition);
        lastPathSquare.GetComponent<MeshRenderer>().material.color = lastPathColor;
    }

    public void DestroyLevel()
    {
        foreach (var obstacle in items)
            Destroy(obstacle);

        items.Clear();
    }

    public ScriptablePaths[] GetObstacles()
    {
        return pathPrefabs;
    }

    public bool MovePath(Vector2Int origin, Vector2Int destiny)
    {
        Path destinyPath = GetPath(destiny);

        if (!destinyPath)
        {
            Path originPath = GetPath(origin);
            paths[destiny.x, destiny.y] = originPath;
            paths[origin.x, origin.y] = null;

            return true;
        }
        else
            return false;
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
    public const uint maxRotation = 3;

    public const string firstPathTag = "FirstPath";
    public const string lastPathTag = "LastPath";
    public const string PathTag = "Path";
    public const string PortalTag = "Portal";
}


