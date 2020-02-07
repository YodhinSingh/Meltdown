using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour
{

    public GameObject LoseMenuUI;

    private void Start()
    {
        LoseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (LoseMenuUI.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Return))
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

