using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInstanceGenerator : MonoBehaviour
{
    PlayerInputManager Instance;
    private readonly int GoatIndexStart = 13;
    private readonly int PlatformIndexStart = 7;

    private void Awake()
    {
        Instance = GetComponent<PlayerInputManager>();
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnPlayerJoined(PlayerInput player)
    {
        Transform pos = player.gameObject.GetComponentInChildren<Transform>();
        pos.position = new Vector3(-15 + (5 * (Instance.playerCount-1)), pos.position.y, pos.position.z);
        player.gameObject.GetComponentInChildren<GoatSlingShot>().ground.transform.position = new Vector3(-15 + (5 * (Instance.playerCount - 1)), pos.position.y, pos.position.z);

        player.gameObject.GetComponentInChildren<GoatSlingShot>().playerIndex = Instance.playerCount;
        player.gameObject.layer = GoatIndexStart + Instance.playerCount;
        player.gameObject.GetComponentsInChildren<Transform>()[1].gameObject.layer = GoatIndexStart + Instance.playerCount;
        player.gameObject.GetComponentInChildren<GoatSlingShot>().ground.gameObject.layer = PlatformIndexStart + Instance.playerCount;

    }
}
