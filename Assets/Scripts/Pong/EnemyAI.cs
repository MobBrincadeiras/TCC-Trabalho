using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    
    public Transform ball;
    
    public float speed = 20f;

    public float minY = -3.5f;
    public float maxY = 3.5f;

    private Rigidbody2D rb;
    private Rigidbody2D ballRb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (ball != null)
        {
            ballRb = ball.GetComponent<Rigidbody2D>();
        }
    }

    void FixedUpdate()
    {
        if (ball == null || ballRb == null)
            return;

        if (ballRb.velocity.x > 0)
        {
            float distanceX = transform.position.x - ball.position.x;

            float timeToReach = distanceX / ballRb.velocity.x;

            float predictedY = ball.position.y +
                ballRb.velocity.y * timeToReach;

            float range = maxY - minY;

            float reflectedY = Mathf.PingPong(
                predictedY - minY,
                range
            ) + minY;

            MoveEnemy(reflectedY);
        }
    }

    void MoveEnemy(float targetY)
    {
        float newY = Mathf.MoveTowards(
            rb.position.y,
            targetY,
            speed * Time.fixedDeltaTime
        );

        rb.MovePosition(
            new Vector2(rb.position.x, newY)
        );
    }
}