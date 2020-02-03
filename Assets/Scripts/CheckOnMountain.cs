using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnMountain : MonoBehaviour
{
    private bool isOnMountain;
    private Ray aim;

    public GameObject mountain;
    // Start is called before the first frame update
    void Start()
    {
        isOnMountain = true;
        aim = new Ray(transform.position, new Vector3(0,0,50));
       
    }

    // Update is called once per frame
    void Update()
    {
        aim = new Ray(transform.position, new Vector3(0, 0, 50));
        Debug.DrawRay(aim.origin, aim.direction * 50, Color.blue);

        RaycastHit hit;

        if (Physics.Raycast(aim, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Mountain"))
            {
                isOnMountain = true;
            }
            else
                isOnMountain = false;

        }
        else
        {
            isOnMountain = false;
        }
        Debug.Log(isOnMountain);
    }

    public bool GetIsOnMountain()
    {
        return isOnMountain;
    }
}
