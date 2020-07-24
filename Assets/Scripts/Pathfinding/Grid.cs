using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] Path[,] paths = new Path[3,3];
    [SerializeField] GameObject[] pathPrefabs;

    [SerializeField] Vector2Int firstPath;

    public Material materialRed;
    public Material materialGreen;
    public Material firstPathMaterial;

    List<Node> nodes = new List<Node>();

    private void Start()
    {
        for (int i = 0; i < paths.GetLength(0); i++)
        {
            for (int j = 0; j < paths.GetLength(1); j++)
            {
                GameObject prefab = pathPrefabs[0];

                paths[i, j] = Instantiate<GameObject>(prefab, new Vector3(i, 1, j), Quaternion.identity).GetComponent<Path>();
            }
        }


    }

    public void FindPath()
    {
        Path path = paths[firstPath.x, firstPath.y];
        int[] nodes = path.GetNodes();

        path.GetComponent<MeshRenderer>().material = materialRed;

        Vector2Int currentPathPosition = firstPath;
        Vector2Int nextPathPosition;

        uint connectionNodeIndex;

        for (int i = 0; i < nodes.Length; i++)
        {
            uint bias = Constants.nodesCount / 2;

            if (nodes[i] == 1)
            {
                if (i < bias) connectionNodeIndex = (uint)i + bias; else connectionNodeIndex = (uint)i - bias;
                //Debug.Log("Nodo actual " + i + " Nodo de conexion " + connectionNodeIndex);

                nextPathPosition = currentPathPosition + Constants.pathsOrder[i];

                if (nextPathPosition.x >= 0 && nextPathPosition.x < paths.GetLength(0) 
                    && nextPathPosition.y >= 0 && nextPathPosition.y < paths.GetLength(1))
                {
                    Path nextPath = paths[nextPathPosition.x, nextPathPosition.y];

                    int[] nextPathNodes = nextPath.GetNodes();

                    if (nextPathNodes[connectionNodeIndex] == 1)
                    {
                        nextPath.GetComponent<MeshRenderer>().material = materialGreen;
                    }

                }

            }
        }
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


