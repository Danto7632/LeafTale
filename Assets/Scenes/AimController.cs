using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

public class AimController : MonoBehaviour
{
    public Text debugText; // 디버그 텍스트

    private LeapServiceProvider leapProvider;
    private Vector3 aimOffset; // 주먹을 쥐었을 때 에임을 원점으로 이동하기 위한 오프셋

    void Start()
    {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        aimOffset = Vector3.zero; // 초기에는 원점 오프셋을 없도록 설정
    }

    void OnDestroy()
    {
        leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    void OnUpdateFrame(Frame frame)
    {
        if (frame.Hands.Count > 0)
        {
            Hand hand = frame.Hands[0];

            Vector3 handPosition = hand.PalmPosition; // Leap Motion 좌표를 Unity 좌표로 변환
            handPosition.z = 0; // 2D 기준으로 변환하기 위해 z 축 값을 0으로 설정

            if (hand.GrabStrength >= 0.9f)
            {
                // 주먹을 쥐었을 때
                aimOffset = -handPosition; // 에임을 원점으로 이동시키는 오프셋 설정
            }

            // 에임 위치 설정
            transform.position = handPosition + aimOffset;

            // 디버그 텍스트 업데이트
            debugText.text = "Hand Position: " + handPosition.ToString("F2") + "\nAim Offset: " + aimOffset.ToString("F2");
        }
    }
}