using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsTextChange : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] Text displayStarsTxt;
    [SerializeField] Text displayMaxStarsTxt;

    private void OnEnable()
    {
        displayStarsTxt.text = "Current Playthrough: " + GameManager.singleton.GetStars();
        displayMaxStarsTxt.text = "Max Stars: " + GameManager.singleton.GetMaxStars();
    }
}