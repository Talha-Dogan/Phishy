using UnityEngine;
using TMPro;
using Dan.Main;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text[] _namesTextObjects;
    [SerializeField] private TMP_Text[] _scoresTextObjects;
    [SerializeField] private TMP_InputField _usernameInputField;
    [SerializeField] private TMP_Text _currentScoreText;

    private string PlayerName
    {
        get => PlayerPrefs.GetString("PlayerName", "");
        set => PlayerPrefs.SetString("PlayerName", value);
    }

    public class TestEntry
    {
        public string Username;
        public int Score;

        public TestEntry(string username, int score)
        {
            Username = username;
            Score = score;
        }
    }

    private string[] funnyNames = new string[]
    {
        "Ahmet Yılmaz", "Mehmet Demir", "Ali Kaya", "Can Yıldız", "Emre Şahin",
        "Mustafa Koç", "Burak Aydın", "Kerem Arslan", "Mert Özkan", "Serkan Kaplan"
    };

    private void Start()
    {
        if (!string.IsNullOrEmpty(PlayerName))
            _usernameInputField.text = PlayerName;

        RefreshCurrentScore();
        LoadEntries();
    }

    private void Update()
    {
        // İstersen bunu Update yerine sadece Start'ta çalıştırabilirsin, daha performanslı olur.
        RefreshCurrentScore();
    }

    private void RefreshCurrentScore()
    {
        // DÜZENLENDİ: Score.score yerine PlayerPrefs'ten okuyoruz
        int finalScore = PlayerPrefs.GetInt("CurrentScore", 0);

        if (_currentScoreText != null)
            _currentScoreText.text = $"Your Score: {finalScore}";
    }

    public void SaveNameAndUploadScore()
    {
        string name = _usernameInputField.text;
        if (string.IsNullOrEmpty(name)) return;

        PlayerName = name;
        StartCoroutine(UploadNextFrame());
    }

    private System.Collections.IEnumerator UploadNextFrame()
    {
        yield return null;

        // DÜZENLENDİ: Score.score yerine PlayerPrefs'ten okuyoruz
        int playerScore = PlayerPrefs.GetInt("CurrentScore", 0);

        Leaderboards.PhishyLeaderBoard.UploadNewEntry(PlayerName, playerScore, success =>
        {
            if (success)
                LoadEntries();
        });
    }

    public void LoadEntries()
    {
        Leaderboards.PhishyLeaderBoard.GetEntries(entries =>
        {
            int total = Mathf.Max(entries.Length, Mathf.Min(_namesTextObjects.Length, _scoresTextObjects.Length));

            var fullEntries = new List<TestEntry>();

            foreach (var e in entries)
            {
                fullEntries.Add(new TestEntry(e.Username, e.Score));
            }

            int currentCount = fullEntries.Count;
            if (currentCount < total)
            {
                for (int i = 0; i < (total - currentCount); i++)
                {
                    string name = funnyNames[i % funnyNames.Length];
                    fullEntries.Add(new TestEntry(name, Random.Range(1, 10)));
                }
            }

            fullEntries.Sort((a, b) => b.Score.CompareTo(a.Score));

            int safeLength = Mathf.Min(_namesTextObjects.Length, _scoresTextObjects.Length);

            for (int i = 0; i < safeLength; i++)
            {
                if (i < fullEntries.Count)
                {
                    _namesTextObjects[i].text = fullEntries[i].Username;
                    _scoresTextObjects[i].text = fullEntries[i].Score.ToString();
                }
                else
                {
                    _namesTextObjects[i].text = "";
                    _scoresTextObjects[i].text = "";
                }
            }
        });
    }
}