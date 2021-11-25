using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource houseSound;
    [SerializeField] private AudioSource roadSound;
    [SerializeField] private AudioSource riverSound;
    [SerializeField] private AudioSource parkSound;
    
    public static GameManager Instance { get; private set; }
    
    public TileObject[,] board { get; private set; } = new TileObject[11, 11];
    public bool[,] pattern { get; private set; }  = new bool[5, 5]; //On s'en sert pour les patterns tetris
    public Tetris currentTetris { get; private set; } = Tetris.Square;
    public Tile currentTile { get; private set; } = Tile.Empty;

    public Sprite[] tileSprites;
    [SerializeField] private Sprite[] emptySprites;
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

    public void DefaulTileSprite(TileObject tile)
    {
        tile.GetComponent<Image>().sprite = tileSprites[(int)tile.tile];
    }

    public void LoadBoardPreset(Tile[,] preset)
    {
        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y <11; y++)
            {
                board[x, y].tile = preset[x, y];
            }
        }
    }

    private void Update()
    {
        if (currentTile == Tile.Empty)
        {
            DrawCard();
        }

        var dmp = DetectMousePosition.Instance;

        if (Input.GetMouseButtonDown(0))
        {
            if(dmp.placementValidate)
            {
                switch(currentTile)
                {
                    case Tile.House:
                        houseSound.Play();
                        break;

                    case Tile.Park:
                        parkSound.Play();
                        break;

                    case Tile.River:
                        riverSound.Play();
                        break;

                    case Tile.Road:
                        roadSound.Play();
                        break;
                }

                foreach(Vector2Int pos in dmp.selectedCells)
                {
                    board[pos.x, pos.y].tile = currentTile;
                }

                if (DeckContents.Instance.DeckCount() == 0)
                {
                    //Finir la game
                }
                else
                {
                    DrawCard();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            pattern = RotatePattern(pattern);
        }

        if (Input.GetMouseButtonDown(2))
        {
            dmp.checkmodeTetris = !dmp.checkmodeTetris;

            //currentTetris = (Tetris)(((int)currentTetris + 1) % ((int)Tetris.S_Inverted+1));
            //ResetPattern(currentTetris);
        }
    }

    private void DrawCard()
    {
        currentTetris = DeckContents.Instance.GetTetris();
        currentTile = DeckContents.Instance.GetTile();
        ResetPattern(currentTetris);

        DeckContents.Instance.RefreshTetris(pattern);
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
                    bool up = false;
                    bool down = false;
                    bool left = false;
                    bool right = false;

                    if (tile.y != 0)
                    {
                        up = (board[tile.x, tile.y - 1].tile == Tile.Road);
                    }

                    if (tile.y != 10)
                    {
                        down = (board[tile.x, tile.y + 1].tile == Tile.Road);
                    }

                    if (tile.x != 0)
                    {
                        left = (board[tile.x - 1, tile.y].tile == Tile.Road);
                    }

                    if (tile.x != 10)
                    {
                        right = (board[tile.x + 1, tile.y].tile == Tile.Road);
                    }

                    if (tile.y == 0)
                    {
                        if ((left && right) == false)
                        {
                            up = true;
                        }
                    }

                    if (tile.y == 10)
                    {
                        if ((left && right) == false)
                        {
                            down = true;
                        }
                    }

                    if (tile.x == 0)
                    {
                        if ((up && down) == false)
                        {
                            left = true;
                        }
                    }

                    if (tile.x == 10)
                    {
                        if ((up && down) == false)
                        {
                            right = true;
                        }
                    }

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
                    else if (up && down)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[0];
                    }
                    else if (left && right)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[1];
                    }
                    else if (up)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[13];
                    }
                    else if (down)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[11];
                    }
                    else if (left)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[12];
                    }
                    else if (right)
                    {
                        tile.GetComponent<Image>().sprite = roadSprites[14];
                    }
                    else
                    {
                        tile.GetComponent<Image>().sprite = tileSprites[(int)tile.tile];
                    }
                }
                else if (tile.tile == Tile.River) //Tile pour les rivières
                {
                    bool up = false;
                    bool down = false;
                    bool left = false;
                    bool right = false;

                    if (tile.y != 0)
                    {
                        up = (board[tile.x, tile.y - 1].tile == Tile.River);
                    }

                    if (tile.y != 10)
                    {
                        down = (board[tile.x, tile.y + 1].tile == Tile.River);
                    }

                    if (tile.x != 0)
                    {
                        left = (board[tile.x - 1, tile.y].tile == Tile.River);
                    }

                    if (tile.x != 10)
                    {
                        right = (board[tile.x + 1, tile.y].tile == Tile.River);
                    }

                    if (tile.y == 0)
                    {
                        if ((left && right) == false)
                        {
                            up = true;
                        }
                    }

                    if (tile.y == 10)
                    {
                        if ((left && right) == false)
                        {
                            down = true;
                        }
                    }

                    if (tile.x == 0)
                    {
                        if ((up && down) == false)
                        {
                            left = true;
                        }
                    }

                    if (tile.x == 10)
                    {
                        if ((up && down) == false)
                        {
                            right = true;
                        }
                    }

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
                    else if (up && down)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[0];
                    }
                    else if (left && right)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[1];
                    }
                    else if (up)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[11];
                    }
                    else if (down)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[12];
                    }
                    else if (left)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[13];
                    }
                    else if (right)
                    {
                        tile.GetComponent<Image>().sprite = riverSprites[14];
                    }
                    else
                    {
                        tile.GetComponent<Image>().sprite = tileSprites[(int)tile.tile];
                    }
                }
                else if (tile.tile == Tile.Empty)
                {
                    if (tile.randomFlower < 0.6f)
                    {
                        tile.GetComponent<Image>().sprite = emptySprites[0];
                    }
                    else if (tile.randomFlower < 0.8f)
                    {
                        tile.GetComponent<Image>().sprite = emptySprites[1];
                    }
                    else
                    {
                        tile.GetComponent<Image>().sprite = emptySprites[2];
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

    private void calculateScore()
    {
        var brd = board;
        int finalScore = 0;

        RiverScore();

        foreach (TileObject tile in board)
        {
            finalScore += tile.getScore();
        }
    }

    private void RiverScore()
    {
        var typebrd = new Tile[11,11];
        var listRead = new List<Vector2Int>();
        var parsedLists = new List<List<Vector2Int>>();
        
        int x = 0;
        int y = 0;

        var checkVector = new Vector2Int(x, y);

        foreach (TileObject tile in board)
        {
            typebrd[tile.x, tile.y] = tile.tile;
        }

        while(listRead.Count < 121)
        {
            x = 0;
            y = 0;

            /*while (listRead.Exists())
            {

            }*/
        }

        //On part d’en haut à gauche, on check le type de la tuile
        //Si c’est de l’eau on remplace la ref board[x,y] par null
        //Si c’est pas de l’eau on ajoute la tuile à une liste puis on check toutes les tuiles adjacentes
        //Si elles sont null on fait rien
        //Si c’est de l’eau on remplace par null
        //Si c’est autre chose on ajoute la tuile à une liste puis on check toutes les tuiles adjacentes
        //Une fois qu’on a plus rien à détecter on a mappé une zone entière et on peut savoir quelles tuiles sont dedans

        //On recommence le processus à partir de la première tuile qui n’est pas null en formant une nouvelle liste
        //Quand toutes les tuiles sont null on a autant de listes que de zones de map séparées par des rivières. On regarde la liste avec le count le plus petit et on double les points de toutes les tuiles de cette liste

    }
}