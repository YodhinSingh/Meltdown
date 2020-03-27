using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWaterTrigger : MonoBehaviour
{
    private bool hasCollided;

    public GameObject water;

    public WaterRedIndicator red;


    private void Start()
    {
        hasCollided = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasCollided)
        {
            water.GetComponent<InstantKillWater>().StartWaterRise();
            red.allow = true;
            hasCollided = true;
        }
    }



}
