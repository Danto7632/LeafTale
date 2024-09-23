using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingValue : MonoBehaviour
{
    public static EndingValue Instance;
    // Start is called before the first frame update
    public static int[] endingValue = new int[6] { 0, 0, 0, 0, 0, 0 };
    // 0: �̾ƶ� ����ä��
    // 1: ���ƶ� ���ڷ�
    // 2: ����� ���ɽ�Ʈ��
    // 3: �׷��� ������
    // 4: �޷��� �����
    // 5: ������ ����

    void Awake()
    {
        if(Instance != this && Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void EndingSet(int i)
    {
        endingValue[i] = 1;
        Debug.Log($"���� ������ ���� ���� :{endingValue[0]} {endingValue[1]} {endingValue[2]} {endingValue[3]} {endingValue[4]}");
        if (endingValue[0] == 1 && endingValue[1] == 1 && endingValue[2] == 1 && endingValue[3] == 1 && endingValue[4] == 1)
        {
            endingValue[5] = 1;
        }
        Debug.Log($"������ ���� : {endingValue[5]}");
    }
}
