using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    public GameObject WinMenuUI;
    private PlayerInstanceGenerator instance;
    public GameObject water;

    private void Start()
    {
        WinMenuUI.SetActive(false);
        instance = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerInstanceGenerator>();
    }

    private void Update()
    {
        if (WinMenuUI.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Return))
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
            WinMenuUI.GetComponentInChildren<Text>().text = "P" + other.gameObject.GetComponent<GoatSlingShot>().playerIndex + " Wins";
            //other.gameObject.GetComponent<GoatSlingShot>().DisablePlayerControl(true);
            //other.gameObject.GetComponent<GoatSlingShot>().AddGround();
            instance.DisableAllPlayers();
            water.GetComponent<InstantKillWater>().StopWaterRise();
        }
    }

}
