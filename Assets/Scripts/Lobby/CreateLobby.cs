using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;

public class CreateLobby : MonoBehaviour
{
    public TMP_InputField lobbyName;
    public TMP_Dropdown maxPlayers;
    public TMP_Dropdown gameMode;
    public Toggle isLobbyPrivate;

    public TMP_Text joinCodeText;

    public async void CreateLobbyMethod()
    {
        string lobby_name = lobbyName.text;
        int max_players = Convert.ToInt32(maxPlayers.options[maxPlayers.value].text);
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = isLobbyPrivate.isOn;


        // Player Creation
        options.Player = new Player(AuthenticationService.Instance.PlayerId);
        options.Player.Data = new Dictionary<string, PlayerDataObject>()
        {
            {"PlayerLevel", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "5")}
        };

        // Lobby Data
        options.Data = new Dictionary<string, DataObject>()
        {
            {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode.options[gameMode.value].text, DataObject.IndexOptions.S1)}
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobby_name, max_players, options);
        GetComponent<CurrentLobby>().currentLobby = lobby;
        DontDestroyOnLoad(this);
        Debug.Log("Create Lobby Done!");

        LobbyStatic.LogLobby(lobby);
        LobbyStatic.LogPlayerInLobby(lobby);
        joinCodeText.text = lobby.LobbyCode;

        StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15f));

        LobbyStatic.LoadLobbyRoom();
    }

    IEnumerator HeartbeatLobbyCoroutine(string lobbyID, float waitTimeSeconds)
    {
        var delay = new WaitForSeconds(waitTimeSeconds);
        while (true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyID);
            yield return delay;
        }
    }
}
