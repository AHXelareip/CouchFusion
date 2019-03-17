using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CFMainMenuManager : MonoBehaviour
{
    public string sceneFolder;
    public GameObject firstButton;
    public CFSceneLoader sceneLoader;
    
	void Start ()
    {
        string[] scenes = AssetDatabase.FindAssets("", new string[] { sceneFolder });

        for (int i = 0; i < scenes.Length; ++i)
        {
            string name = AssetDatabase.GUIDToAssetPath(scenes[i]);

            string[] split = name.Split(new char[] { '/' });
            name = split[split.Length - 1].Replace(".unity", "");

            GameObject newButton = Instantiate(firstButton, firstButton.transform.parent);
            RectTransform rectTransform = newButton.transform as RectTransform;
            rectTransform.anchoredPosition = new Vector2(0, -15 - i * 40);
            newButton.GetComponentInChildren<Text>().text = name;
            newButton.GetComponentInChildren<Button>().onClick.AddListener(() => sceneLoader.LoadScene(name));
            newButton.SetActive(true);
        }
        Destroy(firstButton);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
