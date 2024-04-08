using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayerAction : ActionNode
{
    public MoveToPlayerAction()
    {
        name = "Move to Player Action";
    }
    public override void Action()
    {
        // Enter action function.
        if (Agent.GetComponent<AgentObject>().state != ActionState.MOVE_TO_PLAYER)
        {
            Debug.Log("Starting " + name);
            AgentObject ao = Agent.GetComponent<AgentObject>();
            ao.state = ActionState.MOVE_TO_PLAYER;

            // Custom actions.
            if (AgentScript is CloseCombatEnemy cce)
            {

            }


        }
        // Action in everyframe.
        Debug.Log("Performing " + name);
    }
}
