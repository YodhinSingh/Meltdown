using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class AriunoListener : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM3", 9600);
    public bool isHoldingJump;
    
    // Start is called before the first frame update
    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 1;
        isHoldingJump = false;
    }

    // Update is called once per frame
    void Update()
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
}
