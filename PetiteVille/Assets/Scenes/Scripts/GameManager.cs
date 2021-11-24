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

    [SerializeField] private Sprite[] tileSprites;
    [SerializeField] private Sprite[] roadSprites;
    [SerializeField] private Sprite[] riverSprites;

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
            currentTetris = (Tetris)(((int)currentTetris + 1) % ((int)Tetris.S_Inverted+1));
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

    public void UpdateTiles()
    {
        foreach (TileObject tile in board)
        {
            if (tile != null)
            {
                if (tile.tile == Tile.Road) //Tile pour les routes
                {
                    var up = (board[tile.x,tile.y-1].tile == Tile.Road);
                    var down = (board[tile.x,tile.y+1].tile == Tile.Road);
                    var left = (board[tile.x-1,tile.y].tile == Tile.Road);
                    var right = (board[tile.x+1,tile.y].tile == Tile.Road);

                    if (up && down && left && right)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[10];
                    }
                    else if (up && down && left)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[7];
                    }
                    else if (up && down && right)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[9];
                    }
                    else if (up && left && right)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[8];
                    }
                    else if (down && left && right)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[6];
                    }
                    else if (up && right)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[2];
                    }
                    else if (up && left)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[5];
                    }
                    else if (down && right)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[3];
                    }
                    else if (down && left)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[4];
                    }
                    else if (up || down)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[0];
                    }
                    else if (left || right)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[1];
                    }
                    else
                    {
                        tile.GetComponent<Image>().sprite = tileSprites[(int)tile.tile];
                    }
                }
                else if (tile.tile == Tile.River) //Tile pour les rivi�res
                {
                    var up = (board[tile.x, tile.y - 1].tile == Tile.River);
                    var down = (board[tile.x, tile.y + 1].tile == Tile.River);
                    var left = (board[tile.x - 1, tile.y].tile == Tile.River);
                    var right = (board[tile.x + 1, tile.y].tile == Tile.River);

                    if (up && down && left && right)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[10];
                    }
                    else if (up && down && left)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[7];
                    }
                    else if (up && down && right)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[9];
                    }
                    else if (up && left && right)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[8];
                    }
                    else if (down && left && right)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[6];
                    }
                    else if (up && right)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[2];
                    }
                    else if (up && left)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[5];
                    }
                    else if (down && right)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[3];
                    }
                    else if (down && left)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[4];
                    }
                    else if (up || down)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[0];
                    }
                    else if (left || right)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[1];
                    }
                    else
                    {
                        tile.GetComponent<Image>().sprite = tileSprites[(int)tile.tile];
                    }
                }
                else
                {
                    tile.GetComponent<Image>().sprite = tileSprites[(int)tile.tile];
                }
            }
        }
    }

    public void ClearColors()
    {
        foreach (TileObject tile in board)
        {
            if (tile != null)
            {
                tile.GetComponent<Image>().color = Color.white;
                /*switch (tile.tile)
                {
                    case Tile.Empty:
                        tile.GetComponent<Image>().color = Color.white;
                        break;

                    case Tile.Factory:
                        tile.GetComponent<Image>().color = Color.gray;
                        break;

                    case Tile.Road:
                        tile.GetComponent<Image>().color = Color.black;
                        break;

                    case Tile.River:
                        tile.GetComponent<Image>().color = Color.blue;
                        break;

                    case Tile.House:
                        tile.GetComponent<Image>().color = Color.red;
                        break;

                    case Tile.Park:
                        tile.GetComponent<Image>().color = Color.green;
                        break;
                }*/
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
