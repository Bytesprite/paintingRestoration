using System.IO;
using UnityEngine;

//Push SPACE to render an PNG to the project folder under Renders
public class RenderImage : MonoBehaviour{
    [SerializeField]
    RenderTexture renderTexture;
    [SerializeField]
    string fileName;
    int renderNumber = 0;
    string prefsKey = "RenderNumber";

    public void ExportImage() {
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
        Debug.Log("Render exported to: " + Application.dataPath + "/Renders/" + fileName);

    }

    public Texture2D Render() {
        Texture2D render = new Texture2D(1920, 1080, TextureFormat.ARGB32, false);
        RenderTexture.active = renderTexture;
        render.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height),0,0);
        render.Apply();
        return render;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)){
            ExportImage();
        }
    }
}
