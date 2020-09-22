using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRotation : MonoBehaviour
{
    [SerializeField] Vector3 rotationAxis;
    [SerializeField] float speed;

    void Update()
    {
        transform.Rotate(rotationAxis * Time.deltaTime * speed, Space.World);
    }
}
