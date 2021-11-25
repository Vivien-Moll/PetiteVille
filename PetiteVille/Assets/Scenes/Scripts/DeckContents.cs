using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckContents : MonoBehaviour
{
    public static DeckContents Instance { get; private set; }

    [SerializeField]
    private bool randomizeDeck = true;
    private int nbOfTilesPlayed = 0;
    private int size = 15;
    
    [SerializeField] private GameObject handContents;

    [SerializeField]
    private Text houseAmount;
    [SerializeField]
    private Text parkAmount;
    [SerializeField]
    private Text riverAmount;
    [SerializeField]
    private Text roadAmount;

    private int nbHouse = 0;
    private int nbRoad = 0;
    private int nbRiver = 0;
    private int nbPark = 0;
    dontDestroy persist;

    private Color tileSprite;
    private int[] cellsToShow = new int[4] { 11, 12, 13, 21 };
    private bool isHandEmpty = true;

    private List<Tile> acceptedTiles = new List<Tile> { Tile.House, Tile.Park, Tile.River, Tile.Road };
    private List<Tetris> acceptedTetris = new List<Tetris> { Tetris.L, Tetris.Line, Tetris.L_Inverted, Tetris.S, Tetris.Square, Tetris.S_Inverted, Tetris.T };

    private Queue<Tile> tileType = new Queue<Tile>();
    private Queue<Tetris> forms = new Queue<Tetris>();

    private TileObject[,] previewTetris = new TileObject[3, 4];
    private TileObject previewUnique;

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
        //Plus de raison de randomize le deck à priori mais si on veut test il suffit de mettre true ci-dessous
        randomizeDeck = false;
        if (randomizeDeck)
        {
            for (int i = 0; i < size; i++)
            {
                tileType.Enqueue(acceptedTiles[Random.Range(0, acceptedTiles.Count)]);
                forms.Enqueue(acceptedTetris[Random.Range(0, acceptedTetris.Count)]);
            }
        }
        else
        {
            persist = GameObject.FindGameObjectWithTag("persistant").GetComponent<dontDestroy>();


            tileType = persist.getTiles();
            forms = persist.getTetris();

        }

        setDeckContent(tileType);
        showDeckValues();
    }

    public void AddTileToTetrisPreview(int x, int y, TileObject tile)
    {
        if (previewTetris[x, y] == null)
        {
            previewTetris[x, y] = tile;
        }
        else
        {
            Debug.LogError(x.ToString() + " " + y.ToString() + " - Trying to assign a tile to a filled index");
        }
    }

    public void SetTilePreviewUnique(TileObject tile)
    {
        if (previewUnique == null)
        {
            previewUnique = tile;
        }
        else
        {
            Debug.LogError("Trying to assign a tile to a filled slot");
        }
    }

    public int DeckCount()
    {
        return tileType.Count;
    }

    public Tile TopTile()
    {
        return tileType.Peek();
    }

    public Tetris TopTetris()
    {
        return forms.Peek();
    }

    public Tile GetTile()
    {
        previewUnique.tile = TopTile();

        foreach (TileObject p in previewTetris)
        {
            p.tile = TopTile();
        }
        GameManager.Instance.DefaulTileSprite(previewUnique);

        switch (TopTile())
        {
            case Tile.House:
                nbHouse--;
                break;
            case Tile.Road:
                nbRoad--;
                break;
            case Tile.River:
                nbRiver--;
                break;
            case Tile.Park:
                nbPark--;
                break;
            default:
                break;
        }
        showDeckValues();

        return tileType.Dequeue();
    }

    public Tetris GetTetris()
    {
        return forms.Dequeue();
    }

    public void RefreshTetris(bool[,] pat)
    {
        for (var x = 1; x < 4; x++)
        {
            for(var y = 0; y < 4; y++)
            {
                if(pat[x,y])
                {
                    previewTetris[x - 1, y].GetComponent<Image>().enabled = true;
                    GameManager.Instance.DefaulTileSprite(previewTetris[x - 1, y]);
                }
                else
                {
                    previewTetris[x - 1, y].GetComponent<Image>().enabled = false;
                }
            }
        }
    }

    private void Update()
    {
        
        /*if (isHandEmpty && nbOfTilesPlayed <= size)
        {
            isHandEmpty = false;
            var t = tileType.Dequeue();
            var f = forms.Dequeue();

            findTileSprite(t);

            showHandCard(f);
            showDeckValues();
        }

        if (Input.GetMouseButtonDown(0))
        {
            resetHand();
            isHandEmpty = true;
            nbOfTilesPlayed++;
        }*/
    }

    private void findTileSprite(Tile t)
    {
        switch (t)
        {
            case Tile.Empty:
                break;
            case Tile.House:
                tileSprite = Color.red;
                nbHouse--;
                break;
            case Tile.Road:
                tileSprite = Color.gray;
                nbRoad--;
                break;
            case Tile.River:
                tileSprite = Color.blue;
                nbRiver--;
                break;
            case Tile.Park:
                tileSprite = Color.green;
                nbPark--;
                break;
            case Tile.Factory:
                break;
            default:
                Debug.Log("Tile Color NOT FOUND");
                break;
        }
    }

    private void showHandCard(Tetris f)
    {
        switch (f)
        {
            case Tetris.Square:
                cellsToShow = new int[4]{ 11, 21, 12, 22};
                break;
            case Tetris.Line:
                cellsToShow = new int[4] { 21, 22, 23, 24};
                break;
            case Tetris.T:
                cellsToShow = new int[4] { 11, 22, 21, 31 };
                break;
            case Tetris.L:
                cellsToShow = new int[4] { 21, 22, 23, 31};
                break;
            case Tetris.L_Inverted:
                cellsToShow = new int[4] { 21, 22, 23, 11 };
                break;
            case Tetris.S:
                cellsToShow = new int[4] { 11, 21, 22, 32 };
                break;
            case Tetris.S_Inverted:
                cellsToShow = new int[4] { 12, 22, 31, 21 };
                break;
            default:
                Debug.Log("Tetris form NOT FOUND");
                break;
        }
        handContents.transform.Find(cellsToShow[0].ToString()).GetComponent<Image>().color = tileSprite;
        handContents.transform.Find(cellsToShow[1].ToString()).GetComponent<Image>().color = tileSprite;
        handContents.transform.Find(cellsToShow[2].ToString()).GetComponent<Image>().color = tileSprite;
        handContents.transform.Find(cellsToShow[3].ToString()).GetComponent<Image>().color = tileSprite;
    }

    private void resetHand()
    {
        handContents.transform.Find(cellsToShow[0].ToString()).GetComponent<Image>().color = Color.black;
        handContents.transform.Find(cellsToShow[1].ToString()).GetComponent<Image>().color = Color.black;
        handContents.transform.Find(cellsToShow[2].ToString()).GetComponent<Image>().color = Color.black;
        handContents.transform.Find(cellsToShow[3].ToString()).GetComponent<Image>().color = Color.black;
    }

    private void setDeckContent(Queue<Tile> tile_queue)
    {
        
        foreach (Tile t in tile_queue)
        {
            
            switch (t)
            {
                case Tile.House:
                    nbHouse++;
                    break;
                case Tile.Road:
                    nbRoad++;
                    break;
                case Tile.River:
                    nbRiver++;
                    break;
                case Tile.Park:
                    nbPark++;
                    break;
                default:
                    break;
            }
        }
    }

    private void showDeckValues()
    {
        houseAmount.text = "x" + nbHouse;
        parkAmount.text = "x" + nbPark;
        riverAmount.text = "×" + nbRiver;
        roadAmount.text = "×" + nbRoad;
    }
}
