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

    private float horizontal;
    private float vertical;

    private float lastHorizontal = 0f;
    private float lastVertical = 0f;

    void Start()
    {
        // Per defecte sempre mirant cap avall
        spriteRenderer.sprite = EldrinAbajo;
    }

    void Update()
    {
        //obtenir entrada horizontal (tecles A i D o fletxes esquerra i dreta)
        //dreta = 1, esquerra = -1, no moure's = 0
        horizontal = Input.GetAxisRaw("Horizontal");

        //obtenir entrada vertical (tecles W i S o fletxes amunt i avall)
        //amunt = 1, avall = -1, no moure's = 0
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

        //canviar sprite segons direcció
        if (horizontal > 0)
        {
            spriteRenderer.sprite = EldrinDerecha;
        }
        else if (horizontal < 0)
        {
            spriteRenderer.sprite = EldrinIzquierda;
        }
        else if (vertical > 0)
        {
            spriteRenderer.sprite = EldrinArriba;
        }
        else if (vertical < 0)
        {
            spriteRenderer.sprite = EldrinAbajo;
        }
    }

    // fixedupdate és una funció que s'executa a intervals regulars, ideal per a la física i el moviment
    void FixedUpdate()
    {
        //aplicar moviment al personatge
        //multiplicar per speed per ajustar la velocitat del personatge
        //el resultat és un vector que indica la direcció i la velocitat del moviment
        //rb.velocity és la velocitat actual del rigidbody, que es canvia per moure el personatge
        Vector2 movement = new Vector2(horizontal, vertical).normalized;
        rb.velocity = movement * speed;
    }
}