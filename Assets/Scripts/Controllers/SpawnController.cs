using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class SpawnController : MonoBehaviour {
    public float SpawnInterval;
    public GameObject EnemyPrefab;
    public HostJoinController HostJoinInstance;

    bool spawning = false;
    float timeToSpawn;

    void Awake() {
        timeToSpawn = SpawnInterval;

        HostJoinInstance.HostGame += OnHostGame;
    }

    void OnClientConnected(ulong clientID) {
        if (!NetworkingManager.Singleton.IsHost) {
            this.enabled = false;
        }
    }

    void OnHostGame() {
        spawning = true;
    }

    void Start() {
        NetworkingManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    void Update() {
        if (spawning) {
            timeToSpawn -= Time.deltaTime;

            if (timeToSpawn <= 0) {
                timeToSpawn = SpawnInterval;

                GameObject newEnemy = Instantiate(EnemyPrefab, (Vector3)(Random.insideUnitCircle * 5), Quaternion.identity);

                newEnemy.GetComponent<NetworkedObject>().Spawn();
            }
        }
    }
}
