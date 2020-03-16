using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.SceneManagement;

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

    public GameObject goatTemplate;
    float[] oldDistances = new float[8];
    bool isReBorn;

    // Start is called before the first frame update
    void Start()
    {
        isReBorn = false;

        try
        {
            if (sp != null && !sp.IsOpen)
            {
                sp.Open();
                sp.ReadTimeout = 25;
            }
        }
        catch (System.Exception)
        {
            sp = null;
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
                    isActivated(address, distance);
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

        /*if (Time.timeSinceLevelLoad > 1){
            ReSummonAuto();
        }*/
    }

    void ReSummonAuto()
    {
        if (!isReBorn && SceneManager.GetActiveScene().buildIndex == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                float a = 41 + i;
                float d = 1000 * i + 1000;
                isActivated(a, d);
            }
            isReBorn = true;
        }
        if (isReBorn && SceneManager.GetActiveScene().buildIndex == 1)
        {
            for (int i = 0; i < oldDistances.Length; i++)
            {
                oldDistances[i] = 0;
            }
            isReBorn = false;
        }
    }

    void instantiateGoat()
    {
        if (isConnected || true)
        {
            Instantiate(goatTemplate, goatTemplate.transform.position, goatTemplate.transform.rotation);
            //GetComponentInParent<PlayerInstanceGenerator>().WantToStartGame();
        }
    }


    bool isActivated(float address, float distance)
    {
        float minDistanceChange = 15f;
        int realAddress = (int) (address % 10) -1;

        if (distance > oldDistances[realAddress] && (distance - oldDistances[realAddress] > minDistanceChange))
        {
            instantiateGoat();
        }
        else if (distance <= oldDistances[realAddress] && (oldDistances[realAddress] - distance > minDistanceChange))
        {

        }
        oldDistances[realAddress] = distance;
        return false;
    }




}
