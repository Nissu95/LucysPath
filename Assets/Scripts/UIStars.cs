using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStars : MonoBehaviour
{
    [SerializeField] Image[] stars;
    [SerializeField] Color unlockedStarColor;
    [SerializeField] Color lockedStarColor;

    public void UpdateUI(int starsUnlocked)
    {
        for (int i = 0; i < stars.Length; i++)
            if (i < starsUnlocked)
                stars[i].color = unlockedStarColor;
            else
                stars[i].color = lockedStarColor;
    }
}
