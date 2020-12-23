using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TutorialContainer")]
public class TutorialContainer : ScriptableObject
{
    [SerializeField] int level;
    [SerializeField] Texture[] frames;

    public Texture[] GetFrames()
    {
        return frames;
    }

    public int GetLevel()
    {
        return level - 1;
    }
}
