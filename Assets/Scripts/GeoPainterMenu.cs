using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeoPainterMenu : EditorWindow
{
    private Groups groups = new Groups();

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/My Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        GeoPainterMenu window = (GeoPainterMenu)EditorWindow.GetWindow(typeof(GeoPainterMenu));
        window.Show();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        groups.renderGUI();
    }
}
