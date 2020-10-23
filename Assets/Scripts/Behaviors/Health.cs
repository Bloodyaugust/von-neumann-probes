using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class Health : MonoBehaviour {
    public float Hitpoints;
    public int Team;

    Toolbox toolbox;

    public bool Damage(int team, float amount) {
        if (team != Team) {
            Hitpoints -= amount;
            return true;
        }

        return false;
    }

    void Awake() {
        if (!NetworkingManager.Singleton.IsHost) {
            this.enabled = false;
        } else {
            toolbox = Toolbox.Instance;
        }
    }

    void Update() {
        if (Hitpoints <= 0) {
            if (Team == 1) {
                toolbox.EnemyDied?.Invoke();
            }

            Destroy(gameObject);
        }
    }
}
