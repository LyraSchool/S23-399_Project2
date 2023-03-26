using UnityEngine;
using UnityEngine.AI;


using Random = UnityEngine.Random;

public class GhostController : MonoBehaviour
{
    // Components on this GO
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private VisibilityController visibilityController;
    
    // Components on other GOs
    [SerializeField] private Transform player;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform[] rooms;
    
    // Properties
    // [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float sightDistance = 10f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float health = 100f;
    
    [SerializeField] private AudioClip huntingSound;
    [SerializeField] private AudioClip deathSound;
    
    public bool isHuntingOrFakeHunting = false;
    
    
    private float _timeSinceLastSeenPlayer = 0f;
    private float _timeSinceLastHunt = 0f;
    
    private Vector3 _targetPos;
    
    // Animation hashes
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int DieTrigger = Animator.StringToHash("die");
    private static readonly int Speed = Animator.StringToHash("speed");


    private void Start()
    {
        int roomIndex = Random.Range(0, rooms.Length);
        _targetPos = rooms[roomIndex].position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsPlayerInSight() && (isHuntingOrFakeHunting || _timeSinceLastHunt > 5f))
        {
            if (!isHuntingOrFakeHunting) StartHunting();
            _timeSinceLastSeenPlayer = 0f;
            _targetPos = player.position;
        }
        else
        {
            _timeSinceLastSeenPlayer += Time.deltaTime;
            if (_timeSinceLastSeenPlayer > 5f && isHuntingOrFakeHunting)
            {
                StopHunting();
                int roomIndex = Random.Range(0, rooms.Length);
                _targetPos = rooms[roomIndex].position;
            }
        }
        
        if (!isHuntingOrFakeHunting)
        {
            // Handle wandering
            _timeSinceLastHunt += Time.deltaTime;
            
            if (Vector3.Distance(transform.position, _targetPos) < 1f)
            {
                // Reached target position, get new target position
                int roomIndex = Random.Range(0, rooms.Length);
                _targetPos = rooms[roomIndex].position;
            }
            
        }
        else
        {
            // Handle hunting
            _timeSinceLastHunt = 0;
            
            // If player is in sight and within attack range, attack
            if (IsPlayerInAttackRange()) 
            {
                animator.SetTrigger(Attack);
            }
        }
        
        navMeshAgent.SetDestination(_targetPos);
        animator.SetFloat(Speed, navMeshAgent.velocity.magnitude);
    }
    
    private bool IsPlayerInSight()
    {
        Vector3 position = transform.position;
        Vector3 rayDirection = player.position - position;
        
        // Draw ray from ghost to player
        Debug.DrawRay(position, rayDirection, Color.red);
        
        return Physics.Raycast(position, rayDirection, out RaycastHit hit, sightDistance) && hit.collider.CompareTag("Player");
    }
    
    public void TakeDamage(float damage)
    {
        if (!enabled) return;
        
        health -= damage;
        if (health <= 0f) Die();
    }

    private void Die()
    {
        StopHunting();
        animator.SetTrigger(DieTrigger);
        audioSource.PlayOneShot(deathSound);
        
        visibilityController.SetVisibility(true);

        // Disable all scripts on this GO
        foreach (MonoBehaviour script in GetComponents<MonoBehaviour>())
        {
            script.enabled = false;
        }
        
        // Disable nav mesh agent
        navMeshAgent.enabled = false;
        
        // Disable collider
        GetComponent<Collider>().enabled = false;
    }

    private bool IsPlayerInAttackRange()
    { 
        float distance = Vector3.Distance(transform.position, player.position);
        // Return true if the distance between the ghost and the player is less than the attack range and the player is in sight
        return distance < 0.6f || (distance < attackRange && IsPlayerInSight());
    }

    private void StartHunting()
    {
        isHuntingOrFakeHunting = true;
        
        // Play sound
        audioSource.clip = huntingSound;
        audioSource.loop = true;
        audioSource.Play();
        
        playerController.StartHeartbeat();
    }
    
    private void StopHunting()
    {
        isHuntingOrFakeHunting = false;
        
        // Stop sound
        audioSource.Stop();
        
        playerController.StopHeartbeat();
    }
    
    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(position, sightDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, attackRange);
    }
}
