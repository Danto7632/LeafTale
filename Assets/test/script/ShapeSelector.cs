using UnityEngine;
using UnityEngine.UI; // UI 관련 네임스페이스
using TMPro; // TextMeshPro 관련 네임스페이스
using System.Collections;

public class ShapeSelector : MonoBehaviour {
    public DrawingGame drawingGame;

    public TMP_Text startTimer;
    public TMP_Text gameTimer;
    public Canvas canvas;

    public LineRenderer referenceShape;
    public LineRenderer playerDrawing;

    public int shapeCount;

    public bool isPlaying;
    public bool isNext;

    private void Start() {
        drawingGame = GameObject.Find("DrawingGame").GetComponent<DrawingGame>();

        drawingGame.resultText.gameObject.SetActive(false);
        referenceShape.gameObject.SetActive(false);
        playerDrawing.gameObject.SetActive(false);

        isPlaying = false;

        shapeCount = 0;
    }

    public void gameStart() {
        startTimer = GameObject.Find("StartTimer").GetComponent<TMP_Text>();
        gameTimer = GameObject.Find("GameTimer").GetComponent<TMP_Text>();

        nextStage(true);
    }

    public void nextStage(bool isFirst) {
        if(isFirst) {
            StartCoroutine(SelectShape((Shape)shapeCount));
            StartCoroutine(timer());

            shapeCount++;
        }
        else {
            if(shapeCount < 3) {
                drawingGame.resultText.text = "Complete!";
                drawingGame.sumScore += drawingGame.maxScore;
                drawingGame.accuracy = 0f;
                StartCoroutine(delayStage());
            }
            else {
                isPlaying = false;
                referenceShape.gameObject.SetActive(false);
                playerDrawing.gameObject.SetActive(false);
                startTimer.gameObject.SetActive(true);
                drawingGame.resultText.gameObject.SetActive(false);
                drawingGame.playerPoints.Clear();
                drawingGame.playerDrawing.positionCount = 0;
                startTimer.text = "Done";
                gameTimer.text = drawingGame.sumScore.ToString();
            }
        }
    }

    IEnumerator delayStage() {
        StopCoroutine(stageTimer());
        isPlaying = false;

        yield return new WaitForSeconds(2f);

        StartCoroutine(SelectShape((Shape)shapeCount));
        StartCoroutine(timer());

        shapeCount++;

    }

    IEnumerator SelectShape(Shape shape) {
        referenceShape.gameObject.SetActive(false);
        playerDrawing.gameObject.SetActive(false);
        startTimer.gameObject.SetActive(true);
        drawingGame.resultText.gameObject.SetActive(false);
        drawingGame.playerPoints.Clear();
        drawingGame.playerDrawing.positionCount = 0;

        isPlaying = false;

        yield return new WaitForSeconds(3f);

        StartCoroutine(stageTimer());
        referenceShape.gameObject.SetActive(true);
        playerDrawing.gameObject.SetActive(true);
        startTimer.gameObject.SetActive(false);

        isPlaying = true;

        drawingGame.SetReferenceShape(shape);
    }

    IEnumerator timer() {
        int countdown = 3;

        while (countdown >= 0) {
            startTimer.text = countdown.ToString(); // 텍스트 업데이트
            yield return new WaitForSeconds(1f);    // 1초 대기
            countdown--;
        }
    }

    IEnumerator stageTimer() {
        drawingGame.countdownTimer = 30;

        while (drawingGame.countdownTimer >= 0) {
            gameTimer.text = drawingGame.countdownTimer.ToString(); // 텍스트 업데이트
            yield return new WaitForSeconds(1f);    // 1초 대기
            drawingGame.countdownTimer--;
        }
    }
}
