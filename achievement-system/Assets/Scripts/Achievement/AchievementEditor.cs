using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AchievementEditor : EditorWindow
{
    public AchievementList achievementList;
    private int viewIndex = 1;

    [MenuItem("Window/Achievement Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(AchievementEditor));
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            achievementList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(AchievementList)) as AchievementList;
        }
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Achievement Editor", EditorStyles.boldLabel);
        if (achievementList != null)
        {
            if (GUILayout.Button("Show Achievement List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = achievementList;
            }
        }
        if (GUILayout.Button("Open Achievement List"))
        {
            OpenAchievementList();
        }
        if (GUILayout.Button("New Achievement List"))
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = achievementList;
        }
        GUILayout.EndHorizontal();

        if (achievementList == null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Achievement List", GUILayout.ExpandWidth(false)))
            {
                CreateNewAchievementList();
            }
            if (GUILayout.Button("Open Existing Achievement List", GUILayout.ExpandWidth(false)))
            {
                OpenAchievementList();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (achievementList != null)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 1)
                    viewIndex--;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < achievementList.achievements.Count)
                {
                    viewIndex++;
                }
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Add Achievement", GUILayout.ExpandWidth(false)))
            {
                AddAchievement();
            }
            if (GUILayout.Button("Delete Achievement", GUILayout.ExpandWidth(false)))
            {
                DeleteAchievement(viewIndex - 1);
            }

            GUILayout.EndHorizontal();
            if (achievementList.achievements == null)
                Debug.Log("Achievement is empty");
            if (achievementList.achievements.Count > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Achievement", viewIndex, GUILayout.ExpandWidth(false)), 1, achievementList.achievements.Count);
                EditorGUILayout.LabelField("of   " + achievementList.achievements.Count.ToString() + "  achievements", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                achievementList.achievements[viewIndex - 1].ID = EditorGUILayout.IntField("ID", achievementList.achievements[viewIndex - 1].ID);
                achievementList.achievements[viewIndex - 1].Tittle = EditorGUILayout.TextField("Tittle", achievementList.achievements[viewIndex - 1].Tittle as string);
                achievementList.achievements[viewIndex - 1].Description = EditorGUILayout.TextField("Description", achievementList.achievements[viewIndex - 1].Description as string);

                GUILayout.Space(10);

                achievementList.achievements[viewIndex - 1].Img = EditorGUILayout.ObjectField("Image", achievementList.achievements[viewIndex - 1].Img, typeof(Sprite), false) as Sprite;
                achievementList.achievements[viewIndex - 1].BackgroundImg = EditorGUILayout.ObjectField("Background Image", achievementList.achievements[viewIndex - 1].BackgroundImg, typeof(Sprite), false) as Sprite;

                GUILayout.Space(10);

                achievementList.achievements[viewIndex - 1].Unlocked = (bool)EditorGUILayout.Toggle("Unlocked", achievementList.achievements[viewIndex - 1].Unlocked, GUILayout.ExpandWidth(false));
            }
            else
            {
                GUILayout.Label("This Achievement List is Empty.");
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(achievementList);
        }
    }

    void CreateNewAchievementList()
    {
        viewIndex = 1;
        achievementList = CreateAchievementList.Create();
        if (achievementList)
        {
            achievementList.achievements = new List<AchievementData>();
            string relPath = AssetDatabase.GetAssetPath(achievementList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenAchievementList()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Achievement Item List", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            achievementList = AssetDatabase.LoadAssetAtPath(relPath, typeof(AchievementList)) as AchievementList;
            if (achievementList.achievements == null)
                achievementList.achievements = new List<AchievementData>();
            if (achievementList)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddAchievement()
    {
        AchievementData newItem = new AchievementData();
        newItem.Description = "New Achievement";
        achievementList.achievements.Add(newItem);
        viewIndex = achievementList.achievements.Count;
    }

    void DeleteAchievement(int index)
    {
        achievementList.achievements.RemoveAt(index);
    }
}