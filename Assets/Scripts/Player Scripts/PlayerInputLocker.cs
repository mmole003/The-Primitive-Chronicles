using UnityEngine;

public class PlayerInputLocker : MonoBehaviour
{
    // Populate in Inspector with the player's movement/look scripts (any Behaviour).
    public Behaviour[] behavioursToToggle;

    // If the player GameObject is not assigned behaviours manually, you can
    // optionally set a player root and collect behaviours at runtime.
    public GameObject playerRoot;
    public bool autoCollectFromRoot = false;

    private void Awake()
    {
        if (autoCollectFromRoot && playerRoot != null)
        {
            // Collect common behaviour types; adjust as needed.
            behavioursToToggle = playerRoot.GetComponentsInChildren<Behaviour>(true);
        }
    }

    // locked == true: disable player controls; locked == false: enable
    public void SetLocked(bool locked)
    {
        if (behavioursToToggle == null) return;

        for (int i = 0; i < behavioursToToggle.Length; i++)
        {
            var b = behavioursToToggle[i];
            if (b == null) continue;
            b.enabled = !locked;
        }
    }
}