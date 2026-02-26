using UnityEngine;

public class PJ_Movement : MonoBehaviour
{
    
    //velocitat personatge(com a més gran, més ràpid)
    public float speed = 5f;
    //rigibody del personatge
    public Rigidbody2D rb;

    // Sprites direccionals
    public GameObject EldrinAbajo;
    public GameObject EldrinArriba;
    public GameObject EldrinDerecha;
    public GameObject EldrinIzquierda;

    void Start()
    {
        // Per defecte sempre mirant cap avall
        ActivarSprite(EldrinAbajo);
    }

    // fixedupdate és una funció que s'executa a intervals regulars, ideal per a la física i el moviment
    void FixedUpdate()
    {
        //obtenir entrada horizontal (tecles A i D o fletxes esquerra i dreta)
        //dreta = 1, esquerra = -1, no moure's = 0
        float horizant = Input.GetAxisRaw("Horizontal");
        //obtenir entrada vertical (tecles W i S o fletxes amunt i avall)
        //amunt = 1, avall = -1, no moure's = 0
        float vertical = Input.GetAxisRaw("Vertical");

        //evitar moviment en diagonal (prioritat moviment horitzontal)
        if (horizant != 0)
        {
            vertical = 0;
        }

        //aplicar moviment al personatge
        //multiplicar per speed per ajustar la velocitat del personatge
        //el resultat és un vector que indica la direcció i la velocitat del moviment
        //rb.linearVelocity és la velocitat actual del rigidbody, que es canvia per moure el personatge
        Vector2 movement = new Vector2(horizant, vertical).normalized;
        rb.linearVelocity = movement * speed;

        //canviar sprite segons direcció
        if (horizant > 0)
        {
            ActivarSprite(EldrinDerecha);
        }
        else if (horizant < 0)
        {
            ActivarSprite(EldrinIzquierda);
        }
        else if (vertical > 0)
        {
            ActivarSprite(EldrinArriba);
        }
        else if (vertical < 0)
        {
            ActivarSprite(EldrinAbajo);
        }
    }

    void ActivarSprite(GameObject spriteActivo)
    {
        EldrinAbajo.SetActive(false);
        EldrinArriba.SetActive(false);
        EldrinDerecha.SetActive(false);
        EldrinIzquierda.SetActive(false);

        spriteActivo.SetActive(true);
    }
}