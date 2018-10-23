﻿using UnityEngine;

public class TileOverlay : MonoBehaviour
{
    private Vector3 currentPosition;
    private DungeonBoard dungeon;
    private Vector3 previousPosition;

    private void CheckTile()
    {
        if (currentPosition == previousPosition)
        {
            CoverTile(true, currentPosition);
        }
        else
        {
            CoverTile(false, previousPosition);
            CoverTile(true, currentPosition);

            previousPosition = currentPosition;
        }
    }

    private void CoverTile(bool cover, Vector3 position)
    {
        if (dungeon.CheckBlock(DungeonBoard.DungeonBlock.Pool, position))
        {
            dungeon.GetBlock(position).GetComponent<Renderer>().enabled
                = !cover;
        }
    }

    private void Start()
    {
        dungeon = FindObjects.GameLogic.GetComponent<DungeonBoard>();
    }

    private void Update()
    {
        currentPosition = gameObject.transform.position;
        CheckTile();
    }
}
