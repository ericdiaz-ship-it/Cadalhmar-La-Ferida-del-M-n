using UnityEngine;

public class PJ_Movement : MonoBehaviour
{
    // --- VARIABLES ORIGINALS ---
    public float speed = 5f;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    public Sprite EldrinAbajo;
    public Sprite EldrinArriba;
    public Sprite EldrinDerecha;
    public Sprite EldrinIzquierda;

    public Animator animator;

    private float horizontal;
    private float vertical;
    private float lastHorizontal = 0f;
    private float lastVertical = 0f;
    private Vector2 lastDirection = Vector2.down;

    // --- CONFIGURACIÓ D'INTERACCIÓ (AMB CENTRE AJUSTABLE) ---
    [Header("Configuració d'Interacció")]
    public float interactDistance = 1.2f;
    public LayerMask interactLayer;
    public Vector2 interactOffset;

    [Header("Cinemàtica")]
    public GameObject catherineGameObject;

    void Start()
    {
        spriteRenderer.sprite = EldrinAbajo;
        lastDirection = Vector2.down;
    }

    void Update()
    {
        if (catherineGameObject != null)
        {
            catherineGameObject.SetActive(VariablesGlobals.CatherineCinematica);
        }

        // Si hi ha una batalla activa, parem el jugador completament
        if (OverworldManager.isBattleActive)
        {
            horizontal = 0;
            vertical = 0;
            animator.SetBool("isMoving", false);
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            return;
        }

        // 1. OBTENIR INPUT (ORIGINAL)
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0) lastHorizontal = Time.time;
        if (vertical != 0) lastVertical = Time.time;

        if (horizontal != 0 && vertical != 0)
        {
            if (lastHorizontal > lastVertical)
                vertical = 0;
            else
                horizontal = 0;
        }

        Vector2 movement = new Vector2(horizontal, vertical);
        bool isMoving = movement != Vector2.zero;

        // 2. GESTIÓ D'ANIMACIONS I DIRECCIÓ (ORIGINAL)
        if (isMoving)
        {
            lastDirection = movement;
            animator.SetBool("isMoving", true);
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);

            if (Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y))
            {
                if (lastDirection.x > 0) spriteRenderer.sprite = EldrinDerecha;
                else spriteRenderer.sprite = EldrinIzquierda;
            }
            else
            {
                if (lastDirection.y > 0) spriteRenderer.sprite = EldrinArriba;
                else spriteRenderer.sprite = EldrinAbajo;
            }
        }

        // 3. BOTÓ D'INTERACCIÓ
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Vector2 origin = (Vector2)transform.position + interactOffset;
        RaycastHit2D hit = Physics2D.Raycast(origin, lastDirection, interactDistance, interactLayer);
        Debug.DrawRay(origin, lastDirection * interactDistance, Color.red, 0.5f);

        if (hit.collider != null)
        {
            IInteractuable interactuable = hit.collider.GetComponent<IInteractuable>();
            if (interactuable != null)
            {
                interactuable.Interactuar();
            }
        }
    }

    void FixedUpdate()
    {
        // Si hi ha una batalla activa, parem el Rigidbody
        if (OverworldManager.isBattleActive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 movement = new Vector2(horizontal, vertical).normalized;
        rb.linearVelocity = movement * speed;
    }
}