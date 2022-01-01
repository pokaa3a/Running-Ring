using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BallManager : MonoBehaviour
{
    public class Config
    {
        public static bool clockwise = true;
        public const float angularSpeed = 1.5f;
    }

    public const int maxNumBalls = 60;
    public int currentNumBalls = 0;
    public Ball[] balls = new Ball[maxNumBalls];

    [SerializeField]
    private GameObject ballPrefab;

    void Awake()
    {
        InitializeBalls();
    }

    public void InitializeBalls()
    {
        float anglePerBall = 360f / maxNumBalls;
        for (int i = 0; i < maxNumBalls; ++i)
        {
            GameObject ballObject = Instantiate(ballPrefab);
            ballObject.name = $"Ball{i}";
            balls[i] = ballObject.GetComponent<Ball>() as Ball;
            balls[i].gameObject.transform.SetParent(gameObject.transform);
            balls[i].Initialize(this, i, anglePerBall * i);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < maxNumBalls; ++i)
        {
            balls[i].triggered = false;
        }
        balls[0].triggered = true;
        currentNumBalls = 1;
    }

    public void AddBalls(int idx, int numBallsToAdd)
    {
        int added = 0;
        int curIdx = idx;

        // DEBUG
        int count = 0;

        while (added < numBallsToAdd && currentNumBalls < maxNumBalls)
        {
            if (Config.clockwise)
            {
                curIdx = curIdx - 1 < 0 ? maxNumBalls - 1 : curIdx - 1;
            }
            else    // ccw
            {
                curIdx = curIdx + 1 >= maxNumBalls ? 0 : curIdx + 1;
            }

            if (!balls[curIdx].triggered)
            {
                // find available ball to enable
                balls[curIdx].triggered = true;
                currentNumBalls++;
                added++;
            }

            count++;
            if (count > maxNumBalls)
            {
                Assert.IsTrue(false, "stuck in while loop!");
            }
        }
    }

    public void BallCollides(Ball ball)
    {
        ball.BallExplode();
        currentNumBalls--;

        if (currentNumBalls == 0)
            GameAdmin.Instance.GameOver();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Config.clockwise ^= true;
        }
    }
}
