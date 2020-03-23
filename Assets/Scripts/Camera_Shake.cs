using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Shake : MonoBehaviour
{
    float count;
    public bool allow;
    bool ver;
    bool cooldownWait;
    float oldZVal;

    private void Start()
    {
        count = 60f;
        ver = false;
        cooldownWait = true;
        oldZVal = 0f;
    }

    public void Update()
    {

        if (count > 0f && allow && cooldownWait)
        {
            count -= Time.deltaTime * 60f;
            float valZ = Random.Range(0f, 1f);
            float valX = Random.Range(0f, 1f);
            switch (ver)
            {
                case true:
                    //transform.Rotate(0, 0, Random.Range(0f, 1f));
                    transform.rotation = Quaternion.Euler(valX, 0, valZ);
                    break;
                case false:
                    //transform.Rotate(0, 0, Random.Range(-1f, 0f));
                    transform.rotation = Quaternion.Euler(-valX, 0, -valZ);
                    break;
            }
            ver = !ver;
            


        }
        if (count <= 0f && allow && cooldownWait)
        {
            oldZVal = transform.rotation.z;
            allow = false;
            cooldownWait = false;
            StartCoroutine("cooldown");
        }
        if (!allow)
        {
            count = 60f;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }




        /*
        transform.Rotate(Random.Range(-1f, 1f), Random.Range(-0.2f, 0.2f), 0);
        
        
        if (count > 59)
        {
            this.enabled = false;
        }

        count++;
        */
    }

    private IEnumerator cooldown()
    {
        yield return new WaitForSeconds(0.25f);
        cooldownWait = true;
    }
}
