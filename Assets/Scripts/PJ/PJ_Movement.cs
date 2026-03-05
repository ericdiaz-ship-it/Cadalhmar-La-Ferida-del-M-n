using UnityEngine;

public class PJ_Movement : MonoBehaviour
{
    //velocitat personatge(com a més gran, més ràpid)
    public float speed = 5f;
    //rigibody del personatge
    public Rigidbody2D rb;

    // SpriteRenderer del personatge
    public SpriteRenderer spriteRenderer;

    // Sprites direccionals
    public Sprite EldrinAbajo;
    public Sprite EldrinArriba;
    public Sprite EldrinDerecha;
    public Sprite EldrinIzquierda;

    // Animator per controlar animacions
    public Animator animator;

    private float horizontal;
    private float vertical;

    private float lastHorizontal = 0f;
    private float lastVertical = 0f;

    // Última dirección de movimiento
    private Vector2 lastDirection = Vector2.down;

    void Start()
    {
        // Per defecte sempre mirant cap avall
        spriteRenderer.sprite = EldrinAbajo;
        lastDirection = Vector2.down;
    }

    void Update()
    {
        //obtenir entrada horizontal (tecles A i D o fletxes esquerra i dreta)
        horizontal = Input.GetAxisRaw("Horizontal");

        //obtenir entrada vertical (tecles W i S o fletxes amunt i avall)
        vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0) lastHorizontal = Time.time;
        if (vertical != 0) lastVertical = Time.time;

        //evitar moviment diagonal respectant la tecla premuda primer
        if (horizontal != 0 && vertical != 0)
        {
            if (lastHorizontal > lastVertical)
                vertical = 0;
            else
                horizontal = 0;
        }

        Vector2 movement = new Vector2(horizontal, vertical);

        //comprovar si s'està movent
        bool isMoving = movement != Vector2.zero;

        if (isMoving)
        {
            // Guardamos la última dirección para usarla cuando paremos
            lastDirection = movement;

            // Activamos animación instantánea
            animator.SetBool("isMoving", true);
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        else
        {
            // Cuando está quieto, desactivamos la animación moviente
            animator.SetBool("isMoving", false);
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);

            // Cambiamos el sprite manual según la última dirección
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
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(horizontal, vertical).normalized;
        rb.linearVelocity = movement * speed;
    }
}