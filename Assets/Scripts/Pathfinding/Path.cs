using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] GameObject nodePrefab;

    [SerializeField] bool[] nodesAvailable = new bool[Constants.nodesCount];

    float nodeSpacing;
    float nodeHeight;
    int[] nodes = new int[Constants.nodesCount];

    

    private void Start()
    {
        nodeSpacing = transform.localScale.x / 2;
        nodeHeight = transform.localScale.y / 2;

        for (int i = 0; i < nodesAvailable.Length; i++)
            if (nodesAvailable[i] == true) nodes[i] = 1; else nodes[i] = 0;
    }

    public int[] GetNodes()
    {
        return nodes;
    }
}

