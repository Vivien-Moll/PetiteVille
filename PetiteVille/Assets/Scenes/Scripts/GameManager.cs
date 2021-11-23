using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public TileObject[,] board { get; private set; } = new TileObject[11, 11];
    public bool[,] pattern { get; private set; }  = new bool[5, 5]; //On s'en sert pour les patterns tetris
    public Tetris currentTetris { get; private set; } = Tetris.Square;

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

    private void Start()
    {
        ResetPattern(currentTetris);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            pattern = RotatePattern(pattern);
        }

        if (Input.GetMouseButtonDown(2))
        {
            currentTetris = (Tetris)(((int)currentTetris + 1) % (int)Tetris.S_Inverted);
            ResetPattern(currentTetris);
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

    public void ClearColors()
    {
        foreach (TileObject tile in board)
        {
            if (tile != null)
            {
                if (tile.tile == Tile.Empty)
                {
                    tile.GetComponent<Image>().color = Color.white;
                }
                else
                {
                    tile.GetComponent<Image>().color = Color.gray;
                }
            }
        }
    }

    public void ResetPattern(Tetris tetris)
    {
        for (var x = 0; x < 5; x++)
        {
            for (var y = 0; y < 5; y++)
            {
                pattern[x, y] = false;
            }
        }

        switch (tetris)
        {
            case Tetris.T:
                pattern[2, 2] = true;
                pattern[1, 2] = true;
                pattern[3, 2] = true;
                pattern[2, 3] = true;
                break;

            case Tetris.L:
                pattern[2, 2] = true;
                pattern[2, 1] = true;
                pattern[2, 3] = true;
                pattern[3, 3] = true;
                break;

            case Tetris.L_Inverted:
                pattern[2, 2] = true;
                pattern[2, 1] = true;
                pattern[2, 3] = true;
                pattern[1, 3] = true;
                break;

            case Tetris.S:
                pattern[2, 2] = true;
                pattern[2, 3] = true;
                pattern[1, 3] = true;
                pattern[3, 2] = true;
                break;

            case Tetris.S_Inverted:
                pattern[2, 2] = true;
                pattern[2, 3] = true;
                pattern[3, 3] = true;
                pattern[1, 2] = true;
                break;

            case Tetris.Line:
                pattern[2, 2] = true;
                pattern[2, 3] = true;
                pattern[2, 1] = true;
                pattern[2, 0] = true;
                break;

            case Tetris.Square:
                pattern[2, 2] = true;
                pattern[2, 3] = true;
                pattern[3, 2] = true;
                pattern[3, 3] = true;
                break;
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
