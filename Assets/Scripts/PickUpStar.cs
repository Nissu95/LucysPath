using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpStar : MonoBehaviour
{
    [SerializeField] string starTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(starTag))
        {
            other.gameObject.SetActive(false);
            GameManager.singleton.StarsCount();
        }
    }
}
