﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : Interactive
{
    public override void Interact()
    {
        transform.Rotate(0, 90, 0, Space.Self);
    }
}