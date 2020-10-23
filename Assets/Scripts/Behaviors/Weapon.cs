using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.LagCompensation;

public class Weapon : NetworkedBehaviour {
    public GameObject ProjectilePrefab;
    public int Team;
    public SOWeapon WeaponData;

    bool loaded;
    float timeToReload;

    [MLAPI.Messaging.ServerRPC]
    public void Fire() {
        if (loaded) {
            loaded = false;
            timeToReload = WeaponData.reload;

            if (NetworkingManager.Singleton.IsHost) {
                SpawnProjectile();
            } else {
                InvokeServerRpc(Fire);
            }
        }
    }

    void SpawnProjectile() {
        GameObject newProjectile = Instantiate(ProjectilePrefab, transform.position, transform.rotation);
        Projectile projectile = newProjectile.GetComponent<Projectile>();

        projectile.ProjectileData = WeaponData.projectile;
        projectile.Team = Team;

        newProjectile.GetComponent<NetworkedObject>().Spawn();
    }

    void Start() {
        loaded = false;
        timeToReload = WeaponData.reload;
    }

    void Update() {
        if (!loaded) {
            timeToReload -= Time.deltaTime;

            if (timeToReload <= 0) {
                timeToReload = 0;
                loaded = true;
            }
        }
    }
}
