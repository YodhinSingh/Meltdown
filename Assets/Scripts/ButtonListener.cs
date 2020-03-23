using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ButtonListener : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM3", 9600);   // replace with button info *****

    public int buttonNum = 0;
    public bool isPressed = false;

    // Start is called before the first frame update
    void Start()
    {
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

    }

    // Update is called once per frame
    void Update()
    {
        if (sp != null)
        {
            //print(sp.ReadByte());
            //print("line of arduino: " + sp.ReadLine());

            if (sp.IsOpen)
            {
                try
                {
                    //store entire line from ardunio script and spilt it to get button number and on/off. SnowballFire script will access these values and deal with calculations
                    string line = sp.ReadLine();
                    //print("line of arduino: " + line);
                    string[] values = line.Split(',');
                    buttonNum = int.Parse(values[0]);
                    int pressNum = int.Parse(values[1]);

                    /* If 1 is equal to pressed, leave this statement as is. If 1 is equal to not pressed, then switch true and false *****
                    to become >>>  ...? false : true;
                    This is the only line needing to be changed (also the port info at top), everything else will work on its own. 
                    Just make sure the button numbers are 1 - 6 on the arduino script */
                    isPressed = (pressNum == 1) ? true: false;


                    print("Button Num: " + buttonNum + " || has been pressed: " + isPressed );


                }
                catch (System.Exception)
                {

                }
            }
        }
    }
}
