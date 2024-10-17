using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour  // InfiniteScroll이라는 MonoBehaviour 클래스를 정의합니다.
{
    public ScrollRect scrollRect;  // ScrollRect UI 요소를 참조하는 공개 변수입니다.
    public RectTransform viewPortTransform;  // 뷰포트의 RectTransform을 참조하는 공개 변수입니다.
    public RectTransform contentPanelTransform;  // 콘텐츠 패널의 RectTransform을 참조하는 공개 변수입니다.
    public HorizontalLayoutGroup hlg;  // HorizontalLayoutGroup 컴포넌트를 참조하는 공개 변수입니다.

    public RectTransform[] itemList;  // 스크롤 항목들에 대한 RectTransform 배열을 정의합니다.

    Vector2 oldVelocity;  // 이전 속도를 저장하는 벡터입니다.
    bool isUpdated;  // 업데이트 상태를 나타내는 불리언 변수입니다.

    void Start()  // MonoBehaviour의 Start 메서드입니다.
    {
        Canvas.ForceUpdateCanvases();

        isUpdated = false;  // 업데이트 상태를 초기화합니다.
        oldVelocity = Vector2.zero;  // 이전 속도를 0으로 초기화합니다.

        int itemToAdd = Mathf.CeilToInt(viewPortTransform.rect.width / (itemList[0].rect.width + hlg.spacing));  // 뷰포트의 너비에 맞는 항목 수를 계산합니다.
        Debug.Log("itemToAdd = " + itemToAdd);
        
        for (int i = 0; i < itemToAdd; i++)  // 뷰포트 너비에 맞는 수만큼 항목을 추가합니다.
        {
            RectTransform rectTransform = Instantiate(itemList[i % itemList.Length], contentPanelTransform);  // 항목을 인스턴스화하여 콘텐츠 패널의 자식으로 추가합니다.
            rectTransform.SetAsLastSibling();  // 항목을 콘텐츠 패널의 마지막 자식으로 설정합니다.
        }

        for (int i = 0; i < itemToAdd; i++)  // 뷰포트 앞에 아이템을 추가하는 루프입니다.
        {
            int num = itemList.Length - i - 1;  // 역순으로 항목 인덱스를 계산합니다.
            while (num < 0)  // 인덱스가 음수일 경우 배열의 길이에 맞춰 조정합니다.
            {
                num += itemList.Length;  // 음수 인덱스를 배열의 크기에 맞게 조정합니다.
            }
            RectTransform rectTransform = Instantiate(itemList[num], contentPanelTransform);  // 인스턴스화된 항목을 콘텐츠 패널에 추가합니다.
            rectTransform.SetAsFirstSibling();  // 항목을 콘텐츠 패널의 첫 번째 자식으로 설정합니다.
        }
        
        contentPanelTransform.localPosition = new Vector3((0 - (itemList[0].rect.width + hlg.spacing) * itemToAdd),  // 콘텐츠 패널의 위치를 설정합니다.
            contentPanelTransform.localPosition.y,
            contentPanelTransform.localPosition.z);

        Debug.Log(contentPanelTransform.localPosition);
    }


    void Update()  // MonoBehaviour의 Update 메서드입니다.
    {
        if (isUpdated)  // 만약 업데이트가 이루어졌다면,
        {
            isUpdated = false;  // 업데이트 상태를 초기화합니다.
            scrollRect.velocity = oldVelocity;  // 이전 속도로 스크롤 속도를 복원합니다.
        }
        if (contentPanelTransform.localPosition.x > 0)  // 콘텐츠 패널이 화면 왼쪽으로 넘쳤을 경우,
        {
            Canvas.ForceUpdateCanvases();  // 캔버스를 강제로 업데이트합니다.
            oldVelocity = scrollRect.velocity;  // 현재 속도를 저장합니다.

            contentPanelTransform.localPosition -= new Vector3(itemList.Length * (itemList[0].rect.width + hlg.spacing), 0, 0);  // 콘텐츠 패널을 왼쪽으로 이동시킵니다.
            isUpdated = true;  // 업데이트가 이루어졌음을 표시합니다.

            // 위치가 정확히 넘어갔을 때, 조건을 만족하지 않도록 다시 설정합니다.
            if (contentPanelTransform.localPosition.x > 0)
            {
                contentPanelTransform.localPosition = new Vector3(0, contentPanelTransform.localPosition.y, contentPanelTransform.localPosition.z);
            }
        }

        if (contentPanelTransform.localPosition.x < 0 - (itemList.Length * (itemList[0].rect.width + hlg.spacing)))  // 콘텐츠 패널이 화면 오른쪽으로 넘쳤을 경우,
        {
            Canvas.ForceUpdateCanvases();  // 캔버스를 강제로 업데이트합니다.
            oldVelocity = scrollRect.velocity;  // 현재 속도를 저장합니다.

            contentPanelTransform.localPosition += new Vector3(itemList.Length * (itemList[0].rect.width + hlg.spacing), 0, 0);  // 콘텐츠 패널을 오른쪽으로 이동시킵니다.
            isUpdated = true;  // 업데이트가 이루어졌음을 표시합니다.

            // 위치가 정확히 넘어갔을 때, 조건을 만족하지 않도록 다시 설정합니다.
            if (contentPanelTransform.localPosition.x < 0 - (itemList.Length * (itemList[0].rect.width + hlg.spacing)))
            {
                contentPanelTransform.localPosition = new Vector3(0 - (itemList.Length * (itemList[0].rect.width + hlg.spacing)), contentPanelTransform.localPosition.y, contentPanelTransform.localPosition.z);
            }
        }
        // Debugging: 현재 위치 출력
        Debug.Log("현재 패널 위치: " + contentPanelTransform.localPosition);
    }
}
