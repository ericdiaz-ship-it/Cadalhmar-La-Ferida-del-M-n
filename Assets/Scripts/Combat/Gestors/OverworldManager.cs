using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    public static OverworldManager current;
    public static bool isBattleActive = false;

    public CreatureProfile[] humanCreatureProfiles;
    private CreatureData[] humanCreatures;

    public int[] humanCreatureLevels;
    public List<Item> startUpItems = new List<Item>();

    [Header("Scene names")]
    public string battleSceneName = "Battle";

    [Header("Default Battle Map")]
    public TextAsset defaultBattleMapData;

    [Header("Overworld Settings")]
    public OverworldEnemySpawner enemySpawner;
    public GameObject overworldVisuals;

    private Scene currentScene;

    private TeamUI teamUI;
    private InventoryUI inventoryUI;

    private List<ItemStack> inventory;

    private EventSystem overworldEventSystem;

    private CreatureData currentEnemy;
    private BattleDescriptor currentBattleDescriptor;

    void Awake()
    {
        current = this;
        isBattleActive = false; // Siempre reseteamos al iniciar
        Debug.Log("[OWM] Awake — isBattleActive reseteado a false");

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

        this.overworldEventSystem = EventSystem.current;

        if (this.overworldEventSystem == null)
            Debug.LogError("[OWM] Awake — NO se encontró EventSystem en la escena");

        this.currentScene = SceneManager.GetActiveScene();
        Debug.Log($"[OWM] Awake — escena actual: {this.currentScene.name}");
    }

    private void PauseOverworld()
    {
        Debug.Log("[OWM] PauseOverworld");
        isBattleActive = true;

        EnemyAI[] enemies = Object.FindObjectsByType<EnemyAI>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        Debug.Log($"[OWM] Desactivando {enemies.Length} enemigos");
        foreach (var enemy in enemies)
        {
            enemy.enabled = false;
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }

        if (this.overworldVisuals != null)
            this.overworldVisuals.SetActive(false);
        else
            Debug.LogWarning("[OWM] overworldVisuals no asignado — el mapa del overworld seguirá visible");
    }

    private void ResumeOverworld()
    {
        Debug.Log("[OWM] ResumeOverworld");
        isBattleActive = false;

        EnemyAI[] enemies = Object.FindObjectsByType<EnemyAI>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            enemy.enabled = true;
        }

        if (this.overworldVisuals != null)
            this.overworldVisuals.SetActive(true);
    }

    public void ToggleTeamView()
    {
        if (this.inventoryUI.isVisible)
            this.inventoryUI.Hide();

        this.teamUI.ToggleDisplay(this.humanCreatures);
    }

    public void ToggleInventoryView()
    {
        if (this.teamUI.isVisible)
            this.teamUI.Hide();

        this.inventoryUI.ToggleDisplay(this.inventory, this.humanCreatures);
    }

    private IEnumerator LoadBattle()
    {
        Debug.Log($"[OWM] LoadBattle — cargando escena: {this.battleSceneName}");

        this.PauseOverworld();

        AsyncOperation operation = SceneManager.LoadSceneAsync(this.battleSceneName, LoadSceneMode.Additive);
        if (operation == null)
        {
            Debug.LogError($"[OWM] LoadBattle — NO se pudo cargar la escena '{this.battleSceneName}'. " +
                           "¿Está añadida en File > Build Settings > Scenes In Build?");
            isBattleActive = false;
            yield break;
        }

        while (operation.isDone == false)
        {
            yield return null;
        }

        Debug.Log("[OWM] LoadBattle — escena Battle cargada");

        Scene battleScene = SceneManager.GetSceneByName(this.battleSceneName);
        SceneManager.SetActiveScene(battleScene);

        BattleDescriptor descriptor = new BattleDescriptor();
        descriptor.humanCreatures = this.humanCreatures;
        descriptor.aiCreatures = new CreatureData[] { this.currentEnemy };

        if (this.defaultBattleMapData != null)
        {
            descriptor.mapStringData = this.defaultBattleMapData.text;
            Debug.Log($"[OWM] Mapa asignado: {this.defaultBattleMapData.name}");
        }
        else
        {
            Debug.LogError("[OWM] defaultBattleMapData es NULL — asigna un .txt de mapa en el Inspector del OverworldManager");
        }

        descriptor.Validate();

        if (BattleManager.current == null)
        {
            Debug.LogError("[OWM] BattleManager.current es NULL — ¿existe BattleManager en la escena Battle?");
            yield break;
        }

        Debug.Log("[OWM] Iniciando batalla...");
        BattleManager.current.StartBattle(descriptor);
        this.gameObject.SetActive(false);
    }

    private IEnumerator LoadBattleWithDescriptor()
    {
        Debug.Log("[OWM] LoadBattleWithDescriptor");

        this.PauseOverworld();

        AsyncOperation operation = SceneManager.LoadSceneAsync(this.battleSceneName, LoadSceneMode.Additive);
        if (operation == null)
        {
            Debug.LogError($"[OWM] NO se pudo cargar '{this.battleSceneName}'. ¿Está en Build Settings?");
            isBattleActive = false;
            yield break;
        }

        while (operation.isDone == false)
        {
            yield return null;
        }

        Scene battleScene = SceneManager.GetSceneByName(this.battleSceneName);
        SceneManager.SetActiveScene(battleScene);

        BattleManager.current.StartBattle(this.currentBattleDescriptor);
        this.gameObject.SetActive(false);
    }

    public void StartBattleWithEnemy(CreatureData enemy)
    {
        Debug.Log($"[OWM] StartBattleWithEnemy — isBattleActive: {isBattleActive}");

        if (isBattleActive)
        {
            Debug.LogWarning("[OWM] Batalla ya activa, ignorando llamada");
            return;
        }

        this.currentEnemy = enemy;
        this.gameObject.SetActive(true);
        StartCoroutine(this.LoadBattle());
    }

    public void StartBattle(BattleDescriptor descriptor)
    {
        if (isBattleActive) return;

        this.currentBattleDescriptor = descriptor;
        this.currentBattleDescriptor.humanCreatures = this.humanCreatures;
        this.currentBattleDescriptor.Validate();

        this.gameObject.SetActive(true);
        StartCoroutine(this.LoadBattleWithDescriptor());
    }

    public void EndBattle(BattleOverMessage message)
    {
        Debug.Log("[OWM] EndBattle");

        SceneManager.UnloadSceneAsync(this.battleSceneName);

        SceneManager.SetActiveScene(this.currentScene);
        this.gameObject.SetActive(true);

        this.overworldEventSystem.enabled = true;
        this.ResumeOverworld();

        this.StoreResultingCreatureData(message.creatureBattleOverData.ToArray());
        this.StoreItemRewards(message.itemRewards.ToArray());

        if (this.currentBattleDescriptor != null)
        {
            if (message.isHumanWin)
                this.currentBattleDescriptor.onHumanWin();
            else if (message.isHumanLoss)
                this.currentBattleDescriptor.onHumanLoss();
        }
    }

    protected void StoreResultingCreatureData(BattleOverCreatureData[] creatureBattleOverData)
    {
        List<CreatureData> afterBattleData = new List<CreatureData>();
        foreach (var battleOverCreatureData in creatureBattleOverData)
        {
            afterBattleData.Add(battleOverCreatureData.final);
        }

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