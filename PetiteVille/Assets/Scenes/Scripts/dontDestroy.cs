using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dontDestroy : MonoBehaviour
{

    private Missions selectedMission;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("persistent");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
