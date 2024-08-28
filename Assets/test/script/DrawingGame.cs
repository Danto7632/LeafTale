using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrawingGame : MonoBehaviour
{
    public LineRenderer referenceShape; // 참조 모양 (원 또는 사각형)
    public LineRenderer playerDrawing;  // 플레이어가 그린 모양
    public TMP_Text resultText;         // 정확도를 표시할 TextMeshPro 텍스트

    public Vector3[] circleShape;
    public Vector3[] squareShape;
    public Vector3[] starShape;

    private List<Vector3> playerPoints = new List<Vector3>(); // 플레이어가 그린 점들
    private bool isDrawing = false; // 그리기 중 여부

    private void Start()
    {
        referenceShape.gameObject.SetActive(false); // 게임 시작 시 도형 비활성화
        playerDrawing.gameObject.SetActive(true);   // 플레이어의 그리기 활성화

        // Sorting Order 설정
        SetLineRendererSortingOrder(referenceShape, 0);
        SetLineRendererSortingOrder(playerDrawing, 1);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 누를 때
        {
            // 새 그림을 그릴 때 기존 점들 초기화
            playerPoints.Clear();
            playerDrawing.positionCount = 0;
            isDrawing = true;
        }

        if (Input.GetMouseButton(0) && isDrawing) // 마우스 왼쪽 버튼을 누르고 있을 때
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // 2D 게임이므로 Z 축을 0으로 고정
            playerPoints.Add(mousePos);
            playerDrawing.positionCount = playerPoints.Count;
            playerDrawing.SetPositions(playerPoints.ToArray());
        }
        
        if (Input.GetMouseButtonUp(0)) // 마우스를 놓을 때
        {
            isDrawing = false;
            float accuracy = CalculateAccuracy();
            resultText.text = "Accuracy: " + (accuracy * 100f).ToString("F2") + "%";
        }
    }

    private void SetLineRendererSortingOrder(LineRenderer lineRenderer, int order)
    {
        Material material = new Material(Shader.Find("Sprites/Default"));
        material.renderQueue = 3000 + order; // Material의 renderQueue 설정
        lineRenderer.material = material;
    }

    public void SetReferenceShape(Shape shape)
    {
        switch (shape)
        {
            case Shape.Circle:
                SetReferenceShape(CreateCircle(Vector3.zero, 5f, 100));
                break;
            case Shape.Square:
                SetReferenceShape(CreateSquare(Vector3.zero, 5f));
                break;
            case Shape.Star:
                SetReferenceShape(CreateStar(Vector3.zero, 5f, 5));
                break;
        }
    }

    private void SetReferenceShape(Vector3[] shapePoints)
    {
        referenceShape.positionCount = shapePoints.Length;
        referenceShape.SetPositions(shapePoints);
        referenceShape.gameObject.SetActive(true); // 도형이 보이도록 설정
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

    private Vector3[] CreateSquare(Vector3 center, float size)
    {
        Vector3[] points = new Vector3[4];
        points[0] = new Vector3(-size / 2, -size / 2, 0) + center;
        points[1] = new Vector3(size / 2, -size / 2, 0) + center;
        points[2] = new Vector3(size / 2, size / 2, 0) + center;
        points[3] = new Vector3(-size / 2, size / 2, 0) + center;
        return points;
    }

    private Vector3[] CreateStar(Vector3 center, float size, int points)
    {
        Vector3[] starPoints = new Vector3[points * 2];
        float angleStep = Mathf.PI * 2 / points;
        for (int i = 0; i < points; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            starPoints[i * 2] = new Vector3(x * size, y * size, 0) + center;
            angle += angleStep / 2;
            x = Mathf.Cos(angle) * (size / 2);
            y = Mathf.Sin(angle) * (size / 2);
            starPoints[i * 2 + 1] = new Vector3(x, y, 0) + center;
        }
        return starPoints;
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
