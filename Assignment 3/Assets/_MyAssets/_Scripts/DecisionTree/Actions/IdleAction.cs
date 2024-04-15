using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : ActionNode
{
    public IdleAction()
    {
        name = "Move to LOS Action";
    }
    public override void Action()
    {
        // Enter action function.
        if (Agent.GetComponent<AgentObject>().state != ActionState.IDLE)
        {
            Debug.Log("Starting " + name);
            AgentObject ao = Agent.GetComponent<AgentObject>();
            ao.state = ActionState.IDLE;
            // Custom actions.
            if (AgentScript is RangedCombatEnemy rce)
            {

            }
        }
        // Action in everyframe.
        Debug.Log("Performing " + name);
    }
}