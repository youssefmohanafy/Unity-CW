using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Firestore;
using Firebase.Auth;
using System.Collections.Generic;

public class DefectReportManager : MonoBehaviour
{
    public TMP_InputField defectTypeInput;
    public TMP_Dropdown rootCauseDropdown;
    public TMP_Dropdown severityDropdown;
    public TextMeshPro statusText;
    public UnityEngine.UI.Image photoPreview;

    void Start()
    {
        statusText.text = "";
        if (photoPreview != null) photoPreview.gameObject.SetActive(false);
    }

    public void OnUploadPhotoClick()
    {
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path == null) return;
            Texture2D tex = NativeGallery.LoadImageAtPath(path, 512);
            if (tex == null) return;
            photoPreview.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            photoPreview.gameObject.SetActive(true);
            statusText.text = "Photo uploaded!";
        }, "Select a defect photo");
    }

    public async void OnSubmitClick()
    {
        if (string.IsNullOrEmpty(defectTypeInput.text))
        {
            statusText.text = "Please enter defect type!";
            return;
        }

        statusText.text = "Submitting...";

        try
        {
            var defect = new Dictionary<string, object>
            {
                { "type", defectTypeInput.text },
                { "rootCause", rootCauseDropdown.options[rootCauseDropdown.value].text },
                { "severity", severityDropdown.options[severityDropdown.value].text },
                { "status", "Open" },
                { "userId", FirebaseAuth.DefaultInstance.CurrentUser?.UserId ?? "unknown" },
                { "timestamp", Timestamp.GetCurrentTimestamp() }
            };

            await FirebaseFirestore.DefaultInstance.Collection("defects").AddAsync(defect);

            statusText.text = "Report submitted!";
            defectTypeInput.text = "";
            if (photoPreview != null) photoPreview.gameObject.SetActive(false);
        }
        catch (System.Exception e)
        {
            statusText.text = "Failed to submit!";
            Debug.LogError("Firestore error: " + e.Message);
        }
    }

    public void OnBackClick() => SceneManager.LoadScene("Dashboard");
}