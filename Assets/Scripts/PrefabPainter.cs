using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabPainter
{

    public bool paintMode = false;
    public bool instantiateMeshOnly = false;
    public float eraseRadius = 10;
    public float placementRadius = 9;
    public double intensity = 0.01;
    private double timer = 0;
    double timeElapsed = 0;
    GameObject prefab;
    private GameObject group;


    public void DrawPainterGUI()
    {

        GUILayout.Label("Prefab Painter", EditorStyles.boldLabel);
        GUILayout.Label("Paint Mode: ");
        paintMode = EditorGUILayout.Toggle(paintMode);
        GUILayout.Label("Prefab: ");
        prefab = EditorGUILayout.ObjectField(prefab, typeof(GameObject), true) as GameObject;
        GUILayout.Label("Instantiate Mesh Only: ");
        instantiateMeshOnly = EditorGUILayout.Toggle(instantiateMeshOnly);
        GUILayout.Label("Placement Radius (Blue): ");
        placementRadius = EditorGUILayout.Slider(placementRadius, 0, 100);
        GUILayout.Label("Erase Radius (Red): ");
        eraseRadius = EditorGUILayout.Slider(eraseRadius, 0, 100);
        GUILayout.Label("Placement Timer: ");
        intensity = EditorGUILayout.DoubleField(intensity);
    }

    bool placementReady()
    {
        Debug.Log(timeElapsed);
        return (timeElapsed > timer);
    }

    public void CustomUpdate(SceneView sv)
    {
        timeElapsed += Time.deltaTime;

        if (paintMode)
        {
            drawHandles();
            Event e = Event.current;
            if (((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0) && !e.control)
            {
                RaycastHit hit;
                Tools.current = Tool.View;
                int layer = 1 << 8;
                Vector3 mousePos = new Vector3(e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0);
                if (Physics.Raycast(Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0)), out hit, Mathf.Infinity, layer))
                {
                    if (prefab != null)
                    {
                        if (instantiateMeshOnly)
                        {
                            //Graphics.DrawMesh (prefab.GetComponent<MeshFilter> ().sharedMesh, mousePos, Quaternion.Euler(0,0,0),layer);
                        }
                        else
                        {
                            if (placementReady())
                            {
                                GameObject placedObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                                placedObject.transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.5f, 2.0f);

                                Vector2 randomPos = (Random.insideUnitCircle * placementRadius);
                                placedObject.transform.position = new Vector3(hit.point.x + randomPos.x, hit.point.y + (placedObject.transform.localScale.y / 2), hit.point.z + randomPos.y);
                                Undo.RegisterCreatedObjectUndo(placedObject, "undo prefab paint");
                                placedObject.transform.parent = group.transform;
                                timer = timeElapsed + intensity;
                            }
                        }
                        e.Use();
                    }
                }
            }
            else if (((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0) && e.control)
            {
                RaycastHit[] hits;
                Tools.current = Tool.View;

                int layer = 1 << 8;
                Vector3 mousePos = new Vector3(e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0);
                hits = Physics.SphereCastAll(Camera.current.ScreenPointToRay(mousePos), eraseRadius, layer);

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
