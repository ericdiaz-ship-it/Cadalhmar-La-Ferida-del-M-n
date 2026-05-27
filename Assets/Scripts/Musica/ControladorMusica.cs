using UnityEngine;

public class ControladorMusica : MonoBehaviour
{
    private static ControladorMusica instancia;

    void Awake()
    {
        // Si ja existeix una instancia d'aquest objecte, destrueix el nou per no duplicar la música
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        // Si es el primer, li assignem com a la instancia principal
        instancia = this;

        // Li diem a Unity que no destrueixi aquest objecte al canviar d'escena
        DontDestroyOnLoad(gameObject);
    }
}