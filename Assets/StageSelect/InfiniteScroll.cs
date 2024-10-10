using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour  // InfiniteScroll�̶�� MonoBehaviour Ŭ������ �����մϴ�.
{
    public ScrollRect scrollRect;  // ScrollRect UI ��Ҹ� �����ϴ� ���� �����Դϴ�.
    public RectTransform viewPortTransform;  // ����Ʈ�� RectTransform�� �����ϴ� ���� �����Դϴ�.
    public RectTransform contentPanelTransform;  // ������ �г��� RectTransform�� �����ϴ� ���� �����Դϴ�.
    public HorizontalLayoutGroup hlg;  // HorizontalLayoutGroup ������Ʈ�� �����ϴ� ���� �����Դϴ�.

    public RectTransform[] itemList;  // ��ũ�� �׸�鿡 ���� RectTransform �迭�� �����մϴ�.

    Vector2 oldVelocity;  // ���� �ӵ��� �����ϴ� �����Դϴ�.
    bool isUpdated;  // ������Ʈ ���¸� ��Ÿ���� �Ҹ��� �����Դϴ�.

    void Start()  // MonoBehaviour�� Start �޼����Դϴ�.
    {
        Canvas.ForceUpdateCanvases();

        isUpdated = false;  // ������Ʈ ���¸� �ʱ�ȭ�մϴ�.
        oldVelocity = Vector2.zero;  // ���� �ӵ��� 0���� �ʱ�ȭ�մϴ�.

        int itemToAdd = Mathf.CeilToInt(viewPortTransform.rect.width / (itemList[0].rect.width + hlg.spacing));  // ����Ʈ�� �ʺ� �´� �׸� ���� ����մϴ�.
        Debug.Log("itemToAdd = " + itemToAdd);
        
        for (int i = 0; i < itemToAdd; i++)  // ����Ʈ �ʺ� �´� ����ŭ �׸��� �߰��մϴ�.
        {
            RectTransform rectTransform = Instantiate(itemList[i % itemList.Length], contentPanelTransform);  // �׸��� �ν��Ͻ�ȭ�Ͽ� ������ �г��� �ڽ����� �߰��մϴ�.
            rectTransform.SetAsLastSibling();  // �׸��� ������ �г��� ������ �ڽ����� �����մϴ�.
        }

        for (int i = 0; i < itemToAdd; i++)  // ����Ʈ �տ� �������� �߰��ϴ� �����Դϴ�.
        {
            int num = itemList.Length - i - 1;  // �������� �׸� �ε����� ����մϴ�.
            while (num < 0)  // �ε����� ������ ��� �迭�� ���̿� ���� �����մϴ�.
            {
                num += itemList.Length;  // ���� �ε����� �迭�� ũ�⿡ �°� �����մϴ�.
            }
            RectTransform rectTransform = Instantiate(itemList[num], contentPanelTransform);  // �ν��Ͻ�ȭ�� �׸��� ������ �гο� �߰��մϴ�.
            rectTransform.SetAsFirstSibling();  // �׸��� ������ �г��� ù ��° �ڽ����� �����մϴ�.
        }
        
        contentPanelTransform.localPosition = new Vector3((0 - (itemList[0].rect.width + hlg.spacing) * itemToAdd),  // ������ �г��� ��ġ�� �����մϴ�.
            contentPanelTransform.localPosition.y,
            contentPanelTransform.localPosition.z);

        Debug.Log(contentPanelTransform.localPosition);
    }


    void Update()  // MonoBehaviour�� Update �޼����Դϴ�.
    {
        if (isUpdated)  // ���� ������Ʈ�� �̷�����ٸ�,
        {
            isUpdated = false;  // ������Ʈ ���¸� �ʱ�ȭ�մϴ�.
            scrollRect.velocity = oldVelocity;  // ���� �ӵ��� ��ũ�� �ӵ��� �����մϴ�.
        }
        if (contentPanelTransform.localPosition.x > 0)  // ������ �г��� ȭ�� �������� ������ ���,
        {
            Canvas.ForceUpdateCanvases();  // ĵ������ ������ ������Ʈ�մϴ�.
            oldVelocity = scrollRect.velocity;  // ���� �ӵ��� �����մϴ�.

            contentPanelTransform.localPosition -= new Vector3(itemList.Length * (itemList[0].rect.width + hlg.spacing), 0, 0);  // ������ �г��� �������� �̵���ŵ�ϴ�.
            isUpdated = true;  // ������Ʈ�� �̷�������� ǥ���մϴ�.

            // ��ġ�� ��Ȯ�� �Ѿ�� ��, ������ �������� �ʵ��� �ٽ� �����մϴ�.
            if (contentPanelTransform.localPosition.x > 0)
            {
                contentPanelTransform.localPosition = new Vector3(0, contentPanelTransform.localPosition.y, contentPanelTransform.localPosition.z);
            }
        }

        if (contentPanelTransform.localPosition.x < 0 - (itemList.Length * (itemList[0].rect.width + hlg.spacing)))  // ������ �г��� ȭ�� ���������� ������ ���,
        {
            Canvas.ForceUpdateCanvases();  // ĵ������ ������ ������Ʈ�մϴ�.
            oldVelocity = scrollRect.velocity;  // ���� �ӵ��� �����մϴ�.

            contentPanelTransform.localPosition += new Vector3(itemList.Length * (itemList[0].rect.width + hlg.spacing), 0, 0);  // ������ �г��� ���������� �̵���ŵ�ϴ�.
            isUpdated = true;  // ������Ʈ�� �̷�������� ǥ���մϴ�.

            // ��ġ�� ��Ȯ�� �Ѿ�� ��, ������ �������� �ʵ��� �ٽ� �����մϴ�.
            if (contentPanelTransform.localPosition.x < 0 - (itemList.Length * (itemList[0].rect.width + hlg.spacing)))
            {
                contentPanelTransform.localPosition = new Vector3(0 - (itemList.Length * (itemList[0].rect.width + hlg.spacing)), contentPanelTransform.localPosition.y, contentPanelTransform.localPosition.z);
            }
        }
        // Debugging: ���� ��ġ ���
        Debug.Log("���� �г� ��ġ: " + contentPanelTransform.localPosition);
    }
}
