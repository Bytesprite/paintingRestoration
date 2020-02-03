using System.Collections.Generic;
using UnityEngine;

public class DecalSpawner : MonoBehaviour{
    [SerializeField]
    Transform spawnParent;
    int spriteNumber = 0;
    int lineNumber = 0;
    List<GameObject> currentDecals = new List<GameObject>();

    private void OnEnable() {
        RoundController.OnRoundCleanup += DestroyCurrentDecals;
    }

    private void OnDisable() {
        RoundController.OnRoundCleanup -= DestroyCurrentDecals;
    }

    public void Spawn(Vector3 worldPosition, Vector2 size, float rotation, Material material, Sprite spriteDecal) {
        GameObject sprite = new GameObject("Paint", new System.Type[] { typeof (SpriteRenderer)});
        sprite.GetComponent<SpriteRenderer>().material = material;
        sprite.GetComponent<SpriteRenderer>().sprite = spriteDecal;
        sprite.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        sprite.GetComponent<SpriteRenderer>().sortingOrder = spriteNumber;
        spriteNumber++;
        sprite.transform.position = worldPosition;
        sprite.transform.localScale = new Vector3(size.x * 0.1f,size.y * 0.1f, 1);

        if (spawnParent) {
            sprite.transform.parent = spawnParent;
            sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x, sprite.transform.localPosition.y, 0);
        }

        sprite.transform.localRotation = Quaternion.Euler(0, 180, rotation);
        currentDecals.Add(sprite);

        if (spriteNumber >= 10) {
            //Perhaps flatten the decals here for optimisation purposes
        }
    }
    public void Spawn(Material material, LineRenderer line) {
        lineNumber++;

        if (spawnParent) {
            line.transform.parent = spawnParent;
        }

        currentDecals.Add(line.gameObject);

        if (lineNumber >= 10) {
            //Perhaps flatten the decals here for optimisation purposes
        }
    }

    public void DestroyCurrentDecals() {
        foreach (GameObject decal in currentDecals) {
            Destroy(decal);
        }
        currentDecals.Clear();
    }
}
