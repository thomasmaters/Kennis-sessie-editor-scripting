using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{

    private bool edit = false;
    public int radius = 0;
    //TO-DO: Variabelen voor random plaatsing hier

    public void addObject()
    {
        
    }

    public bool isEditing()
    {
        return edit;
    }

    public void isEditing(bool edit)
    {
        this.edit = edit;
    }
}
