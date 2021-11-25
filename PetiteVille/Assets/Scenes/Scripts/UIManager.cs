using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text missionsText;
    [SerializeField] private GameObject accessMissionEnum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void dailyShuffleClick()
    {
        Debug.Log("Daily");
    }

    public void randomShuffleClick()
    {
        SceneManager.LoadScene(1);
    }

    public void missionClick()
    {
        Debug.Log("Mission");
    }

    public void howToPlayClick()
    {
        Debug.Log("How to Play");
    }

    public void updateMissionsText()
    {
        string s;
        switch (dontDestroy.selectedMission)
        {
            case Missions.IndustrialArea:
                s = "Industrial Area";
                break;
            case Missions.neighborhood:
                s = "Neighborhood";
                break;
            case Missions.Island:
                s = "Island";
                break;
            case Missions.HabitatedIsland:
                s = "Habitated Island";
                break;
            case Missions.Aqueduct:
                s = "Aqueduct";
                break;
            case Missions.Highways:
                s = "Highways";
                break;
            default:
                s = "Something broke :(";
                break;
        }
        missionsText.text = s;
    }

}
