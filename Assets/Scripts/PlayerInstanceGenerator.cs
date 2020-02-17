using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInstanceGenerator : MonoBehaviour
{
    PlayerInputManager Instance;
    private readonly int GoatIndexStart = 13;
    private readonly int PlatformIndexStart = 7;
    private bool changed;
    public static PlayerInstanceGenerator instanceOBJ;

    //private float timer;          //any comments related to timer or timeLimit variable is for having game start in 10 seconds mode. Not Needed right now
    //[SerializeField] private int timeLimit = 10;

    private bool startGame;

    public bool PlayerWin;
    public List<GameObject> players = new List<GameObject>();
    public Material[] goatMaterials = new Material[6];
    public GameObject[] planes = new GameObject[6];
    public Material[] planeMaterials = new Material[2];

    public Text TitleScreen;

    private readonly float[] xPosOfGoats = {-15, -9, -3, 3, 9 ,15};

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

            for (int i = 0; i < planes.Length; i++)
            {
                planes[i] = GameObject.Find("PlaneP" + (i+1));      // This is UI stuff, find all the panels so when goat selected, a white square will form
            }
            TitleScreen = GameObject.Find("Timer").GetComponent<Text>();    // find reference to text obj that is the countdown timer
        }
    }

    private void OnPlayerJoined(PlayerInput player)     // this is called whenever a player joins the game via the start button
    {
        players.Add(player.gameObject);         // add all joined players to a list so can reference later
        TitleScreen.text = "PRESS B or SPACE TO START GAME";
        GetComponent<RankingSystem>().addGoat(player.gameObject);

        Transform pos = player.gameObject.GetComponentInChildren<Transform>();

        float RandXPos = xPosOfGoats[Random.Range(0,6)];

        while (checkAvailablePos(RandXPos) == false)        // give each goat random unique starting point
        {
            RandXPos = xPosOfGoats[Random.Range(0, 6)];
        }

        // Initialize goat position and the invisible ground pos to be below goat
        pos.position = new Vector3(RandXPos, pos.position.y, pos.position.z);
        player.gameObject.GetComponentInChildren<GoatSlingShot>().ground.transform.position = new Vector3(RandXPos, pos.position.y, pos.position.z);

        // Set each goat's index to the player number
        player.gameObject.GetComponentInChildren<GoatSlingShot>().playerIndex = Instance.playerCount;

        //Set goat's colour based on player count (P1 = red, P2 = blue, etc) and change panel material to a white square to show that it is selected 
        player.gameObject.GetComponentInChildren<MeshRenderer>().material = goatMaterials[Instance.playerCount - 1];
        planes[Instance.playerCount - 1].GetComponent<MeshRenderer>().material = planeMaterials[1];
        planes[Instance.playerCount - 1].GetComponentInChildren<Text>().text = "P" + Instance.playerCount;

        //Set both the goat and it's invisible platform to the right layer (P1 = Layer 14, P2 = Layer 15, etc)
        player.gameObject.layer = GoatIndexStart + Instance.playerCount;
        player.gameObject.GetComponentsInChildren<Transform>()[1].gameObject.layer = GoatIndexStart + Instance.playerCount;
        player.gameObject.GetComponentInChildren<GoatSlingShot>().ground.gameObject.layer = PlatformIndexStart + Instance.playerCount;

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
        startGame = true;
    }
}
