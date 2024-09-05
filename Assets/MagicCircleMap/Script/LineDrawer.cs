using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour {
    private LineRenderer[] lineRenderer = new LineRenderer[5];

    public GameObject[] lines;

    void Start() {
        Vector3[][] positions = new Vector3[5][];
        positions[0] = new Vector3[2] { new Vector3(3, 0, 0), new Vector3(-2.427f, 1.764f, 0) };
        positions[1] = new Vector3[2] { new Vector3(-2.427f, 1.764f, 0), new Vector3(0.927f, -2.853f, 0) };
        positions[2] = new Vector3[2] { new Vector3(0.927f, -2.853f, 0), new Vector3(0.927f, 2.853f, 0) };
        positions[3] = new Vector3[2] { new Vector3(0.927f, 2.853f, 0), new Vector3(-2.427f, -1.764f, 0) };
        positions[4] = new Vector3[2] { new Vector3(-2.427f, -1.764f, 0), new Vector3(3, 0, 0) };

        lines = new GameObject[5];

        for(int i = 0; i < 5; i++) {
            lines[i] = GameObject.Find("Line" + (i + 1));
            lineRenderer[i] = lines[i].GetComponent<LineRenderer>();
            lineRenderer[i].startWidth = 0.1f;
            lineRenderer[i].endWidth = 0.1f;

            lineRenderer[i].startColor = Color.red;
            lineRenderer[i].endColor = Color.blue;

            lineRenderer[i].positionCount = positions[i].Length;
            lineRenderer[i].SetPositions(positions[i]);
        } 
    }
}