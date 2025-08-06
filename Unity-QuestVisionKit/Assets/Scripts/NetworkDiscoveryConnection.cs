using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;
using System;
using System.Linq;

public class NetworkDiscoveryConnection : MonoBehaviour
{

    public NetworkDiscovery networkDiscovery;
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();



    private void Start()
    {
        if (networkDiscovery == null)
        {
            networkDiscovery = GetComponent<NetworkDiscovery>();
            // Unity    Editor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
            // UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
        }
    }


    public void OnDiscoveredServer(ServerResponse info)
    {
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        discoveredServers[info.serverId] = info;
    }


    public void FindServers()
    {
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
    }

    // LAN Host
    public void StartHost()
    {
        discoveredServers.Clear();
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();
        PositionObject.instance.BroadcastDebug("Starting server");

    }



    public void StartClient()
    {
        FindServers();

        StartCoroutine(DoWithDelay(() =>
        {
            if (discoveredServers.Count > 0)
                // Connect(discoveredServers[0]);
                Connect(discoveredServers.FirstOrDefault().Value);
            else
                PositionObject.instance.BroadcastDebug("No sever to connect to");
        }, 2f));

    }

    void Connect(ServerResponse info)
    {
        networkDiscovery.StopDiscovery();
        NetworkManager.singleton.StartClient(info.uri);
        PositionObject.instance.BroadcastDebug("Trying to connect to " + info.uri);

    }


    IEnumerator DoWithDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

}
