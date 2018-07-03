using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController2D : MonoBehaviour
{
    //Movement speed of enemy(this object)
    public float speed = 1, counter;

    //entry and exiting a room
    private RoomTrigger trigger;
  
    // main objects
    private Rigidbody2D em, player;
    
    //random variables
    private int h, v;

    //entry and exit boolean
    private Boolean entry,goHome=true,atHome=true;
    
    //boolean for enemy sight on character
    private Boolean seen;

    // Vectors for positions
    Vector3 startpos, targetDir, newDir, targetpos, empos;

    //Enemy health
    int emhealth = 50;

    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //  ----------------Used for initialization---------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    void Start()
    {
        //pulling rooms value of entry or exit
        trigger = GameObject.FindGameObjectWithTag("Room").GetComponent<RoomTrigger>();

        // grabbing players rigidbody 
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        em = GetComponent<Rigidbody2D>();

        //Declares where to return to when not present
        startpos=new Vector3(em.position.x,em.position.y);
        
    }
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------Update is called once per frame-----------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    void Update()
    {//player enters room
        entry = trigger.e;
  
        if (entry)
        {
            //new positions
            empos = em.position;
            targetpos = player.position;

            // search method2: enemy sight
            //if seen enemy will chase
            if (sight()) { }
            else
            {

                // search method1: enemy movement
                movement();
            }
        }
        else if((entry==false)&&(atHome==false))
        {
            //search method3: Go Home
            gohome();
        }

    }
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------method1: player movement------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//


    void movement()
    {
        //resetting variables to readjust movement
        h = 0; v = 0;
        //movements allowed within this range(go to far, stops, get to close stop to attack
        if (((Math.Abs(empos.x - targetpos.x) > 15) || (Math.Abs(empos.y - targetpos.y) > 15)) && ((Math.Abs(empos.x - targetpos.x) < 150) && (Math.Abs(empos.y - targetpos.y) < 150)) && ((Math.Abs(empos.x - startpos.x) < 400) && (Math.Abs(empos.y - startpos.y) < 400)))
        {
            //moving enemy away from home;
            atHome = false;
            //right
            if ((Math.Round(empos.x - targetpos.x) < 0)&&(Math.Abs(empos.x-targetpos.x)>16))
            {
                h = 1;
            }
            //left
            else if ((Math.Round(empos.x - targetpos.x) > 0)&& (Math.Abs(empos.x - targetpos.x) > 16))
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            //down
            if ((Math.Round(empos.y - targetpos.y) > 0) && (Math.Abs(empos.y - targetpos.y) > 16))
            {
                v = -1;
            }
            //up
            else if ((Math.Round(empos.y - targetpos.y) < 0) && (Math.Abs(empos.y - targetpos.y) > 16))
            {
                v = +1;
            }
            else
            {
                v = 0;
            }
            //emspeed can be changed, but this sets targets position x,y
            em.MovePosition(new Vector3(empos.x + (h * speed), empos.y + (v * speed)));








        }
        if((Math.Abs(empos.x - startpos.x) > 400) && (Math.Abs(empos.y - startpos.y) > 400))
        {
            gohome();
        }
        if (((Math.Abs(empos.x - targetpos.x) < 17) || (Math.Abs(empos.y - targetpos.y) < 17)))
        {
            attack();
        }
    }

    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------method2: enemy sight----------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//

    protected Boolean sight()
    {
        //linecast connects player and enemy and sees if things are inbetween
        seen = Physics.Linecast(empos, targetpos, -1 >> 8);
        //debuggers delight--
        //might switch to draw line just for kicks
        //or turn linecast into raycast hit
        return seen;
    }

    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------method3: Go Home--------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//

    void gohome()
    {
        //sets timer to go home
        if (goHome)
        {   //counter sets timer for countdown to go home
            counter = Time.realtimeSinceStartup;
            //sets boolean false so doesnt save over counter next update
            goHome = false;
            Debug.Log(counter);
        }
        else
        {
            if (entry == false && (Time.realtimeSinceStartup > counter + 3))
            {
                transform.position = startpos;
                atHome = true;
                Debug.Log(Time.realtimeSinceStartup);
                goHome = true;
            }
        }
 
        //

    }

    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------method4: Collision Detection--------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    
    void collisionDetection()
    {

    }


    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //-------------------------method5: attack----------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//
    //--------------------------------------------------------------//

    void attack()
    {
        if (((Math.Abs(empos.x - targetpos.x) < 20) || (Math.Abs(empos.y - targetpos.y) <20)))
        {
            
        }
    }

}