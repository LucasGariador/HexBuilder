using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour // temporal
{
    public void BackToInitMenu()
    {
        SceneManager.LoadScene("Init");
    }
}
