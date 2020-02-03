using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GoatSlingShot : MonoBehaviour
{
    private bool HoldingJump;
    private Vector3 AimInput;

    private bool onGround;
    private float jumpPressure;
    private float minJumpValue;
    private float maxJumpPressure;
    private float maxfallSpeed;
    private bool PlayerControlActive;

    private Rigidbody rb;
    private Vector3 offset;
    private bool isCreated;

    private LineRenderer VisibleAimLine;

    public GameObject arrow;
    private float arrowColourR;
    private float arrowColourG;

    public GameObject ground;
    private bool isOnMountain;

    public float jumpPower = 1f;
    public Vector3 aimPoint;

    public int playerIndex;



    // Start is called before the first frame update
    void Start()
    {
        onGround = false;
        jumpPressure = 0f;
        minJumpValue = 5f;
        maxJumpPressure = 5f;
        maxfallSpeed = -1f;
        rb = GetComponent<Rigidbody>();
        offset = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        isCreated = true;
        PlayerControlActive = true;
        isOnMountain = true;
        arrowColourR = 1f;
        arrowColourG = 0f;
        VisibleAimLine = GetComponent<LineRenderer>();

        Camera.main.GetComponent<CameraFollow>().addGoat(this.gameObject);

        
    }

    // Update is called once per frame
    void Update()
    {
        offset = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);    // where to place platform below goat
        CheckOnMountain();

        if (onGround && PlayerControlActive)
        {
            //Vector3 target = new Vector3(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2, Input.mousePosition.z);
            Vector3 target = new Vector3(AimInput.x, AimInput.y, 0) * 50 * Time.deltaTime;
            aimPoint = target;
            Ray aim = new Ray(transform.position, target);
            VisibleAimLine.SetPosition(1, aim.direction * -0.5f * (jumpPressure + 3f));
            Debug.DrawRay(aim.origin, aim.direction * 50, Color.red);

            // Holding the jump charge
            if (HoldingJump)
            {
                if (jumpPressure < maxJumpPressure)
                {
                    jumpPressure += Time.deltaTime * 20f;

                }
                else
                {
                    jumpPressure = maxJumpPressure;
                }
                arrowColourR -= Time.deltaTime * 3f;
                arrowColourG += Time.deltaTime * 3f;
            }

            // Not holding the jump charge anymore
            else
            {
                if (jumpPressure > 0f)
                {
                    jumpPressure += minJumpValue;
                    arrowColourR -= 0.01f;
                    arrowColourG += 0.01f;
                }

                // Apply jump force to rigidbody and reset jump value
                Vector3 jumpDir = new Vector3(aim.direction.x/1.5f, aim.direction.y, aim.direction.z);
                rb.AddForce(jumpDir * jumpPressure * jumpPower);
                arrowColourR = 1f;
                arrowColourG = 0f;
                jumpPressure = 0f;
                isCreated = false;
                

            }
        }
        //Debug.Log(isCreated + " ||" + isOnMountain);
        if (!isCreated && isOnMountain) // if the goat is still on the mountain, then create a platform for it to stand on (if no platform already)
            CreateGround();
        if (!isOnMountain)
            DestroyGround();
        arrow.GetComponent<Image>().color = new Color(arrowColourR, arrowColourG, 0); // change colour of arrow based on how long jump is pressed
        VisibleAimLine.endColor = new Color(arrowColourR, arrowColourG, 0);
    }



    private void OnJump()
    {
        HoldingJump = true;
    }

    private void OnReleaseJump()
    {
        HoldingJump = false;
    }

    private void OnAim(InputValue value)
    {
        if (value.Get<Vector2>() != new Vector2())
            AimInput = value.Get<Vector2>();
    }

    public Vector3 GetAimPoint()
    {
        return aimPoint;
    }

    public void DisablePlayerControl(bool allow)
    {
        if (allow)
            PlayerControlActive = false;
        else
            PlayerControlActive = true;
    }

    private void CheckOnMountain()  // gets result of if the goat is on mountain from other script
    {
        isOnMountain = GetComponentInChildren<CheckOnMountain>().GetIsOnMountain();
    }


    private void CreateGround() // create platform below goat if its velocity reaches maximum fall speed and if its on the mountain
    {
        if (rb.velocity.y < maxfallSpeed)
        {
            AddGround();
        }
    }

    private void DestroyGround()
    {
        //Debug.Log("destroy");
        ground.transform.position = new Vector3();
        isCreated = false;
        //onGround = false;
    }

    public void AddGround() // Add final platform for goat once the player reaches top of mountain
    {
        ground.transform.position = offset;
        isCreated = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        onGround = true;
        isCreated = true;
        
        if (collision.gameObject.tag == "Platform")
        {
            Vector3 dir = collision.GetContact(0).point - transform.position;
            dir = -dir.normalized;
            // This will push back the player
            GetComponent<Rigidbody>().AddForce(dir * 100f);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
        isCreated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            other.GetComponent<InstantKillWater>().SetGameOver();
            DisablePlayerControl(true);
        }
    }


    private void OnBecameInvisible()
    {
        //DisablePlayerControl(true);
        Debug.Log("this player lost");
    }
}
