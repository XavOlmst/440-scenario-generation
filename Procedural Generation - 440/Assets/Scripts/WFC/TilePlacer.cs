using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacer : MonoBehaviour
{
    [SerializeField] private Tilemap mainTilemap;
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
        Vector3Int tileCoords = mainTilemap.WorldToCell(worldPos);
        
        mainTilemap.SetTile(tileCoords, selectedTile);
    }
}
