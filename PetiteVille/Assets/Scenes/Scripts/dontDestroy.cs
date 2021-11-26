using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroy : MonoBehaviour
{

    [SerializeField] public int bugfixVariable;
    public static Missions selectedMission;

    public static Tile[,] gameData = new Tile[11,11];
    private static Queue<Tile> tileType = new Queue<Tile>();
    private static Queue<Tetris> forms = new Queue<Tetris>();
    public static Missions lastGameMode = Missions.IndustrialArea;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("persistant");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateEmptyField();
        selectedMission = (Missions)(-1);
    }

    // Update is called once per frame
    void Update()
    {
        bugfixVariable = (int)selectedMission;
    }

    //Juste pour tester
    public static void randomShuffleClick()
    {
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                gameData[i,j] = (Tile)UnityEngine.Random.Range(0, 7); 
            }
        }
    }


    public static void CreateEmptyField()
    {
        for (int i = 0; i< 11; i++)
        {
            for (int j = 0; j< 11; j++)
            {
                gameData[i,j] = Tile.Empty;
            }
        }
    }

    public Queue<Tile> getTiles()
    {
        return tileType;
    }

    public Queue<Tetris> getTetris()
    {
        return forms;
    }

    public static void setupIndustrialArea()
    {
        lastGameMode = Missions.IndustrialArea;

        CreateEmptyField();

        gameData[3, 0] = Tile.Road;
        gameData[8, 0] = Tile.River;
        gameData[1, 1] = Tile.Factory;
        gameData[2, 1] = Tile.Factory;
        gameData[3, 1] = Tile.Road;
        gameData[7, 1] = Tile.River;
        gameData[8, 1] = Tile.River;
        gameData[1, 2] = Tile.Factory;
        gameData[2, 2] = Tile.Factory;
        gameData[3, 2] = Tile.Road;
        gameData[6, 2] = Tile.River;
        gameData[7, 2] = Tile.River;
        gameData[3, 3] = Tile.Road;
        gameData[7, 6] = Tile.Road;
        gameData[8, 6] = Tile.Road;
        gameData[9, 6] = Tile.Road;
        gameData[4, 7] = Tile.River;
        gameData[7, 7] = Tile.House;
        gameData[8, 7] = Tile.House;
        gameData[0, 8] = Tile.Mountain;
        gameData[1, 8] = Tile.Park;
        gameData[2, 8] = Tile.Park;
        gameData[4, 8] = Tile.River;
        gameData[5, 8] = Tile.River;
        gameData[7, 8] = Tile.House;
        gameData[8, 8] = Tile.Park;
        gameData[9, 8] = Tile.Mountain;
        gameData[0, 9] = Tile.Mountain;
        gameData[1, 9] = Tile.Mountain;
        gameData[2, 9] = Tile.Park;
        gameData[5, 9] = Tile.River;
        gameData[7, 9] = Tile.Park;
        gameData[8, 9] = Tile.Mountain;
        gameData[9, 9] = Tile.Mountain;
        gameData[10, 9] = Tile.Mountain;
        gameData[0, 10] = Tile.Mountain;
        gameData[1, 10] = Tile.Mountain;
        gameData[2, 10] = Tile.Mountain;
        gameData[3, 10] = Tile.Mountain;
        gameData[4, 10] = Tile.Park;
        gameData[5, 10] = Tile.River;
        gameData[6, 10] = Tile.Park;
        gameData[7, 10] = Tile.Mountain;
        gameData[8, 10] = Tile.Mountain;
        gameData[9, 10] = Tile.Mountain;
        gameData[10, 10] = Tile.Mountain;

        
        tileType.Enqueue(Tile.River);
        tileType.Enqueue(Tile.House);
        tileType.Enqueue(Tile.Road);
        tileType.Enqueue(Tile.River);
        tileType.Enqueue(Tile.Park);
        tileType.Enqueue(Tile.House);
        tileType.Enqueue(Tile.Road);
        tileType.Enqueue(Tile.River);
        tileType.Enqueue(Tile.Park);
        tileType.Enqueue(Tile.Road);
        tileType.Enqueue(Tile.House);
        tileType.Enqueue(Tile.River);
        tileType.Enqueue(Tile.House);
        tileType.Enqueue(Tile.Park);
        tileType.Enqueue(Tile.River);
        
        forms.Enqueue(Tetris.T);
        forms.Enqueue(Tetris.Square);
        forms.Enqueue(Tetris.Line);
        forms.Enqueue(Tetris.S);
        forms.Enqueue(Tetris.L);
        forms.Enqueue(Tetris.L_Inverted);
        forms.Enqueue(Tetris.T);
        forms.Enqueue(Tetris.Line);
        forms.Enqueue(Tetris.S_Inverted);
        forms.Enqueue(Tetris.L_Inverted);
        forms.Enqueue(Tetris.S);
        forms.Enqueue(Tetris.Line);
        forms.Enqueue(Tetris.T);
        forms.Enqueue(Tetris.Square);
        forms.Enqueue(Tetris.L);
    }

    public static void setupNeighborhood()
    {
        lastGameMode = Missions.neighborhood;

        CreateEmptyField();

        gameData[0, 0] = Tile.Mountain;
        gameData[1, 0] = Tile.Mountain;
        gameData[7, 0] = Tile.River;
        gameData[9, 0] = Tile.Mountain;

        gameData[0, 1] = Tile.Mountain;
        gameData[1, 1] = Tile.Park;
        gameData[4, 1] = Tile.House;
        gameData[5, 1] = Tile.Road;
        gameData[7, 1] = Tile.River;
        gameData[8, 1] = Tile.Mountain;
        gameData[9, 1] = Tile.Mountain;

        gameData[0, 2] = Tile.Park;
        gameData[1, 2] = Tile.Park;
        gameData[3, 2] = Tile.Mountain;
        gameData[4, 2] = Tile.Mountain;
        gameData[5, 2] = Tile.Road;
        gameData[6, 2] = Tile.Road;
        gameData[7, 2] = Tile.River;

        gameData[3, 3] = Tile.Mountain;
        gameData[4, 3] = Tile.Factory;
        gameData[5, 3] = Tile.Factory;
        gameData[6, 3] = Tile.Road;
        gameData[7, 3] = Tile.River;

        gameData[3, 4] = Tile.Mountain;
        gameData[10, 4] = Tile.Park;

        gameData[9, 5] = Tile.Park;
        gameData[10, 5] = Tile.Park;

        gameData[7, 6] = Tile.Mountain;
        gameData[8, 6] = Tile.Mountain;

        gameData[1, 7] = Tile.Mountain;
        gameData[2, 7] = Tile.Mountain;
        gameData[3, 7] = Tile.Factory;
        gameData[8, 7] = Tile.Mountain;

        gameData[2, 8] = Tile.Mountain;
        gameData[3, 8] = Tile.Factory;
        gameData[6, 8] = Tile.River;

        gameData[3, 9] = Tile.Factory;
        gameData[5, 9] = Tile.Mountain;
        gameData[6, 9] = Tile.River;
        gameData[7, 9] = Tile.River;
        gameData[10, 9] = Tile.Park;

        gameData[5, 10] = Tile.Mountain;
        gameData[6, 10] = Tile.Mountain;
        gameData[7, 10] = Tile.River;
        gameData[9, 10] = Tile.Park;
        gameData[10, 10] = Tile.Park;


        tileType.Enqueue(Tile.Road); //1
        tileType.Enqueue(Tile.House); //2
        tileType.Enqueue(Tile.Park); //3
        tileType.Enqueue(Tile.House); //4
        tileType.Enqueue(Tile.Road); //5
        tileType.Enqueue(Tile.River); //6
        tileType.Enqueue(Tile.Park); //7
        tileType.Enqueue(Tile.Road); //8
        tileType.Enqueue(Tile.House); //9
        tileType.Enqueue(Tile.Road); //10
        tileType.Enqueue(Tile.River); //11
        tileType.Enqueue(Tile.Park); //12
        tileType.Enqueue(Tile.House); //13
        tileType.Enqueue(Tile.River); //14
        tileType.Enqueue(Tile.Road); //15

        forms.Enqueue(Tetris.S_Inverted); //1
        forms.Enqueue(Tetris.Square); //2
        forms.Enqueue(Tetris.Line); //3
        forms.Enqueue(Tetris.L_Inverted); //4
        forms.Enqueue(Tetris.Square); //5
        forms.Enqueue(Tetris.S); //6
        forms.Enqueue(Tetris.L); //7
        forms.Enqueue(Tetris.T); //8
        forms.Enqueue(Tetris.Line); //9
        forms.Enqueue(Tetris.L_Inverted); //10
        forms.Enqueue(Tetris.T); //11
        forms.Enqueue(Tetris.Square); //12
        forms.Enqueue(Tetris.S_Inverted); //13
        forms.Enqueue(Tetris.L); //14
        forms.Enqueue(Tetris.Line); //15
    }

    public static void setupDailyShuffle()
    {
        lastGameMode = Missions.DailyShuffle;

        System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("fr-FR");
        DateTime dt = DateTime.Parse(DateTime.Today.ToString(), cultureinfo);
        UnityEngine.Random.InitState(dt.GetHashCode());

        setupRandomGame();
    }

    public static void setupRandomGame()
    {
        if (lastGameMode != Missions.DailyShuffle)
        {
            lastGameMode = Missions.RandomShuffle;
        }

        CreateEmptyField();

        //Placement des rivières
        var waterPlacement1 = UnityEngine.Random.Range(0f, 1f);
        var waterPlacement2 = UnityEngine.Random.Range(0f, 1f);

        var wp1 = waterPlacement1 * 3.99f; //Random side
        var wp2 = (wp1 + 2) % 4; //Opposite side from the first one

        var wp1Square = Mathf.FloorToInt(waterPlacement1 * 6.99f) + 2;
        var wp2Square = Mathf.FloorToInt(waterPlacement2 * 6.99f) + 2;

        Vector2Int centerSquare1;
        Vector2Int centerSquare2;

        if (wp1 < 1) //Haut
        {
            centerSquare1 = new Vector2Int(wp1Square, 0);
        }
        else if (wp1 < 2) //Droite
        {
            centerSquare1 = new Vector2Int(10, wp1Square);
        }
        else if (wp1 < 3) //Bas
        {
            centerSquare1 = new Vector2Int(wp1Square, 10);
        }
        else //Gauche
        {
            centerSquare1 = new Vector2Int(0, wp1Square);
        }

        if (wp2 < 1) //Haut
        {
            centerSquare2 = new Vector2Int(wp2Square, 0);
        }
        else if (wp2 < 2) //Droite
        {
            centerSquare2 = new Vector2Int(10, wp2Square);
        }
        else if (wp2 < 3) //Bas
        {
            centerSquare2 = new Vector2Int(wp2Square, 10);
        }
        else //Gauche
        {
            centerSquare2 = new Vector2Int(0, wp2Square);
        }

        PlaceRiverPattern(Mathf.FloorToInt(wp1), centerSquare1, UnityEngine.Random.Range(0f, 1f));
        PlaceRiverPattern(Mathf.FloorToInt(wp2), centerSquare2, UnityEngine.Random.Range(0f, 1f));

        //Placement des usines

        var validatedPlace = false;

        var x = 0;
        var y = 0;

        while(!validatedPlace)
        {
            validatedPlace = true;

            x = UnityEngine.Random.Range(1, 9);
            y = UnityEngine.Random.Range(1, 9);

            if ((gameData[x-1,y-1] == Tile.River) || (gameData[x, y - 1] == Tile.River) || (gameData[x + 1, y - 1] == Tile.River) || (gameData[x + 2, y - 1] == Tile.River) || (gameData[x - 1, y] == Tile.River) || (gameData[x, y] == Tile.River) || (gameData[x + 1, y] == Tile.River) || (gameData[x + 2, y] == Tile.River) || (gameData[x - 1, y + 1] == Tile.River) || (gameData[x, y + 1] == Tile.River) || (gameData[x + 1, y + 1] == Tile.River) || (gameData[x + 2, y + 1] == Tile.River) || (gameData[x - 1, y + 2] == Tile.River) || (gameData[x, y + 2] == Tile.River) || (gameData[x + 1, y + 2] == Tile.River) || (gameData[x + 2, y + 2] == Tile.River))
            {
                validatedPlace = false;
            }
        }

        gameData[x, y] = Tile.Factory;
        gameData[x+1, y] = Tile.Factory;
        gameData[x, y+1] = Tile.Factory;
        gameData[x+1, y+1] = Tile.Factory;

        //ALLEZ LEZGONG LES FORMES ARBITRAIRES
        var shapes = new List<List<Vector2Int>>();

        var cube = new List<Vector2Int>();
        cube.Add(new Vector2Int(0, 0));
        cube.Add(new Vector2Int(0, 1));
        cube.Add(new Vector2Int(1, 0));
        cube.Add(new Vector2Int(1, 1));
        shapes.Add(cube);

        var Tv = new List<Vector2Int>();
        Tv.Add(new Vector2Int(0, 0));
        Tv.Add(new Vector2Int(0, 1));
        Tv.Add(new Vector2Int(1, 0));
        Tv.Add(new Vector2Int(-1, 0));
        shapes.Add(Tv);

        var Lv = new List<Vector2Int>();
        Lv.Add(new Vector2Int(0, 0));
        Lv.Add(new Vector2Int(0, 1));
        Lv.Add(new Vector2Int(0, 2));
        Lv.Add(new Vector2Int(1, 2));
        shapes.Add(Lv);

        var Lh = new List<Vector2Int>();
        Lh.Add(new Vector2Int(0, 0));
        Lh.Add(new Vector2Int(1, 0));
        Lh.Add(new Vector2Int(2, 0));
        Lh.Add(new Vector2Int(2, 1));
        shapes.Add(Lh);

        var Th = new List<Vector2Int>();
        Th.Add(new Vector2Int(0, 0));
        Th.Add(new Vector2Int(0, -1));
        Th.Add(new Vector2Int(0, 1));
        Th.Add(new Vector2Int(1, 0));
        shapes.Add(Th);

        float tileSelect = 0f;
        Tile tileChoice = Tile.Empty;
        int shapeSelect = 0;
        var shapeChoice = new List<Vector2Int>();
        bool placetiles = false;
        var targetx = 0;
        var targety = 0;

        for (int v = 0; v < 7; v++)
        {
            tileSelect = UnityEngine.Random.Range(0f, 1f);

            if (tileSelect < 0.3f)
            {
                tileChoice = Tile.House;
            }
            else if (tileSelect < 0.55f)
            {
                tileChoice = Tile.Road;
            }
            else if (tileSelect < 0.85f)
            {
                tileChoice = Tile.Park;
            }
            else
            {
                tileChoice = Tile.Mountain;
            }

            shapeSelect = Mathf.FloorToInt(UnityEngine.Random.Range(0f, 1f) * 5f);
            shapeChoice = shapes[shapeSelect];
            Debug.Log(shapeSelect);
            

            x = UnityEngine.Random.Range(0, 10);
            y = UnityEngine.Random.Range(0, 10);

            foreach(Vector2Int vec in shapeChoice)
            {
                placetiles = false;
                targetx = x + vec.x;
                targety = y + vec.y;

                while(!placetiles)
                {
                    placetiles = true;

                    if ((targetx < 0) || (targetx > 10) || (targety < 0) || (targety > 10))
                    {
                        placetiles = false;
                    }
                    else if(gameData[targetx,targety] != Tile.Empty)
                    {
                        placetiles = false;
                    }

                    if (!placetiles)
                    {
                        targetx++;

                        if (targetx > 10)
                        {
                            targetx = 0;
                            targety++;
                        }

                        if (targety > 10)
                        {
                            targetx = 0;
                            targety = 0;
                        }
                    }
                    else
                    {
                        gameData[targetx, targety] = tileChoice;
                    }
                }
            }
        }
        
        for (int i = 0; i < 7; i++)
        {
            if (i == 0) //Contrôle de l'aléatoire > Garantie d'avoir au moins 2 de chaque type
            {
                tileType.Enqueue(Tile.House);
                tileType.Enqueue(Tile.House);
                tileType.Enqueue(Tile.Road);
                tileType.Enqueue(Tile.Road);
                tileType.Enqueue(Tile.River);
                tileType.Enqueue(Tile.River);
                tileType.Enqueue(Tile.Park);
                tileType.Enqueue(Tile.Park);
            }

            Tile choice = (Tile)Mathf.FloorToInt(UnityEngine.Random.Range(0f, 1f) * 4f + 1f);
            tileType.Enqueue(choice);
        }

        for (int i = 0; i < 15; i++)
        {
            Tetris choice = (Tetris)Mathf.FloorToInt(UnityEngine.Random.Range(0f, 1f) * 7f);
            forms.Enqueue(choice);
        }
    }

    private static void PlaceRiverPattern(int side, Vector2Int origin, float value)
    {
        List<Vector2Int> pattern = new List<Vector2Int>();
        List<Vector2Int> finalPattern = new List<Vector2Int>();

        if (value < 0.1f)
        {
            pattern.Add(new Vector2Int(-1,0));
            pattern.Add(new Vector2Int(-1,1));
            pattern.Add(new Vector2Int(0,1));
            pattern.Add(new Vector2Int(0,2));
            pattern.Add(new Vector2Int(0,3));
            pattern.Add(new Vector2Int(0,4));
        }
        else if (value < 0.2f)
        {
            pattern.Add(new Vector2Int(0,0));
            pattern.Add(new Vector2Int(-1,1));
            pattern.Add(new Vector2Int(0,1));
            pattern.Add(new Vector2Int(0,2));
            pattern.Add(new Vector2Int(0,3));
        }
        else if (value < 0.3f)
        {
            pattern.Add(new Vector2Int(1,0));
            pattern.Add(new Vector2Int(1,1));
            pattern.Add(new Vector2Int(0,1));
            pattern.Add(new Vector2Int(0,2));
            pattern.Add(new Vector2Int(-1,2));
        }
        else if (value < 0.4f)
        {
            pattern.Add(new Vector2Int(-1,0));
            pattern.Add(new Vector2Int(-1,1));
            pattern.Add(new Vector2Int(0,1));
            pattern.Add(new Vector2Int(0,2));
        }
        else if (value < 0.5f)
        {
            pattern.Add(new Vector2Int(-1,0));
            pattern.Add(new Vector2Int(-1,1));
            pattern.Add(new Vector2Int(0,1));
            pattern.Add(new Vector2Int(0,2));
            pattern.Add(new Vector2Int(0,3));
            pattern.Add(new Vector2Int(1,3));
        }
        else if (value < 0.6f)
        {
            pattern.Add(new Vector2Int(0,0));
            pattern.Add(new Vector2Int(0,1));
            pattern.Add(new Vector2Int(0,2));
            pattern.Add(new Vector2Int(0,3));
            pattern.Add(new Vector2Int(0,4));
        }
        else if (value < 0.7f)
        {
            pattern.Add(new Vector2Int(0,1));
            pattern.Add(new Vector2Int(0,2));
            pattern.Add(new Vector2Int(0,3));
            pattern.Add(new Vector2Int(0,0));
            pattern.Add(new Vector2Int(1,2));
        }
        else if (value < 0.8f)
        {
            pattern.Add(new Vector2Int(2,0));
            pattern.Add(new Vector2Int(2,1));
            pattern.Add(new Vector2Int(1,1));
            pattern.Add(new Vector2Int(0,1));
            pattern.Add(new Vector2Int(-1,1));
            pattern.Add(new Vector2Int(-1,2));
        }
        else if (value < 0.9f)
        {
            pattern.Add(new Vector2Int(1,0));
            pattern.Add(new Vector2Int(1,1));
            pattern.Add(new Vector2Int(0,1));
            pattern.Add(new Vector2Int(-1,1));
            pattern.Add(new Vector2Int(-1,2));
            pattern.Add(new Vector2Int(-1,3));
        }
        else if (value < 1f)
        {
            pattern.Add(new Vector2Int(0,0));
            pattern.Add(new Vector2Int(0,1));
            pattern.Add(new Vector2Int(-1,1));
            pattern.Add(new Vector2Int(-1,2));
            pattern.Add(new Vector2Int(-1,3));
            pattern.Add(new Vector2Int(0,3));
        }
        
        if (side > 0)
        {
            foreach (Vector2Int vec in pattern)
            {
                finalPattern.Add(RotateRiver(vec, side));
            }

        }
        else
        {

            foreach (Vector2Int vec in pattern)
            {
                finalPattern.Add(vec);
            }
        }

        foreach(Vector2Int ofs in finalPattern)
        {
            gameData[origin.x + ofs.x,origin.y + ofs.y] = Tile.River;
        }
    }

    private static Vector2Int RotateRiver(Vector2Int input, int quarterRotations)
    {
        var resultvec = new Vector2Int();

        for (var x = 0; x < quarterRotations; x++)
        {
            resultvec.x = -input.y;
            resultvec.y = input.x;

            input = resultvec;
        }

        return resultvec;
    }

    public static void ReplayLastGameMode()
    {
        switch(lastGameMode)
        {
            case Missions.IndustrialArea:
                setupIndustrialArea();
                break;

            case Missions.neighborhood:
                setupNeighborhood();
                break;

            case Missions.DailyShuffle:
                setupDailyShuffle();
                break;

            case Missions.RandomShuffle:
                setupRandomGame();
                break;
        }
    }
}
