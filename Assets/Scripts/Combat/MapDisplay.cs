using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public struct MapTilePair
{
    public TileType type;
    public Tile visualTile;
}
public class MapDisplay : MonoBehaviour
{
    public MapTilePair[] mapTilePairs;
    public Tilemap targetTilemap;

    private Map map;
    public Transform cursor;
    public Camera gameCamera;
    private Creature selectedCreature;
    private List<Creature> creatures;
    void Start()
    {
        this.creatures = new List<Creature>();
    }

    public void RenderMapData(Map mapdata)
    {
        this.map = mapdata;
        this.targetTilemap.ClearAllTiles();

        for(int x = 0; x < this.map.width; x++)
        {
            for(int y = 0; y < this.map.height; y++)
            {
                TileType type = this.map.GetTileType(x,y);
                Tile tile = this.GetTileForType(type);

                // Map already bottom-left origin, no inversión necesaria
                this.targetTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
        // Centrar la cámara después de renderizar
        CenterCamera();
    }

    public void EmplaceCreature(Creature creature, Vector2Int localPosition)
    {
        Vector3 worldPosition = this.LocalToWorld(localPosition);
        creature.localPosition = localPosition;
        creature.transform.position = worldPosition;
        creature.transform.localScale = Vector3.one; // asegurar escala uniforme
        this.creatures.Add(creature);
    }

    private void CenterCamera()
    {
        // Asegurar que Tilemap está en el origen y sin transformación extra
        this.transform.position = Vector3.zero;
        this.targetTilemap.transform.position = Vector3.zero;
        this.targetTilemap.transform.rotation = Quaternion.identity;
        this.targetTilemap.transform.localScale = Vector3.one;
        if (this.targetTilemap.layoutGrid != null)
        {
            this.targetTilemap.layoutGrid.cellGap = Vector3.zero;
        }

        // Rango de celdas según el map renderizado (0..width-1, 0..height-1)
        Vector3Int minCell = new Vector3Int(0, 0, 0);
        Vector3Int maxCell = new Vector3Int(this.map.width - 1, this.map.height - 1, 0);

        Vector3 minWorld = this.targetTilemap.GetCellCenterWorld(minCell);
        Vector3 maxWorld = this.targetTilemap.GetCellCenterWorld(maxCell);

        Vector3 centerWorld = (minWorld + maxWorld) * 0.5f;

        // Posicionar la cámara en el centro del mapa
        this.gameCamera.transform.position = new Vector3(centerWorld.x, centerWorld.y, this.gameCamera.transform.position.z);
    }

    private Tile GetTileForType(TileType type)
    {
        foreach(var pair in this.mapTilePairs)
        {
            if(pair.type == type)
            {
                return pair.visualTile;
            }
        }
        UnityEngine.Debug.LogError("No hay tile para: " + type);
        return null;
    }
    
    private Vector2Int WorldToLocal(Vector3 world)
    {
        // Usar el tilemap para convertir de coordenadas mundiales a de tile
        Vector3Int cellPos = this.targetTilemap.WorldToCell(world);
        return new Vector2Int(cellPos.x, cellPos.y);
    }
    
    private Vector3 LocalToWorld(Vector2Int local)
    {
        // Convertir de coordenadas de tile a mundiales usando el tilemap
        Vector3Int cellPos = new Vector3Int(local.x, local.y, 0);
        return this.targetTilemap.GetCellCenterWorld(cellPos);
    }
    
    private bool IsValidMapPosition(Vector2Int local)
    {
        return local.x >= 0 && local.x < this.map.width && local.y >= 0 && local.y < this.map.height;
    }
    void Update()
    {
        // Desactivar cursor por defecto
        this.cursor.gameObject.SetActive(false);
        
        // Verificar si el ratón está dentro del viewport de la cámara
        Vector3 mouseScreenPos = Input.mousePosition;
        
        // Validar que la posición del ratón está dentro del rectángulo de la cámara
        if(mouseScreenPos.x < 0 || mouseScreenPos.x > this.gameCamera.pixelWidth ||
           mouseScreenPos.y < 0 || mouseScreenPos.y > this.gameCamera.pixelHeight)
        {
            return;
        }
        
        try
        {
            // Establecer la coordenada Z para la conversión (distancia de la cámara al plano del mapa)
            mouseScreenPos.z = Mathf.Abs(this.gameCamera.transform.position.z);
            
            // Convertir posición de pantalla a coordenadas mundiales
            Vector3 world = this.gameCamera.ScreenToWorldPoint(mouseScreenPos);
            var local = this.WorldToLocal(world);
            
            // Validar que la posición está dentro del mapa
            if(!IsValidMapPosition(local))
            {
                return;
            }
            
            // Mostrar cursor solo si es GROUND
            if(this.map.GetTileType(local.x, local.y) == TileType.GROUND)
            {
                this.cursor.gameObject.SetActive(true);
                this.cursor.position = this.LocalToWorld(local);
            }
        }
        catch(System.Exception ex)
        {
            UnityEngine.Debug.LogWarning("Error al actualizar cursor: " + ex.Message);
            this.cursor.gameObject.SetActive(false);
        }
    }
}