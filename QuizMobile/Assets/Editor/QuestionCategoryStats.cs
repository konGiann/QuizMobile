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

    [MenuItem("Ερωτήσεις/Στατιστικά Ερωτήσεων")]
    public static void Create()
    {
        var instance = CreateInstance<QuestionCategoryStats>();
        instance.titleContent = new GUIContent("Columns");
        instance.Show();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Άνοιγμα λίστας"))
        {
            OpenQuestionList();
        }

        if (GUILayout.Button("Ανάπτυξη όλων"))
        {
            showEasyQuestions = true;
            showNormalQuestions = true;
            showHardQuestions = true;
        }

        if (GUILayout.Button("Σύμπτυξη όλων"))
        {
            showEasyQuestions = false;
            showNormalQuestions = false;
            showHardQuestions = false;
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        
        // show list  asset name
        string relPath = AssetDatabase.GetAssetPath(questionList);        
        EditorGUILayout.LabelField("Όνομα αρχείου: ", relPath);

        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();

        GUILayout.Label("ΣΥΝΟΛΙΚΕΣ ΕΡΩΤΗΣΕΙΣ: " + questionList.questionList.Count());

        GUILayout.Space(10);
        showEasyQuestions = EditorGUILayout.Foldout(showEasyQuestions, "Easy Questions" + "(" + questionList.questionList.Count(x => x.Difficulty == QuestionDifficulty.EASY) + ")");
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

        GUILayout.Space(10);
        showNormalQuestions = EditorGUILayout.Foldout(showNormalQuestions, "Normal Questions" + "(" + questionList.questionList.Count(x => x.Difficulty == QuestionDifficulty.NORMAL) + ")" );
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

        GUILayout.Space(10);
        showHardQuestions = EditorGUILayout.Foldout(showHardQuestions, "Hard Questions" + "(" + questionList.questionList.Count(x => x.Difficulty == QuestionDifficulty.HARD) + ")");
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
