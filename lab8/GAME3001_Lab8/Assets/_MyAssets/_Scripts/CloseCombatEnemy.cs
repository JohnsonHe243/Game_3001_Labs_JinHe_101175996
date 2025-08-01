using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombatEnemy : AgentObject
{
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float pointRadius;

    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float whiskerLength;
    [SerializeField] float whiskerAngle;

    [SerializeField] float detectRange;
    // [SerializeField] float avoidanceWeight;
    private Rigidbody2D rb;
    private NavigationObject no;
    // Decision Tree.
    private DecisionTree dt;
    private int patrolIndex; 
    [SerializeField] Transform testTarget; // Planet to seek.

    new void Start() // Note the new.
    {
        base.Start(); // Explicitly invoking Start of AgentObject.
        Debug.Log("Starting Close Combat Enemy.");
        rb = GetComponent<Rigidbody2D>();
        no = GetComponent<NavigationObject>();
        // TODO: Add for Lab 7a.
        dt = new DecisionTree(this.gameObject);
        BuildTree();
        patrolIndex = 0;
    }

    void Update()
    {
        Vector2 direction = (testTarget.position - transform.position).normalized;
        float angleInRadians = Mathf.Atan2(direction.y, direction.x);
        whiskerAngle = angleInRadians * Mathf.Rad2Deg;
        bool hit = CastWhisker(whiskerAngle, Color.red);

        // bool hit = CastWhisker(whiskerAngle, Color.red);
        // transform.Rotate(0f, 0f, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime);

        //if (TargetPosition != null)
        //{
        //    // Seek();
        //    SeekForward();
        //    AvoidObstacles();
        //}

        dt.RadiusNode.IsWithinRadius = Vector3.Distance(transform.position, testTarget.position) <= detectRange;
        dt.LOSNode.HasLOS = hit;
        dt.MakeDecision();

        switch (state)
        {
            case ActionState.PATROL:
                SeekForward();
                break;
            // TODO: other actions later.
            default: // Just for now. Immediately stop the ship or it will keep going.
                rb.velocity = Vector3.zero;
                break;
        }
    }

    //private void AvoidObstacles()
    //{
    //    // Cast whiskers to detect obstacles
    //    bool hitLeft = CastWhisker(whiskerAngle, Color.red);
    //    bool hitRight = CastWhisker(-whiskerAngle, Color.blue);

    //    // Adjust rotation based on detected obstacles
    //    if (hitLeft)
    //    {
    //        // Rotate counterclockwise if the left whisker hit
    //        RotateClockwise();
    //    }
    //    else if (hitRight && !hitLeft)
    //    {
    //        // Rotate clockwise if the right whisker hit
    //        RotateCounterClockwise();
    //    }
    //}

    //private void RotateCounterClockwise()
    //{
    //    // Rotate counterclockwise based on rotationSpeed and a weight.
    //    transform.Rotate(Vector3.forward, rotationSpeed * avoidanceWeight * Time.deltaTime);
    //}

    //private void RotateClockwise()
    //{
    //    // Rotate clockwise based on rotationSpeed.
    //    transform.Rotate(Vector3.forward, -rotationSpeed * avoidanceWeight * Time.deltaTime);
    //}

    private bool CastWhisker(float angle, Color color)
    {
        bool hitResult = false;
        Color rayColor = color;

        // Calculate the direction of the whisker.
        Vector2 whiskerDirection = Quaternion.Euler(0, 0, angle) * Vector2.right;

        if (no.HasLOS(gameObject, "Player", whiskerDirection, whiskerLength))
        {
            // Debug.Log("Obstacle detected!");
            rayColor = Color.green;
            hitResult = true;
        }

        // Debug ray visualization
        Debug.DrawRay(transform.position, whiskerDirection * whiskerLength, rayColor);
        return hitResult;
    }

    private void SeekForward() // A seek with rotation to target but only moving along forward vector.
    {
        // Calculate direction to the target.
        Vector2 directionToTarget = (TargetPosition - transform.position).normalized;

        // Calculate the angle to rotate towards the target.
        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg + 90.0f; // Note the +90 when converting from Radians.

        // Smoothly rotate towards the target.
        float angleDifference = Mathf.DeltaAngle(targetAngle, transform.eulerAngles.z);
        float rotationStep = rotationSpeed * Time.deltaTime;
        float rotationAmount = Mathf.Clamp(angleDifference, -rotationStep, rotationStep);
        transform.Rotate(Vector3.forward, rotationAmount);

        // Move along the forward vector using Rigidbody2D.
        rb.velocity = transform.up * movementSpeed;

        // Continue patrol.
        if (Vector3.Distance(transform.position, TargetPosition) <= pointRadius)
        {
            m_target = GetNextPatrolPoint();
        }
    }
    public void StartPatrol()
    {
        m_target = patrolPoints[patrolIndex];
    }
    private Transform GetNextPatrolPoint()
    {
        patrolIndex++;
        if (patrolIndex >= patrolPoints.Length)
        {
            patrolIndex = 0;
        }    
        return patrolPoints[patrolIndex];
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Target")
    //    {
    //        GetComponent<AudioSource>().Play();
    //    }
    //}

    private void BuildTree()
    {
        // Root condition node.
        dt.RadiusNode = new RadiusCondition();
        dt.treeNodeList.Add(dt.RadiusNode);

        // Second level.

        // PatrolAction leaf.
        TreeNode patrolNode = dt.AddNode(dt.RadiusNode, new PatrolAction(), TreeNodeType.LEFT_TREE_NODE);
        ((ActionNode)patrolNode).SetAgent(this.gameObject, typeof(CloseCombatEnemy));
        dt.treeNodeList.Add(patrolNode);

        // LOSCondition node.
        dt.LOSNode = new LOSCondition();
        dt.treeNodeList.Add(dt.AddNode(dt.RadiusNode, dt.LOSNode, TreeNodeType.RIGHT_TREE_NODE));

        // Third level.

        // MoveToLOSAction leaf.
        TreeNode MoveToLOSNode = dt.AddNode(dt.LOSNode, new MoveToLOSAction(), TreeNodeType.LEFT_TREE_NODE);
        ((ActionNode)MoveToLOSNode).SetAgent(this.gameObject, typeof(CloseCombatEnemy));
        dt.treeNodeList.Add(MoveToLOSNode);

        // CloseCombatCondition node.
        dt.CloseCombatNode = new CloseCombatCondition();
        dt.treeNodeList.Add(dt.AddNode(dt.LOSNode, dt.CloseCombatNode,
            TreeNodeType.RIGHT_TREE_NODE));

        // Fourth level.

        // MoveToPlayerAction leaf.
        TreeNode MoveToPlayerNode = dt.AddNode(dt.CloseCombatNode, new MoveToPlayerAction(),
            TreeNodeType.LEFT_TREE_NODE);
        ((ActionNode)MoveToPlayerNode).SetAgent(this.gameObject, typeof(CloseCombatEnemy));
        dt.treeNodeList.Add(MoveToPlayerNode);

        // AttackAction leaf.
        TreeNode AttackNode = dt.AddNode(dt.CloseCombatNode, new AttackAction(),
            TreeNodeType.RIGHT_TREE_NODE);
        ((ActionNode)AttackNode).SetAgent(this.gameObject, typeof(CloseCombatEnemy));
        dt.treeNodeList.Add(AttackNode);
    }
}
