using UnityEngine;
using System.Collections;
using System;

public class RoomTrigger : MonoBehaviour
{
    public Boolean e=false;
    EnemyController2D em;


        void OnTriggerEnter2D(Collider2D collision)
        {
            if (GameObject.Find("Player").GetComponent<CircleCollider2D>().attachedRigidbody)
            {
                setTrigger(true);
        }
        }
            void OnTriggerExit2D(Collider2D collision)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>().attachedRigidbody)
            {
                setTrigger(false);
            }
        if (GameObject.FindGameObjectWithTag("Enemy").GetComponent<CircleCollider2D>().attachedRigidbody)
        {
            em = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController2D>();

        }
    }
        //player speed setting
            void setTrigger(Boolean a)
        {
            e = a;
        }
    

}