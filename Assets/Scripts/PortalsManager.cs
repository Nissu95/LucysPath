using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalsManager : MonoBehaviour
{

    public static PortalsManager singleton;

    [SerializeField]
    Color[] portalColors;

    Portal[] portals;
    private void Awake()
    {
        if (singleton != null)
            Destroy(gameObject);
        else
            singleton = this;
    }

    public void ResetManager()
    {
        portals = FindObjectsOfType<Portal>();

        for (int i = 0; i < portals.Length; i++)
            portals[i].SetActiveColor(portalColors[i]);
    }
}
