using System.Collections.Generic;
using UnityEngine;

public class Category : MonoBehaviour
{
    public string categoryName;

    public int level = 1;

    public QuestionList questions;

    public QuestionDifficulty CalculateDifficulty()
    {
        if (level <= 5)
        {
            return QuestionDifficulty.EASY;
        }
        else if (level > 5 && level <= 10)
        {
            return QuestionDifficulty.NORMAL;
        }
        else
        {
            return QuestionDifficulty.HARD;
        }
    }
}
