using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColour;
    private Renderer rend;
    private Color startColour;
    [HideInInspector]public GameObject turret;
    public Color errorColor = Color.red;
    private float errorColourDuration = 0.5f;
    public Vector3 Offset;
    BuildManager buildManager;
    [HideInInspector]public bool menuShowing;
    private void Start()
    {
        buildManager = BuildManager.instance;
        rend = GetComponent<Renderer>();
        startColour = rend.material.color;
    }
    private void OnMouseEnter()
    {
        rend.material.color = hoverColour;
    }
    private void OnMouseExit()
    {
        rend.material.color = startColour;
    }
 
    private void OnMouseDown()
    {
        if (!IsPointerOverUI())
        {
            
            if (menuShowing)
            {
                buildManager.BringApproprioteMenu(menuShowing);
                menuShowing = false;
            }
            else
            {
                buildManager.setNodeObjectSelected(gameObject);
                buildManager.BringApproprioteMenu(menuShowing);
                menuShowing = true;
            }
            //Debug.Log(menuShowing);
            
        }
       
       
        //if(buildManager.getTurretToBuild() == null)
        //{
        //    return;
        //}
        //if (turret != null) 
        //{

        //    StartCoroutine(ErrorColorCoroutine());
        //    return;
        //}
        //GameObject turretToBuild = buildManager.getTurretToBuild();
        //Instantiate(turretToBuild,transform.position+Offset,transform.rotation);

    }
    bool IsPointerOverUI()
    {
        // Check if the mouse pointer is over a UI element
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        // Create a list to store the results of the raycast
        var results = new List<RaycastResult>();

        // Raycast using the event data and store the results in the list
        EventSystem.current.RaycastAll(eventData, results);

        // Check if there are any UI elements in the results
        return results.Count > 0;
    }
    IEnumerator ErrorColorCoroutine()
    {
        Color OriginalColour = rend.material.color;
        rend.material.color = errorColor;
        yield return new WaitForSeconds(errorColourDuration);
        rend.material.color = OriginalColour;
    }
}
