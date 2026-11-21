using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    public BallMovement ballMovement;
    public ScoreManager scoreManager;

    private void Bounce(Collision2D collision)
    {
        Vector3 BallPosition = transform.position;
        Vector3 PlayerPosition = collision.transform.position;
        float PlayerHeight = collision.collider.bounds.size.y;

        float PositionX;
        if(collision.gameObject.name == "Player")
        {
            PositionX = 1;
        }

        else
        {
            PositionX = -1;
        }

        float PositionY = (BallPosition.y - PlayerPosition.y) / PlayerHeight;

        ballMovement.IncreaseHitCounter();
        ballMovement.MoveBall(new Vector2(PositionX, PositionY));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player" || collision.gameObject.name == "Enemy")
        {
            Bounce(collision);
        }

        if(collision.gameObject.name == "Enemy")
        {
            scoreManager.PlayerGoal();
        }
    }
}
