using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    public GameObject RightNode;
    public GameObject LeftNode;

    public static int leftNodeCount = 0;
    public static int rightNodeCount = 0;

    // Right와 Left의 임의 패턴 설정
    public List<int> RightPattern;
    public List<int> LeftPattern;

    // Right와 Left 오브젝트 딕셔너리
    [SerializeField]
    private GameObject PaperRightPrefab;
    [SerializeField]
    private GameObject ScissorRightPrefab;
    [SerializeField]
    private GameObject RockRightPrefab;

    [SerializeField]
    private GameObject PaperLeftPrefab;
    [SerializeField]
    private GameObject ScissorLeftPrefab;
    [SerializeField]
    private GameObject RockLeftPrefab;

    private Dictionary<int, GameObject> rightPrefabNames;
    private Dictionary<int, GameObject> leftPrefabNames;

    // 초기 스폰 위치
    private Vector2 rightSpawnPosition = new Vector2(25, 0);
    private Vector2 leftSpawnPosition = new Vector2(-25, 0);

    void Awake()
    {
        // 패턴 초기화 (테스트용)
        RightPattern = new List<int> { 1, 0, 3, 2, 3, 0, 3, 3, 2, 1, 0, 1 };
        LeftPattern = new List<int> { 3, 1, 2, 0, 3, 2, 2, 1, 3, 2, 1, 0 };

        foreach(int item in RightPattern)
        {
            if (item != 0)
                rightNodeCount++;
        }

        foreach (int item in LeftPattern)
        {
            if (item != 0)
                leftNodeCount++;
        }
    }
    void Start()
    {
        // 딕셔너리 초기화
        rightPrefabNames = new Dictionary<int, GameObject>()
        {
            {1, PaperRightPrefab},
            {2, ScissorRightPrefab},
            {3, RockRightPrefab}
        };

        leftPrefabNames = new Dictionary<int, GameObject>()
        {
            {1, PaperLeftPrefab},
            {2, ScissorLeftPrefab},
            {3, RockLeftPrefab}
        };

        // 오브젝트 한 번에 생성
        SpawnObjects(RightPattern, RightNode.transform, ref rightSpawnPosition, 5, rightPrefabNames);
        SpawnObjects(LeftPattern, LeftNode.transform, ref leftSpawnPosition, -5, leftPrefabNames);
    }

    void SpawnObjects(List<int> pattern, Transform parentTransform, ref Vector2 spawnPosition, float positionIncrement, Dictionary<int, GameObject> prefabNames)
    {
        foreach (int patternValue in pattern)
        {
            if (patternValue != 0)
            {
                if (prefabNames.ContainsKey(patternValue))
                {
                    GameObject prefab = prefabNames[patternValue];
                    if (prefab != null)
                    {
                        Instantiate(prefab, spawnPosition, Quaternion.identity, parentTransform);
                    }
                    else
                    {
                        Debug.LogWarning($"Prefab not assigned in inspector for value: {patternValue}");
                    }
                }
            }
            // 좌표 업데이트 (오브젝트를 생성하지 않아도)
            spawnPosition.x += positionIncrement;
        }
    }
}
