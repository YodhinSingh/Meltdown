using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantKillWater : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    [SerializeField] private float endPosValue = 0;
    [SerializeField] private float TimeSecondsToTake = 0;
    private float fraction;

    private bool allowRise;
    private PlayerInstanceGenerator instance;



    // Start is called before the first frame update
    void Start()
    {
        instance = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerInstanceGenerator>();
        allowRise = false;
        startPos = transform.position;
        endPos = new Vector3(transform.position.x, endPosValue, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (instance.GetPlayerCount() == 0 && !instance.PlayerWin)
        {
            SetGameOver();
            allowRise = false;
        }
        if (fraction < 1 && allowRise)
        {
            fraction += Time.deltaTime/TimeSecondsToTake;
        }
        transform.position = Vector3.Lerp(startPos, endPos, fraction);
    }

    private void SetGameOver()
    {
        //Debug.Log("You lose");
        instance.players.Clear();
        GetComponent<LoseMenu>().DisplayMenu();
    }

    public void StopWaterRise()
    {
        allowRise = false;
    }

    public void StartWaterRise()
    {
        allowRise = true;
    }

}
