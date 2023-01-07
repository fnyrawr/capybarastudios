using System.Collections;
using System.Collections.Generic;
using My.Core.Singletons;
using UnityEngine;

public class M_Camera : Singleton<M_Camera>
{
    [HideInInspector] public Camera _camera;
    private Transform _cameraTransform;
    private void Awake() {
        _camera = GetComponent<Camera>();
        _cameraTransform = _camera.transform;
    }

    public void AttachToPlayer(Transform parent) {
        _cameraTransform.SetParent(parent);
    }
}
