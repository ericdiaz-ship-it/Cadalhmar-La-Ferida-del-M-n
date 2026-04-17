using UnityEngine;
using System.Collections.Generic;

public class NPCFollower : MonoBehaviour
{
    // =========================================================
    // REFERÈNCIES
    // =========================================================
    [Header("Referències")]
    public Transform player;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;

    // =========================================================
    // SPRITES DE DIRECCIÓ
    // =========================================================
    [Header("Sprites de Direcció")]
    public Sprite npcAbajo;
    public Sprite npcArriba;
    public Sprite npcDerecha;
    public Sprite npcIzquierda;

    // =========================================================
    // CONFIGURACIÓ DE SEGUIMENT
    // =========================================================
    [Header("Configuració de Seguiment")]
    public float speed = 5f;
    public float minDistance = 0.1f; // REDUÏT: Guarda punts més sovint (recomanat 0.1)
    public int stepDelay = 8;        // REDUÏT: Triga menys passos en començar a seguir-te (recomanat 5-10)
    public int maxPathPoints = 300;

    // =========================================================
    // VARIABLES PRIVADES
    // =========================================================
    private struct PathPoint
    {
        public Vector2 position;
        public Vector2 direction;
    }

    private List<PathPoint> pathPoints = new List<PathPoint>();
    private Vector2 lastDirection = Vector2.down;

    // Controla si el NPC estava en moviment el frame anterior
    private bool estabaMovent = false;

    // =========================================================
    // INICIALITZACIÓ
    // =========================================================
    void Start()
    {
        if (rb != null)
        {
            rb.isKinematic  = true;
            rb.gravityScale = 0f;
        }

        // Comencem amb l'Animator DESACTIVAT i el sprite d'avall
        animator.enabled = false;
        if (npcAbajo != null) spriteRenderer.sprite = npcAbajo;
    }

    // =========================================================
    // BUCLE PRINCIPAL
    // =========================================================
    void FixedUpdate()
    {
        ActualitzarCamí();
        MoureNPC();
    }

    // =========================================================
    // REGISTRAR EL RASTRE DEL JUGADOR
    // =========================================================
    void ActualitzarCamí()
    {
        Vector2 posJugador = player.position;

        if (pathPoints.Count == 0 ||
            Vector2.Distance(posJugador, pathPoints[0].position) > minDistance)
        {
            Vector2 posAnterior = pathPoints.Count > 0
                ? pathPoints[0].position
                : (Vector2)transform.position;

            Vector2 direccio = (posJugador - posAnterior).normalized;

            pathPoints.Insert(0, new PathPoint
            {
                position  = posJugador,
                direction = direccio
            });

            while (pathPoints.Count > maxPathPoints)
                pathPoints.RemoveAt(pathPoints.Count - 1);
        }
    }

    // =========================================================
    // MOURE EL NPC AMB RETARD
    // =========================================================
    void MoureNPC()
    {
        if (pathPoints.Count > stepDelay)
        {
            // ---- NPC EN MOVIMENT ----
            PathPoint puntObjectiu = pathPoints[pathPoints.Count - 1];

            Vector2 novaPosicio = Vector2.MoveTowards(
                transform.position,
                puntObjectiu.position,
                speed * Time.fixedDeltaTime
            );

            if (rb != null)
                rb.MovePosition(novaPosicio);
            else
                transform.position = novaPosicio;

            // Guardem la direcció per quan s'aturi
            if (puntObjectiu.direction != Vector2.zero)
                lastDirection = puntObjectiu.direction;

            // ACTIVEM l'Animator i li diem explícitament que s'està movent
            animator.enabled = true;
            animator.SetBool("isMoving", true); // <--- AFEGIT: Crucial per activar la transició de l'Animator
            animator.SetFloat("Horizontal", puntObjectiu.direction.x);
            animator.SetFloat("Vertical",   puntObjectiu.direction.y);

            estabaMovent = true;

            // Eliminem el punt quan hi hem arribat
            if (Vector2.Distance(transform.position, puntObjectiu.position) < 0.05f)
                pathPoints.RemoveAt(pathPoints.Count - 1);
        }
        else
        {
            // ---- NPC ATURAT ----
            if (estabaMovent)
            {
                // Li diem a l'Animator que s'ha aturat abans de desactivar-lo
                animator.SetBool("isMoving", false); // <--- AFEGIT: Per evitar errors d'estat si es reactiva
                
                // DESACTIVEM l'Animator
                animator.enabled = false;

                // Assignem el sprite directament segons la última direcció
                AssignarSpriteAturat(lastDirection);

                estabaMovent = false;
            }
        }
    }

    // =========================================================
    // SPRITE QUAN S'ATURA
    // =========================================================
    void AssignarSpriteAturat(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0) spriteRenderer.sprite = npcDerecha;
            else           spriteRenderer.sprite = npcIzquierda;
        }
        else
        {
            if (dir.y > 0) spriteRenderer.sprite = npcArriba;
            else           spriteRenderer.sprite = npcAbajo;
        }
    }
}