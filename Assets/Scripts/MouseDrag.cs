using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]


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

    private void Start()
    {
        path = GetComponent<Path>();
    }

    void OnMouseDown()
    {
        maxPosition = LevelCreator.singleton.GetMaxPosition();

        scanPos = gameObject.transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(scanPos);
        offset = scanPos - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        if (tag == Tags.portal)
            GetComponent<Portal>().PortalTouch();
    }



    void OnMouseDrag()
    {
        curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        curPosition.x = (float)((curPosition.x) / gridSize) * gridSize;
        curPosition.z = (float)((curPosition.z) / gridSize) * gridSize;
        curPosition.y = scanPos.y;

        if (movementLock)
            return;

        transform.position = curPosition;
    }

    private void OnMouseUp()
    {
        Vector3 diff = scanPos - curPosition;

        rotate = (diff.magnitude < minMovementSquare);

        if (!movementLock)
        {
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
                        transform.position = curPosition;
                    else transform.position = scanPos;
                }
            }
            else transform.position = scanPos;

            if (rotate)
            {
                transform.Rotate(0, -90, 0, Space.Self);
                path.RotatePath();
            }

            LevelCreator.singleton.FindPath();
        }
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