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
        
    }

    // Update is called once per frame
    void Update()
    {
        float fraction = 0;
        if (fraction <= 1)
        {
            fraction = transform.position.y / MountainTop.transform.position.y;
        }
        float curZ = Mathf.Lerp(-4.3f, MountainTop.transform.position.z - 4.3f, fraction);
        transform.position = new Vector3(cam.position.x, cam.position.y - 16f, curZ); // keep pace with camera but do not zoom in and out


        // As long as the win menu or lose menu are not active/null, then keep reloading and throwing snowballs
        if ((WinMenuUI != null && !WinMenuUI.activeInHierarchy) && (LoseMenuUI != null && !LoseMenuUI.activeInHierarchy))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SnowballEmmitters[0].GetComponent<SnowballAim>().InstantiateSnowBall();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                SnowballEmmitters[1].GetComponent<SnowballAim>().InstantiateSnowBall();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                SnowballEmmitters[2].GetComponent<SnowballAim>().InstantiateSnowBall();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                SnowballEmmitters[3].GetComponent<SnowballAim>().InstantiateSnowBall();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                SnowballEmmitters[4].GetComponent<SnowballAim>().InstantiateSnowBall();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                SnowballEmmitters[5].GetComponent<SnowballAim>().InstantiateSnowBall();
            }
        }
    }

}
