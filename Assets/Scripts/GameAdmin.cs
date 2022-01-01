using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class GameAdmin : MonoBehaviour
{
    [SerializeField]
    private GameObject gameReadyPage;
    [SerializeField]
    private GameObject gameCompletePage;
    [SerializeField]
    private GameObject gameOverPage;
    [SerializeField]
    private GameObject ballManagerObject;
    private BallManager ballManager;
    [SerializeField]
    private GameObject movingRingObject;
    private MovingRing movingRing;
    [SerializeField]
    private GameObject levelManagerObject;
    private LevelManager levelManager;

    private static GameAdmin _instance;
    public static GameAdmin Instance { get => _instance; }

    public bool ringIsMoving { get; private set; } = false;

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

        ballManager = ballManagerObject.GetComponent<BallManager>();
        levelManager = levelManagerObject.GetComponent<LevelManager>();
        movingRing = movingRingObject.GetComponent<MovingRing>();

        // Simulate slow loading time
        Thread.Sleep(1000); // ms
    }

    void Start()
    {
        GamePrepare();
    }

    // Called when the player failed in game. Will wait for the player to tap to continue.
    public void GameOver()
    {
        gameReadyPage.SetActive(false);
        gameCompletePage.SetActive(false);
        gameOverPage.SetActive(true);

        ringIsMoving = false;
    }

    // Called when the player complete the game. Will wait for the player to tap to continue.
    public void GameComplete()
    {
        gameReadyPage.SetActive(false);
        gameCompletePage.SetActive(true);
        gameOverPage.SetActive(false);

        ringIsMoving = false;
    }

    // Called to prepare a new game. Player can tap to start playing.
    public void GamePrepare()
    {
        gameReadyPage.SetActive(true);
        gameCompletePage.SetActive(false);
        gameOverPage.SetActive(false);

        levelManager.DestroyLevel();
        levelManager.MakeLevel();
        movingRing.Reset();
    }

    // Called to start playing. (Player taps when the game is ready)
    public void GameStart()
    {
        gameReadyPage.SetActive(false);
        gameCompletePage.SetActive(false);
        gameOverPage.SetActive(false);

        ringIsMoving = true;
    }
}
