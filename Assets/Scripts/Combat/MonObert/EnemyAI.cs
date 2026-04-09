using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public float moveSpeed = 2f;
    public float patrolRadius = 5f;
    public float changeDirectionTime = 2f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 startPosition;
    private float directionChangeTimer;

    public CreatureData creatureData; // The creature this enemy represents

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        directionChangeTimer = changeDirectionTime;
    }

    void Update()
    {
        // Simple patrol AI: move in random directions within radius
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0)
        {
            // Change direction randomly
            movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            directionChangeTimer = changeDirectionTime;
        }

        // Keep within patrol radius
        if (Vector2.Distance(transform.position, startPosition) > patrolRadius)
        {
            // Move back towards start
            movement = (startPosition - (Vector2)transform.position).normalized;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}