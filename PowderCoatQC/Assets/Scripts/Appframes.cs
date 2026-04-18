using UnityEngine;

public class AppSettings : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 90;
        DontDestroyOnLoad(gameObject);
    }
}