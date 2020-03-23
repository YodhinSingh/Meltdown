using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatCollideParticles : MonoBehaviour
{
    public bool groundCol;
    public bool backCol;
    public bool frontCol;
    public bool topCol;

    ParticleSystem p;

    bool hasCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        p = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Platform") && (topCol || frontCol || backCol) && !hasCollided)
        {
            p.Emit(1);
            hasCollided = true;

        }
        else if (collision.gameObject.CompareTag("Snowball") && !hasCollided)
        {
            p.Emit(1);
            hasCollided = true;
        }

        else if (collision.gameObject.CompareTag("Player") && !hasCollided)
        {
            p.Emit(1);
            hasCollided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        hasCollided = false;
    }

}
