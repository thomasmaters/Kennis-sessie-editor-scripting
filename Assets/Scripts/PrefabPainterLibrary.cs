using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabPainterLibrary
{

    private List<LibraryItem> libraryItemList = new List<LibraryItem>();
    private Vector2 libraryScrollPos;
    private float iconSize = 100;

    private GeoPainterMenu window;
    private int instanceID;

    public PrefabPainterLibrary(GeoPainterMenu window)
    {
        instanceID = window.GetInstanceID();
        this.window = window;
    }

    public List<GameObject> getSelectedLibraryItems()
    {
        List<GameObject> returnList = new List<GameObject>();
        foreach (LibraryItem item in libraryItemList)
        {
            if (item.selected)
            {
                returnList.Add(item.prefab);
            }
        }
        return returnList;
    }

    public void drawGUI()
    {
        if (Event.current.commandName == "ObjectSelectorUpdated" && EditorGUIUtility.GetObjectPickerControlID() == instanceID)
        {
            Object selection = EditorGUIUtility.GetObjectPickerObject();
            if (selection as GameObject != null)
            {
                libraryItemList.Add(new LibraryItem(selection as GameObject, this));
                instanceID = -1;
            }
        }

		GUILayout.Label("Library", EditorStyles.boldLabel);        
		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Add prefab to library"))
		{
			instanceID = window.GetInstanceID();
			EditorGUIUtility.ShowObjectPicker<GameObject>(null, false, "", instanceID);
		}
		EditorGUILayout.EndHorizontal();
		GUILayout.Label ("Scale: ");
        iconSize = EditorGUILayout.Slider(iconSize, 48, 128);

        float windowWidth = EditorGUIUtility.currentViewWidth;
        int maxInWidth = (int)Mathf.Floor(windowWidth / iconSize);
        int maxInHeight = (int)Mathf.Ceil(((float)libraryItemList.Count) / maxInWidth);
        libraryScrollPos = EditorGUILayout.BeginScrollView(libraryScrollPos, false, false, GUILayout.Width(windowWidth), GUILayout.Height(iconSize * maxInHeight), GUILayout.MaxHeight(400));

        EditorGUILayout.BeginVertical();
        for (int i = 0; i < maxInHeight; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = i * maxInWidth; j < i * maxInWidth + maxInWidth; j++)
            {
                if (j < libraryItemList.Count)
                {
                    libraryItemList[j].Draw(new Vector2(iconSize, iconSize));
                }
                else
                {
                    GUILayout.Button("", GUIStyle.none, GUILayout.Width(iconSize), GUILayout.Height(iconSize));
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();

    }

    public void removeFromLibrary(LibraryItem item)
    {
        libraryItemList.Remove(item);
    }

    void Update()
    {

    }

    public List<LibraryItem> getLibraryItemList()
    {
        return libraryItemList;
    }

    public void setLibraryItemList(List<LibraryItem> libraryItemList)
    {
        this.libraryItemList = libraryItemList;
    }
}

public class LibraryItem
{
    public bool selected = false;
    public GameObject prefab;

    protected Texture preview;
    private GUIStyle buttonStyle;
    private GUIStyle labelStyle;
    private PrefabPainterLibrary parent;

    public LibraryItem(GameObject aPrefab, PrefabPainterLibrary aParent)
    {
        prefab = aPrefab;
        parent = aParent;
        updatePreview();
    }

    public void updatePreview()
    {
        if (prefab != null)
        {
            preview = AssetPreview.GetAssetPreview(prefab as Object);
        }
    }

    public void Draw(Vector2 size)
    {
        if (AssetPreview.IsLoadingAssetPreview(prefab.GetInstanceID()))
        {
            updatePreview();
        }
        if (preview == null)
        {
            preview = AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(prefab));
        }
        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.margin = new RectOffset();

        labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.margin = new RectOffset();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(prefab.name, GUILayout.Width(size.x - 28));

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button(" X ", buttonStyle, GUILayout.Width(20)))
        {
            parent.removeFromLibrary(this);
        }

        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button(preview, selected ? buttonStyle : GUIStyle.none, GUILayout.Width(size.x), GUILayout.Height(size.y)))
        {
            selected = !selected;
        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.EndVertical();
    }
}