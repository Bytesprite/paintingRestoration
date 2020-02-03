using UnityEngine;
using UnityEngine.UI;

public class ColorController : MonoBehaviour {
    [SerializeField]
    Image currentImage;
    [SerializeField]
    Image upImage;
    [SerializeField]
    Image downImage;

    static int currentColorIndex = 0;
    static bool colorCanSwap = true;
    static float colorSwapCooldown = 0.2f;
    static Color[] colors = new Color[] {
        Color.red, Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta, Color.white, Color.black
    };

    public static Color GetCurrentColor() {
        return colors[currentColorIndex];
    }

    void ProcessColorSwap() {
        if (!colorCanSwap)
            return;

        if (Input.GetAxis("Mouse ScrollWheel") >= 0.001f) {
            currentColorIndex++;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") <= -0.001f) {
            currentColorIndex--;
        }
        else
            return;

        if (currentColorIndex < 0)
            currentColorIndex = colors.Length - 1;
        if (currentColorIndex >= colors.Length)
            currentColorIndex = 0;

        colorCanSwap = false;
        Invoke("EnableColorSwap", colorSwapCooldown);
    }

    void EnableColorSwap() {
        colorCanSwap = true;
    }

    void Update() {
        currentImage.color = colors[currentColorIndex];
        upImage.color = colors[(currentColorIndex + 1 >= colors.Length ? 0 : currentColorIndex + 1)];
        downImage.color = colors[(currentColorIndex - 1 < 0 ? colors.Length - 1 : currentColorIndex - 1)];

        ProcessColorSwap();
    }


}
