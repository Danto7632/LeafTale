using UnityEngine;
using UnityEngine.UI; // UI 관련 네임스페이스
using TMPro; // TextMeshPro 관련 네임스페이스
using System.Collections;

public class ShapeSelector : MonoBehaviour {
    public DrawingGame drawingGame;

    public TMP_Text startTimer;
    public TMP_Text gameTimer;

    public LineRenderer referenceShape;
    public LineRenderer playerDrawing;

    public int shapeCount;

    public bool isPlaying;
    public bool isNext;

    private Coroutine stageTimerCoroutine;

    public magicSoundManager msm;

    public GameObject CircleMagic;
    public GameObject SquareMagic;
    public GameObject StarMagic;

    private void Start() {
        drawingGame = GameObject.Find("DrawingGame").GetComponent<DrawingGame>();

        drawingGame.resultText.gameObject.SetActive(false);
        referenceShape.gameObject.SetActive(false);
        playerDrawing.gameObject.SetActive(false);

        isPlaying = false;

        shapeCount = 0;

        msm = GameObject.Find("SoundManager").GetComponent<magicSoundManager>();

        CircleMagic = GameObject.Find("CircleMagic");
        SquareMagic = GameObject.Find("SquareMagic");
        StarMagic = GameObject.Find("StarMagic");

        CircleMagic.SetActive(false);
        SquareMagic.SetActive(false);
        StarMagic.SetActive(false);
    }

    public void gameStart() {
        startTimer = GameObject.Find("StartTimer").GetComponent<TMP_Text>();
        gameTimer = GameObject.Find("GameTimer").GetComponent<TMP_Text>();
        
        nextStage(true);
    }

    public void nextStage(bool isFirst) {
        // 중복 실행 방지
        if (stageTimerCoroutine != null) {
            StopCoroutine(stageTimerCoroutine);
            stageTimerCoroutine = null;
        }        

        isPlaying = false;
        if (isFirst) {
            StartCoroutine(SelectShape((Shape)shapeCount));
            StartCoroutine(timer());

            shapeCount++;
        }
        else {
            if (shapeCount < 3) {
                drawingGame.accuracy = 0f;
                StartCoroutine(delayStage());
            }
            else {
                playerDrawing.gameObject.SetActive(false);
                startTimer.gameObject.SetActive(true);
                gameTimer.gameObject.SetActive(false);
                drawingGame.resultText.gameObject.SetActive(false);
                drawingGame.playerPoints.Clear();
                drawingGame.playerDrawing.positionCount = 0;
                startTimer.text = "Done";
                gameTimer.text = drawingGame.sumScore.ToString();
                StartCoroutine(endDelay());
                
            }
        }
    }

    IEnumerator endDelay() {
        yield return new WaitForSeconds(3f);

        msm.endSound.Play();
        GameObject.Find("GameManage").GetComponent<GameManager>().EndGame(0, 0);
    }

    IEnumerator delayStage() {
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
        gameTimer.gameObject.SetActive(false);
        drawingGame.resultText.gameObject.SetActive(false);
        drawingGame.playerPoints.Clear();
        drawingGame.playerDrawing.positionCount = 0;

        isPlaying = false;

        CircleMagic.SetActive(false);
        SquareMagic.SetActive(false);
        StarMagic.SetActive(false);

        yield return new WaitForSeconds(3f);

        // 중복 실행 방지
        if (stageTimerCoroutine != null) {
            StopCoroutine(stageTimerCoroutine);
        }

        stageTimerCoroutine = StartCoroutine(stageTimer());
        referenceShape.gameObject.SetActive(true);
        playerDrawing.gameObject.SetActive(true);
        startTimer.gameObject.SetActive(false);
        gameTimer.gameObject.SetActive(true);

        isPlaying = true;

        if(shapeCount == 1) {
            CircleMagic.SetActive(true);
        }
        else if(shapeCount == 2) {
            SquareMagic.SetActive(true);
        }
        else if(shapeCount == 3){
            StarMagic.SetActive(true);
        }

        drawingGame.SetReferenceShape(shape);
    }

    IEnumerator timer() {
        int countdown = 3;

        while (countdown >= 0) {
            startTimer.text = countdown.ToString(); // 텍스트 업데이트
            msm.timerCountSound.Play();
            yield return new WaitForSeconds(1f);    // 1초 대기
            countdown--;
        }
    }

    public IEnumerator stageTimer() {
        drawingGame.countdownTimer = 30;

        while (drawingGame.countdownTimer >= 0) {
            gameTimer.text = drawingGame.countdownTimer.ToString(); // 텍스트 업데이트

            yield return new WaitForSeconds(1f);    // 1초 대기
            drawingGame.countdownTimer--;
        }
    }
}