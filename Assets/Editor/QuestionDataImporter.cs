using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class QuestionDataImporter : EditorWindow
{
    private TextAsset csvFile;
    private string outputFolder = "Assets/GeneratedQuestions";
    private string listAssetPath = "Assets/GeneratedQuestions/QuestionDataList.asset";

    [MenuItem("Tools/Import Questions from CSV")]
    public static void ShowWindow()
    {
        GetWindow(typeof(QuestionDataImporter));
    }

    void OnGUI()
    {
        GUILayout.Label("CSVインポート設定", EditorStyles.boldLabel);
        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSVファイル", csvFile, typeof(TextAsset), false);
        outputFolder = EditorGUILayout.TextField("出力フォルダ", outputFolder);

        if (GUILayout.Button("インポート"))
        {
            ImportQuestions();
        }
    }

    void ImportQuestions()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSVファイルを指定してください。");
            return;
        }

        string[] lines = csvFile.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        List<QuestionData> questionList = new List<QuestionData>();

        if (!AssetDatabase.IsValidFolder(outputFolder))
            AssetDatabase.CreateFolder("Assets", "GeneratedQuestions");

        for (int i = 1; i < lines.Length; i++) // skip header
        {
            string[] cols = lines[i].Split(',');

            if (cols.Length < 4)
            {
                Debug.LogWarning($"無効な行: {lines[i]}");
                continue;
            }

            QuestionData qData = ScriptableObject.CreateInstance<QuestionData>();
            qData.questionText = cols[0].Trim();
            qData.correctOption = cols[1].Trim();
            qData.wrongOption = cols[2].Trim();
            int.TryParse(cols[3], out qData.difficulty);

            string assetPath = $"{outputFolder}/Question_{i}.asset";
            AssetDatabase.CreateAsset(qData, assetPath);
            questionList.Add(qData);
        }

        // Create or update the QuestionDataList
        QuestionDataList listAsset = AssetDatabase.LoadAssetAtPath<QuestionDataList>(listAssetPath);
        if (listAsset == null)
        {
            listAsset = ScriptableObject.CreateInstance<QuestionDataList>();
            AssetDatabase.CreateAsset(listAsset, listAssetPath);
        }

        listAsset.questions = questionList;
        EditorUtility.SetDirty(listAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("インポート完了。");
    }
}
