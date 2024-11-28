using UnityEngine;
using UnityEngine.Animations;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float playerSensitivity = 5f;
    [SerializeField] private float attackDistance = 4f;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float minimumDetectionRange = 10f;
    [SerializeField] private float attackLength = 0.7f;

    private Animator animator;
    private Rigidbody rb;
    private float attackTimer = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (!IsPlayerDetected())
        {
            animator.SetFloat("speed", 0f);
        }

        if (IsPlayerDetected() && distanceToPlayer > attackDistance)
        {
            Vector3 pos = player.transform.position;
            pos.y = transform.position.y;

            Vector3 direction = (pos - transform.position).normalized;
            rb.position += direction * movementSpeed * Time.deltaTime;
            transform.LookAt(pos);

            animator.SetFloat("speed", movementSpeed);
        }
        else
        {
            animator.SetFloat("speed", 0f);
        }

        if (distanceToPlayer <= attackDistance && attackTimer <= 0f)
        {
            StartAttack();
        }

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    bool IsPlayerDetected()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        float detectionThreshold = distanceToPlayer / Mathf.Clamp(PlayerLightSystem.Instance.LightAmount, 0.1f, Mathf.Infinity);
        if (distanceToPlayer <= minimumDetectionRange)
        {
            return true;
        }
        else
        {
            return detectionThreshold < playerSensitivity;
        }
    }

    void StartAttack()
    {
        animator.SetBool("isAttacking", true);
        Invoke("EndAttack", 1 / attackLength * 0.95f);
        attackTimer = attackCooldown;
    }

    void EndAttack()
    {
        animator.SetBool("isAttacking", false);
        if (Vector3.Distance(player.transform.position, transform.position) < attackDistance + 1f)
        {
            player.GetComponent<PlayerMovement>().Attacked(1f);
        }
    }
}
