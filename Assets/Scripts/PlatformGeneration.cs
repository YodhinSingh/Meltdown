using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGeneration : MonoBehaviour
{
    public GameObject platform; // platforms are taken based on object in game, not seen by camera
    public GameObject boundaries; // boundaries based on object in game
    public float HeightDifferencePlatforms = 6f;
    public float PlatformWidthMin;    // min width of platform
    public float PlatformWidthMax;    // max width of platform
    private float platformPositionX;
    private float platformPositionY;
    [SerializeField] private float numPlatforms = 20;
    
    void Start()
    {
        platformPositionY = 3;

        for (int i = 0 ; i < numPlatforms; i++)  // Generates platforms 20 times
        {
            if (i%2 == 0)
            {
                platformPositionX = Random.Range(-(boundaries.transform.localScale.x - 5f)/2, 0f); // Using boundary cube to get desired X position for even numbered loops
            }
            
            else if (i%2 != 0)
            {
                platformPositionX = Random.Range(0f, (boundaries.transform.localScale.x - 5f)/2); // Using boundary cube to get desired X position for odd numbered loops
            }
    
            platformPositionY = platformPositionY + HeightDifferencePlatforms; // Determines distance between platforms
            platform.transform.localScale = new Vector3 (Random.Range(PlatformWidthMin, PlatformWidthMax), 0.5f, 1f); // Changes the width of the platform randomly (Default between 5 and 8)

            Instantiate(platform, new Vector3(platformPositionX, platformPositionY, 0), Quaternion.identity); // Instantiates new platform using a gameobject, position and rotation
        }
    }
}
