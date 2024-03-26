using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public void BackToInitMenu()
    {
        SceneManager.LoadScene("Init");
    }
}
