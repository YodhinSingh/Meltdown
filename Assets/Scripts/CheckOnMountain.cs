using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnMountain : MonoBehaviour
{
    private bool isOnMountain;
    private Ray aim;
    private Ray aim2;
    private float DistanceFromMountain;
    float maxDistanceAllowed = 50f;

    public GameObject mountain;

    // Start is called before the first frame update
    void Start()
    {
        isOnMountain = true;
        aim = new Ray(transform.position, new Vector3(0,0,50));
        aim2 = new Ray(transform.position, new Vector3(0, 0, -50));

    }

    // Update is called once per frame
    void Update()
    {
        aim = new Ray(transform.position, new Vector3(0, 0, 50));
        aim2 = new Ray(transform.position, new Vector3(0, 0, -50));
        Debug.DrawRay(aim.origin, aim.direction * maxDistanceAllowed, Color.blue);
        Debug.DrawRay(aim2.origin, aim2.direction * maxDistanceAllowed, Color.blue);

        RaycastHit hit;
        RaycastHit hit2;

        if (Physics.Raycast(aim, out hit, maxDistanceAllowed))
        {
            if (hit.transform.gameObject.CompareTag("Mountain"))
            {
                isOnMountain = true;
            }
            else
            {
                isOnMountain = false;
            }

        }
        else if (Physics.Raycast(aim, out hit2, maxDistanceAllowed))
        {
            if (hit.transform.gameObject.CompareTag("Mountain"))
            {
                isOnMountain = true;
            }
            else
            {
                isOnMountain = false;
            }
        }
        else
        {
            isOnMountain = false;
        }
        //Debug.Log(isOnMountain);
    }

    public bool GetIsOnMountain()
    {
        return isOnMountain;
    }

}
