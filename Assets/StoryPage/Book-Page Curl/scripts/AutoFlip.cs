using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Book))]
public class AutoFlip : MonoBehaviour {
    public FlipMode Mode;
    public float PageFlipTime = 1;
    public float TimeBetweenPages = 1;
    public float DelayBeforeStarting = 0;
    public bool AutoStartFlip;
    public Book ControledBook;
    public int AnimationFramesCount;
    bool isFlipping = false;

    public flipSoundManager fsm;

    void Start () {
        AutoStartFlip = false;
        AnimationFramesCount = 400;
        if (!ControledBook)
            ControledBook = GetComponent<Book>();
        if (AutoStartFlip)
            StartFlipping();
        ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));


        fsm = GameObject.Find("SoundManager").GetComponent<flipSoundManager>();
    }

    void PageFlipped() {
        isFlipping = false;
    }

    public void StartFlipping() {
        StartCoroutine(FlipToEnd());
    }

    public void FlipRightPage() {
        if (isFlipping) return;
        if (ControledBook.currentPage == ControledBook.TotalPageCount - 1) return;
        
        isFlipping = true;
        fsm.flipSound.Play();
        StartCoroutine(FlipPage(true));
    }

    public void FlipLeftPage() {
        if (isFlipping) return;
        if (ControledBook.currentPage <= 0) return;

        isFlipping = true;
        fsm.flipSound.Play();
        StartCoroutine(FlipPage(false));
    }

    IEnumerator FlipPage(bool isRight) {
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;

        float x = isRight ? xc + xl : xc - xl;

        for (int i = 0; i < AnimationFramesCount; i++) {
            float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

            if (isRight) {
                ControledBook.DragRightPageToPoint(new Vector3(x, y, 0));
                ControledBook.UpdateBookRTLToPoint(new Vector3(x, y, 0));
            } else {
                ControledBook.DragLeftPageToPoint(new Vector3(x, y, 0));
                ControledBook.UpdateBookLTRToPoint(new Vector3(x, y, 0));
            }

            x += isRight ? -dx : dx; // 페이지 이동
            yield return new WaitForSeconds(frameTime);
        }

        if (isRight) {
            ControledBook.ReleasePage();
        } else {
            ControledBook.ReleasePage();
        }
    }

    IEnumerator FlipToEnd() {
        yield return new WaitForSeconds(DelayBeforeStarting);
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;

        switch (Mode) {
            case FlipMode.RightToLeft:
                while (ControledBook.currentPage < ControledBook.TotalPageCount) {
                    StartCoroutine(FlipPage(true));
                    yield return new WaitForSeconds(TimeBetweenPages);
                }
                break;
            case FlipMode.LeftToRight:
                while (ControledBook.currentPage > 0) {
                    StartCoroutine(FlipPage(false));
                    yield return new WaitForSeconds(TimeBetweenPages);
                }
                break;
        }

        PageFlipped();
    }
}