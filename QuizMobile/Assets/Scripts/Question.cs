using System;
using UnityEngine;

[Serializable]
public class Question {

    public string Text;
    
    public Sprite Image;

    public QuestionDifficulty Difficulty;

    public Answer[] Answers;       
}

public enum QuestionDifficulty
{
    EASY = 1,
    NORMAL = 6,
    HARD = 11
}
