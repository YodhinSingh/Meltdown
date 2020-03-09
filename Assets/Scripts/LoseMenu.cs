using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseMenu : MonoBehaviour
{

    public GameObject LoseMenuUI;
    public GameObject Timer;
    float TimeToWait = 3;

    private void Start()
    {
        LoseMenuUI.SetActive(false);
        TimeToWait = 3;
    }

    private void Update()
    {
        if (LoseMenuUI.activeInHierarchy)
        {
            Timer.GetComponent<Text>().text = Mathf.CeilToInt(TimeToWait).ToString();
            TimeToWait -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Return) || TimeToWait <= 0)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void DisplayMenu()
    {
        LoseMenuUI.SetActive(true);
    }

}

