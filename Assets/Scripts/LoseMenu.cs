using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseMenu : MonoBehaviour
{

    public GameObject LoseMenuUI;
    public GameObject[] Timer = new GameObject[3];
    bool canModify;

    private void Start()
    {
        LoseMenuUI.SetActive(false);
        canModify = true;
    }

    private void Update()
    {
        if (LoseMenuUI.activeInHierarchy && canModify)
        {
            canModify = false;
            StartCoroutine("TimeDisplay");
        }
    }

    public void DisplayMenu()
    {
        LoseMenuUI.SetActive(true);
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

