using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TerrainSpawn : NetworkBehaviour
{
    bool doOnce = true;
    GameObject terrain;
    [SerializeField] GameObject terrainPrefab;



    // Update is called once per frame
    void Update()
    {
        var canPlaceCube = doOnce && isLocalPlayer && isServer && NetworkClient.isConnected && OVRInput.GetDown(OVRInput.RawButton.A);
        if (canPlaceCube)
            SpawnCube();
    }




    [Server]
    private void SpawnCube()
    {
        Debug.Log("Yeet");
        // if (!isServer) return;
        doOnce = false;
        terrain = Instantiate(terrainPrefab, transform.position, transform.rotation);
        NetworkServer.Spawn(terrain.gameObject, NetworkClient.localPlayer.connectionToClient);
        // var mirrorGrabbable = terrain.GetComponent<MirrorThowableObject>();
        // mirrorGrabbable.TransferOwnershipToLocalPlayer();
    }



}
