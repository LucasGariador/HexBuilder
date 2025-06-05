using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PortraitEffect : MonoBehaviour
{
    private Image portraitImage;
    private Color originalColor;
    private Color targetColor;
    [SerializeField] private float duration = .02f;
    
    void Start()
    {
        portraitImage = GetComponent<Image>();

        targetColor = new Color(70,70,70,255);
    }

    public void SetPortraitIn()
    {
        StartCoroutine(ObscureIn());
    }
    public void SetPortraitOut()
    {
        StartCoroutine(ObscureOut());
    }

    IEnumerator ObscureIn()
    {
        while (portraitImage.color.r < 70)
        {
            portraitImage.color = Color.Lerp(portraitImage.color, targetColor, Time.deltaTime * duration);
            yield return null;
        }
    }

    IEnumerator ObscureOut()
    {
        while (portraitImage.color.r > 0)
        {
            portraitImage.color = Color.Lerp(portraitImage.color, originalColor, Time.deltaTime * duration);
            yield return null;
        }
    }
}
