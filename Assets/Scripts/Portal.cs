using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Portal connection;

    Transform connectionTransform;

    private bool active = true;

    private void Start()
    {
        connectionTransform = connection.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active && other.CompareTag(Tags.player))
        {
            DeactivateConnectedPortal();
            other.GetComponent<Pathfinding>().NextNode();
            other.transform.position = connectionTransform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.player))
        {
            active = true;
        }
    }

    private void DeactivateConnectedPortal()
    {
        connection.Deactivate();
    }

    public void Deactivate()
    {
        active = false;
    }

    public Transform GetConnectionTransform()
    {
        return connectionTransform;
    }
}
