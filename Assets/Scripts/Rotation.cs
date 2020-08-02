using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : Interactive
{
    public override void Interact()
    {
        transform.Rotate(0, -90, 0, Space.Self);

        if (gameObject.CompareTag(Constants.PathTag) 
            || gameObject.CompareTag(Constants.firstPathTag) 
            || gameObject.CompareTag(Constants.lastPathTag))
        {
            GetComponent<Path>().RotatePath();
            LevelCreator.singleton.FindPath();
        }
    }
}
