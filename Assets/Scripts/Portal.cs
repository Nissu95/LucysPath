using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] Portal connection;

    Transform connectionTransform;

    private bool active = true;

    private void Start()
    {
        connectionTransform = connection.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active && other.CompareTag(Constants.playerTag))
        {
            DeactivateConnectedPortal();
            other.GetComponent<Pathfinding>().NextNode();
            other.transform.position = connectionTransform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.playerTag))
            active = true;
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
