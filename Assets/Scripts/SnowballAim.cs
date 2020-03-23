using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowballAim : MonoBehaviour
{
    public GameObject Snowball;
    public float Forward_Force;


    GameObject Temporary_Snowball_Handler;
    private RaycastHit Ray_Cast_Collision_Data;

    Ray r;
    private float allowShootTime;
    [SerializeField] float CoolingTime = 3f;

    public Image LoadingBar;
    public GameObject SnowballGraphic;

    public GameObject AimUI;

    private float BarLength;

    private bool hitMax;
    private float currentCount;
    private float currentAngle;
    private Vector2 vectorAngle;

    float tickValue = 0.75f;
    float forwardAimValue = 0.08f;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        allowShootTime = 0;    // timer for each snowball

        r = new Ray(transform.position, new Vector3(0, 1, forwardAimValue));  // the direction in which snowball will be thrown

        currentAngle = -1;
        //currentCount = Random.Range(25,145);
        if (index % 2 == 0)
            currentCount = 30;
        else
            currentCount = 150;
        vectorAngle = new Vector2();
        hitMax = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hitMax)
        {
            currentCount += tickValue;

            if (currentCount >= 150)
            {
                hitMax = true;
                currentAngle = tickValue;
            }
        }

        else if (hitMax)
        {
            currentCount -= tickValue;

            if (currentCount <= 30)
            {
                hitMax = false;
                currentAngle = -tickValue;
            }
        }

        vectorAngle = DegreeToVector2(currentCount + 180); 

        allowShootTime += Time.deltaTime;

        BarLength = Mathf.Min(allowShootTime / CoolingTime, 1); // figure out loading bar length for snowball recharge. Max value should be 1.

        if (BarLength < 1)     // Apply Changes to each bar
        {
            LoadingBar.GetComponent<Transform>().localScale = new Vector3(BarLength, 1, 1);
            SnowballGraphic.GetComponent<MeshRenderer>().enabled = false;
            AimUI.SetActive(false);
        }
        else if (BarLength == 1)   // If any bar is full (== 1) then just hide the loading bar and put graphic of snowball to show player its available
        {
            LoadingBar.GetComponent<Transform>().localScale = new Vector3(0, 1, 1);
            SnowballGraphic.GetComponent<MeshRenderer>().enabled = true;    // Note should replace this with graphic of snowball
            AimUI.SetActive(true);
        }

        //Update postion of ray as camera is constantly moving
        r = new Ray(transform.position, new Vector3(vectorAngle.x, vectorAngle.y, forwardAimValue));

        AimUI.GetComponent<LineRenderer>().SetPosition(1, r.direction * 150f);

        Debug.DrawRay(r.origin, r.direction * 10, Color.green); // This draws the rays in the scene view, its useful for debugging

    }

    public static Vector2 DegreeToVector2(float degree)
    {
        float radian = degree * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radian), -Mathf.Sin(radian)).normalized;
    }

    public bool InstantiateSnowBall()
    {
        if (allowShootTime >= CoolingTime)
        {
            //Debug.Log(hit.transform.gameObject.tag);

            Temporary_Snowball_Handler = Instantiate(Snowball, transform.position, transform.rotation) as GameObject;
            allowShootTime = 0f;

            Temporary_Snowball_Handler.GetComponent<CalculateZValue>().trail.emitting = true;
            Temporary_Snowball_Handler.GetComponent<CalculateZValue>().particles.Play();

            Rigidbody Temporary_RigidBody = Temporary_Snowball_Handler.GetComponent<Rigidbody>();


            Temporary_RigidBody.AddForce(r.direction * Forward_Force * 2);
            Destroy(Temporary_Snowball_Handler, 4f);
            return true;
        }
        return false;
    }


    private void OnCollisionEnter(Collision col)
    {
        Destroy(Temporary_Snowball_Handler);
    }
}
