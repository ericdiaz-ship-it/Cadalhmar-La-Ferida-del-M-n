using UnityEngine;

public class PJ_Movement : MonoBehaviour
{
    
    //velocitat personatge(com a més gran, més ràpid)
    public float speed = 5f;
    //rigibody del personatge
    public Rigidbody2D rb;

    // fixedupdate és una funció que s'executa a intervals regulars, ideal per a la física i el moviment
    void FixedUpdate()
    {
        //obtenir entrada horizontal (tecles A i D o fletxes esquerra i dreta)
        //dreta = 1, esquerra = -1, no moure's = 0
        float horizant = Input.GetAxisRaw("Horizontal");
        //obtenir entrada vertical (tecles W i S o fletxes amunt i avall)
        //amunt = 1, avall = -1, no moure's = 0
        float vertical = Input.GetAxisRaw("Vertical");
        //aplicar moviment al personatge
        //multiplicar per speed per ajustar la velocitat del personatge
        //el resultat és un vector que indica la direcció i la velocitat del moviment
        //rb.linearVelocity és la velocitat actual del rigidbody, que es canvia per moure el personatge
        Vector2 movement = new Vector2(horizant, vertical).normalized;
        rb.linearVelocity = movement * speed;
    }
}
