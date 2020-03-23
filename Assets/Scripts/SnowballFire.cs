using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowballFire : MonoBehaviour
{
    public GameObject[] SnowballEmmitters = new GameObject[6];
    public GameObject WinMenuUI;
    public GameObject LoseMenuUI;
    private Transform cam;

    public GameObject MountainTop;
    public AudioClip[] snowballThrowAudio = new AudioClip[3];
    private AudioSource m_AudioSource;
    float camMinZoom;
    float camMaxZoom;

    ButtonListener buttonScript;


    // Use this for initialization
    private void Awake()
    {
        for (int i = 0; i < SnowballEmmitters.Length; i++)
        {
            SnowballEmmitters[i].GetComponentInChildren<SnowballAim>().index = i + 1;
        }

    }
    void Start()
    {
        cam = Camera.main.transform;
        m_AudioSource = GetComponent<AudioSource>();
        camMaxZoom = Camera.main.GetComponent<CameraFollow>().maxZoom;
        camMinZoom = Camera.main.GetComponent<CameraFollow>().minZoom;

        buttonScript = GameObject.FindGameObjectWithTag("PlayerManager").GetComponentInChildren<ButtonListener>();
    }

    // Update is called once per frame
    void Update()
    {
        float fractionY = 0;
        if (fractionY <= 1)
        {
            fractionY = transform.position.y / MountainTop.transform.position.y;
        }
        float curZ = Mathf.Lerp(-4.3f, MountainTop.transform.position.z - 4.3f, fractionY);

        float fractionZ = 0;
        if (fractionZ <= 1 && fractionZ >= 0) // make sure snowballs always at bottom of screen regardless of cam zoom
        {
            //scaledValue = (rawValue - min) / (max - min);
            fractionZ = (cam.position.z - camMinZoom) / (camMaxZoom - camMinZoom);
        }
        float curY = cam.position.y + Mathf.Lerp(-10.6f, -32, fractionZ);
        Vector3 target = new Vector3(cam.position.x, curY, curZ); 
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 10f);


        // As long as the win menu or lose menu are not active/null, then keep reloading and throwing snowballs
        if ((WinMenuUI != null && !WinMenuUI.activeInHierarchy) && (LoseMenuUI != null && !LoseMenuUI.activeInHierarchy))
        {

            throwSnowballFromArduino();


            if (Input.GetKeyDown(KeyCode.A))
            {
                if (SnowballEmmitters[0].GetComponent<SnowballAim>().InstantiateSnowBall())
                    PlayAudio();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (SnowballEmmitters[1].GetComponent<SnowballAim>().InstantiateSnowBall())
                    PlayAudio();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (SnowballEmmitters[2].GetComponent<SnowballAim>().InstantiateSnowBall())
                    PlayAudio();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (SnowballEmmitters[3].GetComponent<SnowballAim>().InstantiateSnowBall())
                    PlayAudio();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (SnowballEmmitters[4].GetComponent<SnowballAim>().InstantiateSnowBall())
                    PlayAudio();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                if (SnowballEmmitters[5].GetComponent<SnowballAim>().InstantiateSnowBall())
                    PlayAudio();
            }
        }
    }

    private void throwSnowballFromArduino()
    {
        bool isPressed = buttonScript.isPressed;
        int EmitterNum = buttonScript.buttonNum - 1;

        if (isPressed)
        {
            if (SnowballEmmitters[EmitterNum].GetComponent<SnowballAim>().InstantiateSnowBall())
                PlayAudio();
        }
    }


    private void PlayAudio() // Play snowball throw sound
    {
        int n = Random.Range(1, snowballThrowAudio.Length);
        m_AudioSource.clip = snowballThrowAudio[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip, 1);
        // move picked sound to index 0 so it's not picked next time
        snowballThrowAudio[n] = snowballThrowAudio[0];
        snowballThrowAudio[0] = m_AudioSource.clip;
    }

}
