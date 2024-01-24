using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap mainTilemap;
    private List<Vector3Int> placedTileCoords = new();

    public void AddTile(Vector3Int coords) => placedTileCoords.Add(coords);
    public Tilemap GetTilemap() => mainTilemap;
    public List<Vector3Int> GetPlacedTiles() => placedTileCoords;
}
