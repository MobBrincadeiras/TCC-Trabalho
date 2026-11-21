using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float StartSpeed;
    public float ExtraSpeed;
    public float MaxExtraSpeed;

    private int HitCounter = 0;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(Launch());
    }

    public IEnumerator Launch()
    {
        HitCounter = 0;
        yield return new WaitForSeconds(1);

        MoveBall(new Vector2(-1, 0));
    }
    public void MoveBall(Vector2 direction)
    {
        direction = direction.normalized;

        float ballSpeed = StartSpeed + HitCounter * ExtraSpeed;

        rb.velocity = direction * ballSpeed;
    }

    public void IncreaseHitCounter()
    {
        if(HitCounter * ExtraSpeed < MaxExtraSpeed)
        {
            HitCounter++;
        }
    }
}
