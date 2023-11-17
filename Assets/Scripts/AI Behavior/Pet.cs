using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pet : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    public GameObject _petFood;
    private Animator _animator;
    private bool isOwnerInSight;
    public bool isFoodAvailable;
    [Range(1, 10)]public float range;

    // Start is called before the first frame update
    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
        if(!isOwnerInSight) _navMeshAgent.SetDestination(_petFood.transform.position);

        if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) _navMeshAgent.isStopped = true;
        else _navMeshAgent.isStopped = false;

        if(_navMeshAgent.isStopped)
        {
            _animator.SetBool("isEat", isFoodAvailable);
            _animator.SetBool("isSleep", !isFoodAvailable);
        }
        else
        {
            _animator.SetBool("isEat", false);
            _animator.SetBool("isSleep", false);
        }

        Debug.Log("IsStoped " + _navMeshAgent.isStopped);
    }

    void DetectPlayer()
    {
        // Membuat bola area deteksi
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider collider in colliders)
        {
            // Pastikan collider yang terdeteksi memiliki tag "Player"
            if (collider.CompareTag("Player"))
            {
                Debug.Log("ajg");
                _navMeshAgent.SetDestination(collider.transform.position);
                isOwnerInSight = true;
            }
            else isOwnerInSight = false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
