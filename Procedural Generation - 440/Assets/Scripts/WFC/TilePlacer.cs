using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacer : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    //[SerializeField] private Tilemap mainTilemap;
    [SerializeField] private Tile selectedTile;
    private Camera _camera;

    // Update is called once per frame
    private void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _camera != null)
        {
            PlaceTile(_camera.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private void PlaceTile(Vector2 worldPos)
    {
        Vector3Int tileCoords = tileManager.GetTilemap().WorldToCell(worldPos);
        
        tileManager.GetTilemap().SetTile(tileCoords, selectedTile);
        tileManager.AddTile(tileCoords);
    }
}
