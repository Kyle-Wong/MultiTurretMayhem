using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateArc : MonoBehaviour {

    // Use this for initialization
    public float radius;
    public float degrees;
    public float stepSize;
    private LineRenderer lineRenderer;
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        createArc(lineRenderer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void createArc(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = 0;
        int i = 0;
        for(float theta = 0; theta < degrees * Mathf.Deg2Rad; theta += stepSize*Mathf.Deg2Rad)
        {
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition(i, new Vector3(x, y,0));
            
            ++i;
        }

    }

}
