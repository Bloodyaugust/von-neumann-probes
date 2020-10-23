using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkedVar;
using TMPro;

public class SessionDataController : NetworkedBehaviour {
    NetworkedVarInt EnemiesKilled;
    TMP_Text EnemiesKilledText;
    Toolbox toolbox;

    public override void NetworkStart() {
        base.NetworkStart();

        if (NetworkingManager.Singleton.IsHost) {
            toolbox = Toolbox.Instance;

            toolbox.EnemyDied += OnEnemyDied;
        }

        EnemiesKilled.OnValueChanged = OnEnemiesKilledValueChanged;
    }

    void Awake() {
        EnemiesKilled = new NetworkedVarInt(new NetworkedVarSettings {SendTickrate = 1}, 0);
        EnemiesKilledText = GameObject.Find("Canvas/SessionData/EnemiesKilled").GetComponent<TMP_Text>();
    }

    void OnEnemiesKilledValueChanged(int oldValue, int newValue) {
        EnemiesKilledText.text = $"Enemies Killed: {newValue}";
    }

    void OnEnemyDied() {
        EnemiesKilled.Value++;
    }
}
