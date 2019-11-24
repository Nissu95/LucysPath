using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLight : MonoBehaviour
{
    [SerializeField]
    LayerMask layers;
    [SerializeField]
    string mirrorTag;

    void Update()
    {

        Ray ray = new Ray(transform.position, transform.forward);
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

                //Repeat the process
                Raycast(new Ray(hit.point, reflectVec));
            }
            
        }
        else
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
    }
}
