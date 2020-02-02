using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoundController : MonoBehaviour {
    [SerializeField]
    float roundTime = 10;
    float betweenRoundTime = 2;
    [SerializeField]
    GameObject canvasGroup;
    [SerializeField]
    Renderer canvasRenderer;
    [SerializeField]
    Texture2D[] paintings;
    [SerializeField]
    GameObject[] tools;

    List<Texture2D> nextPaintings = new List<Texture2D>();
    List<GameObject> nextTools = new List<GameObject>();
    GameObject currentTool;
    Animator canvasAnimator;

    public delegate void RoundCleanup();
    public static event RoundCleanup OnRoundCleanup;

    public delegate void RoundEnd();
    public static event RoundEnd OnRoundEnd;

    private void Awake() {
        StartCoroutine(RoundCounter());
        canvasAnimator = canvasGroup.GetComponent<Animator>();
    }

    IEnumerator RoundCounter() {
        yield return new WaitForSeconds(betweenRoundTime);
        if(OnRoundCleanup != null)
            OnRoundCleanup.Invoke();

        NewPainting();
        NewTool();
        canvasAnimator.SetBool("Active", true);

        yield return new WaitForSeconds(roundTime);

        if (OnRoundEnd != null)
            OnRoundEnd.Invoke();
        canvasAnimator.SetBool("Active", false);
        StartCoroutine(RoundCounter());
    }

    void NewPaintingList() {
        nextPaintings = paintings.ToList();
    }

    void NewPainting() {
        if(nextPaintings.Count == 0)
            NewPaintingList();

        int paintingNumber = Random.Range((int)0, (int)nextPaintings.Count);
        canvasRenderer.material.SetTexture("_MainTex", nextPaintings[paintingNumber]);
        nextPaintings.RemoveAt(paintingNumber);
    }

    void NewTool() {
        //TODO: Don't actually destroy, swap
        if(currentTool)
            Destroy(currentTool);

        if (nextTools.Count == 0)
            NewToolList();

        int toolNumber = Random.Range((int)0, (int)nextTools.Count);
        currentTool = Instantiate(nextTools[toolNumber]);
        nextTools.RemoveAt(toolNumber);
        
    }

    void NewToolList() {
        nextTools = tools.ToList();
    }
}
