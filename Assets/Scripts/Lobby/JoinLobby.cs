using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;

public class JoinLobby : MonoBehaviour
{
    public TMP_InputField joinCodeInputField;

    public async void JoinLobbyWihtLobbyCode(string lobbyCode)
    {
        var code = joinCodeInputField.text;

        try
        {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
            options.Player = new Player(AuthenticationService.Instance.PlayerId);
            options.Player.Data = new Dictionary<string, PlayerDataObject>()
            {
                {"PlayerLevel", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "8")}
            };

            Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);     
            Debug.Log("Join Lobby With Code : " + code);
            DontDestroyOnLoad(this);
            GetComponent<CurrentLobby>().currentLobby = lobby;
            LobbyStatic.LogPlayerInLobby(lobby);
            LobbyStatic.LoadLobbyRoom();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinLobbyWihtLobbyID(string lobbyID)
    {        
        try
        {
            JoinLobbyByIdOptions options = new JoinLobbyByIdOptions();
            options.Player = new Player(AuthenticationService.Instance.PlayerId);
            options.Player.Data = new Dictionary<string, PlayerDataObject>()
            {
                {"PlayerLevel", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "8")}
            };

            Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyID, options);
            Debug.Log("Join Lobby With ID : " + lobbyID);
            Debug.LogWarning("Lobby Code : " + lobby.LobbyCode);
            DontDestroyOnLoad(this);
            GetComponent<CurrentLobby>().currentLobby = lobby;
            LobbyStatic.LogPlayerInLobby(lobby);
            LobbyStatic.LoadLobbyRoom();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void QuickJoinMethod()
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            Debug.Log("Join Lobby With Quick Join : " + lobby.Id);
            Debug.LogWarning("Lobby Code : " + lobby.LobbyCode);
            DontDestroyOnLoad(this);
            GetComponent<CurrentLobby>().currentLobby = lobby;
            LobbyStatic.LoadLobbyRoom();
        }
        catch (LobbyServiceException e)
        {

            Debug.Log(e);
        }
    }
}
