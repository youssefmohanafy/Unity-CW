using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Firebase.Firestore;

public class DefectHistoryManager : MonoBehaviour
{
    [Header("List UI")]
    public GameObject defectCardPrefab;
    public Transform contentParent;
    public TMP_Text statusText;

    [Header("Review Panel")]
    public GameObject reviewPanel;
    public TMP_Text defectInfoText;
    public TMP_Text reviewStatusText;
    public Button markDoneButton;

    private string selectedDefectId;
    private string userRole;

    async void Start()
    {
        userRole = PlayerPrefs.GetString("userRole", "worker");
        reviewPanel.SetActive(false);
        await LoadDefects();
    }

    async System.Threading.Tasks.Task LoadDefects()
    {
        statusText.text = "Loading...";
        try
        {
            var snapshot = await FirebaseFirestore.DefaultInstance.Collection("defects").GetSnapshotAsync();
            statusText.text = "";

            foreach (var doc in snapshot.Documents)
            {
                string type = doc.ContainsField("type") ? doc.GetValue<string>("type") : "";
                string cause = doc.ContainsField("rootCause") ? doc.GetValue<string>("rootCause") : "";
                string severity = doc.ContainsField("severity") ? doc.GetValue<string>("severity") : "";
                string status = doc.ContainsField("status") ? doc.GetValue<string>("status") : "";

                GameObject card = Instantiate(defectCardPrefab, contentParent);
                card.GetComponentInChildren<TMP_Text>().text = $"Type: {type}\nCause: {cause}\nSeverity: {severity}\nStatus: {status}";

                string docId = doc.Id;
                Button reviewBtn = card.transform.Find("ReviewButton").GetComponent<Button>();

                if (userRole == "manager")
                    reviewBtn.onClick.AddListener(() => OpenReviewPanel(docId, type, cause, severity, status));
                else
                    reviewBtn.gameObject.SetActive(false);
            }

            if (snapshot.Count == 0)
                statusText.text = "No defects reported yet!";
        }
        catch (System.Exception e)
        {
            statusText.text = "Failed to load!";
            Debug.LogError("Firestore error: " + e.Message);
        }
    }

    void OpenReviewPanel(string docId, string type, string cause, string severity, string status)
    {
        selectedDefectId = docId;
        defectInfoText.text = $"Type: {type}\nCause: {cause}\nSeverity: {severity}";
        UpdateStatusUI(status);
        reviewPanel.SetActive(true);
    }

    public async void MarkAsDone()
    {
        if (string.IsNullOrEmpty(selectedDefectId)) return;
        try
        {
            await FirebaseFirestore.DefaultInstance.Collection("defects")
                .Document(selectedDefectId).UpdateAsync("status", "Reviewed");
            UpdateStatusUI("Reviewed");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Update error: " + e.Message);
        }
    }

    public void CloseReviewPanel() => reviewPanel.SetActive(false);

    void UpdateStatusUI(string status)
    {
        bool reviewed = status == "Reviewed";
        reviewStatusText.text = reviewed ? "Status: Done!" : "Status: Pending";
        reviewStatusText.color = reviewed ? Color.green : Color.yellow;
        markDoneButton.interactable = !reviewed;
    }

    public void OnBackClick() => SceneManager.LoadScene("Dashboard");
}