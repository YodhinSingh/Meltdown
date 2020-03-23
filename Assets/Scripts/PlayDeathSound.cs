using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDeathSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] DeathSounds = new AudioClip[5];    // an array of death sounds that will be randomly selected from.
    private static AudioSource m_AudioSource1;
    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource1 = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudioDeath() // Play death sound
    {
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = UnityEngine.Random.Range(1, DeathSounds.Length);
        m_AudioSource1.clip = DeathSounds[n];
        m_AudioSource1.PlayOneShot(m_AudioSource1.clip);
        // move picked sound to index 0 so it's not picked next time
        DeathSounds[n] = DeathSounds[0];
        DeathSounds[0] = m_AudioSource1.clip;
    }
}
