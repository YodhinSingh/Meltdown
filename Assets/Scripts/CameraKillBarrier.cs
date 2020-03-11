using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKillBarrier : MonoBehaviour
{
    private PlayerInstanceGenerator instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerInstanceGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (instance.GetPlayerCount() == 0 && !instance.PlayerWin)
        {
            SetGameOver();
        }
    }

    private void SetGameOver()
    {
        //Debug.Log("You lose");
        instance.players.Clear();
        GetComponent<LoseMenu>().DisplayMenu();
    }

}
