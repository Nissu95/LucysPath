﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    
    void Update()
    {
#if UNITY_EDITOR
        CheckEditorInput();
#elif UNITY_ANDROID
        CheckMobileInput();
#endif
    }

    void CheckEditorInput()
    {
        if (Input.GetMouseButtonDown(0))
            Interaction(Input.mousePosition);
    }

    void CheckMobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch Touch = Input.GetTouch(0);

            if (Touch.phase == TouchPhase.Ended)
            {
                Interaction(Touch.position);
            }
        }
    }

    void Interaction(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Mirror"))
            {
                Debug.Log("Toca espejo");
                hit.transform.GetComponent<Interactive>().Interact();
            }
        }
    }
}