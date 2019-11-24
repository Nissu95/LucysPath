using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLight : MonoBehaviour
{
    [SerializeField]
    LayerMask layers;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.green);
        else
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
    }
}
