using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToRangeAction : ActionNode
{
    public MoveToRangeAction()
    {
        name = "Move to Range Action";
    }
    public override void Action()
    {
        // Enter action function.
        if (Agent.GetComponent<AgentObject>().state != ActionState.MOVE_TO_RANGE)
        {
            Debug.Log("Starting " + name);
            AgentObject ao = Agent.GetComponent<AgentObject>();
            ao.state = ActionState.MOVE_TO_RANGE;

            // Custom actions.
            if (AgentScript is RangedCombatEnemy rce)
            {

            }
        }
        // Action in everyframe.
        Debug.Log("Performing " + name);
    }
}