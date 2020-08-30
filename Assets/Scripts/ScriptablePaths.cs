using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Path", menuName = "Obstacles/Path", order = 1)]
public class ScriptablePaths : ScriptableObject
{
    public Sprite texture;
    public GameObject prefab;
}
