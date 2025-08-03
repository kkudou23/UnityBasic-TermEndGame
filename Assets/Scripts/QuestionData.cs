using UnityEngine;

[CreateAssetMenu(menuName = "Question / QuestionData")]
public class QuestionData : ScriptableObject {
    public string questionText;
    public string correctOption;
    public string wrongOption;
    public int difficulty;
}
