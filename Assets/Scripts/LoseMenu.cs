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

    public void DisplayMenu()
    {
        LoseMenuUI.SetActive(true);
    }
}

