using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PrefabPainter : EditorWindow {

	bool paintMode = false;
	GameObject prefab;	
	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/PrefabPainter")]
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
	}

	void OnGUI()
	{
		GUILayout.Label("Prefab Painter", EditorStyles.boldLabel);
		GUILayout.Label("Paint Mode: ");
		paintMode = EditorGUILayout.Toggle (paintMode);
		GUILayout.Label("Prefab: ");
		prefab = EditorGUILayout.ObjectField(prefab, typeof(GameObject), true) as GameObject;

		/*groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
		myBool = EditorGUILayout.Toggle("Toggle", myBool);
		myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);

		EditorGUILayout.EndToggleGroup();*/               
	}
	void CustomUpdate (SceneView sv){
		if (paintMode) {
			Event e = Event.current;
			if ((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0) {
				RaycastHit hit;
				Tools.current = Tool.View;
				int layer = 1 << 8;		

				if (Physics.Raycast (Camera.current.ScreenPointToRay (new Vector3 (e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0)), out hit, Mathf.Infinity, layer)) {
					GameObject placedObject = (GameObject)PrefabUtility.InstantiatePrefab (prefab);
					//place object on mouse pos
					//TO-DO edit random ranges
					placedObject.transform.localScale = new Vector3 (1, 1, 1) * Random.Range (0.5f, 2.0f);
					placedObject.transform.position = hit.point;
					placedObject.transform.position = new Vector3 (placedObject.transform.position.x, placedObject.transform.position.y + (placedObject.transform.localScale.y / 2), placedObject.transform.position.z);

					e.Use ();
					Undo.RegisterCreatedObjectUndo (placedObject, "undo prefab paint");
				}
			}
		}
	}

	void OnInspectorUpdate() {
		Repaint();
	}
}
