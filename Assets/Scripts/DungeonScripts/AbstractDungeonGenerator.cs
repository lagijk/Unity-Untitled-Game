using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TileMapVisual tileMapVisual = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    public void GenerateDungeon() {
        tileMapVisual.Clear();
        RunProceduralGeneration();
    }

    // Method to generate tilemap based on algorithmns of our choosing
    protected abstract void RunProceduralGeneration();
}
