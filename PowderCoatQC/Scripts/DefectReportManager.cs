using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class DefectReportManager : MonoBehaviour
{
    public TMP_InputField defectTypeInput;
    public TMP_Dropdown rootCauseDropdown;
    public TMP_Dropdown severityDropdown;
    public TMP_Text statusText;
    public Image photoPreview;

    void Start()
    {
        statusText.text = "";
        if (photoPreview != null)
            photoPreview.gameObject.SetActive(false);
    }

    public void OnUploadPhotoClick()
    {
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, 512);
                if (texture != null)
                {
                    photoPreview.sprite = Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f)
                    );
                    photoPreview.gameObject.SetActive(true);
                    statusText.text = "Photo uploaded!";
                }
            }
        }, "Select a defect photo");
    }

    public void OnSubmitClick()
    {
        string defectType = defectTypeInput.text;

        if (string.IsNullOrEmpty(defectType))
        {
            statusText.text = "Please enter defect type!";
            return;
        }

        string rootCause = rootCauseDropdown.options[rootCauseDropdown.value].text;
        string severity = severityDropdown.options[severityDropdown.value].text;

        int count = PlayerPrefs.GetInt("DefectCount", 0);
        PlayerPrefs.SetString("Defect_" + count + "_Type", defectType);
        PlayerPrefs.SetString("Defect_" + count + "_Cause", rootCause);
        PlayerPrefs.SetString("Defect_" + count + "_Severity", severity);
        PlayerPrefs.SetString("Defect_" + count + "_Status", "Open");
        PlayerPrefs.SetInt("DefectCount", count + 1);
        PlayerPrefs.Save();

        statusText.text = "Report saved successfully!";
        defectTypeInput.text = "";

        if (photoPreview != null)
            photoPreview.gameObject.SetActive(false);
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene("Dashboard");
    }
}