using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcScript : AgentObject
{
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float pointRadius;

    [SerializeField] float movementSpeed; 
    [SerializeField] float rotationSpeed;
    [SerializeField] float whiskerLength;
    [SerializeField] float whiskerAngle;

    [SerializeField] TMP_Text stateText;
    [SerializeField] TMP_Text switchStateText;
    [SerializeField] TMP_Text optionsText;

    private Rigidbody2D rb;

    private DecisionTree dt;
    private int patrolIndex;
    [SerializeField] Transform target;

    private int switchStateCount = 0;
    private float idleTimer;

   


    new void Start()
    {
        base.Start(); // Explicitly invoking Start of AgentObject.
        Debug.Log("Starting RangedCombat Enemy.");
        rb = GetComponent<Rigidbody2D>();

        dt = new DecisionTree(this.gameObject);
        BuildTree();
        patrolIndex = 0;
    }
    
    void Update()
    {
        UpdateLineOfSight();

        stateText.text = ("Curent state: " + state);

        if (state == ActionState.IDLE)
        {
            optionsText.text = "Options:\r\n1. switch to patrol\r\n2. find player";
        }
        else if (state == ActionState.PATROL)
        {
            optionsText.text = "Options:\r\n1. switch to idle\r\n2. find player";
        }
        else
        {
            optionsText.text = "Options:";
        }

        dt.MoveToPatrolNode.canPatrol = false;
        dt.ChangeStateNode.hasToChange = true;

        dt.MakeDecision();

        switch (state)
        {
            case ActionState.IDLE:
                Idle();
                break;
            case ActionState.PATROL:
                Patrol();
                break;
            case ActionState.MOVE_TO_PLAYER:
                MoveToPlayer();
                break;
            default:
                rb.velocity = Vector3.zero;
                break;
        }
    }

    private bool CastWiskers(float angle, Color colour)
    {
        bool hitResult = false;
        Color rayColour = colour;

        // calculate direction of the wiskers 
        Vector2 whiskerDirection = Quaternion.Euler(0, 0, angle) * transform.up;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, whiskerDirection, whiskerLength);

        // check if the ray hits an obstacle
        if (hit.collider != null)
        {
            Debug.Log("Player detected");
            rayColour = Color.green;
            hitResult = true;
        }

        Debug.DrawRay(transform.position, whiskerDirection * whiskerLength, rayColour);

        return hitResult;
    }

    private void Idle()
    {
        switchStateText.text = "Switching in: " + idleTimer;
        idleTimer += Time.deltaTime;
        if (idleTimer >= 5)
        {
            ChangeState();
            idleTimer = 0;
        }   
    }

    private void UpdateLineOfSight()
    {
        bool hitLeft = CastWiskers(whiskerAngle, Color.magenta);
        bool hitRight = CastWiskers(-whiskerAngle, Color.red);
        bool hitCenterLeft = CastWiskers(whiskerAngle - 35, Color.cyan);
        bool hitCenterRight = CastWiskers(-whiskerAngle + 35, Color.blue);

        if (hitLeft || hitRight || hitCenterLeft || hitCenterRight) 
        {
            dt.ChangeStateNode.hasToChange = false;
            dt.LOSNode.hasLOS = true;
        }
    }


    private void MoveToPlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * movementSpeed;

        // Rotate towards the player's direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotationSpeed * Time.deltaTime);
    }

    private void Patrol()
    {
        SeekForward();
        
        //if (Vector3.Distance(transform.position, patrolPoints[patrolIndex].position) < pointRadius)
        //{
        //    patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        //}
    }

    private void ChangeState()
    {
        
        if (switchStateCount == 3)
        {
            
            switchStateCount = 0;
            if (dt.MoveToPatrolNode.canPatrol == false)
            {
                dt.MoveToPatrolNode.canPatrol = true;
                dt.ChangeStateNode.hasToChange = false;
            }
            else
            {
                dt.ChangeStateNode.hasToChange = true;
                dt.MoveToPatrolNode.canPatrol = false;
            }
           
        }
        
        int randomNumber = UnityEngine.Random.Range(1, 101);
        dt.MoveToPatrolNode.canPatrol = true;
        if (randomNumber >= 50)
        {
            switchStateCount++;

            dt.MoveToPatrolNode.canPatrol = true;
            dt.ChangeStateNode.hasToChange = false;
            Debug.Log("Switched to patrol");
            
        }
        else
        {
            dt.ChangeStateNode.hasToChange = true;
            dt.MoveToPatrolNode.canPatrol = false;
            Debug.Log("Switched to idle");

        }
        
        
        
    }

    private void SeekForward()
    {
        Vector3 direction = (patrolPoints[patrolIndex].position - transform.position).normalized;
        rb.velocity = direction * movementSpeed;

        // Rotate towards the next patrol point
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Check if reached the patrol point
        if (Vector3.Distance(transform.position, patrolPoints[patrolIndex].position) < pointRadius)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;

            Debug.Log(patrolIndex);

            // If completed a full cycle, change state
            if (patrolIndex == 0)
            {
                ChangeState();
            }
        }
    }

    private void BuildTree()
    {
        dt.ChangeStateNode = new ChangeStateCondition();
        dt.LOSNode = new LOSCondition();
        dt.MoveToPatrolNode = new MoveToPatrolCondition();

        TreeNode changeStateConditionNode = dt.AddNode(dt.ChangeStateNode, dt.LOSNode, TreeNodeType.LEFT_TREE_NODE);
        dt.treeNodeList.Add(changeStateConditionNode);

        TreeNode idleNode = dt.AddNode(dt.ChangeStateNode, new IdleAction(), TreeNodeType.LEFT_TREE_NODE);
        ((ActionNode)idleNode).Agent = this.gameObject;
        dt.treeNodeList.Add(idleNode);

        TreeNode moveToPatrolConditionNode = dt.AddNode(dt.ChangeStateNode, dt.MoveToPatrolNode, TreeNodeType.LEFT_TREE_NODE);
        dt.treeNodeList.Add(moveToPatrolConditionNode);

        TreeNode patrolNode = dt.AddNode(dt.LOSNode, new PatrolAction(), TreeNodeType.LEFT_TREE_NODE);
        ((ActionNode)patrolNode).Agent = this.gameObject;
        dt.treeNodeList.Add(patrolNode);

        TreeNode losConditionNode = dt.AddNode(dt.ChangeStateNode, dt.LOSNode, TreeNodeType.RIGHT_TREE_NODE);
        dt.treeNodeList.Add(losConditionNode);

        // Add MoveToPlayer action node
        TreeNode moveToPlayerNode = dt.AddNode(dt.LOSNode, new MoveToPlayerAction(), TreeNodeType.RIGHT_TREE_NODE);
        ((ActionNode)moveToPlayerNode).Agent = this.gameObject;
        dt.treeNodeList.Add(moveToPlayerNode);

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Destroy(gameObject);
            Game.Instance.SOMA.PlayMusic("defeat");
            SceneManager.LoadScene(2);
            //if (Game.Instance != null)
            //    Game.Instance.SOMA.PlaySound("Explode");
            
        }
    }
}
