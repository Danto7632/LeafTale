using UnityEngine;
using UnityEngine.UI;
using TMPro;  // TextMeshPro를 사용하기 위해 추가
using System.Collections.Generic;
using System.Linq;

public class RankManager : MonoBehaviour
{
    public Button rankButton;
    public Button dummyButton;
    public TMP_Text rankText;  // TextMeshPro 텍스트 컴포넌트를 사용

    private List<PlayerScore> playerScores = new List<PlayerScore>();
    private bool isRankDisplayed = false;  // 상태 변수 추가

    void Start()
    {
        // 더미 데이터 추가
        playerScores.Add(new PlayerScore("Player1", Random.Range(0, 100)));
        playerScores.Add(new PlayerScore("Player2", Random.Range(0, 100)));
        playerScores.Add(new PlayerScore("Player3", Random.Range(0, 100)));
        playerScores.Add(new PlayerScore("Player4", Random.Range(0, 100)));

        // 버튼 클릭 이벤트 추가
        rankButton.onClick.AddListener(ToggleRankings);
        dummyButton.onClick.AddListener(AddDummyScore);
    }

    void ToggleRankings()
    {
        if (isRankDisplayed)
        {
            rankText.text = "";  // 점수판 비우기
            isRankDisplayed = false;
        }
        else
        {
            DisplayRankings();  // 점수판 표시
            isRankDisplayed = true;
        }
    }

    void DisplayRankings()
    {
        // 점수를 내림차순으로 정렬
        var sortedScores = playerScores.OrderByDescending(p => p.score).ToList();

        // 순위를 Text 요소에 표시
        rankText.text = "";
        for (int i = 0; i < sortedScores.Count; i++)
        {
            rankText.text += $"{i + 1}. {sortedScores[i].name}: {sortedScores[i].score}\n";
        }
    }

    void AddDummyScore()
    {
        // 랜덤 플레이어 이름과 점수 추가
        string playerName = "Player" + Random.Range(5, 100);
        int playerScore = Random.Range(0, 100);
        playerScores.Add(new PlayerScore(playerName, playerScore));
    }

    // PlayerScore 클래스 정의
    [System.Serializable]
    public class PlayerScore
    {
        public string name;
        public int score;

        public PlayerScore(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
}
