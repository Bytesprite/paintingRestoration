using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ToolProjectile : ToolShot{
    [SerializeField]
    float speed = 1;

    private void Start() {
        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>()) {
            renderer.material.SetColor("_Color", material.GetColor("_Color"));
        }
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        Destroy(gameObject, 2.0f);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("DecalPlane")) {
            DecalSpawner decalSpawner = other.gameObject.GetComponent<DecalSpawner>();
            if (decalSpawner)
                PrepareDecalForSpawning(decalSpawner, other.contacts[0].point);
        }
        Destroy(gameObject);
    }
}
