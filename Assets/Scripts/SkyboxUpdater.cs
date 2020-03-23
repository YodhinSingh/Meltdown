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
    // Start is called before the first frame update
    void Start()
    {
        canChange = true;
        doneChange = false;
        //ResetSkybox();
        t2 = t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (SceneManager.GetActiveScene().buildIndex == 1) // game scene
        {
            ChangeSkybox();
        }
        */
    }


    void ResetSkybox() // changes skybox settings based on scene;
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) // game scene
        {
            skyboxDay.SetFloat("_Exposure", 1f);
            skyboxNight.SetFloat("_Exposure", skyboxNightStartingValue);
            RenderSettings.skybox = skyboxDay;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0) // menu
        {
            skyboxDay.SetFloat("_Exposure", 1f);
            skyboxNight.SetFloat("_Exposure", 1f);
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
            ChangeSkybox();
            canChange = false;
        }
    }

}
