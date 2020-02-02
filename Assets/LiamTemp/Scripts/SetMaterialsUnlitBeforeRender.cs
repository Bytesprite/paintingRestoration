using System.Collections;
using UnityEngine;

public class SetMaterialsUnlitBeforeRender : MonoBehaviour{
    [SerializeField]
    GameObject[] objectsToSetUnlit;
    ShaderPair[] shaderPairs = new ShaderPair[] {new ShaderPair("Sprites/Diffuse", "Sprites/Default"),
        new ShaderPair("Standard", "Unlit/Texture")};
    bool switchShadersThisFrame = false;
    System.Action postRenderAction;

    private void Awake() {
        foreach (ShaderPair pair in shaderPairs) {
            pair.LinkShaders();
        }   
    }

    void OnPreCull() {
        if (!switchShadersThisFrame)
            return;

        Debug.Log("Swapping shaders...");
        foreach (GameObject obj in objectsToSetUnlit) {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) {
                foreach (ShaderPair pair in shaderPairs) {
                    if (renderer.material.shader == pair.litShader)
                        renderer.material.shader = pair.unlitShader;
                }
            }
        }
    }

    void OnPreRender() {
        if (!switchShadersThisFrame)
            return;

        foreach (GameObject obj in objectsToSetUnlit) {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) {
                foreach (ShaderPair pair in shaderPairs) {
                    if (renderer.material.shader == pair.litShader)
                        renderer.material.shader = pair.unlitShader;
                }
            }
        }
    }

    void OnPostRender() {
        if (!switchShadersThisFrame)
            return;

        foreach (GameObject obj in objectsToSetUnlit) {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) {
                foreach (ShaderPair pair in shaderPairs) {
                    if (renderer.material.shader == pair.unlitShader)
                        renderer.material.shader = pair.litShader;
                }
            }
        }
        switchShadersThisFrame = false;

        StartCoroutine(PostRenderEndOfFrame());
    }

    class ShaderPair {
        public Shader litShader;
        public Shader unlitShader;
        string litShaderPath;
        string unlitShaderPath;

        public ShaderPair(string litShaderPath, string unlitShaderPath) {
            this.litShaderPath = litShaderPath;
            this.unlitShaderPath = unlitShaderPath;
        }

        public void LinkShaders(){
            litShader = Shader.Find(litShaderPath);
            unlitShader = Shader.Find(unlitShaderPath);
        }
    }

    public void SetShaderSwitchFlag(System.Action callback = null) {
        switchShadersThisFrame = true;
        postRenderAction = callback;
    }

    IEnumerator PostRenderEndOfFrame() {
        yield return new WaitForEndOfFrame();
        if (postRenderAction != null) {
            postRenderAction.Invoke();
            postRenderAction = null;
        }
    }
}
