using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DefectHistoryManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject defectCardPrefab;
    public Transform contentParent;
    public TextMeshProUGUI statusText;

    void Start()
    {
        LoadDefects();
    }

    void LoadDefects()
    {
        // Placeholder data for testing
        string[] types = { "Scratch", "Dent", "Chip" };
        string[] causes = { "Human Error", "Machine Issue", "Material Defect" };
        string[] severities = { "Low", "Medium", "High" };

        foreach (string type in types)
        {
            GameObject card = Instantiate(defectCardPrefab, contentParent);
            card.GetComponentInChildren<TextMeshProUGUI>().text = 
                "Type: " + type;

            Button reviewBtn = card.GetComponentInChildren<Button>();
            string capturedType = type;
            reviewBtn.onClick.AddListener(() =>
            {
                PlayerPrefs.SetString("selectedDefectId", capturedType);
                SceneManager.LoadScene("ManagerReview");
            });
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Dashboard");
    }
}