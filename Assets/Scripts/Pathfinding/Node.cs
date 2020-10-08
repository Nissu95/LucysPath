using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    void OnDrawGizmos()
    {
        // Draw a sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, .05f);
    }
}
