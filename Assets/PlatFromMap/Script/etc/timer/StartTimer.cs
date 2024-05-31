using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro Ŭ������ ����Ϸ��� �� ���ӽ����̽��� �߰��ϼ���

public class Timer : MonoBehaviour
{
    public TMP_Text[] timeText;
    public TMP_Text gameOverText;
    float time = 126f; // ���� �ð� 125��
    int min, sec;

    void Start()
    {
        StartCoroutine(delayTimer());
        // timeText �迭�� null�� �ƴϰ� ũ�Ⱑ 2 �̻����� Ȯ��
        if (timeText != null && timeText.Length >= 2 && gameOverText != null)
        {
            // ���� �ð� 02:00���� �ʱ�ȭ
            timeText[0].text = "02";
            timeText[1].text = "06";
            timeText[0].enabled = false;
            timeText[1].enabled = false;
            gameOverText.enabled = false;

            StartCoroutine(delayTimer());
        }
        else
        {
            Debug.LogError("timeText �迭�� null�̰ų� ũ�Ⱑ 2���� �۽��ϴ�.");
        }
    }

    void Update()
    {
        time -= Time.deltaTime;

        min = (int)time / 60;
        sec = (int)time % 60;

        if (time <= 0)
        {
            time = 0;
            gameOverText.text = "���� ����"; // ���� ���� �ؽ�Ʈ�� ǥ���մϴ�
            // �߰��� ������Ʈ�� ��Ȱ��ȭ�ϰų� ���� ���� ������ ó���� �� �ֽ��ϴ�
        }

        // Ÿ�̸� �ؽ�Ʈ ������Ʈ
        if (timeText != null && timeText.Length >= 2)
        {
            timeText[0].text = min.ToString("00");
            timeText[1].text = sec.ToString("00");
        }
    }

    IEnumerator delayTimer()
    {
        yield return new WaitForSeconds(5f);

        // timeText �迭�� gameOverText�� null�� �ƴ��� Ȯ��
        if (timeText != null && timeText.Length >= 2 && gameOverText != null)
        {
            timeText[0].enabled = true;
            timeText[1].enabled = true;
            gameOverText.enabled = true;
        }
    }
}