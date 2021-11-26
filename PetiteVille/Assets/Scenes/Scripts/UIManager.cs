using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text missionsText;
    [SerializeField] private GameObject explanations;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadMenuScene()
    {
        dontDestroy.resetQueues();
        dontDestroy.lastGameMode = Missions.IndustrialArea;
        dontDestroy.selectedMission = (Missions)(-1);
        SceneManager.LoadScene(0);
    }

    public void loadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    public void Replay()
    {
        dontDestroy.ReplayLastGameMode();
        loadGameScene();
    }

    /*
    public void dailyShuffleClick()
    {
        Debug.Log("Daily");
    }*/

    public void randomShuffleClick()
    {
        SceneManager.LoadScene(1);
    }

    public void missionClick()
    {
        if (dontDestroy.selectedMission != (Missions)(-1))
        {
            switch (dontDestroy.selectedMission)
            {
                case Missions.IndustrialArea:
                    dontDestroy.setupIndustrialArea();
                    break;
                case Missions.neighborhood:
                    dontDestroy.setupNeighborhood();
                    break;
                /*case Missions.Island:
                    return;
                    break;
                case Missions.HabitatedIsland:
                    return;
                    break;
                case Missions.Aqueduct:
                    return;
                    break;
                case Missions.Highways:
                    return;
                    break;*/
                case Missions.DailyShuffle:
                    return;
                    break;
                case Missions.RandomShuffle:
                    return;
                    break;
                default:
                    missionRight();
                    break;
            }
            SceneManager.LoadScene(1);
        }
    }

    public void howToPlayClick()
    {
        explanations.SetActive(!explanations.activeSelf);
    }

    public void missionRight()
    {
        dontDestroy.selectedMission = (Missions)(((int)dontDestroy.selectedMission + 1) % ((int)Missions.DailyShuffle));
        updateMissionsText();
    }

    public void missionLeft()
    {
        dontDestroy.selectedMission = (Missions)(int)(dontDestroy.selectedMission - 1);
        if ((int)dontDestroy.selectedMission < 0)
            dontDestroy.selectedMission = (Missions)((int)Missions.DailyShuffle - 1);
        updateMissionsText();
    }

    public void updateMissionsText()
    {
        string s;
        switch (dontDestroy.selectedMission)
        {
            case Missions.IndustrialArea:
                s = "Industrial\nArea";
                break;
            case Missions.neighborhood:
                s = "Neighborhood";
                break;
            /*case Missions.Island:
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
                break;*/
            default:
                s = "Something broke :(";
                break;
        }
        missionsText.text = s;
    }

}
