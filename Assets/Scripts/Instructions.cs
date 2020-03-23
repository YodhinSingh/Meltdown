using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    public Image goal;
    public Image platformRule;
    public Image waterRule;
    public Image snowballRule;

    public Transform water;
    private PlayerInstanceGenerator instance;
    private RankingSystem WaterUI;
    public Transform BottomTriggerScreen;

    bool waterCountdown;

    float minDistance = 2f;

    // Start is called before the first frame update
    void Start()
    {
        goal.enabled = true;
        platformRule.enabled = false;
        waterRule.enabled = false;
        snowballRule.enabled = false;
        StartCoroutine("timerTillDisactive");
        instance = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerInstanceGenerator>();
        //WaterUI = instance.gameObject.GetComponent<RankingSystem>();
        waterCountdown = false;
    }

    // Update is called once per frame
    void Update()
    {

        
        if (waterCountdown)
        {
            if ((water.position.y - BottomTriggerScreen.position.y >= minDistance) || (water.position.y >= 235f))
            {
                StartCoroutine("WaterTimer");
            }
            
        }
    }

    IEnumerator timerTillDisactive()
    {
        yield return new WaitForSeconds(5);
        goal.enabled = false;
        snowballRule.enabled = true;

        yield return new WaitForSeconds(3);
        snowballRule.enabled = false;
        platformRule.enabled = true;

        instance.EnablePlayers();
        GetComponentInChildren<SnowballFire>().allowShoot = true;

        yield return new WaitForSeconds(2f);
        waterCountdown = true;
        waterRule.enabled = true;
    }

    IEnumerator WaterTimer()
    {
        waterCountdown = false;
        yield return new WaitForSeconds(5);
        waterRule.enabled = false;

    }


}
