using Managers;
using UnityEditor;

public class ResetCategoriesStats
{
    [MenuItem("Ερωτήσεις/Διαγραφή στατιστικών κατηγοριών")]
    static void ResetCategoriesLevel()
    {
        if(EditorUtility.DisplayDialog("Διαγραφή στατιστικών κατηγοριών;",
            "Σίγουρα;",
        "Ναι",
        "Όχι"))
        {
            PlayerPrefmanager.ResetAllCategoriesLevel();
        }
    }
}
