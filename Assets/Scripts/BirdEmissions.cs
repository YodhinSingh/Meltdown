using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdEmissions : MonoBehaviour
{
    private float pos;
    private ParticleSystem birdNum;
    private float emissionNum;
    private float multiplier;

    // Start is called before the first frame update
    void Start()
    {
        pos = GetComponentInParent<Transform>().position.y;
        birdNum = GetComponent<ParticleSystem>();
        emissionNum = birdNum.emission.rateOverTime.constant;
        multiplier = 0.005f;
    }

    // Update is called once per frame
    void Update()
    {
        var a = birdNum.emission;
        float curPos = GetComponentInParent<Transform>().position.y;

        a.rateOverTime = emissionNum + ((curPos - pos)  * multiplier);

    }
}
