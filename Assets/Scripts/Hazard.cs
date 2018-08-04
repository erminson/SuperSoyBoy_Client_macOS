using UnityEngine;

public class Hazard : MonoBehaviour
{
    public GameObject playerDeathPrefab;
    public AudioClip deathClip;
    public Sprite hitSprite;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

	void Awake()
	{
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{

        Debug.Log("collision.transform.tag1:" + collision.transform.tag);
        Debug.Log("collision.gameObject.tag1:" + collision.gameObject.tag);
        if (collision.transform.tag == "Player")
        {
            Debug.Log("collision.transform.tag2:" + collision.transform.tag);
            Debug.Log("collision.gameObject.tag2:" + collision.gameObject.tag);
            if (audioSource != null && deathClip != null)
            {
                audioSource.PlayOneShot(deathClip);
            }

            Instantiate(playerDeathPrefab, collision.contacts[0].point, Quaternion.identity);
            spriteRenderer.sprite = hitSprite;

            Destroy(collision.gameObject);
        }
	}
}
