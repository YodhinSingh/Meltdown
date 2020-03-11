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
    public float angle;
    public bool isConnected;

    public List<GameObject> goats = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        if (sp != null && !sp.IsOpen)
        {
            sp.Open();
            sp.ReadTimeout = 25;
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
                    isConnected = true;
                    //store entire line from ardunio script and spilt it to get address and distance. Goat script will access these values and deal with calculations
                    //print("line of arduino: " + sp.ReadLine());
                    string line = sp.ReadLine();
                    string[] values = line.Split(',');
                    address = float.Parse(values[0]);
                    distance = float.Parse(values[1]);
                    angle = float.Parse(values[2]);

                    //print("Goat address: " + address + " || Goat distance: " + distance + " || Angle" + angle);


                }
                catch (System.Exception)
                {

                }
            }
        }

        else
        {
            isConnected = false;
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

    public void addGoat(GameObject goat)
    {
        goats.Add(goat);
    }
    public void RemoveGoat(GameObject goat)
    {
        goats.Remove(goat);
    }
}
