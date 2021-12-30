using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void AddOneBall(int id)
    {
        if (currentNumBalls == maxNumBalls) return;

        int cur;
        if (Config.clockwise)
        {
            cur = id - 1;
            cur = cur < 0 ? maxNumBalls - 1 : cur;

            while (cur != id && balls[cur].triggered == true)
            {
                cur--;
                cur = cur < 0 ? maxNumBalls - 1 : cur;
            }
        }
        else
        {
            cur = id + 1;
            cur = cur >= maxNumBalls ? 0 : cur;

            while (cur != id && balls[cur].triggered == true)
            {
                cur++;
                cur = cur >= maxNumBalls ? maxNumBalls - 1 : cur;
            }
        }
        balls[cur].triggered = true;
        currentNumBalls++;
    }

    public void BallCollideObstacle(Ball ball)
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
