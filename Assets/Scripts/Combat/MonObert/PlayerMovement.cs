using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator; // Opcional per a les animacions

    private Vector2 movement;

    void Update()
    {
        // Obtenim l'entrada del moviment
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalitzem per evitar un moviment diagonal més ràpid
        movement = movement.normalized;

        // Configurem els paràmetres de l'animador si existeix
        if (animator != null)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
    }

    void FixedUpdate()
    {
        // Mou el jugador
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}