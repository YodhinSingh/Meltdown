using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowballFire : MonoBehaviour
{

    public GameObject[] Emitters = new GameObject[3];
    public GameObject Snowball;
    public float Forward_Force;


    GameObject Temporary_Snowball_Handler;
    private RaycastHit Ray_Cast_Collision_Data;

    Ray r1, r2, r3;
    private float allowShootTime1, allowShootTime2, allowShootTime3;
    [SerializeField] float CoolingTime = 3f;

    public Image[] LoadingBars = new Image[3];
    public GameObject[] SnowballGraphics = new GameObject[3];
    public GameObject WinMenuUI;
    public GameObject LoseMenuUI;
    public GameObject AimUI1;
    public GameObject AimUI2;
    public GameObject AimUI3;

    private float Bar0Length;
    private float Bar1Length;
    private float Bar2Length;
    
    private bool hitMax;
    public float currentCount;
    public float currentAngle;
    public float vectorAngle;

    float tickValue = 0.75f;

    // Use this for initialization
    void Start()
    {
        allowShootTime1 = 0;    // timer for each snowball
        allowShootTime2 = 0;
        allowShootTime3 = 0;
        r1 = new Ray(Emitters[0].transform.position, new Vector3(0, 1, 0));  // the direction in which snowball will be thrown
        r2 = new Ray(Emitters[1].transform.position, new Vector3(0, 1, 0)); // for aiming should use rays as well since they are useful
        r3 = new Ray(Emitters[2].transform.position, new Vector3(0, 1, 0));

        currentAngle = -1;
        currentCount = 30;
        vectorAngle = -5f;
        hitMax = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hitMax)
        {
            currentCount+= tickValue;

            if (currentCount == 150)
            {
                hitMax = true;
                currentAngle = tickValue;
            }
        }

        else if (hitMax)
        {
            currentCount-= tickValue;

            if (currentCount == 30)
            {
                hitMax = false;
                currentAngle = -tickValue;
            }
        }

        vectorAngle = ((currentCount - 30)-60)/30f;

        //Mathf.PingPong(currentCount + 30, 150f);

        allowShootTime1 += Time.deltaTime;
        allowShootTime2 += Time.deltaTime;
        allowShootTime3 += Time.deltaTime;

        Bar0Length = Mathf.Min(allowShootTime1 / CoolingTime, 1); // figure out loading bar length for snowball recharge. Max value should be 1.
        Bar1Length = Mathf.Min(allowShootTime2 / CoolingTime, 1);
        Bar2Length = Mathf.Min(allowShootTime3 / CoolingTime, 1);

        // There is a shorter way to write this bar code, but busy right now, Ill do it later
        if (Bar0Length < 1)     // Apply Changes to each bar
        {
            LoadingBars[0].GetComponent<Transform>().localScale = new Vector3(Bar0Length, 1, 1);
            SnowballGraphics[0].GetComponent<MeshRenderer>().enabled = false;
            AimUI1.SetActive(false);
        }
        else if (Bar0Length == 1)   // If any bar is full (== 1) then just hide the loading bar and put graphic of snowball to show player its available
        {
            LoadingBars[0].GetComponent<Transform>().localScale = new Vector3(0, 1, 1);
            SnowballGraphics[0].GetComponent<MeshRenderer>().enabled = true;    // Note should replace this with graphic of snowball
            AimUI1.SetActive(true);
        }

        if (Bar1Length < 1)
        {
            LoadingBars[1].GetComponent<Transform>().localScale = new Vector3(Bar1Length, 1, 1);
            SnowballGraphics[1].GetComponent<MeshRenderer>().enabled = false;
            AimUI2.SetActive(false);
        }
        else if (Bar1Length == 1)
        {
            LoadingBars[1].GetComponent<Transform>().localScale = new Vector3(0, 1, 1);
            SnowballGraphics[1].GetComponent<MeshRenderer>().enabled = true;
            AimUI2.SetActive(true);
        }

        if (Bar2Length < 1)
        {
            LoadingBars[2].GetComponent<Transform>().localScale = new Vector3(Bar2Length, 1, 1);
            SnowballGraphics[2].GetComponent<MeshRenderer>().enabled = false;
            AimUI3.SetActive(false);
        }
        else if (Bar2Length == 1)
        {
            LoadingBars[2].GetComponent<Transform>().localScale = new Vector3(0, 1, 1);
            SnowballGraphics[2].GetComponent<MeshRenderer>().enabled = true;
            AimUI3.SetActive(true);
        }


        //Update postion of ray as camera is constantly moving
        r1 = new Ray(Emitters[0].transform.position, new Vector3(vectorAngle, 1, 0));
        r2 = new Ray(Emitters[1].transform.position, new Vector3(vectorAngle, 1, 0));
        r3 = new Ray(Emitters[2].transform.position, new Vector3(vectorAngle, 1, 0));

        AimUI1.GetComponent<LineRenderer>().SetPosition(1, r1.direction* 150f);
        AimUI2.GetComponent<LineRenderer>().SetPosition(1, r2.direction * 150f);
        AimUI3.GetComponent<LineRenderer>().SetPosition(1, r3.direction * 150f);

        Debug.DrawRay(r1.origin, r1.direction*10, Color.green); // This draws the rays in the scene view, its useful for debugging
        Debug.DrawRay(r2.origin, r2.direction*10, Color.green);
        Debug.DrawRay(r3.origin, r3.direction*10, Color.green);

        // As long as the win menu or lose menu are not active/null, then keep reloading and throwing snowballs
        if ((WinMenuUI != null && !WinMenuUI.activeInHierarchy) && (LoseMenuUI != null && !LoseMenuUI.activeInHierarchy))
        {
            if (allowShootTime1 >= CoolingTime && Input.GetKeyDown(KeyCode.A))
            {
                InstantiateSnowBall(0, r1);
                allowShootTime1 = 0f;
            }
            if (allowShootTime2 >= CoolingTime && Input.GetKeyDown(KeyCode.S))
            {
                InstantiateSnowBall(1, r2);
                allowShootTime2 = 0f;
            }
            if (allowShootTime3 >= CoolingTime && Input.GetKeyDown(KeyCode.D))
            {
                InstantiateSnowBall(2, r3);
                allowShootTime3 = 0f;
            }
        }
    }

    private void InstantiateSnowBall(int emmiterNum, Ray r)
    {
        //Debug.Log(hit.transform.gameObject.tag);

        Temporary_Snowball_Handler = Instantiate(Snowball, Emitters[emmiterNum].transform.position, Emitters[emmiterNum].transform.rotation) as GameObject;

        Rigidbody Temporary_RigidBody = Temporary_Snowball_Handler.GetComponent<Rigidbody>();


        Temporary_RigidBody.AddForce(r.direction * Forward_Force * 2);
        Destroy(Temporary_Snowball_Handler, 1f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Destroy(Temporary_Snowball_Handler);
    }

}
