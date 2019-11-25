using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLight : MonoBehaviour
{
    [SerializeField]
    LayerMask layers;
    [SerializeField]
    string mirrorTag = "Mirror";
    [SerializeField]
    string endTag = "End";

    bool inGame = true;

    List<Vector3> nodes = new List<Vector3>();
    Pathfinding pathfinding;

    private void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
    }

    void Update()
    {
        if (!inGame)
            return;

        Ray ray = new Ray(transform.position, transform.forward);

        // Resetting nodes list.
        nodes.Clear();

        // Sending first raycast.
        Raycast(ray);
    }

    void Raycast(Ray ray)
    {
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.green);

            //If hits a mirror
            if (hit.transform.CompareTag(mirrorTag))
            {
                // Find the line from where the ray started.
                Vector3 incomingVec = hit.point - ray.origin;

                // Use the point's normal to calculate the reflection vector.
                Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);

                nodes.Add(hit.point);

                //Repeat the process
                Raycast(new Ray(hit.point, reflectVec));

            }
            else if (hit.transform.CompareTag(endTag))
            {
                if (inGame)
                {
                    nodes.Add(hit.point);
                    Walk();
                    inGame = false;
                }
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var item in nodes)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(item, 1);
        }
    }

    void Walk()
    {
        Debug.Log("hola");
        pathfinding.StartWalking(nodes);
    }
}
