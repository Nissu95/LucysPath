using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float distanceLimit = .01f;

    FSM fsm = new FSM(2, 2);

    List<Vector3> nodes;

    int nodeIndex = 0;
    
    void Start()
    {
        fsm.SetState(State.Idle);

        fsm.SetRelation(State.Idle, Event.ToWalking, State.Walking);
        fsm.SetRelation(State.Walking, Event.ToIdle, State.Idle);
    }

    private void Update()
    {
        switch (fsm.GetState())
        {
            case State.Idle:
                break;
            case State.Walking:
                
                if (nodes.Count <= 0)
                    return;

                Debug.Log("Nodos: " + nodes.Count);

                Vector3 diff = nodes[nodeIndex] - transform.position;
                Vector3 dir = diff.normalized;

                if (diff.magnitude > distanceLimit)
                {
                    transform.position += dir * speed * Time.deltaTime;
                }
                else
                {
                    nodeIndex++;
                }

                if (nodeIndex >= nodes.Count)
                    fsm.SetEvent(Event.ToIdle);
                break;
        }
    }

    public void StartWalking(List<Vector3> _nodes)
    {
        nodeIndex = 0;
        nodes = _nodes;
        fsm.SetEvent(Event.ToWalking);
    }
}
