using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class ButtonActions : MonoBehaviour
{
    private NetworkManager NetworkManager;
    public TextMeshProUGUI txt;

    void Start()
    {
        NetworkManager = GetComponentInParent<NetworkManager>();
    }

    public void StartHost()
    {
        NetworkManager.StartHost();
        InýtMovementText();
    }

    public void StartClient()
    {
        NetworkManager.StartClient();
        InýtMovementText();
    }

    public void SubmitNewPosition()
    {
        var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        var player = playerObject.GetComponent<PlayerMovement>();
        player.Move();
    }

    public void InýtMovementText()
    {
        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
        {
            txt.text = "MOVE";
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            txt.text = "Request Move";
        }
    }
}
