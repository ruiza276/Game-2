using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController2D : MonoBehaviour
{
    //***********************************work on last seen counter and timer to go home********************************************************
    //************************************search for (animation required) to input animation code**********************************************
    //Enemy
    private enum state { idle, chase, attack };
    private state enemyState;
    private Rigidbody2D em;
    public int health = 50;

    //Player
    GameObject player;
    public PlayerController2D playerScript;
    public Rigidbody2D playerRigidBody;

    //Movement speed of this object
    public float speed = 1,attackTime, lastSeenTimer;

    //player entry and exiting a room variable(RoomTrigger is other script with collison entry and exit)
    private RoomTrigger trigger;
  
    // main objects
    
    //horizontal and vertical 
    private int horizontal, vertical,attackCounter=0, timeSinceAttack,lastSeenCounter;

    //entry and exit boolean
    private Boolean entry,atHome=true;
    
    //boolean for enemy sight on character
    private Boolean seen;

    // Vectors for positions
    Vector3 startpos, targetDir, newDir, targetpos, empos;

    //Health system
  
   
    //--------------------------------------------------------------//
    //  ----------------Used for initialization---------------------//
    //--------------------------------------------------------------//
    void Start()
    {
        enemyState = state.idle;
        //pulling rooms value of entry or exit
        player = GameObject.FindGameObjectWithTag("Player");
        trigger = GameObject.FindGameObjectWithTag("Room").GetComponent<RoomTrigger>();
        playerScript = player.GetComponent<PlayerController2D>();
        // grabbing players rigidbody 
        playerRigidBody = player.GetComponent<Rigidbody2D>();
        em = GetComponent<Rigidbody2D>();

        //Declares where to return to when not present
        startpos=new Vector3(em.position.x,em.position.y);
        Debug.Log(playerScript.getHealth());
    }
    //--------------------------------------------------------------//
    //--------------Update is called once per frame-----------------//
    //--------------------------------------------------------------//
    void Update()
    {//player enters room
        entry = trigger.e;
        if (entry)
        {
            //new positions
            empos = em.position;
            targetpos = playerRigidBody.position;
            // search method2: enemy sight
            statechanger();
            switch (enemyState)
            {
                case state.idle: if(atHome==false)gohome(); break;
                //if seen enemy will chase
                case state.chase: movement(); break;
                //if close enough, attacks, *****possibly add sight in order to not attack through walls****
                case state.attack: attack(); break;
                default: vertical = 0; break;
            }
        }
        else if ((entry == false) && (atHome == false))
        {
            //search method3: Go Home
            gohome();
        }
    }
    //--------------------------------------------------------------//
    //--------------method 1: player movement-----------------------//
    //--------------------------------------------------------------//
    void movement()
    {
        //resetting variables to readjust movement
        horizontal = 0; vertical = 0;
        //movements allowed within this range(go to far, stops, get to close stop to attack
        {
            attackCounter = 0;
            em.velocity = Vector3.zero;
            //moving enemy away from home;
            atHome = false;
            //right
            if ((Math.Round(empos.x - targetpos.x) < 0)&&(Math.Abs(empos.x-targetpos.x)>25))
            {
                horizontal = 1;
            }
            //left
            else if ((Math.Round(empos.x - targetpos.x) > 0)&& (Math.Abs(empos.x - targetpos.x) > 25))
            {
                horizontal = -1;
            }
            else
            {
                horizontal = 0;
            }
            //down
            if ((Math.Round(empos.y - targetpos.y) < 0) && (Math.Abs(empos.y - targetpos.y) > 25))
            {
                vertical = 1;
            }
            //up
            else if ((Math.Round(empos.y - targetpos.y) > 0) && (Math.Abs(empos.y - targetpos.y) > 25))
            {
                vertical = -1;
            }
            else
            {
                vertical = 0;
            }
            if ((Math.Abs(empos.x - startpos.x) > 400) && (Math.Abs(empos.y - startpos.y) > 400))
            {
            
            }
            //emspeed can be changed, but this sets targets position x,y
            em.MovePosition(new Vector3(empos.x + (horizontal * speed), empos.y + (vertical * speed)));
        }


    }
    //--------------------------------------------------------------//
    //--------------method 2: enemy sight---------------------------//
    //--------------------------------------------------------------//
    protected Boolean sight()
    {
        //linecast connects player and enemy and sees if things are inbetween
        seen = Physics.Linecast(empos, targetpos, -1 >> 8);
        //debuggers delight--
        //might switch to draw line just for kicks
        //or turn linecast into raycast hit

        //seen is originally true if item falls inbetween them. aka not seen, if then switches them around
        //or line of sight yes, but to far away to care
        if (seen || (seen ==false && (Math.Abs(empos.x - targetpos.x) > 150) || ((Math.Abs(empos.y - targetpos.y) > 150))))
        {
            seen = false;
            if (lastSeenCounter != 0)
            {
                lastSeenTimer = Time.realtimeSinceStartup;
                lastSeenCounter = 1;
            }
        }

        else
        {
            seen = true;
            lastSeenCounter = 0;
        }
        return seen;
        
    }
    //--------------------------------------------------------------//
    //--------------method 3: Go Home-------------------------------//
    //--------------------------------------------------------------//
    void gohome()
    {
        //grab timer to go home
        if ((Time.realtimeSinceStartup > trigger.timer + 3)||(Time.realtimeSinceStartup>lastSeenTimer+3))
        {
            //possible return animation for recall
            // animation required
            transform.position = startpos;
            atHome = true;
            enemyState = state.idle;
            lastSeenCounter = 0;
        }

    }
    //--------------------------------------------------------------//
    //--------------method 4: Collision Detection-------------------//
    //--------------------------------------------------------------//
    void collisionDetection()
    {

    }
    //--------------------------------------------------------------//
    //-------------------------method 5: attack---------------------//
    //--------------------------------------------------------------//
    void attack()
    {
        //two key variables are attackCounter and timeSinceAttack
        timeSinceAttack = (int)(Time.realtimeSinceStartup - attackTime);
        //attacks:attack1;attack2;attack3

        if (attackCounter == 0 && timeSinceAttack >= 0)
        {
            attackTime = Time.realtimeSinceStartup;
            attackCounter = 1;
        }
        else if (attackCounter == 1 && timeSinceAttack >= 2/** change to increase time of attack*/)
        {
            attackTime = Time.realtimeSinceStartup;
            attackCounter = 2;
            // animation required
            Debug.Log("attack1");
            playerScript.setHealth(-20);
        }
        else if (attackCounter == 2 && timeSinceAttack >= 1/** change to increase time of attack*/)
        {
            attackTime = Time.realtimeSinceStartup;
            attackCounter = 3;
            // animation required
            Debug.Log("attack2");
            playerScript.setHealth(-30);
        }
        else if (attackCounter == 3 && timeSinceAttack >= 1/** change to increase time of attack*/)
        {
            attackTime = Time.realtimeSinceStartup;
            attackCounter = 0;
            // animation required
            Debug.Log("attack3");
            playerScript.setHealth(-50);

        }

    }

    //--------------------------------------------------------------//
    //-------------------------method 6: state Changer--------------//
    //--------------------------------------------------------------//
    void statechanger()
    {
            //chasing state
            if ((((Math.Abs(empos.x - targetpos.x) > 25) || (Math.Abs(empos.y - targetpos.y) > 25)) && ((Math.Abs(empos.x - targetpos.x) < 150) && (Math.Abs(empos.y - targetpos.y) < 150)) && ((Math.Abs(empos.x - startpos.x) < 400) && (Math.Abs(empos.y - startpos.y) < 400))))
            {
                sight();
                if ((enemyState != state.chase)&& (seen))
                {
                    enemyState = state.chase;
                    // animation required
                    Debug.Log("State change chase");
                }
            }
            //attack state if getting to close
            else if ((Math.Abs(empos.x - targetpos.x) < 25) && (Math.Abs(empos.y - targetpos.y) < 25))
            {
                sight();
                if ((enemyState != state.attack) && seen)
                {
                    enemyState = state.attack;
                    // animation required
                    Debug.Log("State change attack");
                }
            }
        //idle state
        else if (((((Math.Abs(empos.x - targetpos.x) > 150) && (Math.Abs(empos.y - targetpos.y) > 150)) || ((Math.Abs(empos.x - startpos.x) > 400) && (Math.Abs(empos.y - startpos.y) > 400)))) && (enemyState != state.idle))
        {
            enemyState = state.idle;
            // animation required
            Debug.Log("State change idle");
        }
    }
    //--------------------------------------------------------------//
    //-------------------------method 7: health Modifier------------//
    //--------------------------------------------------------------//
    public void setHealth(int healthModifier)
    {
        health += healthModifier;
        Debug.Log("Enemy Health = " + health);
    }
    public int getHealth()
    {
        return health;
    }
}