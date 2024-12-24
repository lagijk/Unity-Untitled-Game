using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class RandomWalkDungeonGenerator : AbstractDungeonGenerator
{
     [SerializeField]
    protected RandomWalkData randomWalkParameters;

    protected override void RunProceduralGeneration() {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, startPosition);
        
        // visualization for randomWalk generation
        tileMapVisual.Clear();
        tileMapVisual.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisual);
    }

    protected HashSet<Vector2Int> RunRandomWalk(RandomWalkData parameters, Vector2Int position) {
        var currentPosition = position;
        HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();

        for (int i = 0; i < parameters.iterations; i++) {
            var path = ProceduralGenerationAlgo.RandomWalk(currentPosition, parameters.walkLength);
            floorPosition.UnionWith(path); // copy position from path to floorPos hashSet
            
            if (parameters.startRandomEachIteration)
                currentPosition = floorPosition.ElementAt(Random.Range(0, floorPosition.Count));
            
        }
        return floorPosition;
    }

}
