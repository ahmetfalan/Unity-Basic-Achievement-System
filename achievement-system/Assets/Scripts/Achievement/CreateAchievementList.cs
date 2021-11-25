using UnityEditor;
using UnityEngine;

public class CreateAchievementList : MonoBehaviour
{
    [MenuItem("Assets/Create/Achievement List")]
    public static AchievementList Create()
    {
        AchievementList asset = ScriptableObject.CreateInstance<AchievementList>();

        AssetDatabase.CreateAsset(asset, "Assets/AchievementList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
