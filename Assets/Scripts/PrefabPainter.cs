using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabPainter
{

    public bool paintMode = false;
    public bool eraseMode = false;
    public int radius = 10;
    GameObject prefab;
    private GameObject group;
    // Add menu named "My Window" to the Window menu
    /*[MenuItem("Window/PPPainter")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		EditorWindow window = (PrefabPainter)EditorWindow.GetWindow(typeof(PrefabPainter));
		window.Show();
		window.autoRepaintOnSceneChange = true;


	}

	void OnEnable(){
		SceneView.onSceneGUIDelegate -= CustomUpdate;
		SceneView.onSceneGUIDelegate += CustomUpdate;
	}*/


    public void DrawPainterGUI()
    {

        GUILayout.Label("Prefab Painter", EditorStyles.boldLabel);
        GUILayout.Label("Paint Mode: ");
        paintMode = EditorGUILayout.Toggle(paintMode);
        GUILayout.Label("Prefab: ");
        prefab = EditorGUILayout.ObjectField(prefab, typeof(GameObject), true) as GameObject;
        GUILayout.Label("Radius: ");
        radius = EditorGUILayout.IntField(radius);
    }
    void OnGUI()
    {
        DrawPainterGUI();
    }

    void onSceneGUI()
    {
        drawHandles();
    }

    public void CustomUpdate(SceneView sv)
    {
        if (paintMode)
        {
            drawHandles();
            Event e = Event.current;
            if (((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0) && !e.control)
            {
                RaycastHit hit;
                Tools.current = Tool.View;
                int layer = 1 << 8;

                if (Physics.Raycast(Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0)), out hit, Mathf.Infinity, layer))
                {
                    GameObject placedObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    //place object on mouse pos
                    //TO-DO edit random ranges
                    placedObject.transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.5f, 2.0f);
                    placedObject.transform.position = hit.point;
                    placedObject.transform.position = new Vector3(placedObject.transform.position.x, placedObject.transform.position.y + (placedObject.transform.localScale.y / 2), placedObject.transform.position.z);
					placedObject.transform.parent = group.transform;

                    e.Use();
                    Undo.RegisterCreatedObjectUndo(placedObject, "undo prefab paint");
                }
            }
            else if (((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0) && e.control)
            {
                RaycastHit[] hits;
                Tools.current = Tool.View;
                Vector3 mousePos = new Vector3(e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0);
                int layer = 1 << 8;
                hits = Physics.SphereCastAll(Camera.current.ScreenPointToRay(mousePos), radius, layer);

                if (hits.Length != 0)
                {
                    foreach (RaycastHit hit in hits)
                    {
                        Undo.DestroyObjectImmediate((Object)hit.transform.gameObject);
                    }

                    e.Use();

                }
            }
        }
    }

    void drawHandles()
    {
        RaycastHit hit;
        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        int layerMask = 1 << 8;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
        {
            if (radius != 0)
            {
                Handles.color = Color.blue;
                Handles.CircleCap(1, hit.point, Quaternion.LookRotation(hit.normal), radius);
            }
        }
        SceneView.RepaintAll();
    }

    void OnInspectorUpdate()
    {
        // Repaint();
    }

    public void setGroup(GameObject group)
    {
		this.group = group;
    }
}

