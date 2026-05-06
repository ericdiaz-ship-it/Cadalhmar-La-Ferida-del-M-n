using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    public static OverworldManager current;

    public CreatureProfile[] humanCreatureProfiles;
    private CreatureData[] humanCreatures;

    // Esto es temporal
    public int[] humanCreatureLevels;
    public List<Item> startUpItems = new List<Item>();

    [Header("Scene names")]
    public string battleSceneName = "Battle";

    [Header("Overworld Settings")]
    public OverworldEnemySpawner enemySpawner; // Reference to spawner

    private Scene currentScene;

    private TeamUI teamUI;
    private InventoryUI inventoryUI;

    private List<ItemStack> inventory;

    private EventSystem overworldEventSystem;

    private CreatureData currentEnemy; // For collision-based battles
    private BattleDescriptor currentBattleDescriptor;

    void Awake()
    {
        current = this;

        this.humanCreatures = new CreatureData[this.humanCreatureProfiles.Length];
        for (int i = 0; i < this.humanCreatureProfiles.Length; i++)
        {
            int targetLevel = this.humanCreatureLevels[i];
            this.humanCreatures[i] = this.humanCreatureProfiles[i].GenerateDataForLevel(targetLevel);
        }

        this.teamUI = Object.FindFirstObjectByType<TeamUI>(FindObjectsInactive.Include);

        this.inventoryUI = Object.FindFirstObjectByType<InventoryUI>(FindObjectsInactive.Include);

        this.inventory = new List<ItemStack>();
        foreach (var item in this.startUpItems)
        {
            this.AddItemToInventory(item);
        }

        // NOTE: Esto es para el warning de que hay dos EventSystem activos.
        this.overworldEventSystem = EventSystem.current;

        this.currentScene = SceneManager.GetActiveScene();
    }

    public void ToggleTeamView()
    {
        if (this.inventoryUI.isVisible)
        {
            this.inventoryUI.Hide();
        }

        this.teamUI.ToggleDisplay(this.humanCreatures);
    }

    public void ToggleInventoryView()
    {
        if (this.teamUI.isVisible)
        {
            this.teamUI.Hide();
        }

        this.inventoryUI.ToggleDisplay(this.inventory, this.humanCreatures);
    }

    private IEnumerator LoadBattle()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(this.battleSceneName, LoadSceneMode.Additive);

        while (operation.isDone == false)
        {
            yield return null;
        }

        Scene battleScene = SceneManager.GetSceneByName(this.battleSceneName);
        SceneManager.SetActiveScene(battleScene);

        // Create battle descriptor for enemy
        BattleDescriptor descriptor = new BattleDescriptor();
        descriptor.humanCreatures = this.humanCreatures;
        descriptor.aiCreatures = new CreatureData[] { this.currentEnemy };
        descriptor.Validate();

        BattleManager.current.StartBattle(descriptor);
        this.gameObject.SetActive(false);
    }

    public void StartBattleWithEnemy(CreatureData enemy)
    {
        this.currentEnemy = enemy;
        this.overworldEventSystem.enabled = false;
        StartCoroutine(this.LoadBattle());
    }

    public void StartBattle(BattleDescriptor descriptor)
    {
        this.currentBattleDescriptor = descriptor;
        this.currentBattleDescriptor.humanCreatures = this.humanCreatures;
        this.currentBattleDescriptor.Validate();

        this.overworldEventSystem.enabled = false;

        StartCoroutine(this.LoadBattle());
    }

    public void EndBattle(BattleOverMessage message)
    {
        SceneManager.UnloadSceneAsync(this.battleSceneName);

        SceneManager.SetActiveScene(this.currentScene);
        this.gameObject.SetActive(true);

        this.overworldEventSystem.enabled = true;

        this.StoreResultingCreatureData(message.creatureBattleOverData.ToArray());
        this.StoreItemRewards(message.itemRewards.ToArray());

        if (message.isHumanWin)
        {
            this.currentBattleDescriptor.onHumanWin();
        }
        else if (message.isHumanLoss)
        {
            this.currentBattleDescriptor.onHumanLoss();
        }
        else if (message.isFlee)
        {
            // Ignoramos huidas de momento
        }
    }

    protected void StoreResultingCreatureData(BattleOverCreatureData[] creatureBattleOverData)
    {
        List<CreatureData> afterBattleData = new List<CreatureData>();
        foreach (var battleOverCreatureData in creatureBattleOverData)
        {
            afterBattleData.Add(battleOverCreatureData.final);
        }

        // NOTE: Las criaturas pueden venir en otro órden.
        foreach (var data in afterBattleData)
        {
            data.stats.Restore();
        }

        this.humanCreatures = afterBattleData.ToArray();
    }

    protected void StoreItemRewards(ItemStack[] itemRewards)
    {
        foreach (var stack in itemRewards)
        {
            this.AddItemToInventory(stack.item, stack.amount);
        }
    }

    public void AddItemToInventory(Item item, int amount = 1)
    {
        bool shouldAddNew = true;

        foreach (var itemStack in this.inventory)
        {
            if (itemStack.item == item && itemStack.hasSpace)
            {
                itemStack.amount += amount;
                shouldAddNew = false;
            }
        }

        if (shouldAddNew)
        {
            this.inventory.Add(new ItemStack(item, amount));
        }
    }
}
