using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeoPainterMenu : EditorWindow
{
    private static PrefabPainter painter = new PrefabPainter();
    private Groups groups = new Groups(painter);

    [MenuItem("Window/Leon Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        GeoPainterMenu window = EditorWindow.GetWindow<GeoPainterMenu>();
        window.Show();
        window.autoRepaintOnSceneChange = true;
    }

    void OnEnable()
    {
        SceneView.onSceneGUIDelegate -= painter.CustomUpdate;
        SceneView.onSceneGUIDelegate += painter.CustomUpdate;
    }

    void OnGUI()
    {
        groups.renderGUI();
        painter.DrawPainterGUI();
    }
}
