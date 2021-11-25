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

                gameData[i,j] = (Tile)Random.Range(0, 7); 
                
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

}
