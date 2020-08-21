using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] bool[] nodesAvailable = new bool[Constants.nodesCount];
    [SerializeField] GameObject star;

    Queue<uint> nodes = new Queue<uint>();

    List<GameObject> nodos = new List<GameObject>();

    bool locked = false;

    int rotation = 0;

    private void Start()
    {
        for (int i = 0; i < nodesAvailable.Length; i++)
            if (nodesAvailable[i] == true) nodes.Enqueue(1); else nodes.Enqueue(0);

        for (int i = 0; i < rotation; i++)
        {
            RotatePath();
            transform.rotation = Quaternion.Euler(0, -90 * rotation, 0);
        }
    }

    public uint[] GetNodes()
    {
        uint[] nodesArray;

        nodesArray = nodes.ToArray();

        return nodesArray;
    }

    public void RotatePath()
    {
        for (int i = 0; i < 2; i++)
            nodes.Enqueue(nodes.Dequeue());
    }

    public void Lock()
    {
        locked = true;
    }

    public void Unlock()
    {
        locked = false;
    }

    public bool IsLocked()
    {
        return locked;
    }

    public void HasStar()
    {
        if (star)
            star.SetActive(true);
    }

    public void SetRotation(int _rotation)
    {
        rotation = _rotation;
    }
}

