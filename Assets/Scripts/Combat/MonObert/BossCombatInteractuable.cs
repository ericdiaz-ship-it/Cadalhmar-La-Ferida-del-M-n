using UnityEngine;

// Aquest script serveix per posar-lo a un objecte o NPC perquè funcioni com un Boss.
// Quan el jugador interactuï amb ell (prement la tecla d'interacció), s'activarà el combat.
public class BossCombatInteractuable : MonoBehaviour, IInteractuable
{
    [Header("Configuració del Boss")]
    [Tooltip("El monstre final (Scriptable Object).")]
    public CreatureProfile bossProfile;
    [Tooltip("Nivell del Boss.")]
    public int bossLevel = 5;
    
    [Tooltip("Esbirros que acompanyaran al Boss (Scriptable Objects).")]
    public CreatureProfile[] minionProfiles;
    [Tooltip("Nivell dels esbirros.")]
    public int minionsLevel = 3;
    
    [Header("Configuració del Mapa")]
    [Tooltip("Posa aquí l'arxiu de text (.txt) amb el mapa on es lluitarà.")]
    public TextAsset bossMapData; 

    [Header("Cinemàtica Final")]
    [Tooltip("Cinemàtica que es reproduirà quan derrotem el Boss (opcional).")]
    public CinematicaSO winCinematica;

    public void Interactuar()
    {
        BattleDescriptor bossBattle = new BattleDescriptor();
        
        if (bossMapData != null)
        {
            bossBattle.mapStringData = bossMapData.text;
        }
        else
        {
            Debug.LogWarning("Avís: El Boss no té cap TextAsset de mapa assignat, no s'enviarà cap mapa especial.");
        }
        
        bossBattle.category = BattleCategory.BOSS;

        // Generem les dades (CreatureData) a partir dels ScriptableObjects (CreatureProfile)
        int minionCount = (minionProfiles != null) ? minionProfiles.Length : 0;
        CreatureData[] enemics = new CreatureData[1 + minionCount];
        
        // Generem el Boss
        enemics[0] = bossProfile.GenerateDataForLevel(bossLevel);
        
        // Generem els Minions
        for(int i = 0; i < minionCount; i++) 
        {
            enemics[i + 1] = minionProfiles[i].GenerateDataForLevel(minionsLevel);
        }
        bossBattle.aiCreatures = enemics;

        bossBattle.onHumanWin = () => {
            if (winCinematica != null)
            {
                CinematicaDirector director = Object.FindFirstObjectByType<CinematicaDirector>();
                if (director != null)
                {
                    director.IniciarCinematica(winCinematica);
                }
                else
                {
                    Debug.LogWarning("No s'ha trobat cap CinematicaDirector a l'escena per llançar la cinemàtica.");
                }
            }
            Destroy(this.gameObject);
        };

        OverworldManager.current.StartBattle(bossBattle);
    }
}
