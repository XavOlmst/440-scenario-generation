using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class WaceFunctionCollapse : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize = new Vector2Int(10, 10);
    [SerializeField] private TileManager tileManager;
    [SerializeField] private List<TileNeighbors> allTiles;

    public static Vector3Int[] OddDirections =
    {
        new Vector3Int(0, 1), new Vector3Int(0, -1), new Vector3Int(1, 0), //top right, bottom right, right
        new Vector3Int(-1, 0), new Vector3Int(-1, 1), new Vector3Int(-1, -1) //left, top left, bottom left
    };

    public static Vector3Int[] EvenDirections =
    {
        new Vector3Int(0, 1), new Vector3Int(0, -1), new Vector3Int(1, 0), //top left, bottom left, right
        new Vector3Int(-1, 0), new Vector3Int(1, 1), new Vector3Int(1, -1) //left, top right, bottom right
    };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateMap();
        }
    }

    public void GenerateMap()
    {
        tileManager.GetTilemap().ClearAllTiles();
        
        Vector3Int startCoords = Vector3Int.zero;
        List<Vector3Int> frontier = new();
        List<Vector3Int> explored = new();
        
        if (tileManager.GetPlacedTiles().Count > 0)
        {
            startCoords = tileManager.GetPlacedTiles()[0];

        }

        Camera camera = Camera.main;

        if (!camera)
            return;
        
        frontier.Add(startCoords);
        Vector3 screenPos = camera.WorldToScreenPoint(tileManager.GetTilemap().CellToWorld(frontier[0]));
        
        while (frontier.Count > 0)
        {
            Vector3Int currentCoords = frontier[0];
            explored.Add(frontier[0]);
            frontier.Remove(frontier[0]);
            
            if (currentCoords.y % 2 == 1)
            {
                foreach (var dir in OddDirections)
                {
                    Vector3Int coords = currentCoords + dir;
                    SetTileForCoords(coords);
                    
                    if (!explored.Contains(coords) && Mathf.Abs(coords.x) < gridSize.x / 2 &&
                        Mathf.Abs(coords.y) < gridSize.y / 2)
                    {
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
                    SetTileForCoords(coords);
                    
                    if (!explored.Contains(coords) && Mathf.Abs(coords.x) < gridSize.x / 2 &&
                        Mathf.Abs(coords.y) < gridSize.y / 2)
                    {
                        frontier.Add(coords);
                        explored.Add(coords);
                    }
                }
            }
        }
    }
    
    private Tile SetTileForCoords(Vector3Int tileCoords)
    {
        List<TileNeighbors> possibleTiles = allTiles;

        if (tileCoords.y % 2 == 1) //is odd
        {
            foreach (var dir in OddDirections)
            {
                TileNeighbors tile = CheckTile(tileCoords + dir);
                if (tile)
                {
                    var tempList = GetSimilarItems(possibleTiles, tile.PossibleNeighbors);

                    if (tempList.Count == 0) //fail safe
                    {
                        tempList.AddRange(allTiles);
                    }

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
                    var tempList = GetSimilarItems(possibleTiles, tile.PossibleNeighbors);

                    if (tempList.Count == 0) //fail safe
                    {
                        tempList.AddRange(allTiles);
                    }

                    possibleTiles = tempList;
                }
            }
        }

        Tile returnTile = possibleTiles[Random.Range(0, possibleTiles.Count)];
        
        tileManager.GetTilemap().SetTile(tileCoords, returnTile);
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
