using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Groups
{
    private List<GameObject> groups = new List<GameObject>();

    public Groups()
    {

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
                toggleIsEditing(groups[i]);
                editGroup();
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

    private void editGroup()
    {
        //TO-DO: Toon bijbehorende editing GUI van Paint en Library
    }

    private void deleteGroup(GameObject group)
    {
        groups.Remove(group);
        UnityEngine.Object.DestroyImmediate(group);
    }
}
