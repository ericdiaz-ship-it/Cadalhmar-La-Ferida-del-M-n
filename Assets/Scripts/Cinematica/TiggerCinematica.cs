using UnityEngine;

public class TriggerCinematica : MonoBehaviour
{
    [Header("Cinematica")]
    public CinematicaSO cinematica;
    public CinematicaDirector director;

    [Header("Opcions")]
    public bool nomesMesUnaVegada = true;

    private bool jaExecutada = false;

    // Opció A: Trigger per col·lisió
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        LlancarCinematica();
    }

    // Opció B: Interacció manual (descomentar per usar)
    // public void Interactuar() => LlancarCinematica();

    private void LlancarCinematica()
    {
        if (nomesMesUnaVegada && jaExecutada) return;
        jaExecutada = true;
        director.IniciarCinematica(cinematica);
    }
}