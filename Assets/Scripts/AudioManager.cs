using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioSource audioTheme;
    public AudioClip theme;
    public AudioClip themeLoop;
    public static AudioManager instance;
    public GameObject WinMenuUI, LoseMenuUI;


    void Awake()
    {
        audioTheme = GetComponent<AudioSource>();
        audioTheme.clip = theme;
        audioTheme.volume = 0.2f;
        audioTheme.Play();
        audioTheme.loop = false;
        audioTheme.spatialBlend = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("WaitForMusicDone");
        checkScene();
    }
    private void Update()
    {
        /*
        if ((WinMenuUI != null && WinMenuUI.activeInHierarchy) || (LoseMenuUI != null && LoseMenuUI.activeInHierarchy))
            checkScene();
        */
        
    }

    public void checkScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)      //Menu
        {
            //audioTheme.loop = true;
            audioTheme.volume = 0.25f;
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)      //Game
        {
            //audioTheme.loop = true;
            audioTheme.volume = 0.5f;
        }
    }

    private IEnumerator WaitForMusicDone()
    {
        checkScene();
        yield return new WaitForSeconds(theme.length);
        audioTheme.clip = themeLoop;
        audioTheme.loop = true;
        audioTheme.Play();
    }
}
