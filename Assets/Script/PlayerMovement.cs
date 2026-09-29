using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;

    [Header("Footstep Audio Settings")]
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioClip footstepClip;

    private Vector2 horizontalMovement;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Mengambil AudioSource jika slot di Inspector kosong
        if (footstepAudioSource == null)
        {
            footstepAudioSource = GetComponent<AudioSource>();
        }

        // Konfigurasi audio jika AudioSource dan Clip tersedia
        if (footstepAudioSource != null && footstepClip != null)
        {
            footstepAudioSource.clip = footstepClip;
            footstepAudioSource.loop = true; // Otomatis mengulang audio 4 detik
            footstepAudioSource.playOnAwake = false;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalMovement.x * moveSpeed, rb.linearVelocity.y);
    }

    private void Update()
    {
        float currentSpeed = Mathf.Abs(horizontalMovement.x);
        anim.SetFloat("Speed", currentSpeed);

        if (horizontalMovement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalMovement.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        // Pastikan AudioSource tersedia sebelum menjalankan logika
        if (footstepAudioSource == null) return;

        // Cek apakah player sedang bergerak secara horisontal (input tidak 0)
        bool isMoving = Mathf.Abs(horizontalMovement.x) > 0.1f;

        if (isMoving)
        {
            // Jika sedang bergerak dan audio belum diputar, mainkan suaranya
            if (!footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Play();
            }
        }
        else
        {
            // Jika pemain berhenti bergerak dan audio masih jalan, berhentikan suaranya
            if (footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Stop();
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>();
    }
}