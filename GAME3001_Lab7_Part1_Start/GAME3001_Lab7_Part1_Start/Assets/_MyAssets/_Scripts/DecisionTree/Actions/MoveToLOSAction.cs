using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLOSAction : ActionNode
{
    public MoveToLOSAction()
    {
        name = "Move to LOS Action";
    }
    public override void Action()
    {
        // Enter action function.
        if (Agent.GetComponent<AgentObject>().state != ActionState.MOVE_TO_LOS)
        {
            Debug.Log("Starting " + name);
            AgentObject ao = Agent.GetComponent<AgentObject>();
            ao.state = ActionState.MOVE_TO_LOS;
            // Custom actions.
            if (AgentScript is CloseCombatEnemy cce)
            {

            }
            else if (AgentScript is RangedCombatEnemy rce)
            {

            }
        }
        // Action in everyframe.
        Debug.Log("Performing " + name);
    }
}
