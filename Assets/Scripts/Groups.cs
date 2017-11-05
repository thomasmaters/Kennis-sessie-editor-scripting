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

        foreach (GameObject group in groups)
        {
            EditorGUILayout.BeginHorizontal();
            string myLabel = "" + (groupIndex + 1) + ": ";
            if (GUILayout.Button("Edit"))
            {
                toggleIsEditing(group);
                editGroup();
            }
            if (group.GetComponent<Group>().isEditing())
            {
                myLabel += " (EDIT)";
            }
			if (GUILayout.Button("Delete"))
			{
				deleteGroup(group);
			}
            group.name = EditorGUILayout.TextField(myLabel, text: group.name);
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
