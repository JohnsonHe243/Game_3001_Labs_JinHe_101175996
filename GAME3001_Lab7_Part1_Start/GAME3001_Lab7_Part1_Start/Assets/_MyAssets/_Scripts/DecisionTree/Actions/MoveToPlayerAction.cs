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
        if (Agent.GetComponent<Starship>().state != ActionState.MOVE_TO_PLAYER)
        {
            Debug.Log("Starting " + name);
            Starship ss = Agent.GetComponent<Starship>();
            ss.state = ActionState.MOVE_TO_PLAYER;

            // Custom actions.

        }
        // Action in everyframe.
        Debug.Log("Performing " + name);
    }
}
