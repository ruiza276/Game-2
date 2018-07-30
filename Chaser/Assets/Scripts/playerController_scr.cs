using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController_scr : MonoBehaviour
{
    // Default move speed and modified move speed
    private float playerMoveSpeed = 2;  // Default
    private float moveSpeed;            // Modified assigned to this

    // Values for buffs and nerfs to player move speed
    public float playerMoveBuff;
    public float playerMoveNerf;

    // Speed changes for blocking/dodging
    private const float blockingSpeedMod = -0.7f;
    private const float dodgingSpeedMod = 2;

    // The times for dodge cool down and duration
    private const float dodgeCoolDownTime = 4;    // Default CD
    private const float dodgeDuration = 0.3f;     // Default Duration
    private float dodgeCoolDown = 0;        // Current CD Time
    private float dodgeTimer = 0;           // Current Duration Time

    // Player starting HP
    private int playerHealth = 100;

    // Creating the states for the player state machine
    public enum State
    {
        controlled,
        blocking,
        dodging,
    }
    State playerStates;

    // Values to assign player movement direction
    private int xMovement;
    private int yMovement;

    private Rigidbody2D rb2d;


    //*******************************
    //*                             *
    //* Module void Start()         *
    //* Use this for initialization *
    //*                             *
    //*******************************
    void Start ()
    {
        playerMoveSpeed /= 20;

        xMovement = 0;
        yMovement = 0;

        rb2d = GetComponent<Rigidbody2D>();

        playerStates = State.controlled;

        
	}


    //***********************************
    //*                                 *
    //* Module void Update()            *
    //* Update is called once per frame *
    //* ~Get input to see if player is  *
    //*  moving.                        *
    //* ~Go through switch and perform  *
    //*  the  propper actions for  the  *
    //*  current player state.          *
    //* ~Move player  to new location   *
    //*  relative to current cords and  *
    //*  taking move speed buff and     *
    //*  nerf into account.             *
    //*                                 *
    //***********************************
    void Update()
    {
        // Reset movement direction back to 0
        xMovement = 0;
        yMovement = 0;

        // Test to see if any movement input is being received
        if (Input.GetAxis("Horizontal") > 0)
        {
            xMovement = 1;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            xMovement = -1;
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            yMovement = 1;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            yMovement = -1;
        }

        // Decrement dodge CD if it is active
        if (dodgeCoolDown > 0)
        {
            dodgeCoolDown -= Time.deltaTime;
        }

        // Perform the propper actions for the current state
        switch (playerStates)
        {
            case State.controlled:
                // Test For Player Movement
                if (Input.anyKey)
                {
                    if (Input.GetAxis("Block") > 0)
                    {
                        // Change state to blocking
                        playerStates = State.blocking;

                        // Add move speed nerf
                        playerMoveNerf = blockingSpeedMod;
                    }
                    else if (Input.GetAxis("Dodge") > 0 && dodgeCoolDown <= 0)
                    {
                        // Change state to dodging
                        playerStates = State.dodging;

                        // Begin timer for dodge duration
                        dodgeTimer = dodgeDuration;

                        // Add move speed buff
                        playerMoveBuff = dodgingSpeedMod;
                    }
                }

                break;
            case State.blocking:
                if (Input.GetAxis("Block") <= 0)
                {
                    // Change state to controlled
                    playerStates = State.controlled;

                    // Remove move speed nerf
                    playerMoveNerf = 0;
                }
                else if (Input.GetAxis("Dodge") > 0 && dodgeCoolDown <= 0)
                {
                    // Change state to dodging
                    playerStates = State.dodging;

                    // Begin timer for dodge duration
                    dodgeTimer = dodgeDuration;

                    // Remove move speed nerf & add move speed buff
                    playerMoveNerf = 0;                 
                    playerMoveBuff = dodgingSpeedMod;
                }

                break;
            case State.dodging:
                if (dodgeTimer > 0)
                {
                    // dodgeTimer still running to decrement it
                    dodgeTimer -= Time.deltaTime;
                }
                else
                {
                    // dodgeTimer is done so begin dodgeCoolDown
                    dodgeCoolDown = dodgeCoolDownTime;

                    if (Input.GetAxis("Block") > 0)
                    {
                        // Change state to blocking
                        playerStates = State.blocking;

                        // Remove move speedbuff & add move speed nerf
                        playerMoveBuff = 0;
                        playerMoveNerf = blockingSpeedMod;
                    }
                    else
                    {
                        // Change state to controlled
                        playerStates = State.controlled;

                        // Remove move speed buff
                        playerMoveBuff = 0;
                    }
                }

                break;
        }

        // Set moveSpeed each step taking into account if there are
        // and move speed buffs or nerfs applied to the player
        float moveSpeed = playerMoveSpeed + (playerMoveSpeed * playerMoveBuff) + (playerMoveSpeed * playerMoveNerf);

        // Get the players current x and y coords
        float xPos = transform.position.x;
        float yPos = transform.position.y;

        // Use move speed, move direction, and current coords
        // to create a set of new coords to go to and move the
        // player  there
        Vector2 movement = new Vector2(xPos + (xMovement * moveSpeed), yPos + (yMovement * moveSpeed));
        transform.position = (movement);
    }

    
    //***************************************
    //*                                     *
    //* Module void ModifyPlayerHealth(int) *
    //* Pass this a value to modify player  *
    //* health by (positive or negative)    *
    //* since  it is aprivate variable      *
    //*                                     *
    //***************************************
    public void ModifyPlayerHealth(int mod)
    {
        playerHealth += mod;
    }


    //***********************************
    //*                                 *
    //* Function int GetPlayerHealth()  *
    //* Returns the players current     *
    //* health when called since  it is *
    //* private and can not be accessed *
    //* directly                        *
    //*                                 *
    //***********************************
    public int GetPlayerHealth()
    {
        return playerHealth;
    }
    
}



// IGNORE THIS!!!!!!!!!!!!!
/*if (Input.GetKey(KeyCode.W))
        {
            // Test for diagonal movement
            if (Input.GetKey(KeyCode.A))
            {
                // Move Up Left
                xMovement = -1;
                yMovement = 1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                // Move Up Right
                xMovement = 1;
                yMovement = 1;
            }
            else
            {
                // Move Up
                xMovement = 0;
                yMovement = 1;
            }

        }
        else if (Input.GetKey(KeyCode.S))
        {
            // Test for diagonal movement
            if (Input.GetKey(KeyCode.A))
            {
                // Move Down Left
                xMovement = -1;
                yMovement = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                // Move Down Right
                xMovement = 1;
                yMovement = -1;
            }
            else
            {
                // Move Down
                xMovement = 0;
                yMovement = -1;
            }

        }
        else if (Input.GetKey(KeyCode.A))
        {
            // Move Left
            xMovement = -1;
            yMovement = 0;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // Move Right
            xMovement = 1;
            yMovement = 0;
        }
        else
        {
            // No movement keys are being pressed so set x and y movement to 0
            xMovement = 0;
            yMovement = 0;
        }*/


