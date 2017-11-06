using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeoPainterMenu : EditorWindow
{
    private static PrefabPainter painter;
    private static Groups groups;
    private static PrefabPainterLibrary prefabPainterLibrary;
	private static GeoPainterMenu window;
    [MenuItem("Window/Prefab Painter")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        window = EditorWindow.GetWindow<GeoPainterMenu>();
        window.Show();
		window.autoRepaintOnSceneChange = true;
		prefabPainterLibrary = new PrefabPainterLibrary(window);
		painter = new PrefabPainter (prefabPainterLibrary);
		groups = new Groups (painter);
    }

    void OnEnable()
	{
        SceneView.onSceneGUIDelegate -= painter.CustomUpdate;
        SceneView.onSceneGUIDelegate += painter.CustomUpdate;
        painter.setTimer(Time.time);
    }

	void Start(){

	}

    void OnGUI()
    { 
		groups.renderGUI ();
		painter.DrawPainterGUI ();
		prefabPainterLibrary.drawGUI ();
    }
}
