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
    protected Material material;
    [SerializeField]
    Brush[] brushes;
    Color color;
    [SerializeField]
    protected Transform shotSpawnPoint;
    [SerializeField]
    public float shotsPerSecond = 4;
    private float secondsBetweenShots;
    private float timeOfLastShot;
    [SerializeField]
    GameObject shotObject;

    private void Awake() {
        currentState = new Default();
    }

    private void Update() {
        currentState.Run(this);
    }

    virtual public void Fire() {
        GameObject shotInstance = Instantiate(shotObject, shotSpawnPoint.position, Quaternion.identity);
        shotInstance.transform.forward = shotSpawnPoint.forward;
        if (!shotInstance.GetComponent<ToolShot>())
            return;

        Material materialInstance = new Material(material);
        Brush randomBrush = brushes[Random.Range((int)0, (int)brushes.Length)];
        color = ColorController.GetCurrentColor();//new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f));
        materialInstance.SetColor("_Color", color);

        timeOfLastShot = Time.time;
        shotInstance.GetComponent<ToolShot>().SetValues(randomBrush, materialInstance);
    }
    virtual public void FireOff() //Mouse Up
    {
    }
    virtual public void FireContinue() //Mouse continuing to be down
    {
        secondsBetweenShots = 1 / shotsPerSecond;
        if (Time.time > timeOfLastShot + secondsBetweenShots)
        {
            Fire();
        }
    }
    public void MoveToNextState() {
        CurrentState = currentState.NextState;
    }

    class Default : IState {
        public IState NextState{
            get { return new ExitScene(); }
        }

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
            else if (Input.GetMouseButtonUp(0))
            {
                tool.FireOff();
            }
            else if (Input.GetMouseButton(0))
            {
                tool.FireContinue();
            }
        }
    }

    class EnterScene : IState {
        public IState NextState {
            get { return new Default(); }
        }

        public void OnEnter(IStateMachine controller) {

        }

        public void OnExit(IStateMachine controller) {

        }

        public void Run(IStateMachine controller) {

        }
    }

    class ExitScene : IState {
        public IState NextState {
            get { return new EnterScene(); }
        }

        public void OnEnter(IStateMachine controller) {
            Tool tool = controller as Tool;
            if (tool == null)
                return;

            if(tool.GetComponent<Rigidbody>())
                tool.GetComponent<Rigidbody>().isKinematic = false;
            Destroy(tool.gameObject, 2);
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


