using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private BallManager ballManager;

    public int id = 0;
    private bool _triggered = false;
    public bool triggered
    {
        get => _triggered;
        set
        {
            _triggered = value;

            SpriteRenderer sprRenderer =
                gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sprRenderer.enabled = _triggered;

            CircleCollider2D circleColl =
                gameObject.GetComponent<CircleCollider2D>() as CircleCollider2D;
            circleColl.enabled = _triggered;
        }
    }

    public void Initialize(BallManager manager, int id, float degree)
    {
        gameObject.transform.localPosition = new Vector2(
            Ring.Config.cx + Mathf.Sin(degree * Mathf.Deg2Rad) * Ring.Config.radius,
            Ring.Config.cy + Mathf.Cos(degree * Mathf.Deg2Rad) * Ring.Config.radius
        );
        triggered = false;
        this.id = id;
        this.ballManager = manager;
    }

    void FixedUpdate()
    {
        // Circular moving
        Vector2 vec = gameObject.transform.localPosition -
            new Vector3(Ring.Config.cx, Ring.Config.cy);

        float radian = Vector2.Angle(Vector2.up, vec) * Mathf.Deg2Rad;
        if (gameObject.transform.localPosition.x < Ring.Config.cx)
            radian = 2f * Mathf.PI - radian;

        if (BallManager.Config.clockwise)
            radian += BallManager.Config.angularSpeed * Time.deltaTime;
        else
            radian -= BallManager.Config.angularSpeed * Time.deltaTime;

        gameObject.transform.localPosition = new Vector2(
            Ring.Config.cx + Mathf.Sin(radian) * Ring.Config.radius,
            Ring.Config.cy + Mathf.Cos(radian) * Ring.Config.radius
        );
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            ballManager.BallCollideObstacle(this);
        }
        else if (collision.gameObject.tag == "Food")
        {
            ballManager.AddOneBall(id);
            Destroy(collision.gameObject);
        }
    }

    public void BallExplode()
    {
        triggered = false;
    }
}
