using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using TMPro;
public class RangedCombatEnemy : AgentObject
{
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float pointRadius;

    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float whiskerLength;
    [SerializeField] float whiskerAngle;

    [SerializeField] TMP_Text enemyState;
    [SerializeField] TMP_Text timeText;


    [SerializeField] float detectRange = 0;

    // [SerializeField] float avoidanceWeight;
    private Rigidbody2D rb;
    private NavigationObject no;
    // Decision Tree.
    private DecisionTree dt;
    private int patrolIndex; 
    [SerializeField] Transform testTarget; // Planet to seek.

    private bool patrol = false;
    private float timer;

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

        timer = UnityEngine.Random.Range(6f, 10f);
    }

    void Update()
    {


        bool hit = false;
        // Calculate direction vector from enemy to target (player)
        Vector2 direction = (testTarget.position - transform.position).normalized;

        // Calculate the angle between the direction vector and the ship's forward direction
        float angleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Calculate the angle of the ship's rotation
        float shipRotation = transform.eulerAngles.z;

        // Calculate the difference in angle between the ship's rotation and the angle to the player
        float angleDifference = Mathf.DeltaAngle(shipRotation, angleToPlayer);

        // Check if the player is within the front 180 degrees of the ship
        if (angleDifference <= 180 && angleDifference >=0)
        {
            // Draw the whisker (or perform any action related to it)
            hit = CastWhisker(angleToPlayer, Color.red);
        }

        if (hit == true)
        {
            enemyState.text = "Enenmy is Moving Towards Player!";
        }
        else if (hit == false && patrol == false)
        {
            enemyState.text = "Enemy is in Idle.";
        }
        else if (hit == false && patrol == true)
        {
            enemyState.text = "Enemy is in Patrol";
        }    
        
        timeText.text = "Changing to Idle/Patrol in " + timer.ToString("F2") + "s" ;

        dt.LOSNode.HasLOS = hit;
        dt.PatrolNode.IsPatrolling = patrol;

        dt.MakeDecision();
        switch (state)
        { 
            case ActionState.PATROL:
                SeekForward();
                break;
            case ActionState.IDLE:
                rb.velocity = Vector3.zero;
                break;
            case ActionState.MOVE_TO_LOS:
                SeekForward();
                break;
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            patrol = !patrol;
            timer = UnityEngine.Random.Range(6f, 10f);
        }
    }

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

    public void SeekForward() // A seek with rotation to target but only moving along forward vector.
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
        else if (Vector3.Distance(transform.position, testTarget.position) <= whiskerLength)
        {
            m_target = testTarget;
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
        // LOS Condition node
        dt.LOSNode = new LOSCondition();
        dt.treeNodeList.Add(dt.LOSNode);

        // First level.

        // MoveToLOSAction Leaf.
        TreeNode moveToLOSNode = dt.AddNode(dt.LOSNode, new MoveToLOSAction(), TreeNodeType.RIGHT_TREE_NODE);
        ((ActionNode)moveToLOSNode).SetAgent(this.gameObject, typeof(RangedCombatEnemy));
        dt.treeNodeList.Add(moveToLOSNode);

        // Patrol Condition node
        dt.PatrolNode = new PatrolCondition();
        dt.treeNodeList.Add(dt.AddNode(dt.LOSNode, dt.PatrolNode, TreeNodeType.LEFT_TREE_NODE));

        // Second level.

        // PatrolAction leaf.
        TreeNode patrolNode = dt.AddNode(dt.PatrolNode, new PatrolAction(),
            TreeNodeType.RIGHT_TREE_NODE);
        ((ActionNode)patrolNode).SetAgent(this.gameObject, typeof(RangedCombatEnemy));
        dt.treeNodeList.Add(patrolNode);

        // IdleAction leaf
        TreeNode idleNode = dt.AddNode(dt.PatrolNode, new IdleAction(),
            TreeNodeType.LEFT_TREE_NODE);
        ((ActionNode)idleNode).SetAgent(this.gameObject, typeof(RangedCombatEnemy));
        dt.treeNodeList.Add(idleNode);
    }
}
