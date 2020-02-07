using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioSource audioTheme;
    public string theme;
    public static AudioManager instance;
    public GameObject WinMenuUI, LoseMenuUI;


    void Awake()
    {   /*
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        */
        theme = "MeltdownTheme1";
        audioTheme = GetComponent<AudioSource>();
        audioTheme.clip = Resources.Load<AudioClip>(theme);
        audioTheme.volume = 0.2f;
        audioTheme.Play();
        audioTheme.loop = false;
        audioTheme.spatialBlend = 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    private void Update()
    {
        if ((WinMenuUI != null && WinMenuUI.activeInHierarchy) || (LoseMenuUI != null && LoseMenuUI.activeInHierarchy))
            checkScene();

        if (!audioTheme.isPlaying)
        {
            theme = "MeltdownTheme1Loop";
            audioTheme.clip = Resources.Load<AudioClip>(theme);
            audioTheme.Play();
            audioTheme.loop = true;
        }
    }

    public void checkScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)      //Game
        {
            audioTheme.loop = true;
            audioTheme.volume = 0.25f;
        }
    }
}
