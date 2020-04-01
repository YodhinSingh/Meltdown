using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateZValue : MonoBehaviour
{
    Vector3 MountainTop = new Vector3(-3.4f,407.8f, 0f);
    float startpoint;

    public TrailRenderer trail;
    public ParticleSystem particles;
    public GameObject exploder;

    GameObject temp;

    // Start is called before the first frame update
    void Start()
    {
        startpoint = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float fraction = 0;
        if (fraction <= 1)
        {
            fraction = transform.position.y / MountainTop.y;
        }
        float curZ = Mathf.Lerp(0, MountainTop.z, fraction);
        transform.position = new Vector3(transform.position.x, transform.position.y, curZ);
    }


    public void explode()
    {
        if (this != null)
        {
            temp = Instantiate(exploder, transform.position, Quaternion.identity) as GameObject;
            Destroy(temp, 0.25f);
            particles.gameObject.SetActive(false);
            trail.gameObject.SetActive(false);
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 0.3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Snowball"))
        {
            explode();
        }
    }
}
