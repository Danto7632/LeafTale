using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTransition;

public class GoBackToStart : MonoBehaviour
{
    public TransitionSettings transition_B;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            TransitionManager.Instance().Transition("StartPage", transition_B, 0);
        if (Input.GetKeyDown(KeyCode.C))
            TransitionManager.Instance().Transition("EndingCredit", transition_B, 0);
    }
}
