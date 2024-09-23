using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingValue : MonoBehaviour
{
    public static EndingValue Instance;
    // Start is called before the first frame update
    public static int[] endingValue = new int[6] { 0, 0, 0, 0, 0, 0 };
    // 0: 뽑아라 마법채소
    // 1: 날아라 빗자루
    // 2: 울려라 오케스트라
    // 3: 그려라 마법진
    // 4: 달려라 고양이
    // 5: 진엔딩 여부

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
        Debug.Log($"현재 수집한 게임 엔딩 :{endingValue[0]} {endingValue[1]} {endingValue[2]} {endingValue[3]} {endingValue[4]}");
        if (endingValue[0] == 1 && endingValue[1] == 1 && endingValue[2] == 1 && endingValue[3] == 1 && endingValue[4] == 1)
        {
            endingValue[5] = 1;
        }
        Debug.Log($"진엔딩 여부 : {endingValue[5]}");
    }
}
