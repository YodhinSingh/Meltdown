using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class AriunoListener : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM3", 9600);
    public bool isHoldingJump;
    public Vector3 aim;

    float jumpChargeTime;
    float CurrentJumpCharge;
    //public int playerindex;
    //float oldDistance = 0;

    public float address;
    public float distance;


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
            //print(sp.ReadByte());
            //print("line of arduino: " + sp.ReadLine());

            if (sp.IsOpen)
            {
                try
                {
                    //store entire line from ardunio script and spilt it to get address and distance. Goat script will access these values and deal with calculations
                    string line = sp.ReadLine();
                    string[] values = line.Split(',');
                    address = float.Parse(values[0]);
                    distance = float.Parse(values[1]);

                    print("Goat address: " + address + " || Goat distance: " + distance);
                    
                    
                }
                catch (System.Exception)
                {

                }
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
