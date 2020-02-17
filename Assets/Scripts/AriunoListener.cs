using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class AriunoListener : MonoBehaviour
{
    SerialPort sp; //= new SerialPort("COM3", 9600);
    public bool isHoldingJump;
    public Vector3 aim;

    float jumpChargeTime;
    float CurrentJumpCharge;

    // Start is called before the first frame update
    void Start()
    {
        if (sp != null)
        {
            sp.Open();
            sp.ReadTimeout = 1;
        }
        jumpChargeTime = Random.Range(0.1f, 1f);    // these 3 variables are random and are used instead of arduino since no access to it right now
        isHoldingJump = false;
        aim = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        jumpChargeTime -= Time.deltaTime;
        if (sp != null)
        {
            print(sp.ReadByte());

            if (sp.IsOpen)
            {
                try
                {
                    //note that as of yet we dont know how to get the input numbers, i think its readbyte(), but not sure and cant test
                    int value = sp.ReadByte();
                    if (value > 0)
                    {
                        isHoldingJump = true;   //Set the value to true or false. The goatslingshot script is already accessing this value there.
                    }
                    else
                    {
                        isHoldingJump = false;
                    }
                }
                catch (System.Exception)
                {

                }
            }
            else
            {
                isHoldingJump = false;
            }
        }

        else
        {
            if (jumpChargeTime > 0)         //Since no acess to arduino stuff, creating fake random numbers to test
            {
                isHoldingJump = true;   
            }
            else
            {
                aim = new Vector3(Random.Range(-1f, 1f), Random.Range(-1.1f, 0f), 0);
                isHoldingJump = false;
                jumpChargeTime = Random.Range(0.1f, 1f);
            }
        }
    }
}
