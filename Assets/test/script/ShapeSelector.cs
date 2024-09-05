using UnityEngine;
using UnityEngine.UI; // UI 관련 네임스페이스
using TMPro; // TextMeshPro 관련 네임스페이스

public class ShapeSelector : MonoBehaviour {
    public DrawingGame drawingGame; // DrawingGame 스크립트 참조
    public Button circleButton; // TextMeshPro Button 참조
    public Button squareButton; // TextMeshPro Button 참조
    public Button starButton;   // TextMeshPro Button 참조

    public Canvas canvas;        // Canvas 참조

    private void Start()
    {
        // 버튼 클릭 이벤트 추가
        circleButton.onClick.AddListener(() => SelectShape(Shape.Circle));
        squareButton.onClick.AddListener(() => SelectShape(Shape.Square));
        starButton.onClick.AddListener(() => SelectShape(Shape.Star));
    }

    private void SelectShape(Shape shape) {
        drawingGame.SetReferenceShape(shape);
        canvas.gameObject.SetActive(false); // Canvas 비활성화

        DrawingGame.isBtnClicked = true;
    }
}
