﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoundController : MonoBehaviour {
    float roundTime = 20;
    float betweenRoundTime = 2;
    float roundEndTime = 2;

    [SerializeField]
    GameObject canvasGroup;
    [SerializeField]
    Renderer canvasRenderer;

    [SerializeField]
    GameObject originalCanvasGroup;
    [SerializeField]
    Renderer originalCanvasRenderer;

    [SerializeField]
    Painting[] paintings;
    [SerializeField]
    GameObject[] tools;

    List<Painting> nextPaintings = new List<Painting>();
    List<GameObject> nextTools = new List<GameObject>();
    Tool currentTool;
    Animator canvasAnimator;
    Animator originalCanvasAnimator;

    public delegate void RoundCleanup();
    public static event RoundCleanup OnRoundCleanup;

    public delegate void RoundEnd();
    public static event RoundEnd OnRoundEnd;

    public delegate void GameEnd();
    public static event GameEnd OnGameEnd;

    [System.Serializable]
    public struct Painting {
        public Texture2D originalVersion;
        public List<Texture2D> damagedVersions;

        public Painting(Texture2D originalVersion, Texture2D damagedVersion) {
            this.originalVersion = originalVersion;
            //this.damagedVersion = damagedVersion;
            this.damagedVersions = new List<Texture2D>();
            this.damagedVersions.Append<Texture2D>(damagedVersion);
        }

        public Painting(Texture2D originalVersion, List<Texture2D> damagedVersions) {
            this.originalVersion = originalVersion;
            this.damagedVersions = damagedVersions;
        }


    }

    private void OnEnable() {
        NewPaintingList();
        canvasAnimator = canvasGroup.GetComponent<Animator>();
        originalCanvasAnimator = originalCanvasGroup.GetComponent<Animator>();

        StartCoroutine(RoundCounter());
    }

    IEnumerator RoundCounter() {
        if(OnRoundCleanup != null)
            OnRoundCleanup.Invoke();

        NewPainting();
        NewTool();
        canvasAnimator.SetBool("Active", true);
        yield return new WaitForSeconds(roundTime);

        if (OnRoundEnd != null)
            OnRoundEnd.Invoke();

        DiscardTool();

        originalCanvasAnimator.SetTrigger("Cycle");
        yield return new WaitForSeconds(roundEndTime);

        canvasAnimator.SetBool("Active", false);

        if (nextPaintings.Count == 0)
            EndGame();
        else {
            yield return new WaitForSeconds(betweenRoundTime);
            StartCoroutine(RoundCounter());
        }
    }

    void NewPaintingList() {
        nextPaintings = paintings.ToList();
    }

    void NewPainting() {
        if (nextPaintings.Count == 0)
            return;

        int basePaintingNumber = Random.Range((int)0, (int)nextPaintings.Count);
        int damagedPaintingIndex = Random.Range((int)0, (int)nextPaintings[basePaintingNumber].damagedVersions.Count);
        canvasRenderer.material.SetTexture("_MainTex", nextPaintings[basePaintingNumber].damagedVersions[damagedPaintingIndex]);
        //canvasRenderer.material.SetTexture("_MainTex", nextPaintings[paintingNumber].damagedVersions);
        originalCanvasRenderer.material.SetTexture("_MainTex", nextPaintings[basePaintingNumber].originalVersion);
        nextPaintings.RemoveAt(basePaintingNumber);
    }

    void DiscardTool() {
        if (currentTool) {
            currentTool.MoveToNextState();
            currentTool = null;
        }
    }

    void NewTool() {
        if (nextTools.Count == 0)
            NewToolList();

        int toolNumber = Random.Range((int)0, (int)nextTools.Count);
        currentTool = (Instantiate(nextTools[toolNumber])).GetComponent<Tool>();
        nextTools.RemoveAt(toolNumber);
        
    }

    void NewToolList() {
        nextTools = tools.ToList();
    }

    void EndGame() {
        OnGameEnd.Invoke();
        GameController.Instance.MoveToNextState();
    }
}
