using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolRay : ToolShot{
    private void Start() {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, LayerMask.NameToLayer("DecalPlane"))) {
            DecalSpawner decalSpawner = hit.collider.gameObject.GetComponent<DecalSpawner>();
            if(decalSpawner)
                PrepareDecalForSpawning(decalSpawner, hit.point);
        }
        Destroy(gameObject);
    }
}
