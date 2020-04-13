using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class QuestionCategoryStats : EditorWindow
{
    bool showEasyQuestions = true;
    bool showNormalQuestions = true;
    bool showHardQuestions = true;

    public QuestionList questionList;

    [MenuItem("Example/ColumnsWindow")]
    public static void Create()
    {
        var instance = CreateInstance<QuestionCategoryStats>();
        instance.titleContent = new GUIContent("Columns");
        instance.Show();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Open list"))
        {
            OpenQuestionList();
        }

        if (GUILayout.Button("Expand all"))
        {
            showEasyQuestions = true;
            showNormalQuestions = true;
            showHardQuestions = true;
        }

        if (GUILayout.Button("Collapse all"))
        {
            showEasyQuestions = false;
            showNormalQuestions = false;
            showHardQuestions = false;
        }

        GUILayout.EndHorizontal();


        GUILayout.BeginVertical();
        showEasyQuestions = EditorGUILayout.Foldout(showEasyQuestions, "Easy Questions");
        if (showEasyQuestions)
        {
            if (questionList != null)
            {
                foreach (var question in questionList.questionList.Where(x => x.Difficulty == QuestionDifficulty.EASY))
                {
                    EditorGUILayout.LabelField(question.Text);
                }
            }
        }

        showNormalQuestions = EditorGUILayout.Foldout(showNormalQuestions, "Normal Questions");
        if (showNormalQuestions)
        {
            if (questionList != null)
            {
                foreach (var question in questionList.questionList.Where(x => x.Difficulty == QuestionDifficulty.NORMAL))
                {
                    EditorGUILayout.LabelField(question.Text);
                }
            }
        }

        showHardQuestions = EditorGUILayout.Foldout(showHardQuestions, "Hard Questions");
        if (showHardQuestions)
        {
            if (questionList != null)
            {
                foreach (var question in questionList.questionList.Where(x => x.Difficulty == QuestionDifficulty.HARD))
                {
                    EditorGUILayout.LabelField(question.Text);
                }
            }
        }

        GUILayout.EndVertical();
    }

    private void OnInspectorUpdate()
    {
        this.Repaint();
    }

    void OpenQuestionList()
    {
        string absPath = EditorUtility.OpenFilePanel("Επιλογή λίστας ερωτήσεων", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            questionList = AssetDatabase.LoadAssetAtPath(relPath, typeof(QuestionList)) as QuestionList;
            //if (questionList.questionList == null)
            //    questionList.questionList = new List<Question>();
            if (questionList)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }
}
