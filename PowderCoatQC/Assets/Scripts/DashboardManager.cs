using UnityEngine;
using UnityEngine.SceneManagement;

public class DashboardManager : MonoBehaviour
{
    public void OnNewDefectClick()
    {
        SceneManager.LoadScene("NewDefect");
    }

    public void OnViewHistoryClick()
    {
        SceneManager.LoadScene("DefectHistory");
    }

    public void OnLogoutClick()
    {
        SceneManager.LoadScene("Login");
    }
}