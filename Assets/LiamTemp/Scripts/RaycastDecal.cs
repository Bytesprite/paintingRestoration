using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDecal : MonoBehaviour{
    [SerializeField]
    LayerMask layerMask = new LayerMask();
    [SerializeField]
    Vector2 size = new Vector2(1,1);
    [SerializeField]
    Material material;
    Color color;

    void Update(){
        if (Input.GetMouseButtonDown(0)) {
            FireRay();
        }
    }

    void FireRay() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, layerMask)) {
            DecalSpawner decalSpawner = hit.collider.gameObject.GetComponent<DecalSpawner>();
            if (decalSpawner) {
                Material materialInstance = new Material(material);
                color = new Color(Random.value*2, Random.value*2, Random.value*2);
                materialInstance.SetColor("_Color", color);
                decalSpawner.Spawn(hit.point, size, materialInstance);
            }
        }
    }
}
