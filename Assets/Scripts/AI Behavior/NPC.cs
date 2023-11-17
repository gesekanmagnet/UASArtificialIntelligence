using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    public StarterAssetsInputs _playerCombat;
    bool isDeath;

    [Range(1, 10)]public float radius;
    // Start is called before the first frame update
    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDeath) return;

        DetectPlayer();
        _animator.SetFloat("Speed", _navMeshAgent.speed);

        if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) _navMeshAgent.speed = 0;
    }

    void DetectPlayer()
    {
        // Membuat bola area deteksi
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            // Pastikan collider yang terdeteksi memiliki tag "Player"
            if (collider.CompareTag("Player") && _playerCombat.attack)
            {
                Debug.Log("bagsad;");
                UnityEngine.Vector3 direction = (transform.position - _playerCombat.gameObject.transform.position).normalized;
                _navMeshAgent.SetDestination(_playerCombat.gameObject.transform.position + direction * 10);
                _navMeshAgent.speed = 4;
            }
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Hit"))
        {
            _animator.SetTrigger("isDeath");
            isDeath = true;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color =Color.black;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
