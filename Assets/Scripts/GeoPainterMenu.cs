using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeoPainterMenu : EditorWindow
{
    private static PrefabPainter painter = new PrefabPainter();
    private Groups groups = new Groups(painter);
    private static PrefabPainterLibrary prefabPainterLibrary;

    [MenuItem("Window/Prefab Painter")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        GeoPainterMenu window = EditorWindow.GetWindow<GeoPainterMenu>();
        window.Show();
        window.autoRepaintOnSceneChange = true;
        prefabPainterLibrary = new PrefabPainterLibrary(window);
    }

    void OnEnable()
    {
        SceneView.onSceneGUIDelegate -= painter.CustomUpdate;
        SceneView.onSceneGUIDelegate += painter.CustomUpdate;
        painter.setTimer(Time.time);
    }

    void OnGUI()
    {
        groups.renderGUI();
        painter.DrawPainterGUI();
        prefabPainterLibrary.drawGUI();
    }
}
