using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabPainterLibrary : EditorWindow {

    private List<LibraryItem> libraryItemList = new List<LibraryItem>();
    public GameObject testPrefeb;
    private Vector2 scrollPos;


    [MenuItem("Window/My Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        PrefabPainterLibrary window = EditorWindow.GetWindow<PrefabPainterLibrary>();
        window.Show();       
    }

    private void Awake()
    {
        libraryItemList.Add(new LibraryItem(testPrefeb));
        libraryItemList.Add(new LibraryItem(testPrefeb));
        libraryItemList.Add(new LibraryItem(testPrefeb));
        libraryItemList.Add(new LibraryItem(testPrefeb));
        libraryItemList.Add(new LibraryItem(testPrefeb));
    }

    void OnGUI()
    {
        float windowWidth = EditorGUIUtility.currentViewWidth;

        
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(400));
        int count = 0;
        foreach (LibraryItem item in libraryItemList)
        {
            count++;
            if (count * 100 > windowWidth)
            {
                count = 0;
                item.Draw();
            }
            else
            {
                item.Draw();

            }
        }

        GUILayout.Button("add");
    }

    void Update () {
		
	}
}

public class LibraryItem
{
    public bool selected = false;

    protected GameObject prefab;
    protected Texture preview;
    private GUIStyle buttonStyle;
    private GUIStyle labelStyle;

    private Texture2D green = Resources.Load("Green.asset") as Texture2D;

    int width = 100;
    int height = 100;

    public LibraryItem(GameObject aPrefab)
    {
        prefab = aPrefab;
        updatePreview();
    }

    public void updatePreview()
    {
        if(prefab != null)
        {
            preview = AssetPreview.GetAssetPreview(prefab as Object);
        }
    }

    public void Draw()
    {
        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.margin = new RectOffset();

        labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.margin = new RectOffset();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(prefab.name, GUILayout.Width(width - 28));

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button(" X ", buttonStyle, GUILayout.Width(20)))
        {

        }

        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button(preview, selected ? buttonStyle : GUIStyle.none, GUILayout.Width(100), GUILayout.Height(100)))
        {
            selected = !selected;
        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.EndVertical();
    }
}
