using System.IO;
using UnityEngine;

//Push SPACE to render an PNG to the project folder under Renders
public class RenderImage : MonoBehaviour{
    [SerializeField]
    Camera renderCamera;
    [SerializeField]
    string fileName;
    int renderNumber = 0;
    string prefsKey = "RenderNumber";

    public void ExportImage() {
        Debug.Log("Exporting...");
        if(PlayerPrefs.HasKey(prefsKey)){
            renderNumber = PlayerPrefs.GetInt(prefsKey) + 1;
        }

        byte[] bytes = Render().EncodeToPNG();
        if(!Directory.Exists(Application.dataPath + "/Renders")) {
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
        Debug.Log("Render exported to: " + Application.dataPath + "/Renders/" + fileName + renderNumber);

    }


    public Texture2D Render() {
        Texture2D render = new Texture2D(1920, 1080, TextureFormat.ARGB32, false);
        RenderTexture.active = renderCamera.targetTexture;
        render.ReadPixels(new Rect(0, 0, renderCamera.targetTexture.width, renderCamera.targetTexture.height),0,0);
        render.Apply();
        return render;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)){
            renderCamera.GetComponent<SetMaterialsUnlitBeforeRender>().SetShaderSwitchFlag(ExportImage);
        }
    }
}
