using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroy : MonoBehaviour
{

    [SerializeField] public int bugfixVariable;
    public static Missions selectedMission;

    public static Tile[,] gameData = new Tile[11,11];

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
        createEmptyField();
        selectedMission = (Missions)(-1);
    }

    // Update is called once per frame
    void Update()
    {
        bugfixVariable = (int)selectedMission;
    }

    public void missionRight()
    {
        selectedMission = (Missions)(((int)selectedMission +1)%((int)Missions.DailyShuffle));
    }
    
    public void missionLeft()
    {
        selectedMission = (Missions)(int)(selectedMission - 1);
        if ((int)selectedMission < 0)
            selectedMission = (Missions)((int)Missions.RandomShuffle - 1);
    }

    //Juste pour tester
    public void randomShuffleClick()
    {
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 11; j++)
            {

                gameData[i,j] = (Tile)Random.Range(0, 7); 
                
            }
        }
    }


    public void createEmptyField()
    {
        for (int i = 0; i< 11; i++)
        {
            for (int j = 0; j< 11; j++)
            {
                gameData[i,j] = Tile.Empty;
            }
        }
    }
}
