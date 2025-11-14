using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FarmerMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private IEntity entity;

    // Tốc độ di chuyển
    [SerializeField] private float moveSpeed = 3.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing!");
        }
        else
        {
            agent.speed = moveSpeed; // gán speed cho NavMeshAgent
        }
    }

    /// <summary>
    /// Gán entity cho farmer
    /// </summary>
    public void SetEntity(IEntity newEntity)
    {
        entity = newEntity;
    }

    /// <summary>
    /// Di chuyển tới một vị trí cụ thể
    /// </summary>
    public void MoveTo(Vector3 targetPosition)
    {
        if (agent == null)
            return;

        agent.SetDestination(targetPosition);
    }

    /// <summary>
    /// Kiểm tra farmer đã đến điểm chưa
    /// </summary>
    public bool HasReachedDestination(float stoppingDistance = 0.1f)
    {
        if (agent == null)
            return false;

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= stoppingDistance)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Dừng di chuyển
    /// </summary>
    public void StopMoving()
    {
        if (agent != null && agent.hasPath)
        {
            agent.ResetPath();
        }
    }

    /// <summary>
    /// Set tốc độ động
    /// </summary>
    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
        if (agent != null)
            agent.speed = moveSpeed;
    }

    /// <summary>
    /// Lấy tốc độ hiện tại
    /// </summary>
    public float GetSpeed()
    {
        return moveSpeed;
    }
}
