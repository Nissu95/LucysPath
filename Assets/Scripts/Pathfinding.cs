using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float distanceLimit = .01f;

    FSM fsm = new FSM(3, 3);
    Animator animator;
    List<Vector3> nodes;
    BoxCollider boxCollider;

    Vector3 diff;
    Vector3 dir;

    int nodeIndex = 0;
    
    void Start()
    {
        fsm.SetState(State.Idle);

        fsm.SetRelation(State.Idle, Event.ToWalking, State.Walking);
        fsm.SetRelation(State.Walking, Event.ToWin, State.Win);
        fsm.SetRelation(State.Win, Event.ToIdle, State.Idle);

        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        switch (fsm.GetState())
        {
            case State.Idle:
                animator.SetBool("isWalking", false);
                break;
            case State.Walking:

                if (nodes.Count <= 0)
                    return;

                diff = nodes[nodeIndex] - transform.position;
                dir = diff.normalized;

                if (diff.magnitude > distanceLimit)
                {
                    transform.position += dir * speed * Time.deltaTime;

                    transform.rotation = Quaternion.Lerp(transform.rotation,
                                                         Quaternion.LookRotation(dir),
                                                         Time.deltaTime * speed * 4);
                }
                else
                {
                    nodeIndex++;
                }

                if (nodeIndex >= nodes.Count)
                    fsm.SetEvent(Event.ToWin);
                
                animator.SetFloat("Rotation", Vector3.Cross(transform.TransformDirection(Vector3.forward), diff).y);
                animator.SetBool("isWalking", true);
                break;
            case State.Win:
                animator.SetBool("isWalking", false);
                GameManager.singleton.LevelWin();
                break;
        }
    }

    public void StartWalking(List<Vector3> _nodes)
    {
        nodeIndex = 0;
        nodes = _nodes;
        boxCollider.enabled = true;
        fsm.SetEvent(Event.ToWalking);
    }

    public void NextNode()
    {
        nodeIndex++;
    }

    public FSM GetFSM()
    {
        return fsm;
    }
}
