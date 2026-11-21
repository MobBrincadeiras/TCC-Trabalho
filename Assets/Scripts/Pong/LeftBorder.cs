using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftBorder : MonoBehaviour
{
    public PongManager gameManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            gameManager.EndGame();
        }
    }
}
