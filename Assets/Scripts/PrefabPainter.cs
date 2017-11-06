using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabPainter
{
	public int paintLayer = 8;
	public bool paintMode = false;
	public bool instantiateMeshOnly = false;
	public float eraseRadius = 10;
	public float placementRadius = 9;
	public double intensity = 0.01;
	private double timer = 0;
	double timeElapsed = 0;
	List<GameObject> prefabs;
	public Vector3 randomScaleMin;
	public Vector3 randomScaleMax;
	public Vector3 randomRotationMin;
	public Vector3 randomRotationMax;
	private GameObject group;
	private PrefabPainterLibrary lib;

	public PrefabPainter(PrefabPainterLibrary _lib){
		prefabs = new List<GameObject> ();
		prefabs = _lib.getSelectedLibraryItems();
		randomScaleMin = new Vector3 (0.5f, 0.5f, 0.5f);
		randomScaleMax = new Vector3 (2, 2, 2);
	}

	public void DrawPainterGUI()
	{

		GUILayout.Label("Prefab Painter", EditorStyles.boldLabel);
		GUILayout.Label("LMB Drag to paint, CTRL + LMB Drag to erase");
		GUILayout.Label("Paint Mode: ");
		paintMode = EditorGUILayout.Toggle (paintMode);
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

	bool placementReady()
	{
		Debug.Log(timeElapsed);
		return (timeElapsed > timer);
	}

	public void CustomUpdate (SceneView sv){
		timeElapsed += Time.deltaTime;
		if (paintMode) {
			Debug.Log (prefabs);
			drawHandles ();
			Event e = Event.current;
			if (((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0) && !e.control) {
				RaycastHit hit;
				Tools.current = Tool.View;
				int layer = 1 << paintLayer;		
				Vector3 mousePos = new Vector3 (e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0);
				if (Physics.Raycast (Camera.current.ScreenPointToRay (new Vector3 (e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0)), out hit, Mathf.Infinity, layer)) {
					if (prefabs.Count > 0 ) {
						if (instantiateMeshOnly) {
							//Graphics.DrawMesh (prefab.GetComponent<MeshFilter> ().sharedMesh, mousePos, Quaternion.Euler(0,0,0),layer);
						} else {
							GameObject prefab = getRandomPrefab (prefabs);
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
								placedObject.transform.parent = group.transform;
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

	GameObject getRandomPrefab(List<GameObject> prefabs){
		int index = Random.Range (0, prefabs.Count);
		Debug.Log ("PC: " + prefabs.Count);
		Debug.Log ("i: " + index);
		return prefabs [index-1];
	}

	void drawHandles()
	{
		RaycastHit hit;
		var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
		int layerMask = 1 << 8;
		if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
		{
			if (placementRadius != 0)
			{
				Handles.color = Color.blue;
				Handles.CircleCap(1, hit.point, Quaternion.LookRotation(hit.normal), placementRadius);
			}
			if (eraseRadius != 0)
			{
				Handles.color = Color.red;
				Handles.CircleCap(2, hit.point, Quaternion.LookRotation(hit.normal), eraseRadius);
			}
		}
		SceneView.RepaintAll();
	}

	// void OnInspectorUpdate()
	// {
	//     Repaint();
	// }

	public void setTimer(float time)
	{
		timer = time;
	}

	public void setGroup(GameObject group)
	{
		this.group = group;
	}
}