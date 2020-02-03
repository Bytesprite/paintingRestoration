using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTool : Tool
{
    private LineRenderer line;
    private Vector3 hitPos;
    private int currLines;
    private int currVertex;
    private float startWidth_ = 0.03f;
    private float endWidth_ = 0.03f;
    private bool useWorldSpace_ = false;
    private int numCapVertices_ = 20;
    public float verticesPerSecond = 4;
    private float secondsBetweenVertices;
    private float timeOfLastVertex;

    const float distanceBetweenVertices = 0.03f;

    private void Start()
    {

    }

    private void OnEnable()
    {
        secondsBetweenVertices = 1 / verticesPerSecond;
        RoundController.OnRoundEnd += this.FireOff;
    }
    private void OnDisable()
    {
        RoundController.OnRoundEnd -= this.FireOff;
    }

    // Update is called once per frame
    override public void Fire() //mousedown
    {
        if (line == null)
        {
            createLine();
            currVertex = 1;
        }
        
        Debug.Log("Checking for hit");
        Ray ray = new Ray(shotSpawnPoint.position, shotSpawnPoint.forward);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit)) //, LayerMask.NameToLayer("DecalPlane")))
        {
            Debug.Log(hit.transform.gameObject.name);
            hitPos = hit.point;
            //hitPos = hit.transform.InverseTransformPoint(hit.point);
            //hitPos = hit.transform.TransformPoint(hit.point);

            DecalSpawner decalSpawner = hit.collider.gameObject.GetComponent<DecalSpawner>();
            if (decalSpawner)
                decalSpawner.Spawn(material, line);

            timeOfLastVertex = Time.time;
            line.SetPosition(0, hitPos);
            line.SetPosition(currVertex, hitPos);
        }
    }
    override public void FireOff() //mouse up
    {
        if (line)
        {
            Ray ray = new Ray(shotSpawnPoint.position, shotSpawnPoint.forward);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, LayerMask.NameToLayer("DecalPlane")))
            {
                hitPos = hit.point;
                //hitPos = hit.transform.InverseTransformPoint(hit.point);
                //hitPos = hit.transform.TransformPoint(hit.point);

                line.SetPosition(currVertex, hitPos);
                line = null;
                currLines++;
            }
        }
    }
    override public void FireContinue() //mouse is still down
    {
        if (line)
        {
            Ray ray = new Ray(shotSpawnPoint.position, shotSpawnPoint.forward);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, LayerMask.NameToLayer("DecalPlane")))
            {
                hitPos = hit.point;
                //hitPos = hit.transform.InverseTransformPoint(hit.point);
                //hitPos = hit.transform.TransformPoint(hit.point);

                //if (Random.value > 0.9f) //janky
                // If we're far enough or waited long enough, new vertex
                if (Vector3.Distance(hitPos, line.GetPosition(currVertex - 1)) >= distanceBetweenVertices
                    || Time.time > timeOfLastVertex + secondsBetweenVertices)
                {
                    currVertex++;
                    line.positionCount = line.positionCount + 1;
                }
                line.SetPosition(currVertex, hitPos);
            }
        }
    }

    void createLine()
    {
        Material materialInstance = new Material(material);
        Color color = ColorController.GetCurrentColor();//new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f));
        materialInstance.SetColor("_Color", color);

        line = new GameObject("Line" + currLines).AddComponent<LineRenderer>();
        line.material = materialInstance;
        line.positionCount = 2;  // Can change for curves
        line.startWidth = startWidth_;
        line.endWidth = endWidth_;
        line.useWorldSpace = useWorldSpace_;
        line.numCapVertices = numCapVertices_;
    }

}
