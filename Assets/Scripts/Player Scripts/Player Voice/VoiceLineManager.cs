using UnityEngine;

//observer pattern to listen for player activity events and play voice lines accordingly
public class VoiceLineManager : MonoBehaviour
{
    private PlayerActivityTracker activityTracker;

    private void OnEnable()
    {
        activityTracker = GetComponent<PlayerActivityTracker>();

        if (activityTracker != null)
        {
            activityTracker.OnPlayerIdle += PlayAFKVoiceLine;
        }
    }

    private void OnDisable()
    {
        if (activityTracker != null)
        {
            activityTracker.OnPlayerIdle -= PlayAFKVoiceLine;
        }
    }

    private void PlayAFKVoiceLine()
    {
        Debug.Log("Player has been idle for 60 seconds!");
    }
}