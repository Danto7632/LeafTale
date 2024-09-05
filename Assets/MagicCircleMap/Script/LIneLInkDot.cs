using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LIneLInkDot : MonoBehaviour {
    public GameObject[] linkDots = new GameObject[2];
    private LineRenderer lineRenderer;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.blue;
    }

    void Update() {
        if (linkDots[0].GetComponent<LinkDot>().isClicked && linkDots[1].GetComponent<LinkDot>().isClicked) {
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
        else {
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;
        }
    }
}
