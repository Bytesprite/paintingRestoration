using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour, IStateMachine {

    public static GameController Instance = null;
    IState currentState;
    public IState CurrentState {
        get { return currentState; }
        set {
            if (currentState == value)
                return;
            if (currentState != null)
                currentState.OnExit(this);
            currentState = value;
            currentState.OnEnter(this);
        }
    }

    public GameObject[] menuObjects;
    public GameObject[] paintingObjects;
    public GameObject[] showcaseObjects;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        DisableGameObjectArray(menuObjects);
        DisableGameObjectArray(paintingObjects);
        DisableGameObjectArray(showcaseObjects);

        CurrentState = new Menu();
    }

    private void Update() {
        CurrentState.Run(this);
    }

    public void EnableGameObjectArray (GameObject[] array) {
        foreach (GameObject gameObject in array) {
            gameObject.SetActive(true);
        }
    }

    public void DisableGameObjectArray(GameObject[] array) {
        foreach (GameObject gameObject in array) {
            gameObject.SetActive(false);
        }
    }

    public void MoveToNextState() {
        CurrentState = currentState.NextState;
    }

    class Menu : IState {
        public IState NextState {
            get { return new Painting(); }
        }

        public void OnEnter(IStateMachine controller) {
            GameController gameController = controller as GameController;
            if (gameController == null)
                return;

            gameController.EnableGameObjectArray(gameController.menuObjects);
        }

        public void OnExit(IStateMachine controller) {
            GameController gameController = controller as GameController;
            if (gameController == null)
                return;

            gameController.DisableGameObjectArray(gameController.menuObjects);
        }

        public void Run(IStateMachine controller) {

        }
    }

    class Painting : IState {
        public IState NextState {
            get { return new Showcase(); }
        }

        public void OnEnter(IStateMachine controller) {
            GameController gameController = controller as GameController;
            if (gameController == null)
                return;

            gameController.EnableGameObjectArray(gameController.paintingObjects);
        }

        public void OnExit(IStateMachine controller) {
            GameController gameController = controller as GameController;
            if (gameController == null)
                return;

            gameController.DisableGameObjectArray(gameController.paintingObjects);
        }

        public void Run(IStateMachine controller) {}
    }

    class Showcase : IState {
        public IState NextState {
            get { return new Menu(); }
        }

        public void OnEnter(IStateMachine controller) {
            GameController gameController = controller as GameController;
            if (gameController == null)
                return;

            gameController.EnableGameObjectArray(gameController.showcaseObjects);
        }

        public void OnExit(IStateMachine controller) {
            GameController gameController = controller as GameController;
            if (gameController == null)
                return;

            gameController.DisableGameObjectArray(gameController.showcaseObjects);
        }

        public void Run(IStateMachine controller) {

        }
    }
}
