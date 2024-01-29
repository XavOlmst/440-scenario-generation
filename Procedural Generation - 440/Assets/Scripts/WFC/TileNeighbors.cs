using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tile/WFC Tile")]
public class TileNeighbors : Tile
{
    public List<TileNeighbors> AlwaysPossibleNeighbors;
}
