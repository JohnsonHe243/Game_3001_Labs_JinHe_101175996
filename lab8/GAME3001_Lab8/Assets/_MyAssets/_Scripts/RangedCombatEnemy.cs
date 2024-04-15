using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class RangedCombatEnemy : AgentObject
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

    [SerializeField] float health; // Health

    [Header("Torpedo Properties")]
    private bool readyToFire = true;
    [SerializeField] float torpedoCooldown;
    [SerializeField] float torpedoLifespan;
    [SerializeField] GameObject torpedoPrefab;
    [SerializeField] float combatRange;

    new void Start() // Note the new.
    {
        base.Start(); // Explicitly invoking Start of AgentObject.
        Debug.Log("Starting Ranged Combat Enemy.");
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

        // Using Decision tree to seek temporarily to the target (player).
        dt.RadiusNode.IsWithinRadius = Vector3.Distance(transform.position, testTarget.position) <= detectRange;
        dt.LOSNode.HasLOS = hit;

        dt.HealthNode.IsHealthy = health > 25;
        dt.RangedCombatNode.IsWithinCombatRange = Vector3.Distance(transform.position, testTarget.position) <= combatRange;

        dt.MakeDecision();
        switch (state)
        {
            case ActionState.PATROL:
                SeekForward();
                break;
            case ActionState.FLEE:
                Flee();
                break;
            case ActionState.MOVE_TO_RANGE:
                MoveToRange();
                break;
            case ActionState.MOVE_TO_LOS:
                MoveToLOS();
                break;
            case ActionState.ATTACK:
                Attack();
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
        dt.HealthNode = new HealthCondition();
        dt.treeNodeList.Add(dt.HealthNode);

        // Second level.

        // FleeAction leaf.
        TreeNode FleeNode = dt.AddNode(dt.HealthNode, new FleeAction(), TreeNodeType.LEFT_TREE_NODE);
        ((ActionNode)FleeNode).SetAgent(this.gameObject, typeof(RangedCombatEnemy));
        dt.treeNodeList.Add(FleeNode);

        // HitCondition node.
        dt.HitNode = new HitCondition();
        dt.treeNodeList.Add(dt.AddNode(dt.HealthNode, dt.HitNode, TreeNodeType.RIGHT_TREE_NODE));

        // Third level.

        // Radius Condition
        dt.RadiusNode = new RadiusCondition();
        dt.treeNodeList.Add(dt.AddNode(dt.HitNode, dt.RadiusNode, TreeNodeType.LEFT_TREE_NODE));

        // TODO: Other LOS
        //

        // Fourth level.

        // PatrolAction leaf.
        TreeNode patrolNode = dt.AddNode(dt.RadiusNode, new PatrolAction(),
            TreeNodeType.LEFT_TREE_NODE);
        ((ActionNode)patrolNode).SetAgent(this.gameObject, typeof(RangedCombatEnemy));
        dt.treeNodeList.Add(patrolNode);

        // LOS Condition node
        dt.LOSNode = new LOSCondition();
        dt.treeNodeList.Add(dt.AddNode(dt.RadiusNode, dt.LOSNode, TreeNodeType.RIGHT_TREE_NODE));

        // TODO WaitBehindCover Node to be done later
        //
        // TODO MoveToCover Node to be done later
        //

        // Fifth level.

        // MoveToLOSAction Leaf.
        TreeNode moveToLOSNode = dt.AddNode(dt.LOSNode, new MoveToLOSAction(), TreeNodeType.LEFT_TREE_NODE);
        ((ActionNode)moveToLOSNode).SetAgent(this.gameObject, typeof(RangedCombatEnemy));
        dt.treeNodeList.Add(moveToLOSNode);

        // RangedCombatCondition node.
        dt.RangedCombatNode = new RangedCombatCondition();
        dt.treeNodeList.Add(dt.AddNode(dt.LOSNode, dt.RangedCombatNode, TreeNodeType.RIGHT_TREE_NODE));

        // Sixth level.

        // MoveToRangeAction leaf.
        TreeNode moveToRangeNode = dt.AddNode(dt.RangedCombatNode, new MoveToRangeAction(), TreeNodeType.LEFT_TREE_NODE);
        ((ActionNode)moveToRangeNode).SetAgent(this.gameObject, typeof(RangedCombatEnemy));
        dt.treeNodeList.Add(moveToRangeNode);

        // AttackAction leaf.
        TreeNode attackNode = dt.AddNode(dt.RangedCombatNode, new AttackAction(), TreeNodeType.RIGHT_TREE_NODE);
        ((ActionNode)attackNode).SetAgent(this.gameObject, typeof(RangedCombatEnemy));
        dt.treeNodeList.Add(attackNode);
    }

    private void Flee()
    {

    }
    private void MoveToRange()
    {
        SeekForward();
    }
    public void SetCombatTarget()
    {
        m_target = testTarget;
    }
    private void MoveToLOS()
    {

    }
    private void Attack()
    {
        if (readyToFire)
        {
            FireTorpedo();
        }
    }
    private void FireTorpedo()
    {
        readyToFire = false;
        Game.Instance.SOMA.PlaySound("Torpedo_k");
        Invoke("ReloadTorpedo", torpedoCooldown);
        GameObject torpedoInst = GameObject.Instantiate(torpedoPrefab, transform.position, Quaternion.identity);
        torpedoInst.GetComponent<EnemyTorpedoScript>().LockOnTarget(testTarget);
        Destroy(torpedoInst, torpedoLifespan);
    }
    private void ReloadTorpedo()
    {
        Debug.Log("Torpedo reloaded!");
        readyToFire = true;
    }
}
