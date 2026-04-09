using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ManagerReviewManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI statusText;
    public Button markDoneButton;
    public Button backButton;

    void Start()
    {
        backButton.onClick.AddListener(GoBack);
        markDoneButton.onClick.AddListener(MarkAsDone);

        statusText.text = "Status: Pending";
        statusText.color = Color.yellow;
    }

    void MarkAsDone()
    {
        statusText.text = "Status: Done!";
        statusText.color = Color.green;
        markDoneButton.interactable = false;
    }

    void GoBack()
    {
        SceneManager.LoadScene("DefectHistory");
    }
}