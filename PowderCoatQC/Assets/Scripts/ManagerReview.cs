using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Firestore;

public class ManagerReviewManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI statusText;
    public Button markDoneButton;
    public Button backButton;

    private FirebaseFirestore db;
    private string currentDefectId;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        currentDefectId = PlayerPrefs.GetString("selectedDefectId", "");

        backButton.onClick.AddListener(GoBack);
        markDoneButton.onClick.AddListener(MarkAsDone);

        LoadStatus();
    }

    void LoadStatus()
    {
        if (string.IsNullOrEmpty(currentDefectId)) return;

        db.Collection("defects").Document(currentDefectId)
            .GetSnapshotAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                string status = task.Result.GetValue<string>("status");
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    UpdateStatusUI(status);
                });
            }
        });
    }

    void MarkAsDone()
    {
        if (string.IsNullOrEmpty(currentDefectId)) return;

        db.Collection("defects").Document(currentDefectId)
            .UpdateAsync("status", "reviewed").ContinueWith(task =>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                if (task.IsCompleted)
                {
                    UpdateStatusUI("reviewed");
                }
            });
        });
    }

    void UpdateStatusUI(string status)
    {
        if (status == "reviewed")
        {
            statusText.text = "Status: Done!";
            statusText.color = Color.green;
            markDoneButton.interactable = false;
        }
        else
        {
            statusText.text = "Status: Pending";
            statusText.color = Color.yellow;
            markDoneButton.interactable = true;
        }
    }

    void GoBack()
    {
        SceneManager.LoadScene("DefectHistory");
    }
}