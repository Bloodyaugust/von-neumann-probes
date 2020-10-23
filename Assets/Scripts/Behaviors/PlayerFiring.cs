using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MLAPI;

public class PlayerFiring : MonoBehaviour {
    public InputAction Fire;

    NetworkedObject playerNetworkedObject;
    Weapon weapon;

    void Awake() {
        playerNetworkedObject = GetComponent<NetworkedObject>();
        weapon = GetComponent<Weapon>();

        Fire.performed += OnFire;
    }

    void OnDisable() {
        Fire.Disable();
    }

    void OnEnable() {
        Fire.Enable();
    }

    void OnFire(InputAction.CallbackContext context) {
        if (playerNetworkedObject.IsLocalPlayer) {
            weapon.Fire();
        }
    }

    void Update() {
        if (Fire.ReadValue<float>() > 0) {
            weapon.Fire();
        }
    }
}
