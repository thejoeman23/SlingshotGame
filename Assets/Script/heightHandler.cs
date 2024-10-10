using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class heightHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI heightDisplay;
    GameObject highest = null;


    private void Start()
    {
        heightDisplay.text = "0\' 0\"";
        InvokeRepeating("updateDisplay", 0, 0.5f);
    }

    void updateDisplay()
    {
        GameObject[] tower = GameObject.FindGameObjectsWithTag("projectile");
        if (tower.Count() == 0) return;
        if (!highest) { highest = tower[0]; return; } 

        GameObject curr = getHighestNow(tower); // currently the highest object 
        if (curr.transform.position.y > highest.transform.position.y) highest = curr;// todo add stillness check 
        
        this.heightDisplay.text = formatHeight(highest.transform.position.y); 
    }

    GameObject getHighestNow(GameObject[] tower)
    {
        return tower.Aggregate((curr, highest) =>
            curr.transform.position.y < highest.transform.position.y ? highest : curr
        );
    }

    string formatHeight(float height) {
        return height.ToString();
    }
}