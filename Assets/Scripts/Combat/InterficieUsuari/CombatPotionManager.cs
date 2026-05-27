using UnityEngine;
using UnityEngine.UI;

public class CombatPotionManager : MonoBehaviour
{
    [Header("Configuració")]
    [Tooltip("L'Scriptable Object Potion que s'utilitzarà per curar")]
    public Potion potionToUse;
    
    [Tooltip("Text de la UI que mostra el número de pocions restants")]
    public Text potionCountText;

    private HumanMaster humanMaster;

    void Start()
    {
        // Actualitza el text inicialment
        UpdatePotionText();
    }

    void Update()
    {
        // 1. Només funciona durant la batalla
        if (!OverworldManager.isBattleActive)
            return;

        // 2. Comprovem si l'usuari ha premut la tecla H
        if (Input.GetKeyDown(KeyCode.H))
        {
            TryUsePotion();
        }
    }

    private void TryUsePotion()
    {
        // Comprovem si queden pocions a les variables globals
        if (VariablesGlobals.pocions <= 0)
        {
            Debug.Log("No queden pocions a l'inventari.");
            return;
        }

        // Busquem el HumanMaster si no el tenim cachejat
        if (humanMaster == null)
        {
            humanMaster = FindObjectOfType<HumanMaster>();
        }

        // Comprovem que s'hagi trobat el Master i que tingui una criatura seleccionada
        if (humanMaster != null && humanMaster.hasCreatureSelected)
        {
            Creature targetCreature = humanMaster.selectedCreature;

            // Verifiquem si la criatura pertany al jugador humà
            if (targetCreature.belongToHuman)
            {
                if (potionToUse != null)
                {
                    // Usem l'Scriptable Object Potion (demana CreatureData)
                    potionToUse.Use(targetCreature.innerData);
                    
                    // Restem la poció de la variable global
                    VariablesGlobals.pocions--;
                    
                    // Forcem l'actualització visual de la vida de la criatura 
                    // enviant un canvi de 0, ja que Potion altera la variable internament
                    targetCreature.ModifyHealth(0);
                    
                    // Actualitzem el text de l'UI
                    UpdatePotionText();
                    
                    Debug.Log("Poció utilitzada! Pocions restants: " + VariablesGlobals.pocions);
                }
                else
                {
                    Debug.LogWarning("Avís: El CombatPotionManager no té cap Potion assignat.");
                }
            }
            else
            {
                Debug.Log("La criatura seleccionada no és un heroi teu.");
            }
        }
        else
        {
            Debug.Log("No has seleccionat cap heroi.");
        }
    }

    private void UpdatePotionText()
    {
        if (potionCountText != null)
        {
            potionCountText.text = VariablesGlobals.pocions.ToString();
        }
    }
}
