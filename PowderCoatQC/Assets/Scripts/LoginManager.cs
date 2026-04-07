using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text errorText;

    // Hardcoded admin credentials for testing
    private string adminEmail = "admin@powdercoat.com";
    private string adminPassword = "admin123";

    void Start()
    {
        errorText.text = "";
    }

    public void OnLoginClick()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            errorText.text = "Please fill in all fields!";
            return;
        }

        if (email == adminEmail && password == adminPassword)
        {
            SceneManager.LoadScene("Dashboard");
        }
        else
        {
            errorText.text = "Invalid email or password!";
        }
    }
    
}