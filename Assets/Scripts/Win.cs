using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    public GameObject WinMenuUI;
    public GameObject Timer;
    private PlayerInstanceGenerator instance;
    public GameObject water;
    public GameObject[] players = new GameObject[6];

    float TimeToWait = 3;

    private void Start()
    {
        WinMenuUI.SetActive(false);
        instance = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerInstanceGenerator>();
        TimeToWait = 3;
    }

    private void Update()
    {
        if (WinMenuUI.activeInHierarchy)
        {
            Timer.GetComponent<Text>().text = Mathf.CeilToInt(TimeToWait).ToString();
            TimeToWait -= Time.deltaTime;
            
            if (Input.GetKeyDown(KeyCode.Return) || TimeToWait <= 0)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            WinMenuUI.SetActive(true);
            players[other.gameObject.GetComponent<GoatSlingShot>().playerIndex - 1].SetActive(true);

            //other.gameObject.GetComponent<GoatSlingShot>().DisablePlayerControl(true);
            //other.gameObject.GetComponent<GoatSlingShot>().AddGround();
            instance.DisableAllPlayers();
            instance.players.Clear();
            water.GetComponent<InstantKillWater>().StopWaterRise();
        }
    }

}
