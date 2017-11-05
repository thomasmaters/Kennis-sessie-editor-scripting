using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PrefabPainter : EditorWindow {

	int paintLayer = 8;
	bool paintMode = false;
	bool instantiateMeshOnly = false;
	float eraseRadius = 10;
	float placementRadius = 9;
	double intensity = 1;
	double timer = 0;
	double timeElapsed = 0;
	Vector3 randomScaleMin;
	Vector3 randomScaleMax;
	Vector3 randomRotationMin;
	Vector3 randomRotationMax;
	GameObject prefab;	
	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/PPPainter")]
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

		timer = Time.time;
		randomScaleMin = new Vector3 (0.5f, 0.5f, 0.5f);
		randomScaleMax = new Vector3 (2, 2, 2);

	}


	void DrawPainterGUI(){
		GUILayout.Label("Prefab Painter", EditorStyles.boldLabel);
		GUILayout.Label("LMB Drag to paint, CTRL + LMB Drag to erase");
		GUILayout.Label("Paint Mode: ");
		paintMode = EditorGUILayout.Toggle (paintMode);
		GUILayout.Label("Prefab: ");
		prefab = EditorGUILayout.ObjectField(prefab, typeof(GameObject), true) as GameObject;
		GUILayout.Label("Paint Layer ");
		paintLayer = EditorGUILayout.IntField (paintLayer);
		GUILayout.Label("Instantiate Mesh Only: ");
		instantiateMeshOnly = EditorGUILayout.Toggle (instantiateMeshOnly);
		GUILayout.Label("Placement Radius (Blue): ");
		placementRadius = EditorGUILayout.Slider (placementRadius, 0, 100);
		GUILayout.Label("Erase Radius (Red): ");
		eraseRadius = EditorGUILayout.Slider (eraseRadius, 0, 100);
		GUILayout.Label ("Brush Intensity: ");
		intensity = EditorGUILayout.DoubleField (intensity);
		randomScaleMin = EditorGUILayout.Vector3Field ("Random Scale min. :", randomScaleMin);
		randomScaleMax = EditorGUILayout.Vector3Field ("Random Scale max. :", randomScaleMax);
		randomRotationMin = EditorGUILayout.Vector3Field ("Random Rotation min. :", randomRotationMin);
		randomRotationMax = EditorGUILayout.Vector3Field ("Random Rotation max. :", randomRotationMax);
	}
	void OnGUI()
	{
		DrawPainterGUI ();
	}

	bool placementReady(){
		return (timeElapsed > timer);
	}

	void CustomUpdate (SceneView sv){
		timeElapsed += Time.deltaTime;

		if (paintMode) {
			drawHandles ();
			Event e = Event.current;
			if (((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0) && !e.control) {
				RaycastHit hit;
				Tools.current = Tool.View;
				int layer = 1 << paintLayer;		
				Vector3 mousePos = new Vector3 (e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0);
				if (Physics.Raycast (Camera.current.ScreenPointToRay (new Vector3 (e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0)), out hit, Mathf.Infinity, layer)) {
					if (prefab != null) {
						if (instantiateMeshOnly) {
							//Graphics.DrawMesh (prefab.GetComponent<MeshFilter> ().sharedMesh, mousePos, Quaternion.Euler(0,0,0),layer);
						} else {
							if (placementReady ()) {	
								GameObject placedObject = (GameObject)PrefabUtility.InstantiatePrefab (prefab);
								//scale
								float randomScaleX = Random.Range (randomScaleMin.x, randomScaleMax.x);
								float randomScaleY = Random.Range (randomScaleMin.y, randomScaleMax.y);
								float randomScaleZ = Random.Range (randomScaleMin.z, randomScaleMax.z);
								
								placedObject.transform.localScale = new Vector3 (randomScaleX, randomScaleY, randomScaleZ);
							 
								//rotation
								float randomRotX = Random.Range (randomRotationMin.x, randomRotationMax.x);
								float randomRotY = Random.Range (randomRotationMin.y, randomRotationMax.y);
								float RandomRotZ = Random.Range (randomRotationMin.y, randomRotationMax.y);

								placedObject.transform.rotation = Quaternion.Euler (randomRotX, randomRotY, RandomRotZ); 
								//position

								Vector2 randomPos = (Random.insideUnitCircle * placementRadius);
								placedObject.transform.position = new Vector3 (hit.point.x + randomPos.x, hit.point.y + (placedObject.transform.localScale.y / 2), hit.point.z + randomPos.y);
								Undo.RegisterCreatedObjectUndo (placedObject, "undo prefab paint");
								timer = timeElapsed + intensity;
							}
						}
						e.Use ();
					}
				}
			} 
			else if (((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0) && e.control) {
				RaycastHit[] hits;
				Tools.current = Tool.View;

				int layer = 1 << paintLayer;		
				Vector3 mousePos = new Vector3 (e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0);
				hits = Physics.SphereCastAll (Camera.current.ScreenPointToRay (mousePos), eraseRadius, layer);

				if (hits.Length != 0) {
					foreach (RaycastHit hit in hits) {
						Undo.DestroyObjectImmediate ((Object)hit.transform.gameObject);
					}

					e.Use ();

				}
			}
		} 
	}

	void drawHandles()
	{
		RaycastHit hit;
		var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
		int layerMask = 1 << paintLayer;
		if (Physics.Raycast (ray.origin, ray.direction,out hit, Mathf.Infinity, layerMask))
		{
			if (placementRadius != 0) {
				Handles.color = Color.blue;
				Handles.CircleCap(1,hit.point,Quaternion.LookRotation(hit.normal),placementRadius);
			}
			if(eraseRadius != 0)
			{
				Handles.color = Color.red;
				Handles.CircleCap(2,hit.point,Quaternion.LookRotation(hit.normal),eraseRadius);
			}
		}
		SceneView.RepaintAll ();
	}

	void OnInspectorUpdate() {
		Repaint();
	}
}
