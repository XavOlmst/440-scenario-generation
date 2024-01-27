using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class WaceFunctionCollapse : MonoBehaviour
{
    [SerializeField] private int gridRadius = 10;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private List<TileNeighbors> allTiles;

    public static Vector3Int[] EvenDirections =
    {
        new Vector3Int(0, 1), new Vector3Int(1, 0), new Vector3Int(0, -1), //top right, right, bottom right
        new Vector3Int(-1, -1), new Vector3Int(-1, 0), new Vector3Int(-1, 1) //bottom left, left, top left, 
    };

    public static Vector3Int[] OddDirections =
    {
        new Vector3Int(0, 1), new Vector3Int(-1, 0), new Vector3Int(0, -1), //top left, left, bottom left
        new Vector3Int(1, -1), new Vector3Int(1, 0), new Vector3Int(1, 1),  //bottom right, right, top right
    };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateMap();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            tileManager.GetTilemap().ClearAllTiles();
        }
    }

    public void GenerateMap()
    {
        Vector3Int startCoords = Vector3Int.zero;
        List<Vector3Int> frontier = new();
        List<Vector3Int> explored = new();
        
        if (tileManager.GetPlacedTiles().Count > 0)
        {
            startCoords = tileManager.GetPlacedTiles()[0];
            var tempTile = tileManager.GetTilemap().GetTile(startCoords);
            tileManager.GetTilemap().ClearAllTiles();
            tileManager.GetTilemap().SetTile(startCoords, tempTile);
        }
        else
        {
            tileManager.GetTilemap().ClearAllTiles();
            tileManager.GetTilemap().SetTile(startCoords, allTiles[Random.Range(0, allTiles.Count)]);
        }
        
        Camera camera = Camera.main;

        if (!camera)
            return;
        
        frontier.Add(startCoords);
        
        while (frontier.Count > 0)
        {
            Vector3Int currentCoords = frontier[0];
            explored.Add(frontier[0]);
            frontier.Remove(frontier[0]);
            
            if (Mathf.Abs(currentCoords.y) % 2 == 1)
            {
                foreach (var dir in OddDirections)
                {
                    Vector3Int coords = currentCoords + dir;
                    
                    if (!explored.Contains(coords) && Vector3Int.Distance(coords, startCoords) < gridRadius)
                    {
                        Tile placeTile = GetTileForCoords(coords);

                        if (!placeTile)
                        {
                            placeTile = allTiles[Random.Range(0, allTiles.Count)];
                            Debug.LogWarning($"No possible tile for: {coords}");
                        }
                        
                        tileManager.GetTilemap().SetTile(coords, placeTile);
                        
                        frontier.Add(coords);
                        explored.Add(coords);
                    }
                }
            }
            else
            {
                foreach (var dir in EvenDirections)
                {
                    Vector3Int coords = currentCoords + dir;
                    
                    if (!explored.Contains(coords)  && Vector3Int.Distance(coords, startCoords) < gridRadius)
                    {
                        Tile placeTile = GetTileForCoords(coords);

                        if (!placeTile)
                        {
                            placeTile = allTiles[Random.Range(0, allTiles.Count)];
                            Debug.LogWarning($"No possible tile for: {coords}");
                        }
                        
                        tileManager.GetTilemap().SetTile(coords, placeTile);
                        
                        frontier.Add(coords);
                        explored.Add(coords);
                    }
                }
            }
        }
    }
    
    private Tile GetTileForCoords(Vector3Int tileCoords)
    {
        List<TileNeighbors> possibleTiles = allTiles;
        List<TileNeighbors> neighborTiles = new();
         
        if (Mathf.Abs(tileCoords.y) % 2 == 1)
        {
            foreach (var dir in OddDirections)
            {
                TileNeighbors tile = CheckTile(tileCoords + dir);
                if (tile)
                {
                    neighborTiles.Add(tile);
                    var tempList = GetSimilarItems(possibleTiles, tile.PossibleNeighbors);

                    possibleTiles = tempList;
                }
            }
        }
        else
        {
            foreach (var dir in EvenDirections)
            {
                TileNeighbors tile = CheckTile(tileCoords + dir);
                if (tile)
                {
                    neighborTiles.Add(tile);
                    var tempList = GetSimilarItems(possibleTiles, tile.PossibleNeighbors);

                    possibleTiles = tempList;
                }
            }
        }

        Debug.Log($"Neighbor count for {tileCoords} is {neighborTiles.Count}");
        
        if (possibleTiles.Count == 0)
            return null;
        
        Tile returnTile = possibleTiles[Random.Range(0, possibleTiles.Count)];
        
        return returnTile;
    }

    private TileNeighbors CheckTile(Vector3Int tileCoords)
    {
        return tileManager.GetTilemap().GetTile(tileCoords) as TileNeighbors;
    }

    private List<TileNeighbors> GetSimilarItems(List<TileNeighbors> listA, List<TileNeighbors> listB)
    {
        List<TileNeighbors> tempList = new();

        foreach (var item in listA)
        {
            if(listB.Contains(item))
                tempList.Add(item);
        }
        
        return tempList;
    }
}
