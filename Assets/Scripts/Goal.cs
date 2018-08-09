using UnityEngine;

public class Goal : MonoBehaviour
{
    public AudioClip goalClip;
    private AudioSource audioSource;

	void Awake()
	{
        audioSource = GetComponent<AudioSource>();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.tag == "Player")
        {
            if (audioSource != null && goalClip != null)
            {
                audioSource.PlayOneShot(goalClip);
            }

            GameManager.Instance.RestartLevel(0.5f);
        }
	}
}
