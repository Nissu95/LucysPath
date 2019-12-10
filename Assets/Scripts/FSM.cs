using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM {

    int[,] fsm;
    int state = 0;

    public FSM(int statesCount, int eventsCount)
    {
        fsm = new int[statesCount, eventsCount];

        for (int i = 0; i < statesCount; i++)
        {
            for (int j = 0; j < eventsCount; j++)
            {
                fsm[i, j] = -1;
            }
        }
    }

    public void SetRelation(State srcState, Event Event, State dstState)
    {
        fsm[(int)srcState, (int)Event] = (int)dstState;
    }

    public State GetState()
    {
        return (State)state;
    }

    public void SetState(State _state)
    {
        state = (int)_state;
    }

    public void SetEvent(Event _event)
    {
        int evt = (int)_event;

        if (fsm[state, evt] != -1)
            state = fsm[state, evt];
    }
}

public enum State { Idle, Walking, Win };

public enum Event { ToWalking, ToWin };
