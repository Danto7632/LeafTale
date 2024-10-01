using UnityEngine;
using UnityEngine.UI;

public class AssignRandomImageToChildren : MonoBehaviour {
    public Sprite[] images; // 9개의 이미지를 배열로 저장
    public Sprite[] pointImages;

    public Image one;
    public Image two;

    public int randomIndex;

    private void Start() {
        AssignSameRandomImageToChildren();
    }

    private void AssignSameRandomImageToChildren() {
        // 9개의 이미지 중 랜덤으로 하나 선택
        randomIndex = Random.Range(0, images.Length);

        one.sprite = images[randomIndex];
        two.sprite = pointImages[randomIndex];
    }
}