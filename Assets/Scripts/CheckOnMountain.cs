using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnMountain : MonoBehaviour
{
    private bool isOnMountain;
    private Ray aim;
    private Ray aim2;
    private float DistanceFromMountain;
    private Vector3[] HitInfo = new Vector3[2];     // Will contain: [0] = hit point, [1] = hit normal
    float maxDistanceAllowed = 5.5f;

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
                DistanceFromMountain = hit.distance;
            }
            else
            {
                isOnMountain = false;
                DistanceFromMountain = -100;
            }
            HitInfo[0] = hit.point;
            HitInfo[1] = hit.normal;

        }
        else if (Physics.Raycast(aim, out hit2, maxDistanceAllowed))
        {
            if (hit.transform.gameObject.CompareTag("Mountain"))
            {
                isOnMountain = true;
                DistanceFromMountain = hit2.distance;
            }
            else
            {
                isOnMountain = false;
                DistanceFromMountain = -100;
            }
            HitInfo[0] = hit2.point;
            HitInfo[1] = hit2.normal;
        }
        else
        {
            isOnMountain = false;
            DistanceFromMountain = -100;
            HitInfo[0] = new Vector3(-100,-100,-100);
            HitInfo[1] = new Vector3(-100, -100, -100);
        }
        //Debug.Log(isOnMountain);
    }

    public bool GetIsOnMountain()
    {
        return isOnMountain;
    }

    public float GetDistanceFromMountain()
    {
        return DistanceFromMountain;
    }
    public Vector3[] GetHitInfo()
    {
        return HitInfo;
    }
}
