using System.IO;
using System.Collections.Generic;
using System.IO.Compression;

public enum TileType
{
    WALL,
    GROUND,
}
public class Map
{
    public int width { get; protected set; }
    public int height { get; protected set; }

    private TileType[,] tilesData;

    protected Map(TileType[,] tilesData)
    {
        this.tilesData = tilesData;
        this.width = tilesData.GetLength(0);
        this.height = tilesData.GetLength(1);
    }

    public TileType GetTileType(int x, int y)
    {
        if(x<0 || y<0)
        {
            return TileType.WALL;
        }
        if(x>= this.width || y>= this.height)
        {
            return TileType.WALL;
        }
        return tilesData[x, y]; 
    }

    public static Map CreateWithStringData(string mapData)
    {
        StringReader reader = new StringReader(mapData);

        int mapWidth = 0;
        int mapHeight = 0;

        List<TileType> flatTilesData = new List<TileType>();

        while(true)
        {
            string line = reader.ReadLine();
            if(line == null)
            {
                break;
            }
            line = line.Trim();

            if(line.Length ==0)
            {
                continue;
            }

            mapWidth = line.Length;
            mapHeight++;

            foreach(var letter in line)
            {
                switch(letter)
                {
                    case '#':
                        flatTilesData.Add(TileType.WALL);
                        break;
                    case '.':
                        flatTilesData.Add(TileType.GROUND);
                        break;
                    default:
                        throw new System.Exception("Invalid character in map data: " + letter);
                }
            }
        }
        TileType[,] finalMapTiles = new TileType[mapWidth, mapHeight];
        for(int x = 0; x < mapWidth; x++)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                // Convertir al sistema de coordenadas bottom-left (y=0 en la base)
                finalMapTiles[x, y] = flatTilesData[(mapHeight - 1 - y) * mapWidth + x];
            }
        }
        return new Map(finalMapTiles);
    }
}