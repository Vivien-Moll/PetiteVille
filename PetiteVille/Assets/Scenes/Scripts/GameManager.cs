using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private TileObject[,] board = new TileObject[11, 11];
    private bool[,] pattern = new bool[5, 5]; //On s'en sert pour les patterns tetris

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    public void AddTile(int x, int y, TileObject tile)
    {
        if (board[x,y] == null)
        {
            board[x,y] = tile;
        }
        else
        {
            Debug.LogError(x.ToString() +" " +y.ToString() + " - Trying to assign a tile to a filled index");
        }
    }

    private bool[,] RotatePattern(bool[,] tabl)
    {
        var newtabl = new bool[5, 5]; //X = Y; Y = -X;

        for (var x = 0; x < 5; x++)
        {
            for (var y = 0; y < 5; y++)
            {
                var val = tabl[x, y];

                var _x = -(y - 2) + 2;
                var _y = -( - (x - 2) - 2);

                newtabl[_x, _y] = val;
            }
        }

        return newtabl;
    }
}
