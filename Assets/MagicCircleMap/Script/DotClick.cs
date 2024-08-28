using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotClick : MonoBehaviour {
    public GameObject[] Dots = new GameObject[5];
    public int dotCount = 0;

    public GameObject previousDot;

    void Start() {
        for(int i = 0; i < Dots.Length; i++) {
            int index = i;
            Dots[i].AddComponent<ClickHandler>().Setup(index, Dots[i], OnDotClicked);
        }
    }

    void OnDotClicked(int index, GameObject dot) {
        Debug.Log("Dot " + (index + 1) + " clicked!");

        if(dotCount == 0) {
            dot.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
            dot.GetComponent<LinkDot>().isClicked = true;
            previousDot = dot;
            dotCount++;
        }
        else if(dot.GetComponent<LinkDot>().isClicked) {
            Debug.Log("is already clicked");
        }
        else {
            if(dot != previousDot.GetComponent<LinkDot>().linkDots[0] && dot != previousDot.GetComponent<LinkDot>().linkDots[1]) {
                colorReset();
                dotCount = 0;
            }   
            else {
                dot.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
                dot.GetComponent<LinkDot>().isClicked = true;
                previousDot = dot;
                dotCount++;
            }
        }
    }

    void colorReset() {
        for(int i = 0; i < 5; i++) {
            Dots[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            Dots[i].GetComponent<LinkDot>().isClicked = false;
        }
    }
}

public class ClickHandler : MonoBehaviour {
    private int index;
    private GameObject Dot;
    private System.Action<int, GameObject> clickAction;

    public void Setup(int idx, GameObject po, System.Action<int, GameObject> action) {
        index = idx;
        Dot = po;
        clickAction = action;
    }

    void OnMouseDown() {
        clickAction?.Invoke(index, Dot);
    }
}