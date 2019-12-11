using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsTextChange : MonoBehaviour
{
    [SerializeField] Text displayStarsTxt;
    [SerializeField] Text displayMaxStarsTxt;

    private void OnEnable()
    {
        displayStarsTxt.text += GameManager.singleton.GetStars();
        displayMaxStarsTxt.text += GameManager.singleton.GetMaxStars();
    }
}