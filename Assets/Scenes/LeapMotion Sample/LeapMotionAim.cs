using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class LeapMotionAim : MonoBehaviour {
    private LeapServiceProvider leapProvider;

    private bool isTrackingHand = false; // 손 추적 상태 추가
    private Vector3 originPosition = Vector3.zero;
    private GameObject trackingSprite;

    private float leapMinX = -0.5f;
    private float leapMaxX = 0.5f;
    private float leapMinY = -0.5f;
    private float leapMaxY = 0.5f;

    private float unityMinX;
    private float unityMaxX;
    private float unityMinY;
    private float unityMaxY;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        trackingSprite = GameObject.Find("Aim");
        if (trackingSprite == null) {
            Debug.LogError("Aim object not found in the scene.");
        }

        Camera cam = Camera.main;
        unityMinX = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)).x;
        unityMaxX = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, cam.nearClipPlane)).x;
        unityMinY = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)).y;
        unityMaxY = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, cam.nearClipPlane)).y;

        float expansionFactor = 0.4f;
        unityMinX *= expansionFactor;
        unityMaxX *= expansionFactor;
        unityMinY *= expansionFactor;
        unityMaxY *= expansionFactor;
    }

    void OnDestroy() {
        if (leapProvider != null) {
            leapProvider.OnUpdateFrame -= OnUpdateFrame;
        }
    }

    void OnUpdateFrame(Frame frame) {
        if (frame.Hands.Count > 0) {
            Hand hand = frame.Hands[0];

            // 주먹을 쥐었을 때 추적 시작
            if (!isTrackingHand && IsFist(hand)) {
                isTrackingHand = true;
                originPosition = hand.PalmPosition;
                Debug.Log("Fist closed. Origin position set to: " + originPosition);
            }

            // 주먹을 쥐고 있을 때 위치 추적 및 이동
            if (isTrackingHand) {
                Vector3 currentHandPosition = hand.PalmPosition;
                Vector3 relativePosition = currentHandPosition - originPosition;
                Vector3 unityPosition = LeapToUnityPosition(relativePosition);
                trackingSprite.transform.position = new Vector3(unityPosition.x, unityPosition.y, 0);

                if (IsFist(hand)) {
                    Debug.Log("Left click action at: " + trackingSprite.transform.position);
                }
            }
        }
        else {
            trackingSprite.transform.position = Vector2.zero;
            isTrackingHand = false;
        }
    }

    bool IsFist(Hand hand) {
        return hand.GrabStrength > 0.9f;
    }

    Vector3 LeapToUnityPosition(Vector3 leapPosition) {
        float unityX = Map(leapPosition.x, leapMinX, leapMaxX, unityMinX, unityMaxX);
        float unityY = Map(leapPosition.y, leapMinY, leapMaxY, unityMinY, unityMaxY);
        return new Vector3(unityX, unityY, 0);
    }

    float Map(float value, float fromMin, float fromMax, float toMin, float toMax) {
        return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }
}