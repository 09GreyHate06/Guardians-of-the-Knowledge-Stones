using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace Core.GOTKS.Editor
{
    public static class QuestionTextImporter
    {
        [MenuItem("GOTKS/Import Questions From Text")]
        public static void ImportQuestions()
        {
            string filePath = EditorUtility.OpenFilePanel("Select Question Text File", "/Assets", "txt");
            if (string.IsNullOrEmpty(filePath))
                return;

            string savePath = "Assets/Prefabs/Questions/Resources/College/";
            //string savePath = EditorUtility.OpenFolderPanel("Select Destination", "Assets/", "") + "/";
            if (!Directory.Exists(savePath)) 
                Directory.CreateDirectory(savePath);

            string[] lines = File.ReadAllLines(filePath);
            List<(string question, List<string> choices, int answerIndex)> questions = new();

            string currentQuestion = "";
            List<string> currentChoices = new();
            int answerIndex = -1;

            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line))
                {
                    // Save current question
                    if (!string.IsNullOrEmpty(currentQuestion) && currentChoices.Count > 0 && answerIndex >= 0)
                    {
                        questions.Add((currentQuestion, new List<string>(currentChoices), answerIndex));
                    }

                    // Reset
                    currentQuestion = "";
                    currentChoices.Clear();
                    answerIndex = -1;
                    continue;
                }

                if (line.StartsWith("Q:"))
                {
                    currentQuestion = line.Substring(2).Trim();
                }
                else if (line.StartsWith("A:"))
                {
                    if (answerIndex == -1) answerIndex = currentChoices.Count;
                    currentChoices.Add(line.Substring(2).Trim());
                }
                else if (line.StartsWith("C:"))
                {
                    currentChoices.Add(line.Substring(2).Trim());
                }
            }

            // Final push in case last block had no trailing newline
            if (!string.IsNullOrEmpty(currentQuestion) && currentChoices.Count > 0 && answerIndex >= 0)
            {
                questions.Add((currentQuestion, new List<string>(currentChoices), answerIndex));
            }

            foreach (var q in questions)
            {
                var asset = ScriptableObject.CreateInstance<QuestionSO>();

                SerializedObject so = new SerializedObject(asset);
                so.FindProperty("_question").stringValue = q.question;
                so.FindProperty("_choices").arraySize = q.choices.Count;
                for (int i = 0; i < q.choices.Count; i++)
                {
                    so.FindProperty("_choices").GetArrayElementAtIndex(i).stringValue = q.choices[i];
                }
                so.FindProperty("_answerIndex").intValue = q.answerIndex;
                so.ApplyModifiedProperties();

                string safeName = MakeSafeFileName(q.question);
                AssetDatabase.CreateAsset(asset, savePath + safeName + ".asset");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Imported {questions.Count} questions.");
        }

        private static string MakeSafeFileName(string input)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                input = input.Replace(c, '_');
            return input.Length > 40 ? input.Substring(0, 40) : input;
        }
    }

}
