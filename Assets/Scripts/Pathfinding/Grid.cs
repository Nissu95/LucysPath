using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] Path[,] paths = new Path[6,6];
    [SerializeField] GameObject[] pathPrefabs;
    [SerializeField] GameObject firstPathPrefab;

    [SerializeField] GameObject player;

    [SerializeField] Vector2Int firstPathPosition;
    [SerializeField] Vector2Int lastPathPosition;

    public Color firstPathColor;
    public Color lastPathColor;
    public Color intermidiatePathColor;

    const float nodeSpacing = 0.5f;
    const float nodeHeight = 0.1f;

    readonly Vector2Int nullPosition = new Vector2Int(100,100);

    List<Node> nodes = new List<Node>();
    List<Vector3> nodesPosition = new List<Vector3>();

    private void Start()
    {
        for (int i = 0; i < paths.GetLength(0); i++)
        {
            for (int j = 0; j < paths.GetLength(1); j++)
            {
                GameObject prefab = pathPrefabs[Random.Range(0, pathPrefabs.Length)];

                if (i == firstPathPosition.x && j == firstPathPosition.y)
                    prefab = firstPathPrefab;

                paths[i, j] = Instantiate<GameObject>(prefab, new Vector3(i, 1, j), Quaternion.identity).GetComponent<Path>();
            }
        }

        Path firstPathSquare = GetPath(firstPathPosition);
        firstPathSquare.GetComponent<MeshRenderer>().material.color = firstPathColor;
        firstPathSquare.Lock();

        Path lastPathSquare = GetPath(lastPathPosition);
        lastPathSquare.GetComponent<MeshRenderer>().material.color = lastPathColor;
    }

    public void FindPath()
    {
        Vector2Int nextPathPosition = firstPathPosition;

        while (true)
        {
            nextPathPosition = FindNextPath(nextPathPosition);

            if (nextPathPosition == lastPathPosition || nextPathPosition == nullPosition)
            {
                player.GetComponent<Pathfinding>().StartWalking(nodesPosition);
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
}

public static class Constants
{
    /*public static readonly Vector2Int[] nodesOrder =
    {
        new Vector2Int(+1, 0),
        new Vector2Int(+2, 0),
        new Vector2Int(0, -1),
        new Vector2Int(0, -2),
        new Vector2Int(-1, 0),
        new Vector2Int(-2, 0),
        new Vector2Int(0, +1),
        new Vector2Int(0, +2),
    };*/

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
}


