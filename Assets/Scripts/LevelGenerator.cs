using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private IntPair levelSize = new IntPair(4, 4);
    [SerializeField] private IntPair mapPieceSize = new IntPair(32, 32);
    [SerializeField, Min(1)] private int minimumPathDistance = 4;

    [Header("DEBUG")]
    [SerializeField] private int lookupRow = 0;
    [SerializeField] private int lookupColumn = 0;

    private MapPiece[,] mapPieces = default;
    private List<MapPiece> mainPath = new List<MapPiece>();
    private bool mainPathHidden = false;

    void Start()
    {
        mapPieces = new MapPiece[levelSize.y, levelSize.x];

        GameObject map = new GameObject("Map");

        for (int row = 0; row < levelSize.y; ++row)
        {
            for (int column = 0; column < levelSize.x; ++column)
            {
                GameObject mapPieceGO = new GameObject("Map Piece " + row + " " + column);
                mapPieceGO.transform.SetParent(map.transform);
                mapPieces[row,column] = mapPieceGO.AddComponent<MapPiece>();
                mapPieces[row, column].pieceName = "Map Piece " + row + " " + column;
                mapPieces[row, column].pieceSize = mapPieceSize;

                mapPieceGO.transform.position = new Vector3(column * mapPieceSize.x, -row * mapPieceSize.y, 0);
                mapPieces[row, column].DisplayTiles();
            }
        }

        for (int row = 0; row < levelSize.y; ++row)
        {
            for(int column = 0; column < levelSize.x; ++column)
            {
                if (row != 0)
                {
                    mapPieces[row, column].mapPointers.up = mapPieces[row - 1, column];
                }
                if(row != levelSize.y - 1)
                {
                    mapPieces[row, column].mapPointers.down = mapPieces[row+1, column];
                }
                if(column != 0)
                {
                    mapPieces[row, column].mapPointers.left = mapPieces[row, column - 1];

                }
                if(column != levelSize.x -1 )
                {
                    mapPieces[row, column].mapPointers.right = mapPieces[row, column + 1];
                }
            }
        }


        DetermineMainPath();
    }

    private void DetermineMainPath()
    {
        int randomRow, randomColumn;
        randomRow = Random.Range(0, levelSize.y - 1);
        randomColumn = Random.Range(0, levelSize.x - 1);
        MapPiece start = mapPieces[randomRow, randomColumn];
        start.SetEntrance();
        start.SetIsOnMainPath();
        MapPiece end = null;
        MapPiece curr = start;
        mainPath.Add(start);
        int endChecker = 20;
        while(end == null && endChecker != 0)
        {
            MapPiece temp = curr.GetRandomNextTile();
            if (mainPath.Contains(temp))
            {
                endChecker--;
                continue;
            }
            curr = temp;
            mainPath.Add(curr);
            curr.SetIsOnMainPath();
            if(mainPath.Count == minimumPathDistance)
            {
                end = curr;
                end.SetExit();
                end.SetIsOnMainPath();
            }
        }
        if(endChecker == 0)
        {
            end = mainPath[mainPath.Count];
            end.SetExit();
            end.SetIsOnMainPath();
        }
    }

    [Button()]
    public void ToggleMainPath()
    {
        for(int i = 0; i < mainPath.Count; ++i)
        {
            mainPath[i].gameObject.SetActive(!mainPath[i].gameObject.activeInHierarchy);
        }
        mainPathHidden = !mainPathHidden;
    }


    [Button()]
    public void GetMapPiecesNames()
    {
        mapPieces[lookupRow, lookupColumn].PrintPointers();
    }
}
