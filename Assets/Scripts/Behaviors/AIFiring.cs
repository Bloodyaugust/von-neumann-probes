using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class AIFiring : MonoBehaviour {
    Weapon weapon;

    void Awake() {
        if (!NetworkingManager.Singleton.IsHost) {
            this.enabled = false;
        }

        weapon = GetComponent<Weapon>();
    }

    void Update() {
        weapon.Fire();
    }
}
