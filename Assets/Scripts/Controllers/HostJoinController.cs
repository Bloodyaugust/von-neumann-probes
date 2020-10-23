using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MLAPI;
using MLAPI.Logging;

public class HostJoinController : MonoBehaviour {
    public Action HostGame;

    public Button HostButton;
    public Button JoinButton;
    public GameObject SessionDataControllerPrefab;
    public TMP_InputField IPInput;
    public MLAPI.Transports.UNET.UnetTransport UNETTransport;

    float connectionTimeout = 5;
    float timeToConnect = 5;
    MLAPI.Transports.Tasks.SocketTasks socketTasks;
    RectTransform rectTransform;

    public void HandleHostClick() {
        NetworkingManager.Singleton.StartHost();
        
        GameObject newSessionDataController = Instantiate(SessionDataControllerPrefab, Vector3.zero, Quaternion.identity);
        newSessionDataController.GetComponent<NetworkedObject>().Spawn();

        HostGame?.Invoke();
        Hide();
    }
    
    public void HandleJoinClick() {
        UNETTransport.ConnectAddress = IPInput.text;

        socketTasks = NetworkingManager.Singleton.StartClient();

        if (socketTasks.IsDone && (!socketTasks.Success || !NetworkingManager.Singleton.IsConnectedClient)) {
            Debug.Log("Couldn't connect to server!");
            NetworkingManager.Singleton.StopClient();
            socketTasks = null;
        } else {
            DisableButtons();
        }
    }

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    void OnClientConnectedCallbackHandler(ulong clientID) {
        NetworkLog.LogInfoServer($"Connected {clientID}!");
        timeToConnect = connectionTimeout;
        socketTasks = null;

        EnableButtons();
        Hide();
    }

    void OnClientDisconnectCallbackHandler(ulong clientID) {
        timeToConnect = connectionTimeout;
        NetworkLog.LogInfoServer($"Disconnected {clientID}!");
    }

    void Start() {
        NetworkingManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallbackHandler;
        NetworkingManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallbackHandler;
    }

    void DisableButtons() {
        JoinButton.interactable = false;
        HostButton.interactable = false;
    }

    void EnableButtons() {
        JoinButton.interactable = true;
        HostButton.interactable = true;
    }

    void Hide() {
        rectTransform.anchoredPosition = new Vector3(0, 1080, 0);
        IPInput.interactable = false;
    }

    void Update() {
        if (socketTasks != null) {
            timeToConnect -= Time.deltaTime;

            if (timeToConnect <= 0) {
                Debug.Log("Failed to connect!");
                NetworkingManager.Singleton.StopClient();
                timeToConnect = connectionTimeout;
                socketTasks = null;

                EnableButtons();
            }
        }
    }
}
