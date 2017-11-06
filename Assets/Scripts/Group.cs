using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    public int paintLayer = 8;
    public bool paintMode = false;
    public bool instantiateMeshOnly = false;
    public float eraseRadius = 10;
    public float placementRadius = 9;
    public double intensity = 0.01;
    public Vector3 randomScaleMin = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 randomScaleMax = new Vector3(2, 2, 2);
    public Vector3 randomRotationMin;
    public Vector3 randomRotationMax;

    private bool edit = false;

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
