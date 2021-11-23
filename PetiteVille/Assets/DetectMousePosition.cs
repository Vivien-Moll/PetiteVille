using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DetectMousePosition : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    GameObject lastHovered;
    bool lastHoveredFound;

    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();

        lastHoveredFound = false;
    }

    void Update()
    {
        lastHoveredFound = false;

        //Check if mouse is hovering over something
        if (true)
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);

            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "Image (1)")
                {
                    result.gameObject.GetComponent<Image>().color = Color.red;
                }
                if (result.gameObject.name == "Image (2)")
                {
                    result.gameObject.GetComponent<Image>().color = Color.green;
                }
                if (result.gameObject.name == "Image (3)")
                {
                    result.gameObject.GetComponent<Image>().color = Color.red;
                }
                if (result.gameObject.name == "Image (4)")
                {
                    result.gameObject.GetComponent<Image>().color = Color.green;
                }

                if (lastHovered == result.gameObject)
                {
                    lastHoveredFound = true;
                }
                lastHovered = result.gameObject;

                //Debug.Log("Hit " + result.gameObject.name);
            }
        }

        if (!lastHoveredFound)
        {
            lastHovered.GetComponent<Image>().color = Color.white;
        }
        
    }
}
    /*void Update()
    {
        if (true)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Mouse Over: " + EventSystem.current.currentSelectedGameObject.name);
            }
            else
            {
                Debug.Log("Its NOT over UI elements");
            }

        }
        
    }*/
