using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MLAPI;

public class VirtualCamPlayerFollow : MonoBehaviour {
    CinemachineVirtualCamera virtualCamera;
    GameObject followObject;
    NetworkingManager networkingManager;

    void Awake() {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Start() {
        networkingManager = NetworkingManager.Singleton;
    }

    void Update() {
        if (followObject == null) {
            NetworkedObject playerNetworkedObject = null;

            if (networkingManager.ConnectedClients.ContainsKey(networkingManager.LocalClientId)) {
                playerNetworkedObject = networkingManager.ConnectedClients[networkingManager.LocalClientId].PlayerObject;
            }

            if (playerNetworkedObject != null) {
                followObject = playerNetworkedObject.gameObject;

                virtualCamera.Follow = followObject.transform;
            }
        }
    }
}
