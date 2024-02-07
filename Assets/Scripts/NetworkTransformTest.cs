using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkTransformTest : NetworkBehaviour
{
    void Update()
    {
        if (IsOwner && IsServer)
        {
            transform.RotateAround(GetComponent<Transform>().position, Vector3.up, 100f * Time.deltaTime);
        }
    }
}
