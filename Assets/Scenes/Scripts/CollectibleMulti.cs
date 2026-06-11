using UnityEngine;
using Mirror;

public class CollectibleMulti : NetworkBehaviour
{
	[ServerCallback]
	void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player touched it
        if (collision.gameObject.CompareTag("Player"))
        {
			// Destroy the object after 1 second
			NetworkServer.Destroy(gameObject);
		}
    }
}
