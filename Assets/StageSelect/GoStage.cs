using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using EasyTransition;

public class GoStage : MonoBehaviour {
    public TransitionSettings transition_R;

    public static void SelectLevel()
    {
        GoStage transtition = FindObjectOfType<GoStage>(); // 기존의 GoStage 인스턴스를 찾음
        if (transtition == null || transtition.transition_R == null)
        {
            Debug.LogError("GoStage instance or transition_R is not set.");
            return;
        }

        switch (ScrollStage.btnIndex)
        {
            case 0:
                TransitionManager.Instance().Transition("BroomstickScene", transtition.transition_R, 0);
                break;
            case 1:
                TransitionManager.Instance().Transition("platformScene", transtition.transition_R, 0);
                break;
            case 2:
                TransitionManager.Instance().Transition("RhythmScene", transtition.transition_R, 0);
                break;
            case 3:
                TransitionManager.Instance().Transition("test", transtition.transition_R, 0);
                break;
            case 4:
                TransitionManager.Instance().Transition("ClawMachineScenes", transtition.transition_R, 0);
                break;
            default:
                break;
        }
    }
}
