using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System.Collections;

public class DefectHistoryManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject defectCardPrefab;
    public Transform contentParent;
    public TextMeshProUGUI statusText;

    private FirebaseFirestore db;
    private FirebaseAuth auth;

    void Start()
    {
        StartCoroutine(InitializeAndLoad());
    }

    IEnumerator InitializeAndLoad()
    {
        var task = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Result == DependencyStatus.Available)
        {
            db = FirebaseFirestore.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
            StartCoroutine(LoadDefects());
        }
    }

    IEnumerator LoadDefects()
    {
        if (auth.CurrentUser == null)
        {
            if (statusText != null)
                statusText.text = "Not logged in!";
            yield break;
        }

        string userId = auth.CurrentUser.UserId;

        // Check user role first
        var roleTask = db.Collection("users").Document(userId).GetSnapshotAsync();
        yield return new WaitUntil(() => roleTask.IsCompleted);

        string role = "worker";
        if (roleTask.IsCompleted && roleTask.Result.Exists)
        {
            role = roleTask.Result.GetValue<string>("role");
        }

        // Load based on role
        Query query;
        if (role == "manager")
        {
            query = db.Collection("defects");
        }
        else
        {
            query = db.Collection("defects").WhereEqualTo("userId", userId);
        }

        var defectsTask = query.GetSnapshotAsync();
        yield return new WaitUntil(() => defectsTask.IsCompleted);

        if (defectsTask.IsCompleted && !defectsTask.IsFaulted)
        {
            foreach (DocumentSnapshot doc in defectsTask.Result.Documents)
            {
                string type = doc.ContainsField("type") ? doc.GetValue<string>("type") : "-";
                string severity = doc.ContainsField("severity") ? doc.GetValue<string>("severity") : "-";
                string status = doc.ContainsField("status") ? doc.GetValue<string>("status") : "-";
                string docId = doc.Id;

                GameObject card = Instantiate(defectCardPrefab, contentParent);
                card.GetComponentInChildren<TextMeshProUGUI>().text =
                    "Type: " + type + "\nSeverity: " + severity + "\nStatus: " + status;

                Button reviewBtn = card.GetComponentInChildren<Button>();

                // Only show review button for managers
                if (role != "manager")
                {
                    reviewBtn.gameObject.SetActive(false);
                }
                else
                {
                    string capturedId = docId;
                    reviewBtn.onClick.AddListener(() =>
                    {
                        PlayerPrefs.SetString("selectedDefectId", capturedId);
                        SceneManager.LoadScene("ManagerReview");
                    });
                }
            }
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Dashboard");
    }
}