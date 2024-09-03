using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Leap;
using Leap.Unity;
using System.Linq;

// public class DrawingGame : MonoBehaviour
// {
//     public LineRenderer referenceShape; // 참조 모양 (원 또는 사각형)
//     public LineRenderer playerDrawing;  // 플레이어가 그린 모양
//     public TMP_Text resultText;         // 정확도를 표시할 TextMeshPro 텍스트

//     public Vector3[] circleShape;
//     public Vector3[] squareShape;
//     public Vector3[] starShape;

//     private List<Vector3> playerPoints = new List<Vector3>(); // 플레이어가 그린 점들
//     private bool isDrawing = false; // 그리기 중 여부

//     private void Start()
//     {
//         referenceShape.gameObject.SetActive(false); // 게임 시작 시 도형 비활성화
//         playerDrawing.gameObject.SetActive(true);   // 플레이어의 그리기 활성화

//         // Sorting Order 설정
//         SetLineRendererSortingOrder(referenceShape, 0);
//         SetLineRendererSortingOrder(playerDrawing, 1);
//     }

//     private void Update()
//     {
//         if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 누를 때
//         {
//             // 새 그림을 그릴 때 기존 점들 초기화
//             playerPoints.Clear();
//             playerDrawing.positionCount = 0;
//             isDrawing = true;
//         }

//         if (Input.GetMouseButton(0) && isDrawing) // 마우스 왼쪽 버튼을 누르고 있을 때
//         {
//             Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             mousePos.z = 0; // 2D 게임이므로 Z 축을 0으로 고정
//             playerPoints.Add(mousePos);
//             playerDrawing.positionCount = playerPoints.Count;
//             playerDrawing.SetPositions(playerPoints.ToArray());
//         }
        
//         if (Input.GetMouseButtonUp(0)) // 마우스를 놓을 때
//         {
//             isDrawing = false;
//             float accuracy = CalculateAccuracy();
//             resultText.text = "Accuracy: " + (accuracy * 100f).ToString("F2") + "%";
//         }
//     }

//     private void SetLineRendererSortingOrder(LineRenderer lineRenderer, int order)
//     {
//         Material material = new Material(Shader.Find("Sprites/Default"));
//         material.renderQueue = 3000 + order; // Material의 renderQueue 설정
//         lineRenderer.material = material;
//     }

//     public void SetReferenceShape(Shape shape)
//     {
//         switch (shape)
//         {
//             case Shape.Circle:
//                 SetReferenceShape(CreateCircle(Vector3.zero, 5f, 100));
//                 break;
//             case Shape.Square:
//                 SetReferenceShape(CreateSquare(Vector3.zero, 5f));
//                 break;
//             case Shape.Star:
//                 SetReferenceShape(CreateStar(Vector3.zero, 5f, 5));
//                 break;
//         }
//     }

//     private void SetReferenceShape(Vector3[] shapePoints)
//     {
//         referenceShape.positionCount = shapePoints.Length;
//         referenceShape.SetPositions(shapePoints);
//         referenceShape.gameObject.SetActive(true); // 도형이 보이도록 설정
//     }

//     private Vector3[] CreateCircle(Vector3 center, float radius, int segments)
//     {
//         Vector3[] points = new Vector3[segments];
//         for (int i = 0; i < segments; i++)
//         {
//             float angle = 2 * Mathf.PI * i / segments;
//             points[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius + center;
//         }
//         return points;
//     }

//     private Vector3[] CreateSquare(Vector3 center, float size)
//     {
//         Vector3[] points = new Vector3[4];
//         points[0] = new Vector3(-size / 2, -size / 2, 0) + center;
//         points[1] = new Vector3(size / 2, -size / 2, 0) + center;
//         points[2] = new Vector3(size / 2, size / 2, 0) + center;
//         points[3] = new Vector3(-size / 2, size / 2, 0) + center;
//         return points;
//     }

//     private Vector3[] CreateStar(Vector3 center, float size, int points)
//     {
//         Vector3[] starPoints = new Vector3[points * 2];
//         float angleStep = Mathf.PI * 2 / points;
//         for (int i = 0; i < points; i++)
//         {
//             float angle = i * angleStep;
//             float x = Mathf.Cos(angle);
//             float y = Mathf.Sin(angle);
//             starPoints[i * 2] = new Vector3(x * size, y * size, 0) + center;
//             angle += angleStep / 2;
//             x = Mathf.Cos(angle) * (size / 2);
//             y = Mathf.Sin(angle) * (size / 2);
//             starPoints[i * 2 + 1] = new Vector3(x, y, 0) + center;
//         }
//         return starPoints;
//     }

//     private float CalculateAccuracy()
//     {
//         float totalDistance = 0f;
//         for (int i = 0; i < playerPoints.Count; i++)
//         {
//             float minDistance = float.MaxValue;
//             for (int j = 0; j < referenceShape.positionCount; j++)
//             {
//                 float distance = Vector3.Distance(playerPoints[i], referenceShape.GetPosition(j));
//                 if (distance < minDistance)
//                 {
//                     minDistance = distance;
//                 }
//             }
//             totalDistance += minDistance;
//         }

//         float averageDistance = totalDistance / playerPoints.Count;
//         float maxPossibleDistance = 1f; // 조정 가능한 임계값
//         return Mathf.Clamp01(1f - averageDistance / maxPossibleDistance);
//     }
// }

// public class DrawingGame : MonoBehaviour
// {
//     private LeapServiceProvider leapProvider;
    
//     public LineRenderer referenceShape; // 참조 모양 (원 또는 사각형)
//     public LineRenderer playerDrawing;  // 플레이어가 그린 모양
//     public TMP_Text resultText;         // 정확도를 표시할 TextMeshPro 텍스

//     public Vector3[] circleShape;
//     public Vector3[] squareShape;
//     public Vector3[] starShape;
//     public GameObject handVisualizerPrefab; // 손 위치를 시각화할 프리팹 (2D 오브젝트)

//     private List<Vector3> playerPoints = new List<Vector3>(); // 플레이어가 그린 점들
//     private bool isDrawing = false; // 그리기 중 여부

//     private Controller leapController; // Leap Motion 컨트롤러
//     private GameObject handVisualizer; // 손의 위치를 시각화할 오브젝트

//     private void Start()
//     {
//         leapProvider = FindObjectOfType<LeapServiceProvider>();
//         leapProvider.OnUpdateFrame += OnUpdateFrame;
//         leapController = new Controller();

//         referenceShape.gameObject.SetActive(false); // 게임 시작 시 도형 비활성화
//         playerDrawing.gameObject.SetActive(true);   // 플레이어의 그리기 활성화

//         // Sorting Order 설정
//         SetLineRendererSortingOrder(referenceShape, 0);
//         SetLineRendererSortingOrder(playerDrawing, 1);

//         handVisualizer = Instantiate(handVisualizerPrefab);
//         handVisualizer.SetActive(false); // 초기에는 비활성화
//     }

//     void OnDestroy() {
//         if (leapProvider != null) {
//             leapProvider.OnUpdateFrame -= OnUpdateFrame;
//         }
//     }

//     private Vector3 initialHandPosition;
//     private bool isInitialPositionSet = false;

//     private void OnUpdateFrame(Frame frame) {
//         if (frame.Hands.Count > 0) // 손이 화면에 있을 때
//         {
//             Hand primaryHand = frame.Hands[0]; // 첫 번째 손 선택

//             // 손의 위치를 화면 좌표로 변환 (중앙 정렬)
//             Vector3 leapHandPosition = primaryHand.PalmPosition;

//             if (!isInitialPositionSet) {
//                 initialHandPosition = leapHandPosition; // 첫 손 위치를 초기 위치로 설정
//                 isInitialPositionSet = true;
//             }

//             // 초기 위치를 원점(0, 0, 0)으로 하고 상대적인 위치 계산
//             Vector3 relativePosition = leapHandPosition - initialHandPosition;
//             Vector3 screenPosition = LeapToWorld(relativePosition);

//             // 손 위치 시각화 (2D 오브젝트로)
//             handVisualizer.SetActive(true);
//             handVisualizer.transform.position = screenPosition;

//             if (IsPointingPose(primaryHand)) // 주먹을 쥐고 있을 때
//             {

//                 if (!isDrawing)
//                 {
//                     playerPoints.Clear();
//                     playerDrawing.positionCount = 0;
//                     isDrawing = true;
//                 }

//                 playerPoints.Add(screenPosition);
//                 playerDrawing.positionCount = playerPoints.Count;
//                 playerDrawing.SetPositions(playerPoints.ToArray());
//             }
//             else // 주먹을 풀었을 때
//             {
//                 isDrawing = false;

//                 // playerPoints가 비어있지 않을 때만 CalculateAccuracy 호출
//                 if (playerPoints.Count > 0)
//                 {
//                     float accuracy = CalculateAccuracy();
//                     resultText.text = "Accuracy: " + (accuracy * 100f).ToString("F2") + "%";
//                 }
//                 else
//                 {
//                     resultText.text = "No drawing detected!";
//                 }
//             }


//             if(IsFist(primaryHand)) {
//                 initialHandPosition = leapHandPosition;
//             }
//         }
//         else
//         {
//             // 손이 감지되지 않을 때 시각화 비활성화
//             handVisualizer.SetActive(false);
//             // isInitialPositionSet 재설정 코드 제거
//         }
//     }

//     private Vector3 LeapToWorld(Vector3 leapHandPosition)
//     {
//         // Leap Motion 좌표계를 Unity 월드 좌표로 변환 (간단한 스케일링 및 위치 조정)
//         // 이 값을 조정하여 손의 움직임이 적절히 매핑되도록 설정
//         float scale = 10f; // 손 움직임을 좀 더 크게 보이도록 스케일 조정
//         Vector3 worldPosition = new Vector3(
//             leapHandPosition.x * scale,
//             leapHandPosition.y * scale,
//             leapHandPosition.z * scale);

//         worldPosition.z = 0; // 2D 게임이므로 z를 0으로 고정

//         return worldPosition;
//     }

//     private void SetLineRendererSortingOrder(LineRenderer lineRenderer, int order)
//     {
//         Material material = new Material(Shader.Find("Sprites/Default"));
//         material.renderQueue = 3000 + order; // Material의 renderQueue 설정
//         lineRenderer.material = material;
//     }

//     public void SetReferenceShape(Shape shape)
//     {
//         switch (shape)
//         {
//             case Shape.Circle:
//                 SetReferenceShape(CreateCircle(Vector3.zero, 5f, 100));
//                 break;
//             case Shape.Square:
//                 SetReferenceShape(CreateSquare(Vector3.zero, 5f));
//                 break;
//             case Shape.Star:
//                 SetReferenceShape(CreateStar(Vector3.zero, 5f, 5));
//                 break;
//         }
//     }

//     private void SetReferenceShape(Vector3[] shapePoints)
//     {
//         referenceShape.positionCount = shapePoints.Length;
//         referenceShape.SetPositions(shapePoints);
//         referenceShape.loop = true; // 도형의 끝점을 시작점과 연결
//         referenceShape.gameObject.SetActive(true); // 도형이 보이도록 설정
//     }

//     private Vector3[] CreateCircle(Vector3 center, float radius, int segments)
//     {
//         Vector3[] points = new Vector3[segments];
//         for (int i = 0; i < segments; i++)
//         {
//             float angle = 2 * Mathf.PI * i / segments;
//             points[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius + center;
//         }
//         return points;
//     }

//     private Vector3[] CreateSquare(Vector3 center, float size)
//     {
//         Vector3[] points = new Vector3[4];
//         points[0] = new Vector3(-size / 2, -size / 2, 0) + center;
//         points[1] = new Vector3(size / 2, -size / 2, 0) + center;
//         points[2] = new Vector3(size / 2, size / 2, 0) + center;
//         points[3] = new Vector3(-size / 2, size / 2, 0) + center;
//         return points;
//     }

//     private Vector3[] CreateStar(Vector3 center, float size, int points)
//     {
//         Vector3[] starPoints = new Vector3[points * 2];
//         float angleStep = Mathf.PI * 2 / points;
//         for (int i = 0; i < points; i++)
//         {
//             float angle = i * angleStep;
//             float x = Mathf.Cos(angle);
//             float y = Mathf.Sin(angle);
//             starPoints[i * 2] = new Vector3(x * size, y * size, 0) + center;
//             angle += angleStep / 2;
//             x = Mathf.Cos(angle) * (size / 2);
//             y = Mathf.Sin(angle) * (size / 2);
//             starPoints[i * 2 + 1] = new Vector3(x, y, 0) + center;
//         }
//         return starPoints;
//     }

//     private float CalculateAccuracy()
//     {
//         float totalDistance = 0f;

//         // 참조 도형의 점 개수만큼 반복
//         for (int i = 0; i < referenceShape.positionCount; i++)
//         {
//             float minDistance = float.MaxValue;

//             // 참조 도형의 각 점과 플레이어가 그린 점들 중 가장 가까운 점을 비교
//             for (int j = 0; j < playerPoints.Count; j++)
//             {
//                 float distance = Vector2.Distance(playerPoints[j], referenceShape.GetPosition(i));
//                 if (distance < minDistance)
//                 {
//                     minDistance = distance;
//                 }
//             }
//             totalDistance += minDistance;
//         }

//         float averageDistance = totalDistance / referenceShape.positionCount;
//         float maxPossibleDistance = 2.5f; // 조정 가능한 임계값

//         Debug.Log(playerPoints.Count);

//         return Mathf.Clamp01(1f - averageDistance / maxPossibleDistance);
//     }

//     bool IsPointingPose(Hand hand) {
//         foreach (Finger finger in hand.Fingers) { //손의 손가락을 모두 가져와 반복문 실행
//             if (finger.Type == Finger.FingerType.TYPE_INDEX) {
//                 if (!finger.IsExtended) return false;
//             } //검지가 펴져있지 않다면 false를 반환
//             else {
//                 if (finger.IsExtended) return false;
//             } //검지를 제외한 다른 손가락이 펴져있다면 false를 반환
//         }
        
//         return true; //검지만 펴져있다면 true를 반환
//     }

//     bool IsFist(Hand hand) {
//         return hand.GrabStrength > 0.9f;
//     } //손의 쥐기 강도를 감지하여 주먹을 쥐었는지 감지하여 true를 반환하는 함수
// }


public class DrawingGame : MonoBehaviour
{
    private LeapServiceProvider leapProvider;

    public LineRenderer referenceShape; // 참조 모양 (원 또는 사각형)
    public LineRenderer playerDrawing;  // 플레이어가 그린 모양
    public TMP_Text resultText;         // 정확도를 표시할 TextMeshPro 텍스트

    public Vector2[] circleShape;
    public Vector2[] squareShape;
    public Vector2[] starShape;

    private List<Vector2> playerPoints = new List<Vector2>(); // 플레이어가 그린 점들
    private bool isDrawing = false; // 그리기 중 여부

    private Controller leapController; // Leap Motion 컨트롤러
    private GameObject handVisualizer; // 손의 위치를 시각화할 오브젝트

    public GameObject handVisualizerPrefab; // 손 위치를 시각화할 프리팹 (2D 오브젝트)

    private void Start()
    {
        referenceShape.gameObject.SetActive(false); // 게임 시작 시 도형 비활성화
        playerDrawing.gameObject.SetActive(true);   // 플레이어의 그리기 활성화

        // Sorting Order 설정
        SetLineRendererSortingOrder(referenceShape, 0);
        SetLineRendererSortingOrder(playerDrawing, 1);

        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;
        leapController = new Controller();

        handVisualizer = Instantiate(handVisualizerPrefab);
        handVisualizer.SetActive(false); // 초기에는 비활성화
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
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            playerPoints.Add(mousePos);
            playerDrawing.positionCount = playerPoints.Count;

            // LineRenderer는 여전히 Vector3를 사용하므로 변환 필요
            Vector3[] playerPositions = playerPoints.Select(p => new Vector3(p.x, p.y, 0)).ToArray();
            playerDrawing.SetPositions(playerPositions);
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

    private Vector3 initialHandPosition;
    private bool isInitialPositionSet = false;

    private void OnUpdateFrame(Frame frame) {
        if (frame.Hands.Count > 0) // 손이 화면에 있을 때
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
            handVisualizer.transform.position = screenPosition;

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
                    float accuracy = CalculateAccuracy();
                    resultText.text = "Accuracy: " + (accuracy * 100f).ToString("F2") + "%";
                }
                else
                {
                    resultText.text = "No drawing detected!";
                }
            }

            if(IsFist(primaryHand)) {
                initialHandPosition = leapHandPosition;
            }
        }
        else
        {
            // 손이 감지되지 않을 때 시각화 비활성화
            handVisualizer.SetActive(false);
            // isInitialPositionSet 재설정 코드 제거
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
                SetReferenceShape(CreateCircle(Vector2.zero, 5f, 100));
                break;
            case Shape.Square:
                SetReferenceShape(CreateSquare(Vector2.zero, 5f));
                break;
            case Shape.Star:
                SetReferenceShape(CreateStar(Vector2.zero, 5f, 5));
                break;
        }
    }

    private void SetReferenceShape(Vector2[] shapePoints)
    {
        // 도형의 점 개수에 따라 포인트 수 설정
        referenceShape.positionCount = shapePoints.Length + 1; // 마지막 점을 추가하여 닫히게 만듦
        Vector3[] positions = shapePoints.Select(p => new Vector3(p.x, p.y, 0)).ToArray();
        referenceShape.SetPositions(positions);
        referenceShape.SetPosition(shapePoints.Length, positions[0]); // 마지막 점을 시작점으로 설정
        referenceShape.loop = true; // 도형의 끝점을 시작점과 연결
        referenceShape.gameObject.SetActive(true); // 도형이 보이도록 설정
    }

    private Vector2[] CreateCircle(Vector2 center, float radius, int segments)
    {
        Vector2[] points = new Vector2[segments];
        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            points[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius + center;
        }
        return points;
    }

    private Vector2[] CreateSquare(Vector2 center, float size)
    {
        Vector2[] points = new Vector2[4];
        points[0] = new Vector2(-size / 2, -size / 2) + center;
        points[1] = new Vector2(size / 2, -size / 2) + center;
        points[2] = new Vector2(size / 2, size / 2) + center;
        points[3] = new Vector2(-size / 2, size / 2) + center;
        return points;
    }

    private Vector2[] CreateStar(Vector2 center, float size, int points)
    {
        Vector2[] starPoints = new Vector2[points * 2];
        float angleStep = Mathf.PI * 2 / points;
        for (int i = 0; i < points; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            starPoints[i * 2] = new Vector2(x * size, y * size) + center;
            angle += angleStep / 2;
            x = Mathf.Cos(angle) * (size / 2);
            y = Mathf.Sin(angle) * (size / 2);
            starPoints[i * 2 + 1] = new Vector2(x, y) + center;
        }
        return starPoints;
    }

    private float CalculateAccuracy()
    {
        float totalDistance = 0f;
        Vector3[] refPositions = new Vector3[referenceShape.positionCount];
        referenceShape.GetPositions(refPositions);

        // 참조 도형의 점 개수만큼 반복
        for (int i = 0; i < refPositions.Length; i++)
        {
            float minDistance = float.MaxValue;

            // 참조 도형의 각 점과 플레이어가 그린 점들 중 가장 가까운 점을 비교
            for (int j = 0; j < playerPoints.Count; j++)
            {
                float distance = Vector2.Distance(playerPoints[j], new Vector2(refPositions[i].x, refPositions[i].y));
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
            totalDistance += minDistance;
        }

        float averageDistance = totalDistance / refPositions.Length;
        float maxPossibleDistance = 2.5f; // 조정 가능한 임계값

        return Mathf.Clamp01(1f - averageDistance / maxPossibleDistance);
    }

    public void DrawPerfectShape()
    {
        // 참조 도형의 점들과 동일한 점들을 플레이어의 그린 점들로 설정
        playerPoints.Clear();
        Vector3[] refPoints = new Vector3[referenceShape.positionCount];
        referenceShape.GetPositions(refPoints);

        for (int i = 0; i < refPoints.Length - 1; i++) // 마지막 점은 제외
        {
            Vector3 refPoint = refPoints[i];
            playerPoints.Add(new Vector2(refPoint.x, refPoint.y));
        }

        // LineRenderer 업데이트
        playerDrawing.positionCount = playerPoints.Count + 1; // 마지막 점을 추가하여 닫히게 만듦
        Vector3[] playerPositions = playerPoints.Select(p => new Vector3(p.x, p.y, 0)).ToArray();
        playerDrawing.SetPositions(playerPositions);
        playerDrawing.SetPosition(playerPoints.Count, playerPositions[0]); // 마지막 점을 시작점으로 설정

        // 정확도 100% 표시
        float accuracy = CalculateAccuracy();
        resultText.text = "Accuracy: " + (accuracy * 100f).ToString("F2") + "%";
    }

    private Vector3 LeapToWorld(Vector3 leapHandPosition)
    {
        // Leap Motion 좌표계를 Unity 월드 좌표로 변환 (간단한 스케일링 및 위치 조정)
        // 이 값을 조정하여 손의 움직임이 적절히 매핑되도록 설정
        float scale = 10f; // 손 움직임을 좀 더 크게 보이도록 스케일 조정
        Vector3 worldPosition = new Vector3(
            leapHandPosition.x * scale,
            leapHandPosition.y * scale,
            leapHandPosition.z * scale);

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