using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float plSpeed = 1;
    private Rigidbody2D player;
    private int h, v;
    //private Boolean crouch,block;
    Vector2 playerpos;

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
        h = 0; v = 0;
        //movements
        //left
        if (Input.GetAxis("Horizontal") < 0)
        {
            h = -1;
        }
        //right
        if (Input.GetAxis("Horizontal") > 0)
        {
            h = +1;
        }
        //up
        if (Input.GetAxis("Vertical") > 0)
        {
            v = +1;
        }
        //down
        if (Input.GetAxis("Vertical") < 0)
        {
            v = -1;
        }
        player.MovePosition(new Vector2(playerpos.x + h * plSpeed, playerpos.y + v * plSpeed));
    }
}
