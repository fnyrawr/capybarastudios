using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerVisuals : NetworkBehaviour
{
    private NetworkVariable<Color> _netColor = new();
    [SerializeField] private SkinnedMeshRenderer[] renderers;
    
    private void Awake() {
        _netColor.OnValueChanged += OnValueChanged;
    }

    private void OnValueChanged(Color prev, Color next) {
        foreach(var renderer in renderers) {
            renderer.material.color = next;
        }
    }

    public override void OnNetworkSpawn()
    {
        if(IsOwner) {
            Color random = Random.ColorHSV(0.5f, 0.75f);
            ColorServerRpc(random);
        } else {
            foreach(var renderer in renderers) {
                renderer.material.color = _netColor.Value;
            }
        }
    }

    [ServerRpc]
    private void ColorServerRpc(Color color) {
        _netColor.Value = color;
    }
}
