using UnityEngine;
using UnityEngine.UI;

public class AssignRandomImageToChildren : MonoBehaviour {
    public Sprite[] images; // 9개의 이미지를 배열로 저장

    private void Start() {
        if (images.Length != 9) {
            Debug.LogError("9개의 이미지를 할당해주세요!");
            return;
        }

        AssignSameRandomImageToChildren();
    }

    private void AssignSameRandomImageToChildren() {
        // 9개의 이미지 중 랜덤으로 하나 선택
        int randomIndex = Random.Range(0, images.Length);
        Sprite selectedImage = images[randomIndex];

        // 자식 오브젝트들을 순회
        foreach (Transform child in transform) {
            Image childImage = child.GetComponent<Image>();

            if (childImage != null) {
                // 선택된 이미지를 자식에게 할당
                childImage.sprite = selectedImage;
            }
        }
    }
}