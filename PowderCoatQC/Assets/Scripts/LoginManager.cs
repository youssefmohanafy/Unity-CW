using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase;
using Firebase.Auth;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI statusText;
    public Button loginButton;

    private FirebaseAuth auth;
    private bool firebaseReady = false;

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginClick);
        StartCoroutine(InitializeFirebase());
    }

    IEnumerator InitializeFirebase()
    {
        var task = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Result == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            firebaseReady = true;
            statusText.text = "";
        }
        else
        {
            statusText.text = "Firebase error!";
        }
    }

    void OnLoginClick()
    {
        if (!firebaseReady)
        {
            statusText.text = "Please wait...";
            return;
        }

        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            statusText.text = "Please enter email and password";
            return;
        }

        StartCoroutine(LoginCoroutine(email, password));
    }

    IEnumerator LoginCoroutine(string email, string password)
    {
        statusText.text = "Logging in...";

        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.IsFaulted || loginTask.IsCanceled)
        {
            statusText.text = "Login failed. Check credentials.";
        }
        else
        {
            statusText.text = "Login successful!";
            SceneManager.LoadScene("Dashboard");
        }
    }
}