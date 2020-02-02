using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolShot : MonoBehaviour{
    protected Brush brush;
    protected Material material;

    public void SetValues(Brush brush, Material material) {
        this.brush = brush;
        this.material = material;
    }

    protected void PrepareDecalForSpawning(DecalSpawner spawner, Vector3 spawnPosition) {
        float rotation = Random.Range(0, 360);
        Vector2 scaledSize = brush.size * Random.Range(0.7f, 1.3f);
        spawner.Spawn(spawnPosition, scaledSize, rotation, material, brush.decal);
    }
}
