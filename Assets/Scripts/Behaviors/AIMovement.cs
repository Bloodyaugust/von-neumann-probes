using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class AIMovement : MonoBehaviour {
    GameObject targetPlayer;

    void Awake() {
        if (!NetworkingManager.Singleton.IsHost) {
            this.enabled = false;
        }
    }

    void Update() {
        if (targetPlayer == null) {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            targetPlayer = players[Random.Range(0, players.Length)];
        } else {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, targetPlayer.transform.position - transform.position);
        }
    }
}
