﻿using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(BoxCollider))]


public class MouseDrag : MonoBehaviour
{
    Vector3 screenPoint,
            offset,
            scanPos,
            curPosition,
            curScreenPoint;


    const float gridSize = 1f;
    const float minMovementSquare = 0.5f;

    Path path;
    Vector2Int maxPosition;

    bool rotate = false;

    bool movementLock = false;

    Timer timer;

    private void Start()
    {
        path = GetComponent<Path>();
        timer = new Timer();
        timer.SetTime(GameManager.singleton.GetMouseDragTime());
    }

    void OnMouseDown()
    {
        if (GameManager.singleton.PathFound || GameManager.singleton.IsPause())
            return;

        maxPosition = LevelCreator.singleton.GetMaxPosition();

        scanPos = gameObject.transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(scanPos);
        offset = scanPos - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        if (tag == Constants.PortalTag)
            GetComponent<Portal>().PortalTouch();
        else if (!movementLock)
            SoundManager.singleton.PickUpClip();
    }

    void OnMouseDrag()
    {
        if (GameManager.singleton.PathFound || GameManager.singleton.IsPause() || movementLock)
            return;

        timer.Update();

        if (timer.TimeUp())
            rotate = false;
        else
            rotate = true;

        if (!rotate)
        {
            if (scanPos.y <= GameManager.singleton.GetGrabHeight())
                scanPos.y += GameManager.singleton.GetGrabHeight();

            curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            curPosition.x = (float)((curPosition.x) / gridSize) * gridSize;
            curPosition.z = (float)((curPosition.z) / gridSize) * gridSize;
            curPosition.y = scanPos.y;

            transform.position = curPosition;
        }
    }

    private void OnMouseUp()
    {
        if (GameManager.singleton.PathFound || GameManager.singleton.IsPause() || movementLock)
            return;

        if (!rotate)
        {
            if (scanPos.y > GameManager.singleton.GetGrabHeight())
                scanPos.y -= GameManager.singleton.GetGrabHeight();

            curPosition.y = scanPos.y;

            curPosition.x = (float)Math.Round(curPosition.x);
            curPosition.z = (float)Math.Round(curPosition.z);

            if (curPosition.x < maxPosition.x
                && curPosition.z < maxPosition.y
                && curPosition.x >= 0
                && curPosition.z >= 0)
            {
                Vector2Int pathIndex = LevelCreator.singleton.GetPath(GetComponent<Path>());

                if (pathIndex != LevelCreator.singleton.nullPosition)
                {
                    bool movementCorrect = LevelCreator.singleton.MovePath(pathIndex, new Vector2Int((int)curPosition.x, (int)curPosition.z));

                    if (movementCorrect)
                    {
                        transform.position = curPosition;
                        SoundManager.singleton.PickUpClip();
                    }
                    else
                    {
                        transform.position = scanPos;
                        SoundManager.singleton.PickUpClip();
                    }
                }
            }
            else
            {
                transform.position = scanPos;
                SoundManager.singleton.PickUpClip();
            }
        }
        else
        {
            path.RotateSquare();
            path.RotatePath();
        }

        timer.Reset();
        LevelCreator.singleton.FindPath();
    }

    public void Lock()
    {
        movementLock = true;
    }

    public void Unlock()
    {
        movementLock = false;
    }
}