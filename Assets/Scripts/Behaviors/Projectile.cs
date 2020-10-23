using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class Projectile : MonoBehaviour {
    public SOProjectile ProjectileData;
    public int Team;

    float distanceTravelled = 0;

    void Awake() {
        GetComponent<SpriteRenderer>().sprite = ProjectileData.sprite;

        if (!NetworkingManager.Singleton.IsHost) {
            this.enabled = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (NetworkingManager.Singleton.IsHost) {
            Health health = collision.gameObject.GetComponent<Health>();

            if (health != null) {
                if (health.Damage(Team, ProjectileData.damage)) {
                    Destroy(gameObject);
                }
            }
        }
    }

    void Start() {
        if (NetworkingManager.Singleton.IsHost) {
            GetComponent<Rigidbody2D>().WakeUp();
        }
    }

    void Update() {
        Vector3 movementVector = Vector3.up * ProjectileData.speed * Time.deltaTime;

        transform.Translate(movementVector, Space.Self);
        distanceTravelled += movementVector.magnitude;

        if (distanceTravelled >= ProjectileData.range) {
            Destroy(gameObject);
        }
    }
}
