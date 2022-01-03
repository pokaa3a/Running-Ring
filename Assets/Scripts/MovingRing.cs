using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRing : MonoBehaviour
{
    [SerializeField]
    private GameObject ballManagerObj;
    private BallManager ballManager;

    public float finishHeight = 10f;
    public class Config
    {
        public const float raisingSpeed = 1.5f;
    }

    void Awake()
    {
        ballManager = ballManagerObj.GetComponent<BallManager>();
    }

    public void Reset()
    {
        gameObject.transform.position = Vector2.zero;

        ballManager.Reset();
    }

    void FixedUpdate()
    {
        if (gameObject.transform.position.y >= finishHeight - Ring.Config.originY)
        {
            GameAdmin.Instance.GameComplete();
            return;
        }

        if (GameAdmin.Instance.ringIsMoving)
        {
            gameObject.transform.position +=
                Vector3.up * Config.raisingSpeed * Time.fixedDeltaTime;
        }
    }
}
