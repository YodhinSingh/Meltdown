using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkyboxUpdater : MonoBehaviour
{
    public Material skyboxNight;
    public Material skyboxDay;
    bool canChange;
    bool doneChange;
    public float speed = 0.1f;
    float t;
    float t2;
    float skyboxNightStartingValue = 0.15f;

    public Color32 tintValueDayTarget = new Color32(38,0 ,209,255);
    public float exposureDayTarget = 0.33f;

    Color32 tintValueDayOriginal = new Color32(128, 128, 128, 255);
    float exposureDayOriginal = 1;

    public Color32 tintValueNightTarget = new Color32(38, 0, 209, 255);
    public float exposureNightTarget = 0.1f;

    Color32 tintValueNightOriginal = new Color32(128, 128, 128, 255);
    float exposureNightOriginal = 1;
    /*
     * Day:
     *  exp target: 0.33
     *  exp original: 1
     *  tint target: 2600D2
     *  tint original: 808080
     *  
     *  Night:
     *  exp target: 0.1
     *  exp original: 1
     *  tint target: 2600D2
     *  tint original: 808080
     *  
     */

    // Start is called before the first frame update
    void Start()
    {
        canChange = true;
        doneChange = false;
        ResetSkybox();
        t2 = t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!canChange) // game scene
        {
            ChangeSkyboxGradient2();
        }
    }


    void ResetSkybox() // changes skybox settings based on scene;
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) // game scene
        {
            skyboxDay.SetFloat("_Exposure", exposureDayOriginal);
            skyboxDay.SetColor("_TintColor", tintValueDayOriginal);
            skyboxNight.SetFloat("_Exposure", exposureNightTarget);
            RenderSettings.skybox = skyboxDay;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0) // menu
        {
            skyboxDay.SetFloat("_Exposure", exposureDayOriginal);
            skyboxDay.SetColor("_TintColor", tintValueDayOriginal);
            skyboxNight.SetFloat("_Exposure", exposureNightOriginal);
            RenderSettings.skybox = skyboxNight;
        }
        
    }

    void ChangeSkyboxGradient()
    {
        if (!canChange && !doneChange)
        {
            if (t <= 1)
            {
                t += speed * Time.deltaTime;
            }
            float newValue = Mathf.Lerp(1, 0, t);
            skyboxDay.SetFloat("_Exposure", newValue);

            if (newValue <= 0)
            {
                RenderSettings.skybox = skyboxNight;
                if (t2 <= 1)
                {
                    t2 += speed * Time.deltaTime;
                }
                float newValue2 = Mathf.Lerp(skyboxNightStartingValue, 1, t2);
                skyboxNight.SetFloat("_Exposure", newValue2);

                if (newValue2 >= 1)
                {
                    doneChange = true;
                }
            }
        }
    }

    void ChangeSkyboxGradient2()
    {
        if (!canChange && !doneChange)
        {
            if (t <= 1)
            {
                t += speed * Time.deltaTime;
            }
            float expValue = Mathf.Lerp(exposureDayOriginal, exposureDayTarget, t);
            Color tintValue = Color.Lerp(tintValueDayOriginal, tintValueDayTarget, t);
            skyboxDay.SetFloat("_Exposure", expValue);
            skyboxDay.SetColor("_TintColor", tintValue);

            if (expValue <= exposureDayTarget)
            {
                RenderSettings.skybox = skyboxNight;
                if (t2 <= 1)
                {
                    t2 += speed * Time.deltaTime;
                }
                float exp2Value = Mathf.Lerp(exposureNightTarget, exposureNightOriginal, t2);
                skyboxNight.SetFloat("_Exposure", exp2Value);

                if (exp2Value >= exposureNightOriginal)
                {
                    doneChange = true;
                    canChange = true;
                }

            }
        }
    }

    void ChangeSkybox()
    {
        if (!doneChange)
        {
            RenderSettings.skybox = skyboxNight;
            doneChange = true;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (canChange && col.gameObject.CompareTag("Player"))
        {
            //ChangeSkybox();
            canChange = false;
        }
    }

}
