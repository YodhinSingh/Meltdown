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

    public GameObject clouds;

    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        instance = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerInstanceGenerator>();
        allowRise = false;
        startPos = transform.position;
        endPos = new Vector3(transform.position.x, endPosValue, transform.position.z);
        speed = 1.25f;
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
            if (fraction < 0.5f && fraction >= 0.25f)
            {
                speed = 1.5f;
            }
            else if (fraction < 0.75f && fraction >= 0.5f)
            {
                speed = 1.7f;
            }
            else if (fraction >= 0.75f)
            {
                speed = 1.95f;
            }

            fraction += (Time.deltaTime * speed) / TimeSecondsToTake;
        }
        transform.position = Vector3.Lerp(startPos, endPos, fraction);

        if (transform.position.y >= 256f && transform.position.y <= 258f)
        {
            clouds.GetComponent<CloudScript>().makeCloudsDisappear();
        }
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
