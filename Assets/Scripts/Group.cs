using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{

    private bool edit = false;
    public float placementRadius = 0;
    public bool paintMode = false;
    public bool instantiateMeshOnly = false;
    public float eraseRadius = 10;
    public double intensity = 0.01;

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
