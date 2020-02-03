using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    public GameObject WinMenuUI;

    private void Start()
    {
        WinMenuUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            WinMenuUI.SetActive(true);
            other.gameObject.GetComponent<GoatSlingShot>().DisablePlayerControl(true);
            other.gameObject.GetComponent<GoatSlingShot>().AddGround();
        }
    }

}
