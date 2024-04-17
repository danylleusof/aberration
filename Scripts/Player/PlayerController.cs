using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Slider healthBar;
    public Slider staminaBar;
    // Knockback
    public GameObject enemy;
    EnemyController enemyController;

    [Header("Player Attributes")]
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    public float maxHealth;
    public float maxStamina;
    float movementSpeed, currentHealth, currentStamina;
    Rigidbody rb;
    bool grounded;

    [Header("Animation State Controller")]
    Animator animator;
    int isWalkingHash, isRunningHash;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyController = enemy.GetComponentInChildren<EnemyController>();
        animator = GetComponent<Animator>();

        movementSpeed = walkSpeed;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        healthBar.maxValue = maxHealth;
        staminaBar.maxValue = maxStamina;

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool forwardPressed = Input.GetButton("Vertical");
        bool runPressed = Input.GetButton("Fire3");
        bool jumpPressed = Input.GetButtonDown("Jump");

        if (Input.GetButton("Horizontal"))
            if (Input.GetAxis("Horizontal") > 0)
                transform.position += transform.right * Time.deltaTime * movementSpeed;
            else if (Input.GetAxis("Horizontal") < 0)
                transform.position -= transform.right * Time.deltaTime * movementSpeed;

        if (Input.GetButton("Vertical"))
            if (Input.GetAxis("Vertical") > 0)
                transform.position += transform.forward * Time.deltaTime * movementSpeed;
            else if (Input.GetAxis("Vertical") < 0)
                transform.position -= transform.forward * Time.deltaTime * movementSpeed;

        staminaBar.value = currentStamina;
        if (runPressed && (isWalking || isRunning) && currentStamina > 0)
        {
            movementSpeed = runSpeed;
            currentStamina = Mathf.Clamp(currentStamina - 1f * Time.deltaTime, 0f, maxStamina);
            CancelInvoke("StaminaRegen");
        }
        else
        {
            movementSpeed = walkSpeed;
            Invoke("StaminaRegen", 1f);
        }

        if (grounded && Input.GetButtonDown("Jump"))
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Walking and running animation
        if (!isWalking && forwardPressed)
            animator.SetBool(isWalkingHash, true);
        else if (isWalking && !forwardPressed)
            animator.SetBool(isWalkingHash, false);

        if (!isRunning && (forwardPressed && runPressed) && currentStamina < 0)
            animator.SetBool(isRunningHash, true);
        else if (isRunning && (!forwardPressed || !runPressed))
            animator.SetBool(isRunningHash, false);

        // Knockback
        if (enemyController.enemyAttack)
            rb.AddForce(Vector3.forward * -enemyController.knockbackForce, ForceMode.Impulse);

        // Player health
        healthBar.value = currentHealth;
        if (currentHealth <= 0)
            SceneManager.LoadScene(2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
            grounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
            grounded = false;
    }

    public void TakeDamage(float damageValue)
    {
        currentHealth -= damageValue;
        AudioManager.instance.PlaySFX("Take Damage");
        Invoke("HealthRegen", 1f);
    }

    void HealthRegen() => currentHealth = Mathf.Clamp(currentHealth + 1f * Time.deltaTime, 0f, maxHealth);

    void StaminaRegen() => currentStamina = Mathf.Clamp(currentStamina + 1f * Time.deltaTime, 0f, maxStamina);

    public void WeaponAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool shootPressed = Input.GetButton("Fire1");
        bool reloadPressed = Input.GetKey("r");

        if (!isWalking && !isRunning)
            if (shootPressed)
                animator.Play("Fire Idle");
            else if (reloadPressed)
                animator.Play("Reload Idle");

        if (isWalking)
            if (shootPressed)
                animator.Play("Fire Walk");
            else if (reloadPressed)
                animator.Play("Reload Walk");

        if (isRunning)
            if (shootPressed)
                animator.Play("Fire Run");
            else if (reloadPressed)
                animator.Play("Reload Run");
    }
}