using UnityEngine;

[CreateAssetMenu(fileName = "NewQuiz", menuName = "Quiz/QuizData", order = 0)]
public class QuizDataSO : ScriptableObject
{
    [TextArea]
    public string question;

    [Header("보기 (4개)")]
    public string[] choices = new string[4];

    [Tooltip("정답 인덱스 (0~3)")]
    public int correctAnswerIndex;

    public QuizType type;
}

public enum QuizType
{
    Science,
    History,
    Vocabulary,
    Math
}