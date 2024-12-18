using System;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace Assets.PlayId.Scripts.Editor
{
    [InitializeOnLoad]
    internal static class PackageFeedback
    {
        private const string PackageId = "287608";
        private const string PackageName = "Play ID";

        private static readonly string PrefsKeyImportTime = $"PackageFeedback.ImportTime.{PackageId}";
        private static readonly string PrefsKeyReviewTime = $"PackageFeedback.ReviewTime.{PackageId}";
        private static readonly string PrefsKeySkipTime = $"PackageFeedback.SkipTime.{PackageId}";
        
        static PackageFeedback()
        {
            if (EditorPrefs.HasKey(PrefsKeyImportTime))
            {
                var time = DateTime.Parse(EditorPrefs.GetString(PrefsKeyImportTime), CultureInfo.InvariantCulture);

                if ((DateTime.UtcNow - time).TotalHours < 1) return;
            }
            else
            {
                EditorPrefs.SetString(PrefsKeyImportTime, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            }

            if (EditorPrefs.HasKey(PrefsKeyReviewTime)) return;

            if (EditorPrefs.HasKey(PrefsKeySkipTime))
            {
                var time = DateTime.Parse(EditorPrefs.GetString(PrefsKeySkipTime), CultureInfo.InvariantCulture);

                if ((DateTime.UtcNow - time).TotalDays < 7) return;
            }

            var confirm = EditorUtility.DisplayDialog("Package Feedback", $"Would you like to write a review about {PackageName}?\n\nYour feedback is very important for improving and extending our assets. We appreciate your time and have a small gift you!", "Yes", "No");

            EditorPrefs.SetString(confirm ? PrefsKeyReviewTime : PrefsKeySkipTime, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));

            if (confirm)
            {
                Application.OpenURL($"https://assetstore.unity.com/packages/slug/{PackageId}#reviews");

                if (EditorUtility.DisplayDialog("Package Feedback Bonus", "We really appreciate your review! We want to gift you our package Simple Encryption as a token of gratitude. With it, you can easily encrypt and decrypt data, compute hashes and create digital signatures.", "Download", "No, thanks"))
                {
                    Application.OpenURL("https://bit.ly/PackageFeedbackBonus");
                }
            }
        }
    }
}