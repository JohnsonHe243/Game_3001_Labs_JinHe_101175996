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
        if (Agent.GetComponent<Starship>().state != ActionState.PATROL)
        {
            Debug.Log("Starting " + name);
            Starship ss = Agent.GetComponent<Starship>();
            ss.state = ActionState.PATROL;
            
            // Custom actions.
            ss.StartPatrol();
        }
        // Action in everyframe.
        Debug.Log("Performing " + name);
    }
}
