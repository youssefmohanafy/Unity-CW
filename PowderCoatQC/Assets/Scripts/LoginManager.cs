using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Auth;
using Firebase.Firestore;
using System.Collections.Generic;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text errorText;

    void Start() => errorText.text = "";

    public async void OnLoginClick()
    {

        if (string.IsNullOrEmpty(emailInput.text) || string.IsNullOrEmpty(passwordInput.text))
        {
            errorText.text = "Please fill in all fields!";
            return;
        }

        errorText.text = "Logging in...";

        try
        {
            var result = await FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text);
            string userId = result.User.UserId;

            var userDoc = await FirebaseFirestore.DefaultInstance.Collection("users").Document(userId).GetSnapshotAsync();
            
            if (!userDoc.Exists)
            {
                await FirebaseFirestore.DefaultInstance.Collection("users").Document(userId).SetAsync(new Dictionary<string, object>
                {
                    { "email", emailInput.text },
                    { "role", "worker" }
                });
                PlayerPrefs.SetString("userRole", "worker");
            }
            else
            {
                string role = userDoc.ContainsField("role") ? userDoc.GetValue<string>("role") : "worker";
                PlayerPrefs.SetString("userRole", role);
            }

            PlayerPrefs.Save();
            SceneManager.LoadScene("Dashboard");
        }
        catch
        {
            errorText.text = "Invalid credentials!";
        }
    }
}