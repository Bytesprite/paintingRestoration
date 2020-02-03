using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseController : MonoBehaviour{
    void Update(){
        if (Input.GetMouseButtonDown(1)) {
            GameController.Instance.MoveToNextState();
            WallCanvas.ResetPaintingNumber();
        }
    }
}
