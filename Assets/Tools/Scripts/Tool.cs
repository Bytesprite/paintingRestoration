using UnityEngine;

public class Tool : MonoBehaviour, IStateMachine{
    IState currentState;
    public IState CurrentState {
        get { return currentState; }
        set {
            if (currentState == value)
                return;
            if(currentState != null)
                currentState.OnExit(this);
            currentState = value;
            currentState.OnEnter(this);
        }
    }
    private Vector3 moveTarget = new Vector3(-0.6f,1.27f,2.55f);
    [SerializeField]
    LayerMask layerMask = new LayerMask();
    [SerializeField]
    Material material;
    [SerializeField]
    Brush[] brushes;
    Color color;
    [SerializeField]
    Transform shotSpawnPoint;
    [SerializeField]
    GameObject shotObject;

    private void Awake() {
        currentState = new Default();
    }

    private void Update() {
        currentState.Run(this);
    }

    public void Fire() {
        GameObject shotInstance = Instantiate(shotObject, shotSpawnPoint.position, Quaternion.identity);
        shotInstance.transform.forward = shotSpawnPoint.forward;
        if (!shotInstance.GetComponent<ToolShot>())
            return;

        Material materialInstance = new Material(material);
        Brush randomBrush = brushes[Random.Range((int)0, (int)brushes.Length)];
        color = new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f));
        materialInstance.SetColor("_Color", color);

        shotInstance.GetComponent<ToolShot>().SetValues(randomBrush, materialInstance);
    }

    class Default : IState {
        public void OnEnter(IStateMachine controller) {
            Cursor.visible = false;
        }

        public void OnExit(IStateMachine controller) {
            Cursor.visible = true;
        }

        public void Run(IStateMachine controller) {
            Tool tool = controller as Tool;
            if (tool == null)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, LayerMask.NameToLayer("ToolPlane"))) {
                tool.moveTarget = hit.point;
            }
            tool.transform.position = Vector3.Lerp(tool.transform.position, tool.moveTarget, Time.deltaTime * 10);

            if (Input.GetMouseButtonDown(0)) {
                tool.Fire();
            }
        }
    }

    class EnterScene : IState {
        public void OnEnter(IStateMachine controller) {

        }

        public void OnExit(IStateMachine controller) {

        }

        public void Run(IStateMachine controller) {

        }
    }

    class ExitScene : IState {
        public void OnEnter(IStateMachine controller) {

        }

        public void OnExit(IStateMachine controller) {

        }

        public void Run(IStateMachine controller) {

        }
    }
}

[System.Serializable]
public struct Brush {
    public Sprite decal;
    public Vector2 size;
}


