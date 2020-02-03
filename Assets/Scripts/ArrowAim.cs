using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAim : MonoBehaviour
{
    Vector3 aimPoint;
    public GameObject goat;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = goat.GetComponent<GoatSlingShot>().GetAimPoint();
        //Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        //Vector3 dir = Input.mousePosition - pos;
        Vector3 dir = pos;
        dir.y = Mathf.Clamp(dir.y, 65, Screen.height);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
