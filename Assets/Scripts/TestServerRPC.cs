using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TestServerRPC : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            TestServerRpc(0);
        }
    }

    [ClientRpc]
    void TestClientRpc(int value)
    {
        if (IsClient)
        {
            Debug.Log("Client Received the RPC #" + value);
            TestServerRpc(value + 1);
        }
    }

    [ServerRpc]
    void TestServerRpc(int value)
    {
        Debug.Log("Server Received the RPC #" + value);
        TestClientRpc(value);
    }
}
