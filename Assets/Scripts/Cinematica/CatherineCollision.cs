using UnityEngine;

public class CatherineCollision : MonoBehaviour
{
    private void DesactivarColliderSiEsJugador(GameObject altreObjecte)
    {
        // Afegim logs per veure amb què està xocant exactament
        Debug.Log("Catherine ha xocat amb: " + altreObjecte.name + " | Tag: " + altreObjecte.tag);

        if (altreObjecte.CompareTag("Player") || altreObjecte.CompareTag("player"))
        {
            Debug.Log("S'ha detectat el jugador! Desactivant el CapsuleCollider2D de Catherine...");
            CapsuleCollider2D col = GetComponent<CapsuleCollider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DesactivarColliderSiEsJugador(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        DesactivarColliderSiEsJugador(collider.gameObject);
    }
}
