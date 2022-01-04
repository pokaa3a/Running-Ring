using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRing : MonoBehaviour
{
    [SerializeField]
    private BallManager ballManager;

    public float finishHeight = 10f;
    public class Config
    {
        public const float raisingSpeed = 1.5f;
    }

    private bool reachedGoal = false;

    void FixedUpdate()
    {
        if (!reachedGoal && gameObject.transform.position.y >= finishHeight - Ring.Config.originY)
        {
            GameAdmin.Instance.GameComplete();
            reachedGoal = true;
            return;
        }

        if (GameAdmin.Instance.ringIsMoving)
        {
            gameObject.transform.position +=
                Vector3.up * Config.raisingSpeed * Time.fixedDeltaTime;
        }
    }

    public void Reset()
    {
        gameObject.transform.position = Vector2.zero;
        reachedGoal = false;

        ballManager.Reset();
    }

    public float GetProgress()
    {
        return gameObject.transform.position.y / (finishHeight - Ring.Config.originY);
    }
}
