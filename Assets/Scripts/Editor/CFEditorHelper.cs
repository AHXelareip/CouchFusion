using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CFEditorHelper : EditorWindow
{
    private static CFEditorHelper instance;
    public static CFEditorHelper Instance
    {
        get
        {
            return instance;
        }
    }

    private static string sceneFolder = "Assets/Scenes/Levels";

    private bool foldLevels;
    private string[] levels;

    [MenuItem("CouchFusion/Management")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CFEditorHelper window = (CFEditorHelper)GetWindow(typeof(CFEditorHelper));
        window.InitWindow();
        window.Show();

    }

    void InitWindow()
    {
        levels = AssetDatabase.FindAssets("", new string[] { sceneFolder });
    }

    void InitSkin()
    {

    }

    void OnGUI()
    {
        InitSkin();
        GUILayout.BeginVertical();
        GUILayout.Label("Jump height : 2.5");

        if (GUILayout.Button("Main menu"))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity", OpenSceneMode.Single);
        }

        foldLevels = EditorGUILayout.Foldout(foldLevels, "Levels");
        if (foldLevels)
        {
            for (int i = 0; i < levels.Length; ++i)
            {
                string name = AssetDatabase.GUIDToAssetPath(levels[i]);

                string[] split = name.Split(new char[] { '/' });
                name = split[split.Length - 1].Replace(".unity", "");

                if (GUILayout.Button(name))
                {
                    LoadLevel(name);
                }
            }
        }
        GUILayout.EndVertical();
    }

    void LoadLevel(string name)
    {
        EditorSceneManager.OpenScene("Assets/Scenes/PlayableScene.unity", OpenSceneMode.Single);
        EditorSceneManager.OpenScene(sceneFolder + "/" + name + ".unity", OpenSceneMode.Additive);
    }
}
