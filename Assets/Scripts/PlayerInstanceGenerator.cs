using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInstanceGenerator : MonoBehaviour
{
    PlayerInputManager Instance;
    private readonly int GoatIndexStart = 15;
    private readonly int PlatformIndexStart = 7;
    private bool changed;
    public static PlayerInstanceGenerator instanceOBJ;

    //private float timer;          //any comments related to timer or timeLimit variable is for having game start in 10 seconds mode. Not Needed right now
    //[SerializeField] private int timeLimit = 10;

    private bool startGame;

    public bool PlayerWin;
    public List<GameObject> players = new List<GameObject>();
    public Material[] goatMaterials = new Material[8];
    public GameObject[] planes = new GameObject[8];
    public Material[] planeMaterials = new Material[2];
    public GameObject[] goatNameGraphics = new GameObject[16];

    public bool gotRankings;

    public Text TitleScreen;

    private readonly float[] xPosOfGoats = {-21, -15, -9, -3, 3, 9 ,15, 21};


    private void Awake()
    {
        if (instanceOBJ == null)    // Only keep this instance of this object alive. Dont destroy it after changing scenes and destroy any duplicates
        {
            instanceOBJ = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        
        Instance = GetComponent<PlayerInputManager>();
        changed = false;
        gotRankings = false;

    }

    private void Start()
    {
        //timer = 0;
        PlayerWin = false;
        startGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!changed && Instance.playerCount >= 1)      // Active timer if at least 1 player has joined // currently disabled for better way
        {
            //timer += Time.deltaTime;
            //TitleScreen.text = "GAME STARTS IN: " + (timeLimit - ((int)timer));
        }

        if (Instance.playerCount >= 1 && !changed && startGame) // load game scene
        {
            SceneManager.LoadScene(1);
            changed = true;
            GetComponent<RankingSystem>().StartRanking();
            Instance.DisableJoining();  // no more players can join
        }

        // This is if the game was in the game scene and has re-entered the main menu. This will reset all variables so that the previous code
        // will work like the first time. It will also re-find all the game objects in the scene that this script needs to run
        if (Instance.playerCount == 0 && changed && SceneManager.GetActiveScene().buildIndex == 0)
        {
            changed = false;
            //timer = 0f;
            Instance.EnableJoining();
            PlayerWin = false;
            startGame = false;
            gotRankings = false;

            for (int i = 0; i < planes.Length; i++)
            {
                planes[i] = GameObject.Find("P" + (i+1));      // This is UI stuff, find all the panels so when goat selected, a white square will form
            }
            TitleScreen = GameObject.Find("Timer").GetComponent<Text>();    // find reference to text obj that is the countdown timer
        }

        if (!gotRankings && SceneManager.GetActiveScene().buildIndex == 1)
        {
            goatNameGraphics[0] = GameObject.Find("RedGoatRanking");
            goatNameGraphics[1] = GameObject.Find("RedGoatDeadRanking");
            goatNameGraphics[2] = GameObject.Find("BlueGoatRanking");
            goatNameGraphics[3] = GameObject.Find("BlueGoatDeadRanking");
            goatNameGraphics[4] = GameObject.Find("YellowGoatRanking");
            goatNameGraphics[5] = GameObject.Find("YellowGoatDeadRanking");
            goatNameGraphics[6] = GameObject.Find("GreenGoatRanking");
            goatNameGraphics[7] = GameObject.Find("GreenGoatDeadRanking");
            goatNameGraphics[8] = GameObject.Find("PurpleGoatRanking");
            goatNameGraphics[9] = GameObject.Find("PurpleGoatDeadRanking");
            goatNameGraphics[10] = GameObject.Find("OrangeGoatRanking");
            goatNameGraphics[11] = GameObject.Find("OrangeGoatDeadRanking");
            goatNameGraphics[12] = GameObject.Find("PinkGoatRanking");
            goatNameGraphics[13] = GameObject.Find("PinkGoatDeadRanking");
            goatNameGraphics[14] = GameObject.Find("AquaGoatRanking");
            goatNameGraphics[15] = GameObject.Find("AquaGoatDeadRanking");

            bool isAnyNull = false;
            for (int i = 0; i < goatNameGraphics.Length; i++)
            {
                if (goatNameGraphics[i] == null)
                {
                    isAnyNull = true;
                }
                if (i+1 > Instance.playerCount * 2)
                {
                    goatNameGraphics[i].GetComponent<RawImage>().enabled = false;
                }
                
            }

            if (!isAnyNull)
            {
                gotRankings = true;
            }
        }
        
    }

    private void OnPlayerJoined(PlayerInput player)     // this is called whenever a player joins the game via the start button
    {
        players.Add(player.gameObject);         // add all joined players to a list so can reference later
        TitleScreen.text = "PRESS B or SPACE TO START GAME";
        GetComponent<RankingSystem>().addGoat(player.gameObject);

        

        if (Instance != null && Instance.playerCount == 1)
        {
            GameObject backG = GameObject.Find("BlackOverlay");
            backG.GetComponent<RawImage>().enabled = true;
        }

        Transform pos = player.gameObject.GetComponentInChildren<Transform>();


        //Set goat's colour based on player count (P1 = red, P2 = blue, etc) and change panel material to a white square to show that it is selected 
        //player.gameObject.GetComponent<MeshRenderer>().enabled = false;
        //player.gameObject.GetComponentInChildren<MeshRenderer>().material = goatMaterials[Instance.playerCount - 1];
        SkinnedMeshRenderer meshBody = player.gameObject.GetComponent<GoatSlingShot>().meshBody;
        meshBody.material = goatMaterials[Instance.playerCount - 1];

        MeshRenderer meshEye1_1 = player.gameObject.GetComponent<GoatSlingShot>().meshEye1_1;
        meshEye1_1.material = goatMaterials[Instance.playerCount - 1];

        meshEye1_1 = player.gameObject.GetComponent<GoatSlingShot>().meshEye1_2;
        meshEye1_1.material = goatMaterials[Instance.playerCount - 1];

        meshEye1_1 = player.gameObject.GetComponent<GoatSlingShot>().meshEye2_1;
        meshEye1_1.material = goatMaterials[Instance.playerCount - 1];

        meshEye1_1 = player.gameObject.GetComponent<GoatSlingShot>().meshEye2_2;
        meshEye1_1.material = goatMaterials[Instance.playerCount - 1];

        /*
        SkinnedMeshRenderer[] meshes = player.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < meshes.Length; i++)
        {
            //meshes[i].material = goatMaterials[Instance.playerCount - 1];
            meshes[i].enabled = false;
        }
        */
        GameObject[] body = player.gameObject.GetComponent<GoatSlingShot>().bodyVisibility;
        for (int i = 0; i < body.Length; i++)
        {
            body[i].SetActive(false);
        }

        GameObject[] trails = player.gameObject.GetComponent<GoatSlingShot>().trails;
        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].GetComponent<TrailRenderer>().startColor = goatMaterials[Instance.playerCount - 1].color;
        }


        player.gameObject.GetComponentInChildren<LineRenderer>().enabled = false;
        planes[Instance.playerCount - 1].GetComponentInChildren<Text>().enabled = true;
        planes[Instance.playerCount - 1].GetComponentInChildren<RawImage>().enabled = true;



        float RandXPos = xPosOfGoats[Random.Range(0,8)];
        while (checkAvailablePos(RandXPos) == false)        // give each goat random unique starting point
        {
            RandXPos = xPosOfGoats[Random.Range(0, 8)];
        }

        // Initialize goat position and the invisible ground pos to be below goat
        pos.position = new Vector3(RandXPos, pos.position.y, pos.position.z);
        player.gameObject.GetComponentInChildren<GoatSlingShot>().ground.transform.position = new Vector3(RandXPos, pos.position.y, pos.position.z);

        // Set each goat's index to the player number
        player.gameObject.GetComponentInChildren<GoatSlingShot>().playerIndex = Instance.playerCount;

        

        //Set both the goat and it's invisible platform to the right layer (P1 = Layer 16, P2 = Layer 17, etc)
        player.gameObject.layer = GoatIndexStart + Instance.playerCount;
        player.gameObject.GetComponentsInChildren<Transform>()[1].gameObject.layer = GoatIndexStart + Instance.playerCount;
        player.gameObject.GetComponentInChildren<GoatSlingShot>().ground.gameObject.layer = PlatformIndexStart + Instance.playerCount;

        if (player.devices.Count > 0)
        {
            //print(player.gameObject.GetComponent<GoatSlingShot>().playerIndex + "||" + player.devices.Count);
            player.gameObject.GetComponent<GoatSlingShot>().isAI = false;
        }
        else
        {
            //print(player.gameObject.GetComponent<GoatSlingShot>().playerIndex + " is AI");
            player.gameObject.GetComponent<GoatSlingShot>().isAI = true;
        }
    }

    private bool checkAvailablePos(float xValue)        // helper method that checks if a goat has already been instantiated at a specific x position
    {
        foreach (GameObject p in players)
        {
            if (Mathf.FloorToInt(p.GetComponent<Transform>().position.x) == Mathf.FloorToInt(xValue))
                return false;
        }
        return true;
    }

    public void DisableAllPlayers() // Destroys all goat objects and removes them from this script's list so it can be re-added when the game starts again
    {

        PlayerWin = true;

        foreach (GameObject p in players)
        {
            p.GetComponent<GoatSlingShot>().DestroyGoat(false);
        }
        players.Clear();
        
    }

    public int GetPlayerCount()
    {
        return Instance.playerCount;
    }

    public void WantToStartGame()
    {
        if (!startGame)
        {
            startGame = true;
        }
    }
}
