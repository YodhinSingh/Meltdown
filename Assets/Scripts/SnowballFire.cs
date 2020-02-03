using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballFire : MonoBehaviour
{

    public GameObject Bullet_Emitter;
    public GameObject Bullet;
    public float Bullet_Forward_Force;


    GameObject Temporary_Bullet_Handler;
    private RaycastHit Ray_Cast_Collision_Data;

    Ray r;
    private bool allowShoot;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {/*

        if (allowShoot)
        {

            if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
            {
                RaycastHit hit;

                if (Physics.Raycast(r, out hit))
                {/*
                    //Debug.Log(hit.transform.gameObject.tag);

                    Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;

                    Rigidbody Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();

                    Temporary_RigidBody.AddForce(r.direction * Bullet_Forward_Force);

                    Temporary_RigidBody.useGravity = false;

                }


            }

        }*/
    }
}
