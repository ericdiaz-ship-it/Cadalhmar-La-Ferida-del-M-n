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

    public CreatureData creatureData; // La criatura que representa aquest enemic

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        directionChangeTimer = changeDirectionTime;
    }

    void Update()
    {
        if (OverworldManager.isBattleActive)
        {
            movement = Vector2.zero;
            return;
        }

        // Simple patrol AI: move in random directions within radius
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0)
        {
            ChangeDirection();
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
        if (OverworldManager.isBattleActive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Usem linearVelocity en lloc de MovePosition per respectar completament la física
        rb.linearVelocity = movement * moveSpeed;
    }

    private void ChangeDirection()
    {
        movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        directionChangeTimer = changeDirectionTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Canvia de direcció immediatament en xocar contra un objecte
        ChangeDirection();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Si es queda encallat contra una paret, accelera el temps de canvi de direcció
        directionChangeTimer -= Time.deltaTime * 5f;
    }
}