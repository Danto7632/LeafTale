using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위해 추가

public class DrawingGame : MonoBehaviour
{
    public LineRenderer referenceShape; // 참조 모양 (원 또는 사각형)
    public LineRenderer playerDrawing;  // 플레이어가 그린 모양
    public TMP_Text resultText;         // 정확도를 표시할 TextMeshPro 텍스트

    private List<Vector3> playerPoints = new List<Vector3>(); // 플레이어가 그린 점들

    private void Start()
    {
        // 예제용으로 원형을 설정
        SetReferenceShape(CreateCircle(Vector3.zero, 5f, 100));
        SetLineRendererColor(referenceShape, Color.blue); // 참조 모양의 색상을 파란색으로 설정
        SetLineRendererColor(playerDrawing, Color.magenta); // 플레이어가 그린 모양의 색상을 분홍색으로 설정
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼을 누르는 동안
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // 2D 게임이므로 Z 축을 0으로 고정
            playerPoints.Add(mousePos);
            playerDrawing.positionCount = playerPoints.Count;
            playerDrawing.SetPositions(playerPoints.ToArray());
        }
        else if (Input.GetMouseButtonUp(0)) // 마우스를 놓을 때
        {
            float accuracy = CalculateAccuracy();
            resultText.text = "Accuracy: " + (accuracy * 100f).ToString("F2") + "%";
            playerPoints.Clear();
            playerDrawing.positionCount = 0;
        }
    }

    private void SetReferenceShape(Vector3[] shapePoints)
    {
        referenceShape.positionCount = shapePoints.Length;
        referenceShape.SetPositions(shapePoints);
    }

    private Vector3[] CreateCircle(Vector3 center, float radius, int segments)
    {
        Vector3[] points = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            points[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius + center;
        }
        return points;
    }

    private void SetLineRendererColor(LineRenderer lineRenderer, Color color)
    {
        // 기본 색상을 설정합니다.
        Gradient gradient = new Gradient();
        gradient.colorKeys = new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) };
        gradient.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) };

        lineRenderer.colorGradient = gradient;
    }

    private float CalculateAccuracy()
    {
        float totalDistance = 0f;
        for (int i = 0; i < playerPoints.Count; i++)
        {
            float minDistance = float.MaxValue;
            for (int j = 0; j < referenceShape.positionCount; j++)
            {
                float distance = Vector3.Distance(playerPoints[i], referenceShape.GetPosition(j));
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
            totalDistance += minDistance;
        }

        float averageDistance = totalDistance / playerPoints.Count;
        float maxPossibleDistance = 1f; // 조정 가능한 임계값
        return Mathf.Clamp01(1f - averageDistance / maxPossibleDistance);
    }
}
