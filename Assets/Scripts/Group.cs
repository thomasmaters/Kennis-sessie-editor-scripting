using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{

    private bool edit = false;
	//TO-DO: Variabelen voor random plaatsing hier

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
