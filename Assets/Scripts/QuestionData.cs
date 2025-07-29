using UnityEngine;

[CreateAssetMenu(menuName = "Question / QuestionData")]
public class QuestionData : ScriptableObject {
    public string questionText;
    public string firstOption;
    public string secondOption;
    public string correctAnswer;
    public int difficulty;
}
