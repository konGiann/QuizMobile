using UnityEditor;
using UnityEngine;
using Managers;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    SaveLoadObject saveLoadObj;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Διαγραφή σκορ παίχτη"))
        {
            if (EditorUtility.DisplayDialog("Διαγραφή του highscore;",
                                       "Είσαι σίγουρος;",
                                       "Ναι",
                                       "Όχι"))
            {
                PlayerPrefs.DeleteAll();
            }            
        }

        //if(GUILayout.Button("Save Category Questions to file"))
        //{
        //    if (EditorUtility.DisplayDialog("Εξαγωγή ερωτήσεων σε αρχεία",
        //                               "Τα αρχεία θα σωθούν στο /Assets/Saves",
        //                               "Ναι",
        //                               "Όχι"))
        //    {
        //        QuestionObject religionObj = new QuestionObject
        //        {
        //            list = GameManager.gm.ReligionQuestions
        //        };

        //        QuestionObject cultureObj = new QuestionObject
        //        {
        //            list = GameManager.gm.CultureQuestions
        //        };

        //        QuestionObject natureObj = new QuestionObject
        //        {
        //            list = GameManager.gm.NatureQuestions
        //        };

        //        QuestionObject covidObj = new QuestionObject
        //        {
        //            list = GameManager.gm.CovidQuestions
        //        };

        //        saveLoadObj = new SaveLoadObject();

        //        saveLoadObj.SaveQuestionList(religionObj, "religion");
        //        saveLoadObj.SaveQuestionList(cultureObj, "culture");
        //        saveLoadObj.SaveQuestionList(natureObj, "nature");
        //        saveLoadObj.SaveQuestionList(covidObj, "covid"); 
        //    }
        //}

        //if(GUILayout.Button("Load All Categories' Questions"))
        //{
        //    if (EditorUtility.DisplayDialog("Επαναφορά ερωτήσεων",
        //                               "Να γίνει ανάκτηση; Βεβαιώσου ότι τα αρχεία είναι στη σωστή κατάσταση!",
        //                               "Ναι",
        //                               "Όχι"))
        //    {
        //        saveLoadObj = new SaveLoadObject();

        //        GameManager.gm.ReligionQuestions = saveLoadObj.LoadQuestionList("religion");
        //        GameManager.gm.CultureQuestions = saveLoadObj.LoadQuestionList("culture");
        //        GameManager.gm.NatureQuestions = saveLoadObj.LoadQuestionList("nature");
        //        GameManager.gm.CovidQuestions = saveLoadObj.LoadQuestionList("covid");
        //    }
        //}
    }
}
