using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Question / QuestionDataList")]
public class QuestionDataList : ScriptableObject {
    public List<QuestionData> questions;
}