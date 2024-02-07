using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class GetLobbies : MonoBehaviour
{
    public GameObject buttonsContainer;
    public GameObject lobbyButtonPrefab;
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
            
    }
    public async void GetLobbiesTest()
    {
        ClearContainer();

        try
        {
            QueryLobbiesOptions options = new();
            Debug.LogWarning("QueryLobbiesTest");
            
            options.Count = 25;

            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)               
            };

            options.Order = new List<QueryOrder>()
            {
                new QueryOrder(true, QueryOrder.FieldOptions.Created)
            };

            ////Filter for open lobbies only 
            //options.Filters = new List<QueryFilter>()
            //{
            //    new QueryFilter(
            //        field: QueryFilter.FieldOptions.AvailableSlots,
            //        op: QueryFilter.OpOptions.GT,
            //        value: "0")
            //};

            //// Order by newest lobbies first
            //options.Order = new List<QueryOrder>()
            //{
            //    new QueryOrder(
            //        asc: false,
            //        field: QueryOrder.FieldOptions.Created)
            //};

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
            Debug.LogWarning("Get Lobbies Done COUNT: " + lobbies.Results.Count);


            foreach (Lobby foundLobby in lobbies.Results)
            {
                LobbyStatic.LogLobby(foundLobby);
                CreateLobbyButton(foundLobby);
            }

            //foreach (Lobby foundLobby in lobbies.Results)
            //{
            //    Debug.Log("Lobby Name : " + foundLobby.Name + "\n" + 
            //              "Lobby Date : " + foundLobby.Created + "\n" + 
            //              "Lobby Code : " + foundLobby.LobbyCode + "\n" +
            //              "Lobby ID : " + foundLobby.Id);
            //}

            ///GetComponent<JoinLobby>().JoinLobbyWihtLobbyID(lobbies.Results[0].Id);

        }
        catch (LobbyServiceException e)
        {

            Debug.Log(e);
        }
    }

    private void CreateLobbyButton(Lobby lobby)
    {
        var button = Instantiate(lobbyButtonPrefab, Vector3.zero, Quaternion.identity);
        button.name = lobby.Name + "_Button";
        button.GetComponentInChildren<TextMeshProUGUI>().text = lobby.Name;
        var recTransform = button.GetComponent<RectTransform>();
        recTransform.SetParent(buttonsContainer.transform);
        button.GetComponent<Button>().onClick.AddListener(delegate () { Lobby_OnClick(lobby); });
    }

    public void Lobby_OnClick(Lobby lobby)
    {
        Debug.Log("Clicked Lobby : " + lobby.Name);
        GetComponent<JoinLobby>().JoinLobbyWihtLobbyID(lobby.Id);
    }

    private void ClearContainer()
    {
        if (buttonsContainer is not null && buttonsContainer.transform.childCount > 0)
        {
            foreach (Transform item in buttonsContainer.transform)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
