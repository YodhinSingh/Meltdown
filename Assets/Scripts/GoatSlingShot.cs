using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoatSlingShot : MonoBehaviour
{
    //Input from controller/arduino
    private bool HoldingJump;
    private Vector3 AimInput = new Vector3(0, -1, 0);
    private bool HoldingJumpController;
    private Vector3 AimInputController = new Vector3(0, -1, 0);

    //Goat properties and components
    private bool onGround;
    private float jumpPressure;
    private float minJumpValue;
    private float maxJumpPressure;
    private float maxfallSpeed;
    private bool PlayerControlActive;
    private bool isOnMountain;
    private bool AddGoatCam;
    private bool isCreated;
    private bool aboutToLand;
    private bool collDone;
    private Ray groundCheck;
    private bool checkIfPlatformBelow;
    private PlayerInstanceGenerator instance;
    private Rigidbody rb;
    private Vector3 offset;
    public GameObject goatParent;
    public float jumpPower = 1f;
    public Vector3 aimPoint;
    public int playerIndex;
    public GameObject ground;

    // charge renderer component
    public GameObject lineRendererAim;
    private LineRenderer VisibleAimLine;
    private float arrowColourR;
    private float arrowColourG;
    //public GameObject arrowAim;

    //Audio Files
    [SerializeField] private AudioClip[] SnowballSounds = new AudioClip[1];    // an array of snowball sounds that will be randomly selected from.
    [SerializeField] private AudioClip[] JumpSounds = new AudioClip[4];           // an array of jump sounds that will be randomly selected from.
    [SerializeField] private AudioClip[] LandSounds = new AudioClip[2];           // an array of landing sounds that will be randomly selected from.
    private AudioSource m_AudioSource;
    public GameObject DeathSoundPlayer;

    //Arduino Stuff
    float oldDistance = 0;
    AriunoListener arduinoScript;
    float jumpChargeTime;

    // For Setting Z postion of goat if mountain is not flat
    Vector3 MountainEndPoint;
    Vector3 MountainStartPoint;
    Vector2 MountainMidPoint;
    Vector3[] MountainPoints = new Vector3[4];
    float fraction;

    //Player Manager related
    public GameObject[] goatNames = new GameObject[2];
    bool rankingAquired;
    public bool isAI;
    public SkinnedMeshRenderer meshBody;
    public MeshRenderer meshEye1_1;
    public MeshRenderer meshEye2_1;
    public MeshRenderer meshEye1_2;
    public MeshRenderer meshEye2_2;
    public GameObject[] bodyVisibility = new GameObject[2];

    //Particle Effects related
    public GameObject[] trails = new GameObject[3];
    public GameObject ChargeParticles;
    public GameObject FullyChargedParticles;
    public GameObject LaunchChargedParticles;
    ParticleSystem LaunchCharged;
    bool hasEmitted = false;
    int numParticles = 1;

    // Animator components
    Animator anim;
    int chargeState = -1;
    bool isMovingForward = false;
    float minForwardMovement = 0.1f;
    bool hasWon = false;
    bool gotHit = false;
    bool playerHeadbutt = false;

    //Camera shake
    private Camera_Shake camOBJ;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject.transform.root.gameObject);

        anim = GetComponent<Animator>();

        MountainPoints[0] = new Vector3(0, -2.35f, 0);
        MountainPoints[1] = new Vector3(0, -76.8f, -48f);
        MountainPoints[2] = new Vector3(0, 214, 115.8f);
        MountainPoints[3] = new Vector3(0, -351.2f, 158.7f);
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
        aboutToLand = false;
        collDone = false;
        arduinoScript = instance.gameObject.GetComponentInChildren<AriunoListener>();
        rankingAquired = false;

        LaunchCharged = LaunchChargedParticles.GetComponent<ParticleSystem>();

        jumpChargeTime = UnityEngine.Random.Range(0.1f, 1f);    // used instead of arduino since no access to it right now

        MountainStartPoint = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.position.z);
        MountainEndPoint = new Vector3(-3.4f,407.8f, 0f); // OLD Z was 11.2
        MountainMidPoint = new Vector2(80, 139);
        MountainPoints[0] = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);



    }


    // Update is called once per frame
    void Update()
    {
        CheckSceneRequirements();
        offset = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);    // where to place platform below goat
        CheckOnMountain();                                  // check if goat is on mountain


        // This checks if the arduino is plugged in. If not it will return false, else return true.
        bool use = CheckJump();

        if (onGround && PlayerControlActive)
        {
            VisibleAimLine.enabled = true;
            //arrowAim.SetActive(true);
            for (int i = 0; i < trails.Length; i++)
            {
                trails[i].SetActive(false);
            }

            chargeState = -1;

            if (!use && !isAI)
                ChargeJump(HoldingJumpController, AimInputController);
            else
                ChargeJump(HoldingJump, AimInput);
        }
        if (!onGround && PlayerControlActive)
        {
            for (int i = 0; i < trails.Length; i++)
            {
                trails[i].SetActive(true);
            }

            VisibleAimLine.enabled = false;
            //arrowAim.SetActive(false);
            RotateBodyInAir();
            chargeState = 2;
        }

        checkIfPlatformBelow = CheckBelowGround();

        if (!isCreated && isOnMountain && !checkIfPlatformBelow) // if the goat is still on the mountain, then create a platform for it to stand on (if no platform already)
            CreateGround();

        if (!isOnMountain)
            DestroyGround();



        anim.SetBool("onGround", onGround);
        anim.SetInteger("chargeState", chargeState);
        anim.SetBool("moveDir", isMovingForward);
        anim.SetBool("gotHit", gotHit);
        anim.SetBool("hasWon", hasWon);
        anim.SetBool("headbutt", playerHeadbutt);

    }


      // This is for using the controllers input
    private void OnJump()               // onJump and releaseJump are for get jump power based on holding the 'A' or 'X' button
    {
        HoldingJumpController = true;
    }

    private void OnReleaseJump()
    {
        HoldingJumpController = false;
    }

    private void OnAim(InputValue value)                    // get aim input from Unity's new input system
    {
        

        if (value.Get<Vector2>() != new Vector2())
        {
            Vector2 temp = value.Get<Vector2>();
            temp = new Vector2(temp.x ,Mathf.Clamp(temp.y,-1.1f,0f));   // make sure imput stays in range

            AimInputController = temp;
        }
        //HoldingJump = (value.Get<Vector2>() == new Vector2()) ? false : true;         // this gets jump power if analog stick is let go of
    }


    public float quadBezierPoint(float p0, float p1, float p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }
    public float cubeBezier3(float p0, float p1, float p2, float p3, float t)
    {
        float r = 1f - t;
        float f0 = r * r * r;
        float f1 = r * r * t * 3;
        float f2 = r * t * t * 3;
        float f3 = t * t * t;
        return f0 * p0 + f1 * p1 + f2 * p2 + f3 * p3;
    }

    void CheckSceneRequirements()
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
            camOBJ = Camera.main.GetComponent<Camera_Shake>();
            DisablePlayerControl(false);
            //GetComponent<MeshRenderer>().enabled = true;
            /*
            SkinnedMeshRenderer[] meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i].enabled = true;
            }
            */
            for (int i = 0; i < bodyVisibility.Length; i++)
            {
                bodyVisibility[i].SetActive(true);
            }

        }
        if (!rankingAquired && instance.gotRankings && SceneManager.GetActiveScene().buildIndex == 1)
        {
            //instance.goatNameGraphics[playerIndex - 1] = goatNames[0];
            //instance.goatNameGraphics[playerIndex] = goatNames[1];
            rankingAquired = true;
        }
    }


    void ChargeJump(bool HoldingJumpVer, Vector3 AimInputVer)
    {
        if (aboutToLand)
        {
            PlayAudio(2);
        }

        aboutToLand = false;
        Vector3 target = new Vector3(AimInputVer.x, AimInputVer.y, 0) * -50 * Time.deltaTime;         // where the player is aiming at

        aimPoint = target;

        Ray aim = new Ray(transform.position, target);
        Vector3 LineRay = new Vector3(aim.direction.x, aim.direction.y, aim.direction.z);

        RotateBodyWhileAiming(LineRay);

        if (transform.rotation == Quaternion.Euler(0, 270f, 0))
        {
            LineRay = new Vector3(-aim.direction.x, aim.direction.y, aim.direction.z).normalized;      // rotate aim line if goat body rotated so it works right
        }

        VisibleAimLine.SetPosition(1, LineRay * -0.02f * (jumpPressure + 3f));

        //float angle = Vector2ToDegree(LineRay);
        //arrowAim.transform.localRotation = Quaternion.Euler(angle, 0f, 0);

        Debug.DrawRay(aim.origin, aim.direction * 50, Color.red);

        // Holding the jump charge
        if (HoldingJumpVer)
        {

            if (jumpPressure < maxJumpPressure)                 // increase jump amount
            {
                jumpPressure += Time.deltaTime * 15f;           //old was 20
                ChargeParticles.SetActive(true);
                numParticles = (int) (jumpPressure *2);
            }
            else
            {
                jumpPressure = maxJumpPressure;
                FullyChargedParticles.SetActive(true);
                numParticles = 30;
            }
            arrowColourR -= Time.deltaTime * 3f;                    //change colour of slingshot aim line
            arrowColourG += Time.deltaTime * 3f;
            chargeState = 0;
            hasEmitted = false;
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
                chargeState = 1;
                if (!hasEmitted)
                {
                    LaunchCharged.Emit(numParticles);
                    hasEmitted = true;
                }
            }
            
            ChargeParticles.SetActive(false);
            FullyChargedParticles.SetActive(false);

            // Apply jump force to rigidbody and reset jump value
            Vector3 jumpDir = new Vector3(aim.direction.x / 1.5f, aim.direction.y, aim.direction.z);
            isMovingForward = (Mathf.Abs(aim.direction.x) > minForwardMovement) ? true : false;
            rb.AddForce(jumpDir * jumpPressure * jumpPower);

            arrowColourR = 1f;
            arrowColourG = 0f;
            jumpPressure = 0f;
            isCreated = false;


        }
        VisibleAimLine.endColor = new Color(arrowColourR, arrowColourG, 0);             // changes colour of aim line to show power
    }

    void RotateBodyWhileAiming(Vector3 aim)
    {
        if (aim.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 270f, 0);
            goatParent.transform.position = new Vector3(-1.242f, goatParent.transform.position.y, goatParent.transform.position.z);
        }
        if (aim.x >= 0)
        {
            transform.rotation = Quaternion.Euler(0, 90f, 0);
            goatParent.transform.position = new Vector3(0, goatParent.transform.position.y, goatParent.transform.position.z);
        }
    }

    void RotateBodyInAir()
    {
        //isMovingForward = false;
        aboutToLand = true;
        if (rb.velocity.x < 0)                                         // rotates goat body if in the air based on velocity
        {
            transform.rotation = Quaternion.Euler(0, 270f, 0);
            goatParent.transform.position = new Vector3(-1.242f, goatParent.transform.position.y, goatParent.transform.position.z);
        }
        if (rb.velocity.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90f, 0);
            goatParent.transform.position = new Vector3(0, goatParent.transform.position.y, goatParent.transform.position.z);
        }
        if (Mathf.Abs(rb.velocity.x) > minForwardMovement)
        {
            //isMovingForward = true;
        }
    }


    void SetZTransform()
    {
        if (fraction <= 1)
        {
            fraction = transform.position.y / MountainEndPoint.y;
        }

        float curZ = Mathf.Lerp(MountainStartPoint.z, MountainEndPoint.z, fraction);
        //float curZ = cubeBezier3(MountainPoints[0].z, MountainPoints[1].z, MountainPoints[2].z, MountainPoints[3].z, fraction);
        //float curZ = quadBezierPoint(MountainStartPoint.x, MountainMidPoint.x, MountainEndPoint.x, fraction);
        //float curZ = raycastInfo[0].z - 2f;
        //print(playerIndex + " || " + (raycastInfo[0].z - 2f));
        transform.position = new Vector3(transform.position.x, transform.position.y, curZ);
    }

    bool CheckJump()
    {
        jumpChargeTime -= Time.deltaTime;
        float address = arduinoScript.address;
        float distance = arduinoScript.distance;
        float angle = arduinoScript.angle;
        float minDistanceChange = 15f;
        bool SensorConnected = arduinoScript.isConnected;

        if (SensorConnected)
        {
            if (address % 10 == playerIndex)
            {
                //print("Goat address: " + address + " || Goat distance: " + distance + " || Angle" + angle + " || ID" + playerIndex);
                if (distance > oldDistance && (distance - oldDistance > minDistanceChange))
                {
                    HoldingJump = false;
                    //print("isholding");
                    
                }
                else if (distance <= oldDistance && (oldDistance - distance > minDistanceChange))
                {
                    HoldingJump = true;
                    //print("not holding");
                }
                oldDistance = distance;
                AimInput = DegreeToVector2(angle);
            }
        }
        else
        {
            if (jumpChargeTime > 0)         //Since no acess to arduino stuff, creating fake random numbers to test
            {
                HoldingJump = true;
            }
            else
            {
                AimInput = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1.1f, 0f), 0);
                //AimInput = new Vector3(0, -1, 0);
                HoldingJump = false;
                jumpChargeTime = UnityEngine.Random.Range(0.75f, 2f);
            }
        }
        return SensorConnected;
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        float radian = degree * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radian), -Mathf.Sin(radian)).normalized;
    }

    public static float Vector2ToDegree(Vector2 aim)
    {
        if (aim.x < 0)
        {
            return 360 - (Mathf.Atan2(aim.x, aim.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(aim.x, aim.y) * Mathf.Rad2Deg;
        }

    }

    private bool CheckBelowGround() // if platform below close enough, then no need to spawn own platform, just land on this
    {
        groundCheck = new Ray(transform.position, new Vector3(0, -1, 0));
        
        float maxDistance = 1.7f;
        Debug.DrawRay(groundCheck.origin, groundCheck.direction * maxDistance, Color.yellow);

        var layerMask = 1 << 25; // only check collisions with layer 25 = Mountain Platforms

        if (Physics.Raycast(groundCheck,  maxDistance, layerMask))
        {
            //print("ground!");
            DestroyGround();
            return true;
        }
        return false;
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
        ground.SetActive(false);
        ground.transform.position = new Vector3();
        isCreated = false;
    }

    public void AddGround() // Add final platform for goat once the player reaches top of mountain
    {
        ground.SetActive(true);
        //ground.transform.position = offset;
        if (transform.rotation == Quaternion.Euler(0, 270f, 0))
        {
            ground.transform.position = new Vector3(offset.x + 0.66f, offset.y, offset.z);
        }
        if (transform.rotation == Quaternion.Euler(0, 90f, 0))
        {
            ground.transform.position = new Vector3(offset.x - 0.66f, offset.y, offset.z);
        }
        isCreated = true;
    }

    public void AddGoatToCam()
    {
        Camera.main.GetComponent<CameraFollow>().addGoat(gameObject);
    }


    private void PlayAudio(int index) // Play sound from array depending on number. ie 1 = jump array, 2 = land array, 3 = snowball array. 
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
            SoundArray = SnowballSounds;
            volumeAmount = 1;
            m_AudioSource.clip = SoundArray[0];
            m_AudioSource.PlayOneShot(m_AudioSource.clip, volumeAmount);
            return;
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

    public void doWinDance()
    {
        hasWon = true;
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



            if (collision.gameObject.CompareTag("Platform") && !checkIfPlatformBelow)
            {
                gotHit = true;
                Vector3 dir = collision.GetContact(0).point - transform.position;
                dir = -dir.normalized;
                // This will push back the player
                GetComponent<Rigidbody>().AddForce(dir * 300f);
            }
            else
            {
                if (rb != null)
                    rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            }
        }
        else if (collision.gameObject.CompareTag("Snowball"))   // collisions for snowball
        {
            collision.gameObject.GetComponent<CalculateZValue>().explode();

            PlayAudio(3);
            gotHit = true;
            
            //Destroy(collision.gameObject, 1.5f);
            DestroyGround();
            Vector3 dir = collision.GetContact(0).point - transform.position;
            //Vector3 dir = new Vector3(0, -1, 0);
            dir = -dir.normalized;
            //Push down the player from snowball
            GetComponent<Rigidbody>().AddForce(dir * 500f);
        }
        else
        {
            if (!collDone)
            {
                playerHeadbutt = true;
                
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
        gotHit = false;
        playerHeadbutt = false;
        //collDone = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {             
            // make sure players cant get stuck on each other
            Vector3 dir = new Vector3(UnityEngine.Random.Range(-1, 1), 1, 0).normalized;
            playerHeadbutt = true;
            // This will push back the player
            GetComponent<Rigidbody>().AddForce(dir * 50f);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(-dir * 50f);
        }
        else if (!collision.gameObject.CompareTag("Snowball"))
        {
            onGround = true;
            gotHit = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))              // if player enters water, kill player
        {
            //other.GetComponent<InstantKillWater>().SetGameOver();
            camOBJ.allow = true;
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
        instance.GetComponent<RankingSystem>().RemoveGoat(gameObject, isDead);
        //instance.players.Remove(gameObject);
        Destroy(gameObject.transform.root.gameObject, 1f);
    }

    private IEnumerator DeathAnim()     // does the killing, with sound effects. Can later put animation calls here. NOTE FOR NOW THIS IS IGNORED
    {
        //PlayAudio(3);
        //DeathSoundPlayer.GetComponent<PlayDeathSound>().PlayAudioDeath();
        yield return new WaitForSeconds(1); // Initial delay before removing goat from view
        Camera.main.GetComponent<CameraFollow>().RemoveGoat(this.gameObject);
        //yield return new WaitForSeconds(2); //Use the length of the animation/sound clip as the wait time for yield
        Destroy(gameObject.transform.root.gameObject);
    }

}
