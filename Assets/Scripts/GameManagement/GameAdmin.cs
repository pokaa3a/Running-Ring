using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class GameAdmin : MonoBehaviour
{
    [SerializeField]
    private GameReadyPage gameReadyPage;
    [SerializeField]
    private GameCompletePage gameCompletePage;
    [SerializeField]
    private GameOverPage gameOverPage;
    [SerializeField]
    private BallManager ballManager;
    [SerializeField]
    private MovingRing movingRing;
    [SerializeField]
    private LevelManager levelManager;

    private static GameAdmin _instance;
    public static GameAdmin Instance { get => _instance; }

    public bool ringIsMoving { get; private set; } = false;

    private const string recordFolder = "data";
    private const string recordFile = "running_ring";

    private int level = 1;
    private int score = 0;
    private int highScore = 0;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }

        // Simulate slow loading time
        Thread.Sleep(1000); // ms
    }

    void Start()
    {
        LoadRecordsFromDisk();
        GamePrepare();
    }

    void LoadRecordsFromDisk()
    {
        GameRecordsJsonObject records =
            DataUtils.LoadData<GameRecordsJsonObject>(recordFolder, recordFile);

        level = records.level;
        highScore = records.highScore;
    }

    // Called when the player failed in game. Will wait for the player to tap to continue.
    public void GameOver()
    {
        gameReadyPage.gameObject.SetActive(false);
        gameCompletePage.gameObject.SetActive(false);
        gameOverPage.gameObject.SetActive(true);

        float progress = movingRing.GetProgress();
        gameOverPage.SetProgressText(progress);

        ringIsMoving = false;
    }

    // Called when the player complete the game. Will wait for the player to tap to continue.
    public void GameComplete()
    {
        gameReadyPage.gameObject.SetActive(false);
        gameCompletePage.gameObject.SetActive(true);
        gameOverPage.gameObject.SetActive(false);

        ringIsMoving = false;

        gameCompletePage.SetLevelText(level);
        level++;
        highScore = Mathf.Max(highScore, score);

        GameRecordsJsonObject newRecords = new GameRecordsJsonObject
        {
            level = this.level,
            highScore = this.highScore
        };
        DataUtils.SaveData<GameRecordsJsonObject>(newRecords, recordFolder, recordFile);
    }

    // Called to prepare a new game. Player can tap to start playing.
    public void GamePrepare()
    {
        gameReadyPage.gameObject.SetActive(true);
        gameCompletePage.gameObject.SetActive(false);
        gameOverPage.gameObject.SetActive(false);

        levelManager.DestroyLevel();
        levelManager.MakeLevel();
        movingRing.Reset();

        score = 0;
        gameReadyPage.SetLevelText(level);
    }

    // Called to start playing. (Player taps when the game is ready)
    public void GameStart()
    {
        gameReadyPage.gameObject.SetActive(false);
        gameCompletePage.gameObject.SetActive(false);
        gameOverPage.gameObject.SetActive(false);

        ringIsMoving = true;
    }
}
