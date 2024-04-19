using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import the UI namespace, this is used to find out the text component

public class LumpView : MonoBehaviour
{
    private Text text; // Now text is of type UnityEngine.UI.Text

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
    }

    public void UpdateLump(int lump)
    {
        text.text = "" + lump + "/3";    
    }
}
