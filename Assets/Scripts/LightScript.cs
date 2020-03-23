using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    float originalIntensity;
    float newIntensity;
    public Light l;
    bool canChange;
    bool doneChange;
    public float speed = 0.1f;
    float t;

    // Start is called before the first frame update
    void Start()
    {
        originalIntensity = 0.5f;
        newIntensity = 0.25f;
        l.intensity = originalIntensity;
        canChange = true;
        doneChange = false;
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canChange && !doneChange)
        {
            if (t <= 1)
            {
                t += speed * Time.deltaTime;
            }
            float value = Mathf.Lerp(originalIntensity, newIntensity, t);
            l.intensity = value;

            if (value <= newIntensity)
            {
                doneChange = true;
                canChange = true;
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (canChange && col.gameObject.CompareTag("Player"))
        {
            canChange = false;
        }
    }
}
