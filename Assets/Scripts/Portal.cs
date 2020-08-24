using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Portal connection;

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
        }
        else
        {
            GameManager.singleton.AddPortalActive(this);
            GameManager.singleton.ConnectPortals();
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
