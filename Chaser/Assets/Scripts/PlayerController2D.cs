using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float plSpeed = 1;
    private Rigidbody2D player;
    private int horizontal, vertical;
    public static int health=100;
    //private Boolean crouch,block;
    Vector2 playerpos;

    GameObject enemy;
    public EnemyController2D enemyScript;
    public Rigidbody2D enemyRigidBody;
    //
    //
    //
    //  -------------------------------------Start------------------------------------------------------------
    //
    //
    //
    // Use this for initialization
    void Start () {
        player = GetComponent< Rigidbody2D>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyScript = enemy.GetComponent<EnemyController2D>();
        enemyRigidBody = enemy.GetComponent<Rigidbody2D>();
    }
    //
    //
    //
    //  --------------------------Update is called once per frame--------------------------------------------------
    //
    //
    //
    void FixedUpdate ()
    {
        //shorthanding for code position of player
        playerpos = player.position;
        movement();
        attack();
    }
    //
    //
    //
    //  --------------------------Method1: Movement--------------------------------------------------
    //
    //
    //
    private void movement()
    {
        horizontal = 0; vertical = 0;
        //movements
        //left
        if (Input.GetAxis("Horizontal") < 0)
        {
            horizontal = -1;
        }
        //right
        if (Input.GetAxis("Horizontal") > 0)
        {
            horizontal = +1;
        }
        //up
        if (Input.GetAxis("Vertical") > 0)
        {
            vertical = +1;
        }
        //down
        if (Input.GetAxis("Vertical") < 0)
        {
            vertical = -1;
        }
        player.MovePosition(new Vector2(playerpos.x + horizontal * plSpeed, playerpos.y + vertical * plSpeed));
    }
    private void attack()
    {
        //animation required
        //make sure enemy is within range of hit
        if (enemy!=null)
        if (Math.Abs(enemyRigidBody.position.x-transform.position.x) < 25 && Math.Abs(enemyRigidBody.position.y-transform.position.y) < 25)
        {
            if (Input.GetMouseButtonUp(0))
            {
                enemyScript.setHealth(-5);
                if (enemyScript.getHealth() <= 0)
                {
                    Destroy(enemy);
                    enemy = null;
                    enemyRigidBody = null;
                }
            }
        }
    }
    public void setHealth(int healthModifier)
    {
        health = health + healthModifier;
        Debug.Log("Player health = " + health);
    }
    public int getHealth()
    {
        return health;
    }
}
