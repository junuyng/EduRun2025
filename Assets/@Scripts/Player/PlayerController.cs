using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SkinData
{
    public string skinName;
    public RuntimeAnimatorController animatorController;
}

public class PlayerController : MonoBehaviour
{
    [Header("Skins")]
    [SerializeField] private List<SkinData> skinDatas; // 인스펙터에서 스킨 목록 등록

    
    [Header("Physics")]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private int jumpForce = 300;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    [Header("Jump")]
    [SerializeField] private float jumpCooldown = 0.5f;
    private bool isJumping = false;
    private float jumpTimer = 0f;

    [Header("Life")] [SerializeField] public int MaxHealth { get; private set;  } = 100;
    public static int currentHealth;

    [Header("Swipe")]
    private Vector2 touchStartPos;
    private float touchStartTime;
    private bool isSwipe;
    private float minSwipeDistance = 50f;
    private float maxSwipeTime = 0.5f;
    private float hpTickTimer = 0f;
    private float hpTickInterval = 1f;

    [Header("Audio")]
   public AudioUnit audioUnit;
        
    public static bool gameOver;

    public event Action<float> OnHit;
    
    void Start()
    {
        GameManager.Instance.PlayerController = this;
        RunnerManager.Instance.OnQuizResult += DamageByQuizResult;
        currentHealth = MaxHealth;
        gameOver = false;
        ApplyAnimatorBySelectedSkin();

    }

    void Update()
    {
        CheckGrounded();
        HandleTouchInput();
        UpdateJumpTimer();
        UpdateAnimator();
        TickHpOverTime();
    }
    
    private void TickHpOverTime()
    {
        hpTickTimer += Time.deltaTime;

        if (hpTickTimer >= hpTickInterval)
        {
            hpTickTimer = 0f;
            
            currentHealth -=2;
            OnHit?.Invoke(currentHealth);

        }
    }

    private void ApplyAnimatorBySelectedSkin()
    {
        string selectedSkin = GameManager.Instance.selectedSkin;

        for (int i = 0; i < skinDatas.Count; i++)
        {
            if (skinDatas[i].skinName == selectedSkin)
            {
                animator.runtimeAnimatorController = skinDatas[i].animatorController;
                return;
            }
        }

    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private void UpdateAnimator()
    {
        animator.SetBool("jump", !isGrounded);
    }

    private void HandleTouchInput()
    {
        if(GameManager.Instance.isSolving || GameManager.Instance.isLoading)
            return;
        
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !gameOver)
        {
            TryJump();
        }
#else
    if (Input.touchCount <= 0 || gameOver) return;

    Touch touch = Input.GetTouch(0);
    if (touch.phase == TouchPhase.Began)
    {
        TryJump();
    }
#endif
    }



    private void TryJump()
    {
        if (isGrounded && !isJumping)
        {
            playerRigidbody.AddForce(Vector2.up * jumpForce);
            audioUnit .PlaySFX(SFX.Jump);
            isJumping = true;
            jumpTimer = 0f;
        }
    }

    private void UpdateJumpTimer()
    {
        if (!isJumping) return;

        jumpTimer += Time.deltaTime;
        if (jumpTimer >= jumpCooldown && isGrounded)
        {
            isJumping = false;
            jumpTimer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            audioUnit.PlaySFX(SFX.Hit);
            OnDamage();
        }
    }

    private void DamageByQuizResult(bool result)
    {
        if(result)
            return;
        OnDamage();
    }

    private void OnDamage()
    {
        animator.SetTrigger("hit");
        currentHealth = Mathf.Max(0, currentHealth - 10);
        OnHit?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Time.timeScale = 0f;
            UIManager.Instance.Show<UIEndGame>();
        }
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("Game Over");
    }
}
