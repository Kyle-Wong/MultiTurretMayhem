using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOutline : MonoBehaviour {

    // Use this for initialization
    public float speedFactor;
    public float tolerance;
    private Vector3[] vertices;
    private Vector3[] targetPos;
    private LineRenderer lineRenderer;
    private Camera mainCam;
    //0 = top left
    //1 = top right
    //2 = bottom right
    //3 = bottom left
	void Start () {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 4;
        targetPos = new Vector3[4];
        vertices = new Vector3[4];
        


    }

    // Update is called once per frame
    void Update () {

        for(int i = 0; i < vertices.Length; ++i)
        {
            vertices[i] = moveToLocation(targetPos[i], vertices[i]);
        }
        lineRenderer.SetPositions(vertices);
        

    }
    public Vector3 moveToLocation(Vector3 currentTarget, Vector3 currentPosition)
    {
        //moves towards location, snaps to location once close enough
        currentPosition += (currentTarget - currentPosition) * (speedFactor / 100f) * (1 - Time.deltaTime);
        if (Vector3.Distance(currentPosition, currentTarget) < tolerance)
        {
            currentPosition = currentTarget;
        }
        if (speedFactor == 0f)
        {
            currentPosition = currentTarget;
        }
        return currentPosition;
    }
    public void setTarget(RectTransform newTarget)
    {
        float x, y;
        x = mainCam.ViewportToWorldPoint(newTarget.anchorMin).x;
        y = mainCam.ViewportToWorldPoint(newTarget.anchorMax).y;
        targetPos[0] = new Vector3(x, y);
        x = mainCam.ViewportToWorldPoint(newTarget.anchorMax).x;
        y = mainCam.ViewportToWorldPoint(newTarget.anchorMax).y;
        targetPos[1] = new Vector3(x, y);
        x = mainCam.ViewportToWorldPoint(newTarget.anchorMax).x;
        y = mainCam.ViewportToWorldPoint(newTarget.anchorMin).y;
        targetPos[2] = new Vector3(x, y);
        x = mainCam.ViewportToWorldPoint(newTarget.anchorMin).x;
        y = mainCam.ViewportToWorldPoint(newTarget.anchorMin).y;
        targetPos[3] = new Vector3(x, y);
    }
    public void setTarget(Vector3 bottomLeft, Vector3 topRight)
    {
        float x, y;
        x = bottomLeft.x;
        y = topRight.y;
        targetPos[0] = new Vector3(x, y);
        x = topRight.x;
        y = topRight.y;
        targetPos[1] = new Vector3(x, y);
        x = topRight.x;
        y = bottomLeft.y;
        targetPos[2] = new Vector3(x, y);
        x = bottomLeft.x;
        y = bottomLeft.y;
        targetPos[3] = new Vector3(x, y);
    }
    public void setPosition(RectTransform newTransform)
    {
        float x, y;
        x = mainCam.ViewportToWorldPoint(newTransform.anchorMin).x;
        y = mainCam.ViewportToWorldPoint(newTransform.anchorMax).y;
        vertices[0] = new Vector3(x, y);
        x = mainCam.ViewportToWorldPoint(newTransform.anchorMax).x;
        y = mainCam.ViewportToWorldPoint(newTransform.anchorMax).y;
        vertices[1] = new Vector3(x, y);
        x = mainCam.ViewportToWorldPoint(newTransform.anchorMax).x;
        y = mainCam.ViewportToWorldPoint(newTransform.anchorMin).y;
        vertices[2] = new Vector3(x, y);
        x = mainCam.ViewportToWorldPoint(newTransform.anchorMin).x;
        y = mainCam.ViewportToWorldPoint(newTransform.anchorMin).y;
        vertices[3] = new Vector3(x, y);
        setTarget(newTransform);
        lineRenderer.SetPositions(vertices);
    }
    public void setPosition(Vector3 bottomLeft, Vector3 topRight)
    {
        float x, y;
        x = bottomLeft.x;
        y = topRight.y;
        vertices[0] = new Vector3(x, y);
        x = topRight.x;
        y = topRight.y;
        vertices[1] = new Vector3(x, y);
        x = topRight.x;
        y = bottomLeft.y;
        vertices[2] = new Vector3(x, y);
        x = bottomLeft.x;
        y = bottomLeft.y;
        vertices[3] = new Vector3(x, y);
        setTarget(bottomLeft, topRight);
        lineRenderer.SetPositions(vertices);
    }
}
