using TMPro;
using UnityEngine;

public class GemUI : MonoBehaviour
{
    public TMP_Text gemText;

    private void Start()
    {
        GemManager.Instance.OnGemsChanged += UpdateGemText;

        UpdateGemText(GemManager.Instance.gemsCollected);
    }

    private void UpdateGemText(int amount)
    {
        gemText.text = "Gems: " + amount;
    }

    private void OnDestroy()
    {
        if (GemManager.Instance != null)
        {
            GemManager.Instance.OnGemsChanged -= UpdateGemText;
        }
    }
}