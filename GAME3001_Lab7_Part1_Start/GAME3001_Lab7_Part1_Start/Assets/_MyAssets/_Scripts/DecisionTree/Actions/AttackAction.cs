using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : ActionNode
{
    public AttackAction()
    {
        name = "Attack Action";
    }

    public override void Action()
    {
        // Enter action functionality.
        if (Agent.GetComponent<Starship>().state != ActionState.ATTACK)
        {
            Debug.Log("Starting " + name);
            Starship ss = Agent.GetComponent<Starship>();
            ss.state = ActionState.ATTACK;

            // Custom actions.
        }
        // Every frame.
        Debug.Log("Performing " + name);
    }
}
