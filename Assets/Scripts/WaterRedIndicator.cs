using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterRedIndicator : MonoBehaviour
{
    public Transform water;
    public Transform Cam;

    float minDistance = 40f;
    float opacityLevel = 0f;

    float scalingFactor = 4f;

    public bool allow;


    CanvasGroup c;

    private void Start()
    {
        c = GetComponent<CanvasGroup>();
        allow = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (allow)
        {
            float camPosAdjusted = Cam.position.y + 10f;

            if (water.position.y <= camPosAdjusted - 32f)
            {
                if (camPosAdjusted - water.position.y <= minDistance)
                {
                    float min = camPosAdjusted - minDistance;
                    float max = camPosAdjusted;

                    //scaledValue = (rawValue - min) / (max - min);
                    float fraction = (water.position.y - min) / (max - min);

                    opacityLevel = Mathf.Clamp(Mathf.Lerp(0f, 1f, fraction) * scalingFactor, 0f, 1f);
                }
                else
                {
                    opacityLevel = 0;
                }
            }
            else
            {
                opacityLevel = 1;
            }


            //print(opacityLevel);
            c.alpha = opacityLevel;
        }
        else
        {
            c.alpha = 0f;
        }

    }

}
