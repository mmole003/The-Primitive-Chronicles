using UnityEngine;

public class Gem : MonoBehaviour
{
    public int gemValue = 1;


    //Tells when the player collides with a game object
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GemManager.Instance.CollectGem(gemValue);

            Destroy(gameObject);
        }
    }
}
