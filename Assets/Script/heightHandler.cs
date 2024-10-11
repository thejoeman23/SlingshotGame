using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class heightHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI heightDisplay;

    GameObject highest = null; 
    public int highestHeight = 0; 

    private void Start()
    {
        heightDisplay.text = "0\' 0\"";
        highestHeight = 0; 
        InvokeRepeating("updateDisplay", 0, 0.5f);
    }

    void updateDisplay()
    {
        GameObject[] tower = GameObject.FindGameObjectsWithTag("projectile");
        if (tower.Count() == 0) return;
        if (!highest) { highest = tower[0]; return; } 

        GameObject curr = getHighestNow(tower); // currently the highest object 
        if (curr.GetComponent<Rigidbody2D>().linearVelocity.magnitude > 0) return;// only save if not moving 
        if (curr.transform.position.y > highest.transform.position.y) highest = curr;
        
        // add some height because
        heightDisplay.text = formatHeight(highest.transform.position.y, 36f); 
    }

    GameObject getHighestNow(GameObject[] tower)
    {// could be slightly off because its comparing the center of the object instead of the top
        return tower.Aggregate((curr, highest) =>
            curr.transform.position.y < highest.transform.position.y ? highest : curr
        );
    }

    string formatHeight(float height, float scale) {
        int inches = (int)Math.Round(height * scale) + 50; // +50 so it isn't negative
        return (inches / 12).ToString() + "' " + (inches % 12).ToString() + "\"";
    }
}