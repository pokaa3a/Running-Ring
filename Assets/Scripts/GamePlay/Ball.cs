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
            Mathf.Sin(degree * Mathf.Deg2Rad) * Ring.Config.radius,
            Ring.Config.originY + Mathf.Cos(degree * Mathf.Deg2Rad) * Ring.Config.radius
        );
        triggered = false;
        this.id = id;
        this.ballManager = manager;
    }

    void FixedUpdate()
    {
        // Circular moving
        Vector2 vec = gameObject.transform.localPosition -
            new Vector3(0, Ring.Config.originY);

        float radian = Vector2.Angle(Vector2.up, vec) * Mathf.Deg2Rad;
        if (gameObject.transform.localPosition.x < 0)
            radian = 2f * Mathf.PI - radian;

        if (BallManager.Config.clockwise)
            radian += BallManager.Config.angularSpeed * Time.fixedDeltaTime;
        else
            radian -= BallManager.Config.angularSpeed * Time.fixedDeltaTime;

        gameObject.transform.localPosition = new Vector2(
            Mathf.Sin(radian) * Ring.Config.radius,
            Ring.Config.originY + Mathf.Cos(radian) * Ring.Config.radius
        );
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            ballManager.BallCollides(this.id);
        }
        else if (collision.gameObject.tag == "Breakable")
        {
            ballManager.BallCollides(this.id);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Food")
        {
            Food food = collision.gameObject.GetComponent<Food>();

            ballManager.AddBalls(id, food.number);
            Destroy(collision.gameObject);
        }
    }

    public void BallExplode()
    {
        triggered = false;
    }
}
