using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public enum CharacterState
{ 
    Idling, 
    Running, 
    Rolling, 
    Jumping
}


public class PlayerScript : MonoBehaviour, IReceiver
{
    [SerializeField] private Transform groundDetect;
    [SerializeField] private bool isGrounded; // Just so we can see in Editor.
    [SerializeField] private float moveForce;
    [SerializeField] private float jumpForce;
    [SerializeField] GameObject sonic;
    public LayerMask groundLayer;
    private float groundCheckWidth = 2f;
    private float groundCheckHeight = 0.25f;
    private Animator an;
    private Rigidbody2D rb;
    private CapsuleCollider2D cc;
    private float invulnerabilityTimer;
    private float deathTimer;
    public bool isInvulnerable;
    private bool isDead;
    int playerHitPoints;

    

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }

    private CharacterState currentState;
    private bool jumpStarted;

    public List<IOobserver> Observers { get; private set; }
    
    void Start()
    {
        an = GetComponentInChildren<Animator>();
        isGrounded = false; // Always start in air.
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();

        Observers = new List<IOobserver>();
        this.AddObserver(new AchievmentObserver());

        playerHitPoints = 3;
        invulnerabilityTimer = 0f;
        isInvulnerable = false;
        isDead = false;

    }

    void Update()
    {
        if(!jumpStarted)
        {
            GroundedCheck();
        }

        if (isInvulnerable)
        {
            invulnerabilityTimer += Time.deltaTime;
            if (invulnerabilityTimer >= 10f)
            {
                Color colour = sonic.GetComponent<SpriteRenderer>().color;
                colour.a = 1f;
                sonic.GetComponent<SpriteRenderer>().color = colour;
                isInvulnerable = false;
            }
        }

        CheckHitPoints();

        Debug.Log(currentState);
        switch(currentState)
        {
            case CharacterState.Idling:
                HandleIdlingState(); 
                break;
            case CharacterState.Running:
                HandleRunningState();
                break;
            case CharacterState.Rolling:
                HandleRollingState();
                break;
            case CharacterState.Jumping:
                HandleJumpingState();
                break;
            

        }
    }

    private void GroundedCheck()
    {
        isGrounded = Physics2D.OverlapBox(groundDetect.position, 
            new Vector2(groundCheckWidth, groundCheckHeight), 0f, groundLayer);
        an.SetBool("isJumping", !isGrounded);
    }

    private void HandleJumpingState()
    {
        //state logic
        MoveCharacter();
        //transitions 
        if (isGrounded)
        {
            this.NotifyObservers(Event.ClassDismissed);
            an.SetBool("isJumping", false);
            currentState = CharacterState.Idling;
        }
    }

    private void HandleRollingState()
    {
        //state logic
        MoveCharacter();
        //transitions 
        if (Input.GetKeyUp(KeyCode.S))
        {
            
            cc.offset = new Vector2(0.33f, -0.25f);
            cc.size = new Vector2(2f, 3.5f);
            Game.Instance.SOMA.StopLoopedSound();

            an.SetBool("isRolling", false);
            currentState = CharacterState.Idling;
        }
    }

    private void HandleRunningState()
    {
        //state logic
        MoveCharacter();
        //transitions 
        if(isGrounded && (Input.GetAxis("Horizontal") == 0))
        {
            an.SetBool("isMoving", false);
            currentState = CharacterState.Idling;
        }
        else if (isGrounded && Input.GetKeyDown(KeyCode.S))
        {
            cc.offset = new Vector2(0.33f, -1f);
            cc.size = new Vector2(2f, 2f);
            Game.Instance.SOMA.PlayLoopedSound("Roll");
            an.SetBool("isMoving", false);

            an.SetBool("isRolling", true);
            currentState = CharacterState.Rolling;
            this.NotifyObservers(Event.FiveRolls);
        }
        else if (isGrounded && Input.GetButtonDown("Jump"))
        {
            this.NotifyObservers(Event.FiveRolls);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            Game.Instance.SOMA.PlaySound("Jump");
            an.SetBool("isMoving", false);
            StartJump();
        }
    }

    private void HandleIdlingState()
    {
        //state logic
        if (isDead == false)
            transform.Translate(-4f * Time.deltaTime, 0f, 0f);
        //transitions 
        if (isGrounded && (Input.GetAxis("Horizontal") != 0))
        {
            an.SetBool("isMoving", true);
            currentState = CharacterState.Running;
        }
        else if (isGrounded && Input.GetButtonDown("Jump"))
        {
            this.NotifyObservers(Event.FiveRolls);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            Game.Instance.SOMA.PlaySound("Jump");

            StartJump();
        }
        else if (isGrounded && Input.GetKeyDown(KeyCode.S))
        {
            this.NotifyObservers(Event.FiveRolls);
            cc.offset = new Vector2(0.33f, -1f);
            cc.size = new Vector2(2f, 2f);
            Game.Instance.SOMA.PlayLoopedSound("Roll");

            an.SetBool("isRolling", true);
            currentState = CharacterState.Rolling;

        }
        
    }

    private void MoveCharacter()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveForce * Time.fixedDeltaTime, rb.velocity.y);
    }

    private void StartJump()
    {
        jumpStarted = true;
        isGrounded = false;

        an.SetBool("isJumping", true);
        currentState = CharacterState.Jumping;

        Invoke("ResetJumpStarted", 0.5f);
        this.NotifyObservers(Event.Playerjumped);
    }

    private void ResetJumpStarted()
    {
        jumpStarted = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            if (playerHitPoints > 0)
                playerHitPoints--;

            Debug.Log("hitpoints left: " + playerHitPoints);

            if (playerHitPoints > 0)
                Invulnerable();
        }
    }

    private void Invulnerable()
    {
        Color colour = sonic.GetComponent<SpriteRenderer>().color;
        colour.a = 0.25f;
        sonic.GetComponent<SpriteRenderer>().color = colour;
        GameObject obstacleManagerObject = GameObject.Find("ObstacleManager");
        if (obstacleManagerObject != null)
        {
            ObstacleManager obstacleManager = obstacleManagerObject.GetComponent<ObstacleManager>();
            obstacleManager.SetObstaclesToTrigger();
            
        }
        invulnerabilityTimer = 0f;
        isInvulnerable = true;
    }

    private void CheckHitPoints()
    {
        if (playerHitPoints == 0)
        {
            Game.Instance.SOMA.PlayMusic("Death");
            isDead = true;
            an.SetBool("isDead", true);
            Debug.Log("Player is dead");
            deathTimer += Time.deltaTime;
            Debug.Log(deathTimer);
            if (deathTimer >= 5f)
            {
                Game.Instance.SOMA.PlayMusic("Adventure");
                SceneManager.LoadScene(2);
            }

            GameObject backgroundManagerObject = GameObject.Find("BackgroundManager");
            if (backgroundManagerObject != null)
            {
                BackgroundManager backgroundManager = backgroundManagerObject.GetComponent<BackgroundManager>();
                backgroundManager.StopBackgroundMovement();  
            }

            GameObject obstacleManagerObject = GameObject.Find("ObstacleManager");
            if (obstacleManagerObject != null)
            {
                ObstacleManager obstacleManager = obstacleManagerObject.GetComponent<ObstacleManager>();
                obstacleManager.StopObstacleMovement();
                rb.constraints = RigidbodyConstraints2D.FreezePosition;
            }
        }
    }

    public int GetPlayerHitPoints()
    {
        return playerHitPoints;
    }
}
