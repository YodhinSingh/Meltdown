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

    public GameObject[] goats;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(transform.position.x, endPosValue, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (fraction < 1)
        {
            fraction += Time.deltaTime/TimeSecondsToTake;
        }
        transform.position = Vector3.Lerp(startPos, endPos, fraction);
    }

    public void SetGameOver()
    {
        //Debug.Log("You lose");
        LoseMenu loseMenuUI = GetComponent<LoseMenu>();
        loseMenuUI.DisplayMenu();
    }
}
