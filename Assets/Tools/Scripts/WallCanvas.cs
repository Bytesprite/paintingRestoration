using System.Collections.Generic;
using UnityEngine;

public class WallCanvas : MonoBehaviour{
    Renderer canvasRenderer;
    static List<WallCanvas> canvasesInScene = new List<WallCanvas>();
    static int paintingNumber = 0;

    private void Start() {
        canvasesInScene.Add(this);
        canvasRenderer = GetComponentInChildren<Renderer>();
        canvasRenderer.material.SetTexture("_MainTex", null);
    }

    public static void AddPainting(Texture2D painting) {
        if(paintingNumber >= canvasesInScene.Count) {
            return;
        }

        canvasesInScene[paintingNumber].SetTexture(painting);
    }

    void SetTexture(Texture2D texture) {
        if (canvasRenderer) {
            canvasRenderer.material.SetTexture("_MainTex", texture);
        }
        paintingNumber++;
    }

    public static void ResetPaintingNumber() {
        paintingNumber = 0;
    }
}
