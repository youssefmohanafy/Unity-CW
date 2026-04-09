using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using System.Collections.Generic;

public class DefectReportManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField defectTypeInput;
    public TMP_Dropdown rootCauseDropdown;
    public TMP_Dropdown severityDropdown;
    public Button submitButton;
    public Button backButton;
    public TextMeshProUGUI statusText;

    private FirebaseFirestore db;
    private FirebaseAuth auth;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;

        submitButton.onClick.AddListener(OnSubmitClick);
        backButton.onClick.AddListener(GoBack);
    }

    void OnSubmitClick()
    {
        string defectType = defectTypeInput.text;
        string rootCause = rootCauseDropdown.options[rootCauseDropdown.value].text;
        string severity = severityDropdown.options[severityDropdown.value].text;
        string userId = auth.CurrentUser != null ? auth.CurrentUser.UserId : "unknown";

        if (string.IsNullOrEmpty(defectType))
        {
            statusText.text = "Please enter defect type!";
            return;
        }

        statusText.text = "Submitting...";

        Dictionary<string, object> defect = new Dictionary<string, object>
        {
            { "type", defectType },
            { "rootCause", rootCause },
            { "severity", severity },
            { "status", "pending" },
            { "userId", userId },
            { "timestamp", Timestamp.GetCurrentTimestamp() }
        };

        db.Collection("defects").AddAsync(defect).ContinueWith(task =>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                if (task.IsCompleted)
                {
                    statusText.text = "Report submitted!";
                }
                else
                {
                    statusText.text = "Failed to submit!";
                }
            });
        });
    }

    void GoBack()
    {
        SceneManager.LoadScene("Dashboard");
    }
}