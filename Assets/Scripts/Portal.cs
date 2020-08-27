﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Portal : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] Portal connection;
    [SerializeField] MeshRenderer portalMesh;
    [SerializeField] Color activeColor;

    Transform connectionTransform;
    Color deactiveColor = Color.white;

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
                connection.SetActiveColor(connection.GetActiveColor());
                connection.SetConnection(null);
                connection = null;
            }

            GameManager.singleton.RemovePortalActive(this);
            portalMesh.material.color = deactiveColor;
        }
        else
        {
            GameManager.singleton.AddPortalActive(this);
            portalMesh.material.color = activeColor;
            GameManager.singleton.ConnectPortals();
        }
    }

    public void SetConnection(Portal _connection)
    {
        connection = _connection;
    }

    public void SetActiveColor(Color color)
    {
        portalMesh.material.color = color;
    }

    public Color GetActiveColor()
    {
        return activeColor;
    }

    public Portal GetConnection()
    {
        return connection;
    }
}
