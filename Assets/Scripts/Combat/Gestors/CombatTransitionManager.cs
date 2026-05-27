using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatTransitionManager : MonoBehaviour
{
    public static CombatTransitionManager current;

    [Header("Transition Settings")]
    public string overworldSceneName = "Overworld";
    public string battleSceneName = "Battle";

    void Awake()
    {
        current = this;
    }

    // This can be used to handle transitions if needed separately
    public void TransitionToBattle(EnemyAI enemy)
    {
        OverworldManager.current.StartBattleWithEnemy(enemy);
    }

    public void TransitionToOverworld()
    {
        // Handled in OverworldManager.EndBattle
    }
}