using UnityEngine;

public static class MapAttributes
{
    public static int IS_ENTRANCE = 1;
    public static int IS_EXIT = 1 << 1;
    public static int IS_ON_MAIN_PATH = 1 << 2;
}

[System.Serializable]
public class IntPair
{
    public int x;
    public int y;

    public IntPair(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}


public struct MapPointers
{
    public MapPiece up;
    public MapPiece down;
    public MapPiece left;
    public MapPiece right;
}

public class MapPiece : MonoBehaviour
{

    public MapPointers mapPointers = default;

    public string pieceName = "defaultMapPointer";
    public IntPair pieceSize = default;
    private int pieceAttributes = 0;

    public bool IsEntrance => (pieceAttributes & MapAttributes.IS_ENTRANCE) != 0;
    public bool IsExit => (pieceAttributes & MapAttributes.IS_EXIT) != 0;
    public bool IsOnMainPath => (pieceAttributes & MapAttributes.IS_ON_MAIN_PATH) != 0;

    public void SetEntrance() => pieceAttributes = pieceAttributes | MapAttributes.IS_ENTRANCE;
    public void SetExit() => pieceAttributes = pieceAttributes | MapAttributes.IS_EXIT;
    public void SetIsOnMainPath() => pieceAttributes = pieceAttributes | MapAttributes.IS_ON_MAIN_PATH;

    public void PrintPointers()
    {
        string output = "";
        if (mapPointers.up != null) output += "UP: " + mapPointers.up.pieceName +"\n";
        if (mapPointers.down != null) output += "DOWN: " + mapPointers.down.pieceName +"\n";
        if (mapPointers.right != null) output += "RIGHT: " + mapPointers.right.pieceName +"\n";
        if (mapPointers.left != null) output += "LEFT: " + mapPointers.left.pieceName + "\n";
        print(output);
    }

    public MapPiece GetRandomNextTile()
    {
        MapPiece output = null;

        while (output == null)
        {
            int randNumber = Random.Range(0, 3);
            switch (randNumber)
            {
                case 0:
                    output = mapPointers.up;
                    break;
                case 1:
                    output = mapPointers.down;
                    break;
                case 2:
                    output = mapPointers.left;
                    break;
                case 3:
                    output = mapPointers.right;
                    break;
            }
        }
        return output;
    }

    public void DisplayTiles()
    {
        Sprite[] tempSprites = Resources.LoadAll<Sprite>("tileSheet");
        int spriteIndex = Random.Range(0, 93);
        Sprite tempSprite = tempSprites[spriteIndex];

        Vector3 position = new Vector3(-pieceSize.x / 2, pieceSize.y / 2);
        for(int row = 0; row < pieceSize.y; ++row)
        {
            position.x = 0;
            for(int column = 0; column < pieceSize.x; ++column)
            {
                GameObject tile = new GameObject("tile " + row + column);
                tile.transform.SetParent(this.transform);
                SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
                renderer.sprite = tempSprite;
                tile.transform.localPosition = position;
                position.x += 1;
            }
            position.y -= 1;
        }
    }
}
