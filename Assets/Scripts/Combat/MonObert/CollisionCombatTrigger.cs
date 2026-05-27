using UnityEngine;

public class CollisionCombatTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"[CCT] OnCollisionEnter2D con: {collision.gameObject.name}");

        if (OverworldManager.isBattleActive)
        {
            Debug.LogWarning("[CCT] BLOQUEADO — isBattleActive es true");
            return;
        }

        if (OverworldManager.current == null)
        {
            Debug.LogError("[CCT] OverworldManager.current es NULL");
            return;
        }

        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
        if (enemy == null)
        {
            Debug.Log($"[CCT] {collision.gameObject.name} no tiene EnemyAI, ignorado");
            return;
        }

        if (enemy.creatureData == null)
        {
            Debug.LogError("[CCT] El EnemyAI no tiene creatureData asignado");
            return;
        }

        Debug.Log("[CCT] Iniciando batalla con: " + enemy.gameObject.name);
        OverworldManager.current.StartBattleWithEnemy(enemy);
    }
}