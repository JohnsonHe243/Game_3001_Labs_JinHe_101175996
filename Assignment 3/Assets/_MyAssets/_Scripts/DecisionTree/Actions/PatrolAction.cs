using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAction : ActionNode
{
    public PatrolAction()
    {
        name = "Patrol Action";
    }
    public override void Action()
    {
        // Enter action function.
        if (Agent.GetComponent<AgentObject>().state != ActionState.PATROL)
        {
            Debug.Log("Starting " + name);
            AgentObject ao = Agent.GetComponent<AgentObject>();
            ao.state = ActionState.PATROL;

            // Custom actions.
        if (AgentScript is RangedCombatEnemy rce)
            {
                rce.StartPatrol();
            }
        }
        // Action in everyframe.
        Debug.Log("Performing " + name);
    }
}
