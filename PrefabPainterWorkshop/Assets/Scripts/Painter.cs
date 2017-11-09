using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Painter
{
    public int paintLayer = 8;
    public bool paintMode = false;
    public float eraseRadius = 10;
    public float placementRadius = 9;
    double timeElapsed = 0;
    List<GameObject> prefabs;
    // GameObject prefab;
    public Vector3 randomScaleMin;
    public Vector3 randomScaleMax;
    public Vector3 randomRotationMin;
    public Vector3 randomRotationMax;
    private GameObject group;
    private PrefabPainterLibrary prefabLibrary;

    public Painter(PrefabPainterLibrary prefabLibrary)
    {
        this.prefabLibrary = prefabLibrary;
        randomScaleMin = new Vector3(0.5f, 0.5f, 0.5f);
        randomScaleMax = new Vector3(2, 2, 2);
    }

    public void DrawPainterGUI()
    {
        GUILayout.Label("Prefab Painter", EditorStyles.boldLabel);
        GUILayout.Label("LMB Drag to paint, CTRL + LMB Drag to erase");
        GUILayout.Label("Paint Mode: ");
        paintMode = EditorGUILayout.Toggle(paintMode);
        GUILayout.Label("Paint Layer ");
        paintLayer = EditorGUILayout.IntField(paintLayer);
        GUILayout.Label("Placement Radius (Blue): ");
        placementRadius = EditorGUILayout.Slider(placementRadius, 0, 100);
    }

   

    public void CustomUpdate(SceneView sv)
    {
        prefabs = prefabLibrary.getSelectedLibraryItems();

        if (paintMode)
        {
            drawHandles();
            Event e = Event.current;
            if (((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0) && !e.control)
            {
                RaycastHit hit;
                Tools.current = Tool.View;
                int layer = 1 << paintLayer;
                Vector3 mousePos = new Vector3(e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0);
                if (Physics.Raycast(Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0)), out hit, Mathf.Infinity, layer))
                {
                    if (prefabs.Count > 0)
                    {
                            GameObject prefab = getRandomPrefab(prefabs);
                                GameObject placedObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                                //scale

                                //rotation

                                Vector2 randomPos = (Random.insideUnitCircle * placementRadius);
                                RaycastHit placedObjectHit;
                                placedObject.transform.position = new Vector3(hit.point.x + randomPos.x, hit.point.y + (placedObject.transform.localScale.y / 2), hit.point.z + randomPos.y);
								//second position to fix floating objects
							if (Physics.Raycast (placedObject.transform.position, Vector3.down, out placedObjectHit, Mathf.Infinity, layer)) {
								placedObject.transform.position = new Vector3 (placedObject.transform.position.x, placedObjectHit.point.y + (placedObject.transform.localScale.y / 2), placedObject.transform.position.z);
								placedObject.transform.parent = group.transform;
							
								Undo.RegisterCreatedObjectUndo (placedObject, "undo prefab paint");
							} else {
								Object.DestroyImmediate (placedObject);
							}
                            
                        
                        e.Use();
                    }
                }
            }
            else if (((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0) && e.control)
            {
                RaycastHit[] hits;
                Tools.current = Tool.View;

                int layer = 1 << paintLayer;
                Vector3 mousePos = new Vector3(e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0);
                hits = Physics.SphereCastAll(Camera.current.ScreenPointToRay(mousePos), eraseRadius, layer);
                if (hits.Length != 0)
                {
                    foreach (RaycastHit hit in hits)
                    {
						if (hit.transform.parent == group.transform) {
							if(prefabs.FindIndex(i => i.name == hit.transform.gameObject.name) > -1) 
								Undo.DestroyObjectImmediate ((Object)hit.transform.gameObject);
						}
                    }
                    e.Use();
                }
            }
        }
    }

    GameObject getRandomPrefab(List<GameObject> prefabs)
    {
        int index = Random.Range(0, prefabs.Count);
        return prefabs[index];
    }

    void drawHandles()
    {
        RaycastHit hit;
        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        int layerMask = 1 << paintLayer;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
        {
            if (placementRadius != 0)
            {
                Handles.color = Color.blue;
                Handles.CircleCap(1, hit.point, Quaternion.LookRotation(hit.normal), placementRadius);
            }
        }
        SceneView.RepaintAll();
    }

    // void OnInspectorUpdate()
    // {
    //     Repaint();
    // }

    public void setGroup(GameObject group)
    {
        this.group = group;
    }

	public void OnDestroy(){
		paintMode = false;
	}
}