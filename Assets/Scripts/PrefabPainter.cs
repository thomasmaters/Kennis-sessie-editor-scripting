using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabPainter : EditorWindow
{
    private static Painter painter;
    private static Groups groups;
    private static PrefabPainterLibrary prefabPainterLibrary;
	private static PrefabPainter window;
    [MenuItem("Window/Prefab Painter")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
		window = EditorWindow.GetWindow<PrefabPainter>();
        window.Show();
        window.autoRepaintOnSceneChange = true;
        prefabPainterLibrary = new PrefabPainterLibrary(window);
		painter = new Painter(prefabPainterLibrary);
		groups = new Groups (painter, prefabPainterLibrary);
    }

    void OnEnable()
    {
    }

    void Start()
    {

    }

    void OnGUI()
    {
        SceneView.onSceneGUIDelegate -= painter.CustomUpdate;
        SceneView.onSceneGUIDelegate += painter.CustomUpdate;
        painter.setTimer(Time.time);
        groups.renderGUI();
        painter.DrawPainterGUI();
        prefabPainterLibrary.drawGUI();
    }
}
