using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextAsset mapData;
    public GameObject creaturePrfb;

    void Start()
    {
        Map map = Map.CreateWithStringData(this.mapData.text);
        MapDisplay display = GameObject.FindObjectOfType<MapDisplay>();
        display.RenderMapData(map);
        this.SpawnRandomCreature(3,map, display);
    }

    private void SpawnRandomCreature(int count, Map map, MapDisplay display)
    {
        int spawnedCount = 0;
        int attemps = 0;
        List<Vector2Int> usedPositions = new List<Vector2Int>();
        while(spawnedCount < count && attemps < 100)
        {
            attemps++;
            int x = Random.Range(0, map.width);
            int y = Random.Range(0, map.height);

            if(map.GetTileType(x,y) != TileType.GROUND)
            {
                continue;
            }
            bool positionUsed = false;
            foreach(var pos in usedPositions)
            {
                if(pos.x == x && pos.y == y)
                {
                    positionUsed = true;
                    break;
                }
            }
            if(positionUsed)
            {
                continue;
            }
            Vector2Int spawnPoint = new Vector2Int(x,y);
            usedPositions.Add(spawnPoint);
            GameObject go = Instantiate(this.creaturePrfb);
            Creature creature = go.GetComponent<Creature>();
            display.EmplaceCreature(creature, spawnPoint);
            spawnedCount++;
        }
    }
}