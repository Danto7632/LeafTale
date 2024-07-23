using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class RankManager : MonoBehaviour {
    public Button rankButton;
    public Button dummyButton;
    public TMP_Text rankText;

    private List<PlayerScore> playerScores = new List<PlayerScore>();
    private bool isRankDisplayed;

    void Start() {
        playerScores.Add(new PlayerScore("Player1", Random.Range(0, 100)));
        playerScores.Add(new PlayerScore("Player2", Random.Range(0, 100)));
        playerScores.Add(new PlayerScore("Player3", Random.Range(0, 100)));
        playerScores.Add(new PlayerScore("Player4", Random.Range(0, 100)));

        rankButton.onClick.AddListener(ToggleRankings);
        dummyButton.onClick.AddListener(AddDummyScore);

        isRankDisplayed = false;
    }

    void ToggleRankings() {
        if (isRankDisplayed) {
            rankText.text = "";
            isRankDisplayed = false;
        }
        else {
            DisplayRankings();
            isRankDisplayed = true;
        }
    }

    void DisplayRankings() {
        var sortedScores = playerScores.OrderByDescending(p => p.score).ToList();
        rankText.text = "";

        for (int i = 0; i < sortedScores.Count; i++) {
            rankText.text += $"{i + 1}. {sortedScores[i].name}: {sortedScores[i].score}\n";
        }
    }

    void AddDummyScore() {
        string playerName = "Player" + Random.Range(5, 100);
        int playerScore = Random.Range(0, 100);

        playerScores.Add(new PlayerScore(playerName, playerScore));
    }

    [System.Serializable]
    public class PlayerScore {
        public string name;
        public int score;

        public PlayerScore(string name, int score) {
            this.name = name;
            this.score = score;
        }
    }
}
