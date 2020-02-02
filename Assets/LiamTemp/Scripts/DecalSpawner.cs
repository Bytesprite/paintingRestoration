using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalSpawner : MonoBehaviour{
    [SerializeField]
    Transform spawnParent;
    int spriteNumber = 0;

    public void Spawn(Vector3 worldPosition, Vector2 size, float rotation, Material material, Sprite spriteDecal) {
        GameObject sprite = new GameObject("Paint", new System.Type[] { typeof (SpriteRenderer)});
        sprite.GetComponent<SpriteRenderer>().material = material;
        sprite.GetComponent<SpriteRenderer>().sprite = spriteDecal;
        sprite.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        sprite.GetComponent<SpriteRenderer>().sortingOrder = spriteNumber;
        spriteNumber++;
        sprite.transform.position = worldPosition;
        sprite.transform.localScale = new Vector3(size.x * 0.1f,size.y * 0.1f, 1);

        if (spawnParent)
            sprite.transform.parent = spawnParent;

        sprite.transform.localRotation = Quaternion.Euler(0, 0, rotation);

        if (spriteNumber >= 10) {

        }
    }
}
