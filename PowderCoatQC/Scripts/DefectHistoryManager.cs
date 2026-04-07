using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DefectHistoryManager : MonoBehaviour
{
    public GameObject defectCardPrefab;
    public Transform contentParent;
    public TMP_Text statusText;

    void Start()
    {
        LoadDefects();
    }

    void LoadDefects()
    {
        int count = PlayerPrefs.GetInt("DefectCount", 0);

        if (count == 0)
        {
            statusText.text = "No defects reported yet!";
            return;
        }

        statusText.text = "";

        for (int i = 0; i < count; i++)
        {
            string type = PlayerPrefs.GetString("Defect_" + i + "_Type");
            string cause = PlayerPrefs.GetString("Defect_" + i + "_Cause");
            string severity = PlayerPrefs.GetString("Defect_" + i + "_Severity");
            string status = PlayerPrefs.GetString("Defect_" + i + "_Status");

            GameObject card = Instantiate(defectCardPrefab, contentParent);
            TMP_Text cardText = card.GetComponentInChildren<TMP_Text>();
            cardText.text = $"Type: {type}\nCause: {cause}\nSeverity: {severity}\nStatus: {status}";
        }
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene("Dashboard");
    }
}