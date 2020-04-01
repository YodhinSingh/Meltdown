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
    public GameObject[] players = new GameObject[8];

    public GameObject[] Timer = new GameObject[3];
    public GameObject winGround;
    public GameObject[] CamKillBoxes = new GameObject[3];

    bool canModify;
    bool danceDone;

    private void Start()
    {
        WinMenuUI.SetActive(false);
        instance = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerInstanceGenerator>();
        canModify = true;
        danceDone = false;
    }

    private void Update()
    {
        if (WinMenuUI.activeInHierarchy && canModify && danceDone)
        {
            canModify = false;
            StartCoroutine("TimeDisplay");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            winGround.SetActive(true);
            for (int i = 0; i < CamKillBoxes.Length; i++)
            {
                CamKillBoxes[i].SetActive(false);
            }
            WinMenuUI.SetActive(true);
            players[other.gameObject.GetComponent<GoatSlingShot>().playerIndex - 1].SetActive(true);
            //other.gameObject.GetComponent<GoatSlingShot>().DisablePlayerControl(true);
            other.gameObject.GetComponent<GoatSlingShot>().AddGround();
            other.gameObject.GetComponent<GoatSlingShot>().doWinDance();
            water.GetComponent<InstantKillWater>().StopWaterRise();
            StartCoroutine("WinDance");
            
        }
    }

    private IEnumerator WinDance()
    {
        yield return new WaitForSeconds(3.1f);
        instance.DisableAllPlayers();
        instance.players.Clear();
        yield return new WaitForSeconds(1f);
        danceDone = true;

    }

    private IEnumerator TimeDisplay()
    {
        Timer[2].GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(1);

        Timer[2].GetComponent<Image>().enabled = false;
        Timer[1].GetComponent<Image>().enabled = true;

        yield return new WaitForSeconds(1);
        Timer[1].GetComponent<Image>().enabled = false;
        Timer[0].GetComponent<Image>().enabled = true;

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);

    }

}
