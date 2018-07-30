using UnityEngine;
using System.Collections;
using System;

public class RoomTrigger : MonoBehaviour
{
    public Boolean e=false;
    public float timer;
    EnemyController2D em;


        void OnTriggerEnter2D(Collider2D collision)
        {
            if (GameObject.Find("Player").GetComponent<CircleCollider2D>().attachedRigidbody)
            {
                setTrigger(true);
                setTimer(0);
            }
        }
        void OnTriggerExit2D(Collider2D collision)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>().attachedRigidbody)
            {
                setTrigger(false);
                setTimer(Time.realtimeSinceStartup);
            }
        }
            void setTrigger(Boolean a)
            {
                e = a;
            }

            void setTimer(float a)
            {
                timer = a;
            }


}