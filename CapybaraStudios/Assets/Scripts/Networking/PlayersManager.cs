using Unity.Netcode;
using My.Core.Singletons;
using UnityEngine;

public class PlayersManager : NetworkSingleton<PlayersManager> {
    private NetworkVariable<int> players = new NetworkVariable<int>();

    public int PlayersInGame 
    {
        get {
            return players.Value;
        }
    }
    private void Start() {
        Debug.Log("aaa");
        NetworkManager.Singleton.OnClientConnectedCallback += (id) => {
            if(NetworkManager.Singleton.IsServer) {
                players.Value++;
            }
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) => {
            if(NetworkManager.Singleton.IsServer) {
                players.Value--;
            }
        };
    }
}