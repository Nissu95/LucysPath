using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Portal : MonoBehaviour
{
#pragma warning disable 649
    public enum SelectPortal { Active, Deactive, Connect}

    [SerializeField] Portal connection;
    [SerializeField] MeshRenderer portalMesh;

    string materialColorReference = "Color_ECE82552";

    Color activeColor;

    SelectPortal selectPortal = SelectPortal.Deactive;

    Transform connectionTransform;
    Color deactiveColor = Color.white;

    private bool active = true;


    private void OnTriggerEnter(Collider other)
    {
        if (active && other.CompareTag(Constants.playerTag))
        {
            DeactivateConnectedPortal();
            other.GetComponent<Pathfinding>().NextNode();
            other.transform.position = connectionTransform.position;
            other.transform.rotation = connectionTransform.rotation;
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

    public void PortalTouch()
    {

        switch (selectPortal)
        {
            case SelectPortal.Active:
                selectPortal = SelectPortal.Deactive;

                GameManager.singleton.RemovePortalActive(this);
                SoundManager.singleton.PortalReverseClip();
                portalMesh.material.SetColor(materialColorReference, deactiveColor);
                break;
            case SelectPortal.Deactive:

                if (GameManager.singleton.IsPortalConnect())
                {
                    GameManager.singleton.AddPortalActive(this);
                    GameManager.singleton.ConnectPortals();
                    selectPortal = SelectPortal.Connect;
                    connection.SetPortalSelect(SelectPortal.Connect);
                }
                else
                {
                    GameManager.singleton.AddPortalActive(this);
                    selectPortal = SelectPortal.Active;
                    portalMesh.material.SetColor(materialColorReference, activeColor);
                }

                SoundManager.singleton.PlayPortalClip();
                break;
            case SelectPortal.Connect:
                if (!GameManager.singleton.IsPortalConnect())
                {
                    connection.SetMeshMaterialColor(connection.GetActiveColor());
                    GameManager.singleton.AddPortalActive(connection);
                    SoundManager.singleton.PortalReverseClip();
                    connection.SetConnection(null);
                    connection.SetPortalSelect(SelectPortal.Active);

                    connection = null;
                    selectPortal = SelectPortal.Deactive;
                    portalMesh.material.SetColor(materialColorReference, deactiveColor);
                }
                else
                {
                    connection.SetMeshMaterialColor(deactiveColor);
                    connection.SetConnection(null);
                    connection.SetPortalSelect(SelectPortal.Deactive);

                    connection = null;
                    GameManager.singleton.AddPortalActive(this);
                    GameManager.singleton.ConnectPortals();
                    selectPortal = SelectPortal.Connect;
                    connection.SetPortalSelect(SelectPortal.Connect);
                }
                
                break;
            default:
                break;
        }

        LevelCreator.singleton.FindPath();
    }

    public void SetPortalSelect(SelectPortal _selectPortal)
    {
        selectPortal = _selectPortal;
    }

    public void SetConnection(Portal _connection)
    {
        connection = _connection;
    }

    public void SetActiveColor(Color color)
    {
        activeColor = color;
    }

    public void SetMeshMaterialColor(Color color)
    {
        portalMesh.material.SetColor(materialColorReference, color);
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
