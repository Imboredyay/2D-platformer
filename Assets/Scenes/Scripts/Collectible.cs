using UnityEngine;

public class Collectible : MonoBehaviour
{
    public AudioClip pickupSound;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player touched it
        if (collision.gameObject.CompareTag("Player"))
        {
            // Play pickup sound
            SoundManager.Instance?.PlaySound(pickupSound);

            // Set the Collected variable in the animator
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("Collected", true);
            }
            
            // Destroy the object after 1 second
            Destroy(gameObject, 0.5f);
        }
    }
}
