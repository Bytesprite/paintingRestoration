using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalSpawner : MonoBehaviour{
    [SerializeField]
    Transform spawnParent;

    public void Spawn(Vector3 worldPosition, Vector2 size, Material material) {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Destroy(quad.GetComponent<Collider>());
        quad.GetComponent<Renderer>().material = material;
        quad.transform.position = worldPosition;
        quad.transform.localScale = new Vector3(size.x,size.y,1);

        if (spawnParent) {
            quad.transform.parent = spawnParent;
            quad.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
