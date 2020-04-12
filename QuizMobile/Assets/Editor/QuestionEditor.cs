using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class QuestionEditor : EditorWindow
{

    public QuestionList questionList;
    private int viewIndex = 1;

    [MenuItem("Ερωτήσεις/Επεξεργασία Ερωτήσεων")]
    static void Init()
    {
        GetWindow(typeof(QuestionEditor));
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            questionList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(QuestionList)) as QuestionList;
        }

    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Επεξεργαστής Ερωτήσεων", EditorStyles.boldLabel);
        if (questionList != null)
        {
            if (GUILayout.Button("Δείξε την Λίστα στο Project"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = questionList;
            }
        }
        if (GUILayout.Button("Άνοιγμα λίστας ερωτήσεων"))
        {
            OpenItemList();
        }
        if (GUILayout.Button("Νέα λίστα ερωτήσεων"))
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = questionList;
        }
        GUILayout.EndHorizontal();

        if (questionList == null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Δημιουργία λίστας ερωτήσεων", GUILayout.ExpandWidth(false)))
            {
                CreateNewQuestionList();
            }
            if (GUILayout.Button("Άνοιγμα υπάρχουσας λίστας", GUILayout.ExpandWidth(false)))
            {
                OpenItemList();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (questionList != null)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Προηγ.", GUILayout.ExpandWidth(false)))
            {
                if (!Array.Exists(questionList.questionList[viewIndex - 1].Answers, element => element.isCorrect == true) ||
                    questionList.questionList[viewIndex - 1].Answers.Count(x => x.isCorrect == true) > 1)
                {
                    EditorUtility.DisplayDialog("Ουπς!", "Σημείωσε τουλάχιστον μια σωστή απάντηση και όχι παραπάνω!", "OK");
                }
                else
                {
                    if (viewIndex > 1)
                        viewIndex--;
                }
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Επόμ.", GUILayout.ExpandWidth(false)))
            {
                if (!Array.Exists(questionList.questionList[viewIndex - 1].Answers, element => element.isCorrect == true) ||
                    questionList.questionList[viewIndex - 1].Answers.Count(x => x.isCorrect == true) > 1 )
                {
                    EditorUtility.DisplayDialog("Ουπς!", "Σημείωσε τουλάχιστον μια σωστή απάντηση και όχι παραπάνω!", "OK");
                }                
                else
                {
                    if (viewIndex < questionList.questionList.Count)
                    {
                        viewIndex++;
                    }
                }                
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Προσθήκη ερώτησης", GUILayout.ExpandWidth(false)))
            {
                if(questionList.questionList.Count > 0)
                {
                    if (!Array.Exists(questionList.questionList[viewIndex - 1].Answers, element => element.isCorrect == true) ||
                    questionList.questionList[viewIndex - 1].Answers.Count(x => x.isCorrect == true) > 1)
                    {
                        EditorUtility.DisplayDialog("Ουπς!", "Σημείωσε τουλάχιστον μια σωστή απάντηση και όχι παραπάνω!", "OK");
                    }
                    else
                    {
                        AddItem();
                    }
                }
                else
                {
                    AddItem();
                }                                
            }
            if (GUILayout.Button("Διαγραφή ερώτησης", GUILayout.ExpandWidth(false)))
            {
                if (questionList.questionList.Count > 0)
                {
                    DeleteItem(viewIndex - 1); 
                }
            }

            // show list  asset name
            string relPath = AssetDatabase.GetAssetPath(questionList);
            EditorGUILayout.LabelField("Όνομα αρχείου: ", relPath);

            GUILayout.EndHorizontal();            

            if (questionList.questionList == null)
                Debug.Log("Άδεια λίστα");
            if (questionList.questionList.Count > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Ερώτηση", viewIndex, GUILayout.ExpandWidth(false)), 1, questionList.questionList.Count);                
                EditorGUILayout.LabelField("από   " + questionList.questionList.Count.ToString(), "", GUILayout.ExpandWidth(false));

                
                GUILayout.EndHorizontal();


                questionList.questionList[viewIndex - 1].Text = EditorGUILayout.TextField("Κείμενο ερώτησης", questionList.questionList[viewIndex - 1].Text as string);

                questionList.questionList[viewIndex - 1].Image = EditorGUILayout.ObjectField("Εικόνα ερώτησης", 
                    questionList.questionList[viewIndex - 1].Image, typeof(Sprite), false) as Sprite;
                
                questionList.questionList[viewIndex - 1].Difficulty = (QuestionDifficulty)EditorGUILayout.EnumPopup("Δυσκολία", questionList.questionList[viewIndex - 1].Difficulty);
                
                GUILayout.Space(10);
                
                // Answers
                for (int i = 0; i < questionList.questionList[viewIndex - 1].Answers.Length; i++)
                {
                    GUILayout.Space(10);
                    questionList.questionList[viewIndex - 1].Answers[i].text = EditorGUILayout.TextField("Απάντηση", questionList.questionList[viewIndex - 1].Answers[i].text as string);
                    questionList.questionList[viewIndex - 1].Answers[i].isCorrect = EditorGUILayout.Toggle("Είναι η σωστή;", questionList.questionList[viewIndex - 1].Answers[i].isCorrect);
                    
                }

                GUILayout.Space(10);                
            }
            else
            {
                GUILayout.Label("Η λίστα ερωτήσεων είναι άδεια.");
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(questionList);
        }
    }

    void CreateNewQuestionList()
    {
        // There is no overwrite protection here!
        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
        // This should probably get a string from the user to create a new name and pass it ...
        viewIndex = 1;
        questionList = CreateQuestionList.Create();
        if (questionList)
        {
            questionList.questionList = new List<Question>();
            string relPath = AssetDatabase.GetAssetPath(questionList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenItemList()
    {
        string absPath = EditorUtility.OpenFilePanel("Επιλογή λίστας ερωτήσεων", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            questionList = AssetDatabase.LoadAssetAtPath(relPath, typeof(QuestionList)) as QuestionList;
            if (questionList.questionList == null)
                questionList.questionList = new List<Question>();
            if (questionList)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddItem()
    {
        Question newQuestion = new Question();
        newQuestion.Text= "Νέα ερώτηση";
        newQuestion.Difficulty = QuestionDifficulty.EASY;
        newQuestion.Answers = new[] {
            new Answer { text = "Πρώτη απάντηση", isCorrect = false },
            new Answer { text = "Δεύτερη απάντηση", isCorrect = false },
            new Answer { text = "Τρίτη απάντηση", isCorrect = false },
            new Answer { text = "Τέταρτη απάντηση", isCorrect = false },
            new Answer { text = "", isCorrect = false },
            new Answer { text = "", isCorrect = false }
            };

        questionList.questionList.Add(newQuestion);
        viewIndex = questionList.questionList.Count;
    }

    void DeleteItem(int index)
    {
        if (EditorUtility.DisplayDialog("Διαγραφή ερώτησης;",
                                       "Είσαι σίγουρος πως θέλεις να διαγράψεις την ερώτηση;",
                                       "Ναι",
                                       "Όχι"))
        {
            questionList.questionList.RemoveAt(index);
        }
            
    }
}