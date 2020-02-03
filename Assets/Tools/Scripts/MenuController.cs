using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            GameController.Instance.MoveToNextState();
        }
    }
}
