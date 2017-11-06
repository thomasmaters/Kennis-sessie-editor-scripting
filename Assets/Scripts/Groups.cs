using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Groups
{
    private List<GameObject> groups = new List<GameObject>();
    private PrefabPainter prefabPainter;
    private GameObject previousEditGroup;

    public Groups(PrefabPainter prefabPainter)
    {
        this.prefabPainter = prefabPainter;
		List<GameObject> existingGroups = Object.FindObjectsOfType<Group> ();
		foreach(GameObject group in existingGroups){
			existingGroups.Add (group);
		}
    }

    public void renderGUI()
    {
        GUILayout.Label("Groups");
        if (GUILayout.Button("+"))
        {
            addGroup();
        }
        renderGroupsGUI();
    }

    private void renderGroupsGUI()
    {
        int groupIndex = 0;

        for (int i = groups.Count - 1; i >= 0; i--)
        {
            EditorGUILayout.BeginHorizontal();
            string myLabel = "" + (groupIndex + 1) + ": ";
            if (GUILayout.Button("Edit"))
            {
                editGroup(groups[i]);
            }
            if (GUILayout.Button("Delete"))
            {
                deleteGroup(groups[i]);
            }
            else
            {
                if (groups[i].GetComponent<Group>().isEditing())
                {
                    myLabel += " (EDIT)";
                }
                groups[i].name = EditorGUILayout.TextField(myLabel, text: groups[i].name);
            }
            groupIndex++;
            EditorGUILayout.EndHorizontal();
        }
    }

    private void toggleIsEditing(GameObject editGroup)
    {
        foreach (GameObject group in groups)
        {
            group.GetComponent<Group>().isEditing(false);
        }
        editGroup.GetComponent<Group>().isEditing(true);
    }

    private void addGroup()
    {
        GameObject group = new GameObject("Test group");
        group.AddComponent<Group>();
        groups.Add(group);
    }

    private void editGroup(GameObject group)
    {
        if (previousEditGroup != null)
        {
            savePrefabPainterSettings(previousEditGroup);
            loadPrefabPainterSettings(group);
        }
        toggleIsEditing(group);
        prefabPainter.setGroup(group);
        previousEditGroup = group;
    }

    private void savePrefabPainterSettings(GameObject group)
    {
        group.GetComponent<Group>().placementRadius = prefabPainter.placementRadius;
        group.GetComponent<Group>().paintMode = prefabPainter.paintMode;
        group.GetComponent<Group>().instantiateMeshOnly = prefabPainter.instantiateMeshOnly;
        group.GetComponent<Group>().eraseRadius = prefabPainter.eraseRadius;
        group.GetComponent<Group>().intensity = prefabPainter.intensity;
    }

    private void loadPrefabPainterSettings(GameObject group)
    {
        prefabPainter.placementRadius = group.GetComponent<Group>().placementRadius;
        prefabPainter.paintMode = group.GetComponent<Group>().paintMode;
        prefabPainter.instantiateMeshOnly = group.GetComponent<Group>().instantiateMeshOnly;
        prefabPainter.eraseRadius = group.GetComponent<Group>().eraseRadius;
        prefabPainter.intensity = group.GetComponent<Group>().intensity;
    }

    private void deleteGroup(GameObject group)
    {
        groups.Remove(group);
        UnityEngine.Object.DestroyImmediate(group);
    }
}
