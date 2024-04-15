using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolCondition : ConditionNode
{
    public bool IsPatrolling { get; set; }
    public PatrolCondition() 
    {
        name = "Patrol Condition";
        IsPatrolling = false;
    }
    public override bool Condition()
    {
        Debug.Log("Checking " + name);
        return IsPatrolling;
    }
}
