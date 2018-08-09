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
        if (collision.transform.tag == "Player")
        {
            if (audioSource != null && deathClip != null)
            {
                audioSource.PlayOneShot(deathClip);
            }

            Instantiate(playerDeathPrefab, collision.contacts[0].point, Quaternion.identity);
            spriteRenderer.sprite = hitSprite;

            Destroy(collision.gameObject);
            GameManager.Instance.RestartLevel(1.25f);
        }
	}
}
