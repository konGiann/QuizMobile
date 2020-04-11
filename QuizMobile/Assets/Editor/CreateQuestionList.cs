using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateQuestionList 
{
    [MenuItem("Questions/Create/Question List")]
    public static QuestionList Create()
    {
        QuestionList asset = ScriptableObject.CreateInstance<QuestionList>();

        AssetDatabase.CreateAsset(asset, "Assets/QuestionList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
