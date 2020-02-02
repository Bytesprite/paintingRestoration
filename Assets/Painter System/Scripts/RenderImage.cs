using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class RenderImage : MonoBehaviour{
    [SerializeField]
    Camera renderCamera;
    [SerializeField]
    string fileName;
    int renderNumber = 0;
    string prefsKey = "RenderNumber";

    List<Texture2D> masterpieces = new List<Texture2D>();

    private void OnEnable() {
        RoundController.OnRoundEnd += RoundEnd;
        RoundController.OnGameEnd += ExportImages;
    }

    private void OnDisable() {
        RoundController.OnRoundEnd -= RoundEnd;
    }

    public void ExportImages() {;
        foreach (Texture2D painting in masterpieces) {
            if (PlayerPrefs.HasKey(prefsKey)) {
                renderNumber = PlayerPrefs.GetInt(prefsKey) + 1;
            }

            //This is a heavy statement & causes a fram stutter; for optimisation could save all textures and Export them at end of game
            byte[] bytes = painting.EncodeToPNG();
            if (!Directory.Exists(Application.dataPath + "/Renders")) {
                Directory.CreateDirectory(Application.dataPath + "/Renders");
            }

            try {
                File.WriteAllBytes(Application.dataPath + "/Renders/" + fileName + renderNumber + ".png", bytes);
            }
            catch (System.Exception ex) {
                Debug.LogError(ex);
                return;
            }
            PlayerPrefs.SetInt(prefsKey, renderNumber);
            PlayerPrefs.Save();
        }
    }


    public void RenderAndStore() {
        Texture2D render = new Texture2D(1920, 1080, TextureFormat.ARGB32, false);
        RenderTexture.active = renderCamera.targetTexture;
        render.ReadPixels(new Rect(0, 0, renderCamera.targetTexture.width, renderCamera.targetTexture.height),0,0);
        render.Apply();

        masterpieces.Add(render);
    }

    private void RoundEnd() {
        renderCamera.GetComponent<SetMaterialsUnlitBeforeRender>().SetShaderSwitchFlag(RenderAndStore);
    }
}
