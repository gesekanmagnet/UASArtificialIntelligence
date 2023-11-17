using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private StarterAssetsInputs _input;
    private ThirdPersonController _playerController;
    private Animator _animator;

    void Awake()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _playerController = GetComponent<ThirdPersonController>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_input.attack && _playerController.Grounded)
        {
            _animator.SetTrigger("isAttack");
            _playerController.canMove = !_input.attack;
        }
            _input.attack = false;
    }

    public void InputAttack()
    {
    }
}
