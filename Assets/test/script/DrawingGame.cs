using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Leap;
using Leap.Unity;
using System.Linq;

public class DrawingGame : MonoBehaviour
{
    private LeapServiceProvider leapProvider;

    private Controller leapController; // Leap Motion 컨트롤러
    private GameObject handVisualizer; // 손의 위치를 시각화할 오브젝트

    public GameObject handVisualizerPrefab; // 손 위치를 시각화할 프리팹 (2D 오브젝트)
    public static bool isBtnClicked;

    private Vector3 initialHandPosition;
    private bool isInitialPositionSet = false;

    public RectTransform[] shapeBtns;
    public CircleCollider2D handCollider;

    public LineRenderer referenceShape; // 참조 모양 (원 또는 사각형)
    public LineRenderer playerDrawing;  // 플레이어가 그린 모양
    public TMP_Text resultText;         // 정확도를 표시할 TextMeshPro 텍스트

    public int circleMaxPoints = 100;   // 원을 그릴 때 최대 점의 수
    public int squareMaxPoints = 50;    // 사각형을 그릴 때 최대 점의 수
    public int starMaxPoints = 150;     // 별을 그릴 때 최대 점의 수

    public List<Vector3> playerPoints = new List<Vector3>(); // 플레이어가 그린 점들
    private bool isDrawing = false; // 그리기 중 여부
    private int maxPoints;          // 현재 선택된 도형에 따른 최대 점의 수

    public ShapeSelector shapeSelector;

    public int countdownTimer;

    public float maxScore;
    public float sumScore;

    public float accuracy;
    
    private void Start()
    {

        // Sorting Order 설정
        SetLineRendererSortingOrder(referenceShape, 0);
        SetLineRendererSortingOrder(playerDrawing, 1);

        // 초기 도형 색상 설정
        SetLineRendererColor(referenceShape, Color.blue); // 초기 도형 색상 파란색

        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;
        leapController = new Controller();

        handVisualizer = Instantiate(handVisualizerPrefab);
        handVisualizer.SetActive(false); // 초기에는 비활성화

        shapeBtns = new RectTransform[3];
        handCollider = handVisualizer.GetComponent<CircleCollider2D>();

        shapeSelector = GameObject.Find("ShapeSelector").GetComponent<ShapeSelector>();

        maxScore = 0f;
        sumScore = 0f;
    }

    private void Update()
    {
        if(shapeSelector.isPlaying) {
            if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 누를 때
            {
                // 새 그림을 그릴 때 기존 점들 초기화
                playerPoints.Clear();
                playerDrawing.positionCount = 0;
                isDrawing = true;
            }   

            if (Input.GetMouseButton(0) && isDrawing) // 마우스 왼쪽 버튼을 누르고 있을 때
            {
                // 점의 개수가 최대치를 초과하지 않도록 조건을 추가
                if (playerPoints.Count < maxPoints)
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = 0; // 2D 게임이므로 Z 축을 0으로 고정

                    // 플레이어가 마우스를 움직이면 새로운 점을 추가
                    if (playerPoints.Count == 0 || Vector3.Distance(playerPoints[playerPoints.Count - 1], mousePos) > 0.01f)
                    {
                        playerPoints.Add(mousePos);
                        playerDrawing.positionCount = playerPoints.Count;
                        playerDrawing.SetPositions(playerPoints.ToArray());
                    }
                }
            }

            if (Input.GetMouseButtonUp(0)) // 마우스를 놓을 때
            {
                isDrawing = false;
                accuracy = CalculateAccuracy();
                resultText.gameObject.SetActive(true);
                resultText.text = "Accuracy: " + (accuracy * 100f).ToString("F2") + "%";
                if(accuracy * 100f >= 90f) {
                    shapeSelector.nextStage(false);
                    maxScore = 100f;
                }
            }

            if (Input.GetKeyDown(KeyCode.P)) {
                DrawPerfectShape();
            }

            if(countdownTimer <= 0) {
                shapeSelector.nextStage(false);
            }
            else {
                if(maxScore <= accuracy * 100f) {
                    maxScore = accuracy * 100f;
                }
            }
        }
        else {
            maxScore = 0f;
        }
    }

    public Vector3 minLimit = new Vector3(-8, -4.5f, 0); // 최소 위치
    public Vector3 maxLimit = new Vector3(8, 4.5f, 0);  // 최대 위치

    private void OnUpdateFrame(Frame frame) {
        if (frame.Hands.Count > 0 && isBtnClicked && shapeSelector.isPlaying) // 손이 화면에 있을 때
        {
            Hand primaryHand = frame.Hands[0]; // 첫 번째 손 선택

            // 손의 위치를 화면 좌표로 변환 (중앙 정렬)
            Vector3 leapHandPosition = primaryHand.PalmPosition;

            if (!isInitialPositionSet) {
                initialHandPosition = leapHandPosition; // 첫 손 위치를 초기 위치로 설정
                isInitialPositionSet = true;
            }

            // 초기 위치를 원점(0, 0, 0)으로 하고 상대적인 위치 계산
            Vector3 relativePosition = leapHandPosition - initialHandPosition;
            Vector3 screenPosition = LeapToWorld(relativePosition);

            // 손 위치 시각화 (2D 오브젝트로)
            handVisualizer.SetActive(true);

            // X, Y, Z 좌표를 개별적으로 제한
            screenPosition.x = Mathf.Clamp(screenPosition.x, -8, 8);
            screenPosition.y = Mathf.Clamp(screenPosition.y, -4.5f, 4.5f);

            if(IsFist(primaryHand)) {
                handVisualizer.transform.position = new Vector3(0, 0, 0);
                initialHandPosition = leapHandPosition;
            }
            else {
                handVisualizer.transform.position = screenPosition;
            }

            if (IsPointingPose(primaryHand)) // 주먹을 쥐고 있을 때
            {
                if (!isDrawing)
                {
                    playerPoints.Clear();
                    playerDrawing.positionCount = 0;
                    isDrawing = true;
                }

                playerPoints.Add(screenPosition);
                playerDrawing.positionCount = playerPoints.Count;
                Vector3[] playerPositions = playerPoints.Select(p => new Vector3(p.x, p.y, 0)).ToArray();
                playerDrawing.SetPositions(playerPositions);
            }
            else // 주먹을 풀었을 때
            {
                isDrawing = false;

                // playerPoints가 비어있지 않을 때만 CalculateAccuracy 호출
                if (playerPoints.Count > 0)
                {
                    isDrawing = false;
                    accuracy = CalculateAccuracy();
                    resultText.gameObject.SetActive(true);
                    resultText.text = "Accuracy: " + (accuracy * 100f).ToString("F2") + "%";
                    if(accuracy * 100f >= 90f) {
                        shapeSelector.nextStage(false);
                        maxScore = 100f;
                    }
                }
            }
            if(countdownTimer <= 0) {
                shapeSelector.nextStage(false);
            }
            else {
                if(maxScore <= accuracy * 100f) {
                    maxScore = accuracy * 100f;
                }
            }
        }
        else
        {
            // 손이 감지되지 않을 때 시각화 비활성화
            handVisualizer.SetActive(false);
            // isInitialPositionSet 재설정 코드 제거
        }
    }

    public void SetReferenceShape(Shape shape)
    {
        switch (shape)
        {
            case Shape.Circle:
                SetReferenceShape(CreateCircle(Vector3.zero, 3f, 100));
                maxPoints = circleMaxPoints; // 원에 대한 최대 점 수 설정
                break;
            case Shape.Square:
                SetReferenceShape(CreateSquare(Vector3.zero, 5f));
                maxPoints = squareMaxPoints; // 사각형에 대한 최대 점 수 설정
                break;
            case Shape.Star:
                SetReferenceShape(CreateStar(Vector3.zero, 3f, 5));
                maxPoints = starMaxPoints; // 별에 대한 최대 점 수 설정
                break;
        }
    }

    private void SetReferenceShape(Vector3[] shapePoints)
    {
        referenceShape.positionCount = shapePoints.Length;
        referenceShape.SetPositions(shapePoints);
        referenceShape.gameObject.SetActive(true); // 도형이 보이도록 설정
    }

    private void SetLineRendererSortingOrder(LineRenderer lineRenderer, int order)
    {
        Material material = new Material(Shader.Find("Sprites/Default"));
        material.renderQueue = 3000 + order; // Material의 renderQueue 설정
        lineRenderer.material = material;
    }

    private void SetLineRendererColor(LineRenderer lineRenderer, Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
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
        Vector3[] points = new Vector3[5]; // 5점 사용 (Square는 4점 + 시작점으로 닫기 위해 1점 추가)
        float halfSize = size / 2;
        points[0] = new Vector3(-halfSize, -halfSize, 0) + center; // Bottom-left
        points[1] = new Vector3(halfSize, -halfSize, 0) + center;  // Bottom-right
        points[2] = new Vector3(halfSize, halfSize, 0) + center;   // Top-right
        points[3] = new Vector3(-halfSize, halfSize, 0) + center;  // Top-left
        points[4] = points[0]; // 첫 번째 점으로 돌아가서 사각형을 닫음

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

    private Vector3 LeapToWorld(Vector3 leapHandPosition)
    {
        // Leap Motion 좌표계를 Unity 월드 좌표로 변환 (간단한 스케일링 및 위치 조정)
        // 이 값을 조정하여 손의 움직임이 적절히 매핑되도록 설정
        float scale = 6f;
        Vector3 worldPosition = new Vector3(
            leapHandPosition.x * scale,
            leapHandPosition.y * scale,
            leapHandPosition.z * 0f
        );

        worldPosition.z = 0; // 2D 게임이므로 z를 0으로 고정

        return worldPosition;
    }

    bool IsPointingPose(Hand hand) {
        foreach (Finger finger in hand.Fingers) { //손의 손가락을 모두 가져와 반복문 실행
            if (finger.Type == Finger.FingerType.TYPE_INDEX) {
                if (!finger.IsExtended) return false;
            } //검지가 펴져있지 않다면 false를 반환
            else {
                if (finger.IsExtended) return false;
            } //검지를 제외한 다른 손가락이 펴져있다면 false를 반환
        }
        
        return true; //검지만 펴져있다면 true를 반환
    }

    bool IsFist(Hand hand) {
        return hand.GrabStrength > 0.9f;
    } //손의 쥐기 강도를 감지하여 주먹을 쥐었는지 감지하여 true를 반환하는 함수
}
