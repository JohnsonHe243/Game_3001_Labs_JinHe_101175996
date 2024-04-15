using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCondition : ConditionNode
{
    public bool IsIdling { get; set; }

    public IdleCondition()
    {
        name = "Idle Condition";
        IsIdling = false;
    }
    public override bool Condition()
    {
        Debug.Log("Checking " + name);
        return IsIdling;
    }
}
