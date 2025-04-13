using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : ActionNode
{
    public IdleAction()
    {
        name = "Idle Action";
    }

    public override void Action()
    {
        if (Agent.GetComponent<AgentObject>().state != ActionState.IDLE)
        {
            Debug.Log("Starting " + name);
            AgentObject ao = Agent.GetComponent<AgentObject>();
            ao.state = ActionState.IDLE;
        }

        Debug.Log("Performing " + name);
    }
}
