using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuizPack", menuName = "Quiz/Quiz Pack", order = 1)]
public class QuizPackSO : ScriptableObject
{
    public List<QuizDataSO> quizList;
}