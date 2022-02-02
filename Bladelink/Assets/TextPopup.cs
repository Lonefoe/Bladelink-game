using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopup : MonoBehaviour
{

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.GetComponent<Player>())
        {
            GetComponent<TextMeshProUGUI>().enabled = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.GetComponent<Player>())
        {
            GetComponent<TextMeshProUGUI>().enabled = false;
        }

    }
}
