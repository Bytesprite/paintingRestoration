using UnityEngine;

public class RaycastDecal : MonoBehaviour {
    [SerializeField]
    LayerMask layerMask = new LayerMask();
    [SerializeField]
    Vector2 size = new Vector2(1, 1);
    [SerializeField]
    Material material;
    [SerializeField]
    Brush[] brushes;
    Color color;

    void LateUpdate() {
        if (Input.GetMouseButtonDown(0)) {
            FireRay();
        }
    }

    void FireRay() {
        if (!Camera.main) {
            Debug.LogError("Where your main camera at!?\nSomething needs to be tagged as MainCamera");
            return;
        }
           
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, layerMask)) {
            DecalSpawner decalSpawner = hit.collider.gameObject.GetComponent<DecalSpawner>();
            if (decalSpawner) {
                Material materialInstance = new Material(material);
                float rotation = Random.Range(0, 360);
                Brush randomBrush = brushes[Random.Range((int)0, (int)brushes.Length)];
                Vector2 scaledSize = randomBrush.size * Random.Range(0.7f, 1.3f);
                color = new Color(Random.value * 2, Random.value * 2, Random.value * 2);
                materialInstance.SetColor("_Color", color);
                decalSpawner.Spawn(hit.point, scaledSize, rotation, materialInstance, randomBrush.decal);
            }
        }
    }

    [System.Serializable]
    struct Brush{
        public Sprite decal;
        public Vector2 size;
    }
}

