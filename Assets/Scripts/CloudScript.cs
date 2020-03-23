using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    private bool hasCollided;

    ParticleSystem clouds;


    private void Start()
    {
        hasCollided = false;
        clouds = GetComponent<ParticleSystem>();
    }

    public void makeCloudsDisappear()
    {
        if (!hasCollided)
        {
            var a = clouds.emission;

            a.rateOverTime = 0f;
            hasCollided = true;
        }
    }

}
