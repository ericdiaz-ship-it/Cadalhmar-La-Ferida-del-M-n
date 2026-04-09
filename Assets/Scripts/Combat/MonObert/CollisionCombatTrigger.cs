using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCombatTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collided with an enemy
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            // Start battle with this enemy
            OverworldManager.current.StartBattleWithEnemy(enemy.creatureData);
        }
    }
}