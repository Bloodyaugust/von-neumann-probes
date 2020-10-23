using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MLAPI;

public class PlayerMovement : MonoBehaviour {
    public float Speed;
    public InputAction WASD;
    public InputAction Look;

    bool lookUsingMouse = false;
    NetworkedObject playerNetworkedObject;
    Vector2 lastLookVector;
    Vector2 lastMouseScreenPosition;

    void OnDisable() {
        WASD.Disable();
        Look.Disable();
    }

    void OnEnable() {
        WASD.Enable();
        Look.Enable();
    }

    void Start() {
        playerNetworkedObject = GetComponent<NetworkedObject>();
    }

    void Update() {
        if (playerNetworkedObject.IsLocalPlayer) {
            Vector2 lookVector = Look.ReadValue<Vector2>();
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

            transform.Translate(WASD.ReadValue<Vector2>() * Time.deltaTime * Speed, Space.World);

            if (!lookUsingMouse && lastMouseScreenPosition != mouseScreenPosition) {
                lookUsingMouse = true;
            }

            if (lookVector.sqrMagnitude > 0) {
                lookUsingMouse = false;
                lastLookVector = lookVector;
            }

            if (lookUsingMouse) {
                transform.rotation = Quaternion.LookRotation(Vector3.forward, mouseWorldPosition - transform.position);
            } else {
                transform.rotation = Quaternion.LookRotation(Vector3.forward, (transform.position + (Vector3)lastLookVector) - transform.position);
            }

            lastMouseScreenPosition = mouseScreenPosition;
        }
    }
}
