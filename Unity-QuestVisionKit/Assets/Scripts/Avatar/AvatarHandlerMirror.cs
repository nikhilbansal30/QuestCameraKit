using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Networking.Types;

public class AvatarHandlerMirror : NetworkBehaviour
{
    public Transform head;
    public Transform leftHand, rightHand;
    private void Start()
    {
        if (isOwned)
            AvatarReference.instance.CurrentAvatar = this;
    }

    public void GrantAuthority(NetworkIdentity networkIdentity)
    {
        Debug.Log("Trying to assign authority");
        CmdGrantAuthority(networkIdentity);
    }


    [Command]
    public void CmdGrantAuthority(NetworkIdentity networkIdentity)
    {
        networkIdentity.RemoveClientAuthority();
        Debug.Log("Assigning authority");
        networkIdentity.AssignClientAuthority(connectionToClient);
    }

    public void SpawnAnnotation(Vector3 position)
    {
        Debug.Log(connectionToClient + " is trying to spawn a annotation");
        CmdSpawnAnnotation(position);
    }

    [Command]
    public void CmdSpawnAnnotation(Vector3 position)
    {
        RpcSpawnAnnotation(position);
    }

    [ClientRpc]
    public void RpcSpawnAnnotation(Vector3 position)
    {
        // AnnotationHandler.instance.CreateAnnotation(position);
    }



}


