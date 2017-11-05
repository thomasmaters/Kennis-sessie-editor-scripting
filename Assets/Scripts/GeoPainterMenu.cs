using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeoPainterMenu : EditorWindow {
private Groups groups = new Groups();

    [MenuItem("Window/Leon Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        GeoPainterMenu window = EditorWindow.GetWindow<GeoPainterMenu>();
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
