using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantKillBarrier : MonoBehaviour
{
    private bool hasCollided;

    private void Start()
    {
        hasCollided = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasCollided)
        {
            other.GetComponent<GoatSlingShot>().DestroyGoat(true);
            hasCollided = true;
            StartCoroutine("reAllowTrigger");
        }
    }

    private IEnumerator reAllowTrigger()
    {
        yield return new WaitForSeconds(1);
        hasCollided = false;
    }
}
