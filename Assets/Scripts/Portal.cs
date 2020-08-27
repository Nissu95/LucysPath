using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Portal : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] Portal connection;
    [SerializeField] GameObject portal;

    Transform connectionTransform;

    private bool active = true;
    bool portalActive = false;

    private void Start()
    {
        Time.timeScale = 0.5f;
        
        //connectionTransform = connection.transform;
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
        if (connection)
            connectionTransform = connection.transform;

        return connectionTransform;
    }

    public bool GetActive()
    {
        return portalActive;
    }

    public void PortalTouch()
    {
        portalActive = !portalActive;

        if (portalActive == false)
        {
            if (connection)
            {
                connection.SetConnection(null);
                connection = null;
            }

            GameManager.singleton.RemovePortalActive(this);
            portal.GetComponent<MeshRenderer>().material.color = Color.white;
        }
        else
        {
            GameManager.singleton.AddPortalActive(this);
            GameManager.singleton.ConnectPortals();
            portal.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }

    public void SetConnection(Portal _connection)
    {
        connection = _connection;
    }

    public Portal GetConnection()
    {
        return connection;
    }
}
