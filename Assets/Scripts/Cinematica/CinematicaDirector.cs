using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicaDirector : MonoBehaviour
{
    [Header("Referències fixes de la escena")]
    public GestorDialegs gestorDialegs;
    public PJ_Movement jugador;
    public PasoCinematica[] passos;
    // Activate a global boolean variable when this cinematic finishes
    public bool posarVariableGlobal = false;
    public string nomVariableGlobal = ""; // name of the static bool in VariablesGlobals to set to true
    public ActorCinematic actorJugador;

    [Header("Actors de la escena (NPCs amb ActorCinematic)")]
    public List<ActorCinematic> actors = new List<ActorCinematic>();

    [Header("Punts de destí de la escena (GameObjects buits)")]
    public List<Transform> puntsDestí = new List<Transform>();

    private bool enCinematica = false;

    // ── Resolució per nom ──────────────────────────────────────────

    private ActorCinematic TrobarActor(string nom)
    {
        foreach (ActorCinematic a in actors)
            if (a != null && a.gameObject.name == nom)
                return a;

        Debug.LogWarning($"[Director] Actor '{nom}' no trobat. Comprova que el nom coincideix exactament amb el GameObject de la escena i que està a la llista d'actors del Director.");
        return null;
    }

    private Transform TrobarDestí(string nom)
    {
        foreach (Transform t in puntsDestí)
            if (t != null && t.gameObject.name == nom)
                return t;

        Debug.LogWarning($"[Director] Punt destí '{nom}' no trobat. Comprova que el nom coincideix exactament amb el GameObject de la escena i que està a la llista de punts del Director.");
        return null;
    }

    // ── Execució ───────────────────────────────────────────────────

    public void IniciarCinematica(CinematicaSO cinematica)
    {
        if (cinematica == null)
        {
            Debug.LogError("[Director] ERROR: La cinematica és null!");
            return;
        }

        if (enCinematica)
        {
            Debug.LogWarning("[Director] Ja hi ha una cinematica en curs, ignorant crida.");
            return;
        }

        if (cinematica.passos == null || cinematica.passos.Length == 0)
        {
            Debug.LogError($"[Director] ERROR: La cinematica '{cinematica.name}' no té cap pas definit!");
            return;
        }

        Debug.Log($"[Director] Iniciant cinematica '{cinematica.name}' amb {cinematica.passos.Length} passos.");
        StartCoroutine(ExecutarCinematica(cinematica));
    }

    private IEnumerator ExecutarCinematica(CinematicaSO cinematica)
    {
        enCinematica = true;
        BloquearJugador(true);

        for (int i = 0; i < cinematica.passos.Length; i++)
        {
            Debug.Log($"[Director] Executant pas {i + 1}/{cinematica.passos.Length}: {cinematica.passos[i].tipusPas}");
            yield return StartCoroutine(ExecutarPas(cinematica.passos[i]));
        }

        Debug.Log("[Director] Cinematica acabada.");
        BloquearJugador(false);
        enCinematica = false;
        // If configured, set the specified global variable to true
        if (cinematica.posarVariableGlobal && !string.IsNullOrEmpty(cinematica.nomVariableGlobal))
        {
            var field = typeof(VariablesGlobals).GetField(cinematica.nomVariableGlobal);
            if (field != null && field.FieldType == typeof(bool))
            {
                field.SetValue(null, true);
                Debug.Log($"[Director] Global variable '{cinematica.nomVariableGlobal}' set to true.");
            }
            else
            {
                Debug.LogWarning($"[Director] Global variable '{cinematica.nomVariableGlobal}' not found or not a bool.");
            }
        }
    }

    private IEnumerator ExecutarPas(PasoCinematica pas)
    {
        switch (pas.tipusPas)
        {
            case TipusPas.MourePJ:
            {
                Transform destí = TrobarDestí(pas.nomDestí);
                if (destí != null)
                    yield return StartCoroutine(actorJugador.MoureA(destí, pas.velocitat));
                else
                    Debug.LogError($"[Director] No s'ha pogut moure el jugador: destí '{pas.nomDestí}' no trobat.");
                break;
            }

            case TipusPas.MoureActor:
            {
                ActorCinematic actor = TrobarActor(pas.nomActor);
                Transform destí = TrobarDestí(pas.nomDestí);
                if (actor != null && destí != null)
                    yield return StartCoroutine(actor.MoureA(destí, pas.velocitat));
                break;
            }

            case TipusPas.Dialeg:
                if (pas.dialeg != null)
                {
                    gestorDialegs.IniciarDialeg(pas.dialeg);
                    yield return new WaitUntil(() => !gestorDialegs.contenidorDialeg.activeInHierarchy);
                }
                else
                {
                    Debug.LogWarning("[Director] Pas de tipus Dialeg sense DialegSO assignat.");
                }
                break;

            case TipusPas.Espera:
                Debug.Log($"[Director] Esperant {pas.tempsEspera}s...");
                yield return new WaitForSeconds(pas.tempsEspera);
                break;

            case TipusPas.CanviarEscena:
                if (!string.IsNullOrEmpty(pas.escenaACanviar))
                {
                    Debug.Log($"[Director] Canviant a l'escena: {pas.escenaACanviar}");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(pas.escenaACanviar);
                }
                else
                {
                    Debug.LogWarning("[Director] No s'ha especificat cap escena per canviar.");
                }
                break;
        }
    }

    private void BloquearJugador(bool bloquejar)
    {
        if (jugador != null)
            jugador.enabled = !bloquejar;
        else
            Debug.LogWarning("[Director] Camp 'Jugador' buit, no es bloquejarà el moviment.");
    }
}