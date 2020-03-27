using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockColour : MonoBehaviour
{

    Transform pos;
    float fraction;
    float highestPoint = 351.2f;

    public Material Rock1;
    public Material Rock2;

    Color32 Rock1C;
    Color32 Rock2C;

    // Start is called before the first frame update
    private void Awake()
    {
        pos = GetComponent<Transform>();
        fraction = 0;
        Rock1C = new Color32(115, 115, 115, 255);
        Rock2C = new Color32(108, 99, 86, 255);
        /*
         * HSV values
         * 1: 0 0 45 100
         * 2: 35 20 42 100 
         * note for some reason hue which should be 35, when set in script HSVToRGB does not make it 35. Through experimenting
         * I got the current value to put in 9
        */
        Rock1.color = Rock1C;
        Rock2.color = Rock2C;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (fraction <= 1 && fraction >= 0)
        {
            float fraction = pos.position.y / highestPoint;

            // 115 - 243 RGB, 45 - 98 HSV
            //varies - 191 RGB, 42 - 95 HSV

            float V1 = Mathf.Lerp(0.45f, 0.98f, fraction);
            float V2 = Mathf.Lerp(0.42f, 0.95f, fraction);

            Rock1.color = Color.HSVToRGB(0, 0, V1);
            Rock2.color = Color.HSVToRGB(0.09f, 0.20f, V2);
        }
    }

    private void OnDisable()
    {
        Rock1.color = Rock1C;
        Rock2.color = Rock2C;
    }
}
