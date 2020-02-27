using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoatSlingShot : MonoBehaviour
{
    private bool HoldingJump;
    private Vector3 AimInput;

    private bool onGround;
    private float jumpPressure;
    private float minJumpValue;
    private float maxJumpPressure;
    private float maxfallSpeed;
    private bool PlayerControlActive;
    private bool AddGoatCam;

    private PlayerInstanceGenerator instance;

    private Rigidbody rb;
    private Vector3 offset;
    private bool isCreated;

    public GameObject lineRendererAim;
    private LineRenderer VisibleAimLine;

    private float arrowColourR;
    private float arrowColourG;

    public GameObject ground;
    private bool isOnMountain;

    public float jumpPower = 1f;
    public Vector3 aimPoint;

    public int playerIndex;

    [SerializeField] private AudioClip[] DeathSounds;    // an array of death sounds that will be randomly selected from.
    [SerializeField] private AudioClip[] JumpSounds;           // an array of jump sounds that will be randomly selected from.
    [SerializeField] private AudioClip[] LandSounds;           // an array of landing sounds that will be randomly selected from.
    private AudioSource m_AudioSource;

    public GameObject DeathSoundPlayer;

    private bool justlanded;
    bool collDone;

    public float oldDistance = 0;
    public GameObject arduino;
    AriunoListener arduinoScript;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject.transform.root.gameObject);

        instance = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerInstanceGenerator>();
        onGround = false;
        jumpPressure = 0f;
        minJumpValue = 5f;
        maxJumpPressure = 5f;
        maxfallSpeed = -1f;
        rb = GetComponent<Rigidbody>();
        offset = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        isCreated = true;
        PlayerControlActive = false;
        isOnMountain = true;
        arrowColourR = 1f;
        arrowColourG = 0f;
        VisibleAimLine = lineRendererAim.GetComponent<LineRenderer>();
        AddGoatCam = true;
        m_AudioSource = GetComponent<AudioSource>();
        justlanded = false;
        collDone = false;
        arduinoScript = GameObject.FindGameObjectWithTag("PlayerManager").GetComponentInChildren<AriunoListener>();

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && !AddGoatCam)   //resets attributes of goat controls (in main menu)
        {
            AddGoatCam = true;
            PlayerControlActive = false;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1 && AddGoatCam)    // adds goat to camera script (in main game)
        {
            AddGoatToCam();
            AddGoatCam = false;
            PlayerControlActive = true;
        }
        offset = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);    // where to place platform below goat
        CheckOnMountain();                                              // check if goat is on mountain



        // This checks if the arduino is returning the distance value (and later aim). For now it is getting random values to test it.
        //HoldingJump = GetComponent<AriunoListener>().isHoldingJump;
        //AimInput = GetComponent<AriunoListener>().aim;
        CheckJump();
        AimInput = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1.1f, 0f), 0);



        if (onGround && PlayerControlActive)
        {
            if (justlanded)
                PlayAudio(2);

            justlanded = false;
            Vector3 target = new Vector3(AimInput.x, AimInput.y, 0) * -50 * Time.deltaTime;         // where the player is aiming at

            aimPoint = target;

            Ray aim = new Ray(transform.position, target);
            Vector3 LineRay = new Vector3(aim.direction.x, aim.direction.y, aim.direction.z);

            //print(transform.rotation.y);
            if (transform.rotation == Quaternion.Euler(0, 270f, 0))
            {
                LineRay = new Vector3(-aim.direction.x, aim.direction.y, aim.direction.z).normalized;      // rotate aim line if goat body rotated so it works right
            }
            VisibleAimLine.SetPosition(1, LineRay * -0.02f * (jumpPressure + 3f));

            Debug.DrawRay(aim.origin, aim.direction * 50, Color.red);

            // Holding the jump charge
            if (HoldingJump)
            {
                
                if (jumpPressure < maxJumpPressure)                 // increase jump amount
                {
                    jumpPressure += Time.deltaTime * 20f;

                }
                else
                {
                    jumpPressure = maxJumpPressure;
                }
                arrowColourR -= Time.deltaTime * 3f;                    //change colour of slingshot aim line
                arrowColourG += Time.deltaTime * 3f;
            }

            // Not holding the jump charge anymore
            else
            {
                if (jumpPressure > 0f)
                {
                    jumpPressure += minJumpValue;
                    arrowColourR -= 0.01f;
                    arrowColourG += 0.01f;
                    PlayAudio(1);
                }

                // Apply jump force to rigidbody and reset jump value
                Vector3 jumpDir = new Vector3(aim.direction.x/1.5f, aim.direction.y, aim.direction.z);
                rb.AddForce(jumpDir * jumpPressure * jumpPower);
                
                arrowColourR = 1f;
                arrowColourG = 0f;
                jumpPressure = 0f;
                isCreated = false;
                

            }

        }
        if (!onGround && PlayerControlActive)
        {
            justlanded = true;
            if (rb.velocity.x < 0)                                         // rotates goat body if in the air based on velocity
            {
                transform.rotation = Quaternion.Euler(0, 270f, 0);
            }
            if (rb.velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 90f, 0);
            }
        }

        if (!isCreated && isOnMountain) // if the goat is still on the mountain, then create a platform for it to stand on (if no platform already)
            CreateGround();
        if (!isOnMountain)
            DestroyGround();
        VisibleAimLine.endColor = new Color(arrowColourR, arrowColourG, 0);             // changes colour of aim line to show power
    }


    /*  // This is for using the controllers input
    private void OnJump()               // onJump and releaseJump are for get jump power based on holding the 'A' or 'X' button
    {
        HoldingJump = true;
    }

    private void OnReleaseJump()
    {
        HoldingJump = false;
    }

    private void OnAim(InputValue value)                    // get aim input from Unity's new input system
    {
        

        if (value.Get<Vector2>() != new Vector2())
        {
            Vector2 temp = value.Get<Vector2>();
            temp = new Vector2(temp.x ,Mathf.Clamp(temp.y,-1.1f,0f));   // make sure imput stays in range

            AimInput = temp;
        }
        //HoldingJump = (value.Get<Vector2>() == new Vector2()) ? false : true;         // this gets jump power if analog stick is let go of
    }
    */

    void CheckJump()
    {
        float address = arduinoScript.address;
        float distance = arduinoScript.distance;
        float minDistanceChange = 15f;
        if (address % 10 == playerIndex)
        {
            //print(playerIndex);
            if (distance > oldDistance && distance - oldDistance >= minDistanceChange)
            {
                HoldingJump = true;   //Set the value to true or false. The goatslingshot script is already accessing this value there.
                //print("isholding");
            }
            else if (distance <= oldDistance && oldDistance - distance >= minDistanceChange)
            {
                HoldingJump = false;

                //print("not holding");
            }
            oldDistance = distance;
        }
    }

    public Vector3 GetAimPoint()
    {
        return aimPoint;
    }

    public void DisablePlayerControl(bool allow)
    {
        if (allow)
            PlayerControlActive = false;
        else
            PlayerControlActive = true;
    }

    private void CheckOnMountain()  // gets result of if the goat is on mountain from other script
    {
        isOnMountain = GetComponentInChildren<CheckOnMountain>().GetIsOnMountain();
    }


    private void CreateGround() // create platform below goat if its velocity reaches maximum fall speed and if its on the mountain
    {
        if (rb.velocity.y < maxfallSpeed)
        {
            AddGround();
            collDone = false;
        }
    }

    private void DestroyGround()
    {
        ground.transform.position = new Vector3();
        isCreated = false;
    }

    public void AddGround() // Add final platform for goat once the player reaches top of mountain
    {
        ground.transform.position = offset;
        isCreated = true;
    }

    public void AddGoatToCam()
    {
        Camera.main.GetComponent<CameraFollow>().addGoat(gameObject);
    }


    private void PlayAudio(int index) // Play sound from array depending on number. ie 1 = jump array, 2 = land array, 3 = death array. 
    {
        AudioClip[] SoundArray;
        float volumeAmount;
        if (index == 1)
        {
            SoundArray = JumpSounds;
            volumeAmount = 1f;
        }
        else if (index == 2)
        {
            SoundArray = LandSounds;
            volumeAmount = 0.1f;
        }
        else
        {
            SoundArray = DeathSounds;
            volumeAmount = 1;
        }

        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = UnityEngine.Random.Range(1, SoundArray.Length);
        m_AudioSource.clip = SoundArray[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip, volumeAmount);
        // move picked sound to index 0 so it's not picked next time
        SoundArray[n] = SoundArray[0];
        SoundArray[0] = m_AudioSource.clip;
    }

    private void OnStartGame()
    {
        instance.WantToStartGame();
    }





    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Snowball"))     // collisions for everything other than player/snowball
        {
            onGround = true;
            isCreated = true;
            if (collision.gameObject.tag == "Platform")
            {
                Vector3 dir = collision.GetContact(0).point - transform.position;
                dir = -dir.normalized;
                // This will push back the player
                GetComponent<Rigidbody>().AddForce(dir * 500f);
            }
            else
            {
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            }
        }
        else if (collision.gameObject.CompareTag("Snowball"))   // collisions for snowball
        {
            Destroy(collision.gameObject);
            DestroyGround();
            Vector3 dir = collision.GetContact(0).point - transform.position;                  // collisions for player & player
            dir = -dir.normalized;
            //Push down the player from snowball
            GetComponent<Rigidbody>().AddForce(dir * 500f);
        }
        else
        {
            if (!collDone)
            {
                Vector3 dir = collision.GetContact(0).point - transform.position;                  // collisions for player & player
                dir = dir.normalized;
                // This will push back the player
                GetComponent<Rigidbody>().AddForce(-dir * 500f);
                collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * 500f);
                collDone = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Snowball"))
        {
            onGround = false;
            isCreated = false;
        }
        //collDone = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")){             // make sure players cant get stuck on each other
            Vector3 dir = new Vector3(UnityEngine.Random.Range(-1,1) ,1,0).normalized;
            // This will push back the player
            GetComponent<Rigidbody>().AddForce(dir * 50f);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(-dir * 50f);
        }
        else if (!collision.gameObject.CompareTag("Snowball"))
            onGround = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))              // if player enters water, kill player
        {
            //other.GetComponent<InstantKillWater>().SetGameOver();
            DisablePlayerControl(true);
            instance.players.Remove(gameObject);
            instance.GetComponent<RankingSystem>().RemoveGoat(gameObject, true);
            //StartCoroutine("DeathAnim");
            DeathSoundPlayer.GetComponent<PlayDeathSound>().PlayAudioDeath();
            Camera.main.GetComponent<CameraFollow>().RemoveGoat(this.gameObject);
            Destroy(gameObject.transform.root.gameObject);

        }
    }

    public void DestroyGoat(bool isDead)       // used for playerInstanceGenerator script when 1 player wins. Destroy all current goats so next round can pick goats
    {
        Camera.main.GetComponent<CameraFollow>().RemoveGoat(this.gameObject);
        instance.players.Remove(gameObject);
        instance.GetComponent<RankingSystem>().RemoveGoat(gameObject, isDead);
        Destroy(gameObject.transform.root.gameObject, 1f);
    }

    private IEnumerator DeathAnim()     // does the killing, with sound effects. Can later put animation calls here. NOTE FOR NOW THIS IS IGNORED
    {
        PlayAudio(3);
        //DeathSoundPlayer.GetComponent<PlayDeathSound>().PlayAudioDeath();
        yield return new WaitForSeconds(1); // Initial delay before removing goat from view
        Camera.main.GetComponent<CameraFollow>().RemoveGoat(this.gameObject);
        //yield return new WaitForSeconds(2); //Use the length of the animation/sound clip as the wait time for yield
        Destroy(gameObject.transform.root.gameObject);
    }


    private void OnBecameInvisible()        // ignore this
    {
        //DisablePlayerControl(true);
        //Debug.Log("this player lost");
    }
}
