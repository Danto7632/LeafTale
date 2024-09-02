using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrawingGame : MonoBehaviour
{
    public LineRenderer referenceShape; // 참조 모양 (원 또는 사각형)
    public LineRenderer playerDrawing;  // 플레이어가 그린 모양
    public TMP_Text resultText;         // 정확도를 표시할 TextMeshPro 텍스트

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

        if(Input.GetKeyDown(KeyCode.P)) {
            DrawPerfectShape();
        }
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
        referenceShape.loop = true; // 도형의 끝점을 시작점과 연결
        referenceShape.gameObject.SetActive(true); // 도형이 보이도록 설정
    }

    private void SetLineRendererSortingOrder(LineRenderer lineRenderer, int order)
    {
        Material material = new Material(Shader.Find("Sprites/Default"));
        material.renderQueue = 3000 + order; // Material의 renderQueue 설정
        lineRenderer.material = material;
    }

    private Vector3[] CreateCircle(Vector3 center, float radius, int segments)
    {
        Vector3[] points = new Vector3[segments + 1]; // +1 to close the circle
        float angleStep = 2 * Mathf.PI / segments;
        for (int i = 0; i < segments; i++)
        {
            float angle = angleStep * i;
            points[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius + center;
        }
        points[segments] = points[0]; // Close the circle
        return points;
    }

    private Vector3[] CreateSquare(Vector3 center, float size)
    {
        Vector3[] points = new Vector3[5]; // +1 to close the square
        float halfSize = size / 2;
        points[0] = new Vector3(-halfSize, -halfSize, 0) + center; // Bottom-left
        points[1] = new Vector3(halfSize, -halfSize, 0) + center;  // Bottom-right
        points[2] = new Vector3(halfSize, halfSize, 0) + center;   // Top-right
        points[3] = new Vector3(-halfSize, halfSize, 0) + center;   // Top-left
        points[4] = points[0]; // Close the square
        return points;
    }

    private Vector3[] CreateStar(Vector3 center, float size, int points)
    {
        Vector3[] starPoints = new Vector3[points * 2 + 1]; // +1 to close the star
        float angleStep = Mathf.PI * 2 / points;
        float innerSize = size * 0.5f; // 내측 별의 크기 (조정 가능)

        for (int i = 0; i < points; i++)
        {
            float angle = i * angleStep;
            float outerX = Mathf.Cos(angle) * size;
            float outerY = Mathf.Sin(angle) * size;
            starPoints[i * 2] = new Vector3(outerX, outerY, 0) + center; // Outer points
            
            // Inner points
            angle += angleStep / 2;
            float innerX = Mathf.Cos(angle) * innerSize;
            float innerY = Mathf.Sin(angle) * innerSize;
            starPoints[i * 2 + 1] = new Vector3(innerX, innerY, 0) + center; // Inner points
        }
        starPoints[points * 2] = starPoints[0]; // Close the star
        return starPoints;
    }

    private float CalculateAccuracy()
    {
        float totalDistance = 0f;

        // 참조 도형의 점 개수만큼 반복
        for (int i = 0; i < referenceShape.positionCount; i++)
        {
            float minDistance = float.MaxValue;

            // 참조 도형의 각 점과 플레이어가 그린 점들 중 가장 가까운 점을 비교
            for (int j = 0; j < playerPoints.Count; j++)
            {
                float distance = Vector2.Distance(playerPoints[j], referenceShape.GetPosition(i));
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
            totalDistance += minDistance;
        }

        float averageDistance = totalDistance / referenceShape.positionCount;
        float maxPossibleDistance = 2.5f; // 조정 가능한 임계값

        Debug.Log(playerPoints.Count);

        return Mathf.Clamp01(1f - averageDistance / maxPossibleDistance);
    }

    public void DrawPerfectShape()
    {
        // 참조 도형의 점들과 동일한 점들을 플레이어의 그린 점들로 설정
        playerPoints.Clear();
        for (int i = 0; i < referenceShape.positionCount; i++)
        {
            Vector3 refPoint = referenceShape.GetPosition(i);
            playerPoints.Add(new Vector2(refPoint.x, refPoint.y));
        }

        // LineRenderer 업데이트
        playerDrawing.positionCount = playerPoints.Count;
        playerDrawing.SetPositions(playerPoints.ConvertAll(p => new Vector3(p.x, p.y, 0)).ToArray());

        // 정확도 100% 표시
        float accuracy = CalculateAccuracy();
        resultText.text = "Accuracy: " + (accuracy * 100f).ToString("F2") + "%";
    }
}
