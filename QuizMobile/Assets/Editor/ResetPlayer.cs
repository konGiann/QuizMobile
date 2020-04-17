using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    [MenuItem("Player/Διαγραφή στατιστικών παίχτη")]
    static void ResetPlayerStats()
    {
        if (EditorUtility.DisplayDialog("Διαγραφή στατιστικών παίχτη;",
            "Σίγουρα;",
        "Ναι",
        "Όχι"))
        {
            PlayerPrefmanager.ResetPlayer();
        }
    }
}
