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
        //TO-DO: Toon bijbehorende editing GUI van Paint en Library
    }

    private void savePrefabPainterSettings(GameObject group)
    {
        group.GetComponent<Group>().radius = prefabPainter.radius;
    }

    private void loadPrefabPainterSettings(GameObject group)
    {
        prefabPainter.radius = group.GetComponent<Group>().radius;
    }

    private void deleteGroup(GameObject group)
    {
        groups.Remove(group);
        UnityEngine.Object.DestroyImmediate(group);
    }
}
