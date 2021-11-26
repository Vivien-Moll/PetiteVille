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

    private int roadScore = 10;
    private int factoryBase = 100;
    private int factoryPenalty = -25;
    private int parkRiverScore = 10;
    private int parkBiggestScore = 10;
    private int houseNearScore = 10;

    private bool isGameOver = false;
    
    public static GameManager Instance { get; private set; }
    
    public TileObject[,] board { get; private set; } = new TileObject[11, 11];
    public bool[,] pattern { get; private set; }  = new bool[5, 5]; //On s'en sert pour les patterns tetris
    public Tetris currentTetris { get; private set; } = Tetris.Square;
    public Tile currentTile { get; private set; } = Tile.Empty;

    public Sprite[] tileSprites;
    [SerializeField] private Sprite[] emptySprites;
    [SerializeField] private Sprite[] roadSprites;
    [SerializeField] private Sprite[] riverSprites;
    [SerializeField] private GameObject deckContentsScript;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private Text currentScore;

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
        isGameOver = false;
    }

    public void DefaulTileSprite(TileObject tile)
    {
        if (tile.tile == Tile.Road)
        {
            tile.GetComponent<Image>().sprite = roadSprites[0];
        }
        else
        {
            tile.GetComponent<Image>().sprite = tileSprites[(int)tile.tile];
        }
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
        currentScore.text = "Current score : " + CalculateScore()[5];

        if (endScreen.activeSelf)
            return;

        if (!isGameOver)
        {
            if (currentTile == Tile.Empty)
            {
                if (GameObject.FindGameObjectWithTag("persistant") != null)
                {
                    LoadBoardPreset(dontDestroy.gameData);
                }
                DrawCard();
            }

            var dmp = DetectMousePosition.Instance;

            if (Input.GetMouseButtonDown(0))
            {
                if (dmp.placementValidate)
                {
                    switch (currentTile)
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

                    foreach (Vector2Int pos in dmp.selectedCells)
                    {
                        board[pos.x, pos.y].tile = currentTile;
                    }

                    currentScore.text = "Current score : " + CalculateScore()[5];

                    if (DeckContents.Instance.DeckCount() == 0)
                    {
                        isGameOver = true;
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
        else
        {
            deckContentsScript.SetActive(false);
            endScreen.SetActive(true);
            int[] endGameScore = CalculateScore();
            
            endScreen.transform.Find("endTxt").Find("House").Find("score").GetComponent<Text>().text = ": " + endGameScore[0] +" points.";
            endScreen.transform.Find("endTxt").Find("Road").Find("score").GetComponent<Text>().text = ": " + endGameScore[1] + " points.";
            endScreen.transform.Find("endTxt").Find("River").Find("score").GetComponent<Text>().text = ": " + endGameScore[2] + " points.";
            endScreen.transform.Find("endTxt").Find("Park").Find("score").GetComponent<Text>().text = ": " + endGameScore[3] + " points.";
            endScreen.transform.Find("endTxt").Find("Factory").Find("score").GetComponent<Text>().text = ": " + endGameScore[4] + " points.";
            endScreen.transform.Find("totalScoreInt").GetComponent<Text>().text = endGameScore[5].ToString();

        }
        
    }

    public int[] CalculateScore()
    {

        foreach (TileObject cell in board)
        {
            cell.score = 0;
            cell.coeff = 1;
        }
        //0 maison
        //1 route
        //2 rivière
        //3 parc
        //4 usine
        //5 total

        var result = new int[6];

        var sHouse = 0;
        var sRoad = 0;
        var sRiver = 0;
        var sPark = 0;
        var sFactory = 0;
        var sTotal = 0;

        RiverScore();

        RoadScore();

        FactoryScore();

        ParkRiverScore();

        HouseNearScore();

        ParkBiggestScore();

        foreach(TileObject cell in board)
        {
            switch(cell.tile)
            {
                case Tile.Empty://Jolie paquerette
                    break;

                case Tile.House:
                    sHouse += cell.GetRawScore();
                    break;

                case Tile.Road:
                    sRoad += cell.GetRawScore();
                    break;

                case Tile.River://Non pas ici
                    break;

                case Tile.Park:
                    sPark += cell.GetRawScore();
                    break;

                case Tile.Factory:
                    sFactory += cell.GetRawScore();
                    break;

                case Tile.Mountain://ptdrlol
                    break;
            }

            sRiver += cell.GetScore() - cell.GetRawScore();
            sTotal += cell.GetScore();
        }
        //0 maison
        //1 route
        //2 rivière
        //3 parc
        //4 usine
        //5 total
        result[0] = sHouse;
        result[1] = sRoad;
        result[2] = sRiver;
        result[3] = sPark;
        result[4] = sFactory;
        result[5] = sTotal;

        return result;
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

    private void RiverScore()//Tuez moi
    {
        //On part d’en haut à gauche, on check le type de la tuile
        //Si c’est de l’eau on remplace la ref board[x,y] par null
        //Si c’est pas de l’eau on ajoute la tuile à une liste puis on check toutes les tuiles adjacentes
        //Si elles sont null on fait rien
        //Si c’est de l’eau on remplace par null
        //Si c’est autre chose on ajoute la tuile à une liste puis on check toutes les tuiles adjacentes
        //Une fois qu’on a plus rien à détecter on a mappé une zone entière et on peut savoir quelles tuiles sont dedans

        //On recommence le processus à partir de la première tuile qui n’est pas null en formant une nouvelle liste
        //Quand toutes les tuiles sont null on a autant de listes que de zones de map séparées par des rivières. On regarde la liste avec le count le plus petit et on double les points de toutes les tuiles de cette liste

        var typebrd = new Tile[11,11];
        var listRead = new List<Vector2Int>();
        var parsedLists = new List<List<Vector2Int>>();

        var checkVector = new Vector2Int(0, 0);

        foreach (TileObject tile in board)
        {
            typebrd[tile.x, tile.y] = tile.tile;
        }

        while(listRead.Count < 121) //Tant qu'il reste des éléments à lister
        {
            checkVector = new Vector2Int(0, 0);

            while (listRead.Contains(checkVector)) //Snap au premier indice non-lu
            {
                if (checkVector.x == 10)
                {
                    if (checkVector.y == 10)
                    {
                        Debug.LogError("PROBLEM");
                        break;
                    }
                    else
                    {
                        checkVector.x = 0;
                        checkVector.y++;
                    }
                }
                else
                {
                    checkVector.x++;
                }
            }

            if (board[checkVector.x, checkVector.y].tile == Tile.River) //Si ce premier indice est une rivière on marque comme lu et on recommence
            {
                listRead.Add(checkVector);
            }
            else //Sinon on check les environs et on crée une "ile"
            {
                var toCheck = new Queue<Vector2Int>();

                toCheck.Enqueue(checkVector);
                
                parsedLists.Add(new List<Vector2Int>());

                while (toCheck.Count > 0)
                {
                    var current = toCheck.Dequeue();

                    if (board[current.x,current.y].tile == Tile.River)
                    {
                        listRead.Add(current);
                        continue;
                    }
                    else
                    {
                        listRead.Add(current);
                        parsedLists[parsedLists.Count - 1].Add(current);
                    }

                    var adj = new List<Vector2Int>();

                    if (current.x == 0)
                    {
                        adj.Add(new Vector2Int(current.x + 1, current.y));
                        if (current.y == 0)
                        {
                            adj.Add(new Vector2Int(current.x, current.y+1));
                            adj.Add(new Vector2Int(current.x + 1, current.y+1));
                        }
                        else if (current.y == 10)
                        {
                            adj.Add(new Vector2Int(current.x, current.y - 1));
                            adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                        }
                        else
                        {
                            adj.Add(new Vector2Int(current.x, current.y + 1));
                            adj.Add(new Vector2Int(current.x + 1, current.y + 1));
                            adj.Add(new Vector2Int(current.x, current.y - 1));
                            adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                        }
                    }
                    else if (current.x == 10)
                    {
                        adj.Add(new Vector2Int(current.x - 1, current.y));
                        if (current.y == 0)
                        {
                            adj.Add(new Vector2Int(current.x, current.y + 1));
                            adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                        }
                        else if (current.y == 10)
                        {
                            adj.Add(new Vector2Int(current.x, current.y - 1));
                            adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                        }
                        else
                        {
                            adj.Add(new Vector2Int(current.x, current.y + 1));
                            adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                            adj.Add(new Vector2Int(current.x, current.y - 1));
                            adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                        }
                    }
                    else
                    {
                        if (current.y == 0)
                        {
                            adj.Add(new Vector2Int(current.x-1, current.y));
                            adj.Add(new Vector2Int(current.x + 1, current.y));
                            adj.Add(new Vector2Int(current.x-1, current.y + 1));
                            adj.Add(new Vector2Int(current.x, current.y + 1));
                            adj.Add(new Vector2Int(current.x+1, current.y + 1));

                        }
                        else if (current.y == 10)
                        {
                            adj.Add(new Vector2Int(current.x - 1, current.y));
                            adj.Add(new Vector2Int(current.x + 1, current.y));
                            adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                            adj.Add(new Vector2Int(current.x, current.y - 1));
                            adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                        }
                        else
                        {
                            adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                            adj.Add(new Vector2Int(current.x, current.y - 1));
                            adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                            adj.Add(new Vector2Int(current.x - 1, current.y));
                            adj.Add(new Vector2Int(current.x + 1, current.y));
                            adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                            adj.Add(new Vector2Int(current.x, current.y + 1));
                            adj.Add(new Vector2Int(current.x + 1, current.y + 1));
                        }
                    }

                    foreach(Vector2Int vec in adj)
                    {
                        if ((!toCheck.Contains(vec)) && (!listRead.Contains(vec)))
                        {
                            toCheck.Enqueue(vec);
                        }
                    }
                }
            }
        }

        if (parsedLists.Count == 1) //La rivière n'est pas connectée
        {
            return;
        }

        //Et là on trouve la plus petite liste sapristi

        var mincount = 999;
        var minlist = new List<Vector2Int>();

        foreach(List<Vector2Int> l in parsedLists)
        {
            if (l.Count < mincount)
            {
                mincount = l.Count;
                minlist = l;
            }
        }

        foreach(Vector2Int cell in minlist)
        {
            board[cell.x, cell.y].coeff = 2;
        }
    }

    private void RoadScore()
    {
        //On récupère ses coordonnées
        //On récupère la ref de chaque tuile adjacente
        //On ajoute X pour chaque maison et usine

        var scor = 0;

        foreach(TileObject tile in board)
        {
            scor = 0;

            if (tile.tile == Tile.Road)
            {
                var type = Tile.Empty;

                if (tile.x-1 >= 0)
                {
                    type = board[tile.x - 1, tile.y].tile;
                    if ((type == Tile.House) || (type == Tile.Factory))
                    {
                        scor++;
                    }
                }

                if (tile.x + 1 <= 10)
                {
                    type = board[tile.x + 1, tile.y].tile;
                    if ((type == Tile.House) || (type == Tile.Factory))
                    {
                        scor++;
                    }
                }

                if (tile.y - 1 >= 0)
                {
                    type = board[tile.x, tile.y-1].tile;
                    if ((type == Tile.House) || (type == Tile.Factory))
                    {
                        scor++;
                    }
                }

                if (tile.y + 1 <= 10)
                {
                    type = board[tile.x, tile.y + 1].tile;
                    if ((type == Tile.House) || (type == Tile.Factory))
                    {
                        scor++;
                    }
                }

                tile.score += scor * roadScore;
            }
        }
    }

    private void FactoryScore()
    {
        //On récupère ses coordonnées
        //On récupère la ref de chaque tuile adjacente
        //On ajoute Y pour chaque rivière et parc, Y est négatif

        var scor = factoryBase;
        var current = new Vector2Int(0, 0);
        var adj = new List<Vector2Int>();

        foreach (TileObject tile in board)
        {
            if (tile.tile == Tile.Factory)
            {
                scor = factoryBase;
                current = new Vector2Int(tile.x, tile.y);
                adj = new List<Vector2Int>();

                if (current.x == 0)
                {
                    adj.Add(new Vector2Int(current.x + 1, current.y));
                    if (current.y == 0)
                    {
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y + 1));
                    }
                    else if (current.y == 10)
                    {
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                    }
                    else
                    {
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y + 1));
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                    }
                }
                else if (current.x == 10)
                {
                    adj.Add(new Vector2Int(current.x - 1, current.y));
                    if (current.y == 0)
                    {
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                    }
                    else if (current.y == 10)
                    {
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                    }
                    else
                    {
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                    }
                }
                else
                {
                    if (current.y == 0)
                    {
                        adj.Add(new Vector2Int(current.x - 1, current.y));
                        adj.Add(new Vector2Int(current.x + 1, current.y));
                        adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y + 1));

                    }
                    else if (current.y == 10)
                    {
                        adj.Add(new Vector2Int(current.x - 1, current.y));
                        adj.Add(new Vector2Int(current.x + 1, current.y));
                        adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                    }
                    else
                    {
                        adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y));
                        adj.Add(new Vector2Int(current.x + 1, current.y));
                        adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y + 1));
                    }
                }

                foreach (Vector2Int vec in adj)
                {
                    if ((board[vec.x,vec.y].tile == Tile.River) || (board[vec.x, vec.y].tile == Tile.Park))
                    {
                        scor += factoryPenalty;
                    }
                }
                tile.score += scor;
            }
        }
    }

    private void ParkRiverScore()
    {
        //On récupère ses coordonnées
        //On récupère la ref de chaque tuile adjacente
        //On ajoute X pour chaque rivière

        var scor = 0;
        var current = new Vector2Int(0, 0);
        var adj = new List<Vector2Int>();

        foreach (TileObject tile in board)
        {
            if (tile.tile == Tile.Park)
            {
                scor = 0;
                current = new Vector2Int(tile.x, tile.y);
                adj = new List<Vector2Int>();

                if (current.x == 0)
                {
                    adj.Add(new Vector2Int(current.x + 1, current.y));
                    if (current.y == 0)
                    {
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y + 1));
                    }
                    else if (current.y == 10)
                    {
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                    }
                    else
                    {
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y + 1));
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                    }
                }
                else if (current.x == 10)
                {
                    adj.Add(new Vector2Int(current.x - 1, current.y));
                    if (current.y == 0)
                    {
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                    }
                    else if (current.y == 10)
                    {
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                    }
                    else
                    {
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                    }
                }
                else
                {
                    if (current.y == 0)
                    {
                        adj.Add(new Vector2Int(current.x - 1, current.y));
                        adj.Add(new Vector2Int(current.x + 1, current.y));
                        adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y + 1));

                    }
                    else if (current.y == 10)
                    {
                        adj.Add(new Vector2Int(current.x - 1, current.y));
                        adj.Add(new Vector2Int(current.x + 1, current.y));
                        adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                    }
                    else
                    {
                        adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y));
                        adj.Add(new Vector2Int(current.x + 1, current.y));
                        adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y + 1));
                    }
                }

                foreach (Vector2Int vec in adj)
                {
                    if (board[vec.x, vec.y].tile == Tile.River)
                    {
                        scor += 1;
                    }
                }
                tile.score += scor*parkRiverScore;
            }
        }
    }

    private void HouseNearScore()
    {
        //On récupère ses coordonnées
        //On récupère la ref de chaque tuile adjacente
        //On ajoute X pour chaque rivière

        var scor = 0;
        var current = new Vector2Int(0, 0);
        var adj = new List<Vector2Int>();

        foreach (TileObject tile in board)
        {
            if (tile.tile == Tile.House)
            {
                scor = 0;
                current = new Vector2Int(tile.x, tile.y);
                adj = new List<Vector2Int>();

                if (current.x == 0)
                {
                    adj.Add(new Vector2Int(current.x + 1, current.y));
                    if (current.y == 0)
                    {
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y + 1));
                    }
                    else if (current.y == 10)
                    {
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                    }
                    else
                    {
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y + 1));
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                    }
                }
                else if (current.x == 10)
                {
                    adj.Add(new Vector2Int(current.x - 1, current.y));
                    if (current.y == 0)
                    {
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                    }
                    else if (current.y == 10)
                    {
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                    }
                    else
                    {
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                    }
                }
                else
                {
                    if (current.y == 0)
                    {
                        adj.Add(new Vector2Int(current.x - 1, current.y));
                        adj.Add(new Vector2Int(current.x + 1, current.y));
                        adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y + 1));

                    }
                    else if (current.y == 10)
                    {
                        adj.Add(new Vector2Int(current.x - 1, current.y));
                        adj.Add(new Vector2Int(current.x + 1, current.y));
                        adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                    }
                    else
                    {
                        adj.Add(new Vector2Int(current.x - 1, current.y - 1));
                        adj.Add(new Vector2Int(current.x, current.y - 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y - 1));
                        adj.Add(new Vector2Int(current.x - 1, current.y));
                        adj.Add(new Vector2Int(current.x + 1, current.y));
                        adj.Add(new Vector2Int(current.x - 1, current.y + 1));
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                        adj.Add(new Vector2Int(current.x + 1, current.y + 1));
                    }
                }

                foreach (Vector2Int vec in adj)
                {
                    if ((board[vec.x, vec.y].tile != Tile.House) && (board[vec.x, vec.y].tile != Tile.Empty))
                    {
                        scor += 1;
                    }
                }
                tile.score += scor* houseNearScore;
            }
        }
    }

    private void ParkBiggestScore()
    {
        //On crée un compteur général et un compteur local (type int, = 0)
        //On part du premier parc de la liste, on le retire de cette liste et on ajoute 1 au compteur local
        //On check chaque tuile adjacente, si on trouve un parc on le retire, on ajoute 1 au local et on check les tuiles adjacentes etc…
        //Ensuite on regarde si local > général, si oui général = local

        //On recommence la boucle avec le prochain parc restant dans la liste

        //Quand la liste est vide on ajoute X * général points

        var typebrd = new Tile[11, 11];
        var listRead = new List<Vector2Int>();
        var parsedLists = new List<List<Vector2Int>>();

        var checkVector = new Vector2Int(0, 0);

        foreach (TileObject tile in board)
        {
            typebrd[tile.x, tile.y] = tile.tile;
        }

        while (listRead.Count < 121) //Tant qu'il reste des éléments à lister
        {
            checkVector = new Vector2Int(0, 0);

            while (listRead.Contains(checkVector)) //Snap au premier indice non-lu
            {
                if (checkVector.x == 10)
                {
                    if (checkVector.y == 10)
                    {
                        Debug.LogError("PROBLEM");
                        break;
                    }
                    else
                    {
                        checkVector.x = 0;
                        checkVector.y++;
                    }
                }
                else
                {
                    checkVector.x++;
                }
            }

            if (board[checkVector.x, checkVector.y].tile != Tile.Park) //Si ce premier indice est pas un parc on s'en fout
            {
                listRead.Add(checkVector);
            }
            else //Sinon on check les environs
            {
                var toCheck = new Queue<Vector2Int>();

                toCheck.Enqueue(checkVector);

                parsedLists.Add(new List<Vector2Int>());

                while (toCheck.Count > 0)
                {
                    var current = toCheck.Dequeue();

                    if (board[current.x, current.y].tile != Tile.Park)
                    {
                        listRead.Add(current);
                        continue;
                    }
                    else
                    {
                        listRead.Add(current);
                        parsedLists[parsedLists.Count - 1].Add(current);
                    }

                    var adj = new List<Vector2Int>();

                    if (current.x - 1 >= 0)
                    {
                        adj.Add(new Vector2Int(current.x-1, current.y));
                    }

                    if (current.x + 1 <= 10)
                    {
                        adj.Add(new Vector2Int(current.x + 1, current.y));
                    }

                    if (current.y - 1 >= 0)
                    {
                        adj.Add(new Vector2Int(current.x, current.y-1));
                    }

                    if (current.y + 1 <= 10)
                    {
                        adj.Add(new Vector2Int(current.x, current.y + 1));
                    }

                    foreach (Vector2Int vec in adj)
                    {
                        if ((!toCheck.Contains(vec)) && (!listRead.Contains(vec)))
                        {
                            toCheck.Enqueue(vec);
                        }
                    }
                }
            }
        }

        var maxcount = 0;
        var maxlist = new List<Vector2Int>();

        foreach (List<Vector2Int> l in parsedLists)
        {
            if (l.Count > maxcount)
            {
                maxcount = l.Count;
                maxlist = l;
            }
        }

        foreach(Vector2Int v in maxlist)
        {
            board[v.x, v.y].score += parkBiggestScore;
        }
    }
}