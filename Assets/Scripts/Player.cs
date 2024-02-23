using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10;
    public float rotaSpeed = 0.5f;
    public float jumpSpeed = 10;
    public float jumpBackSpeed = 10;
    public float gravity = -9.81f;
    
    private CharacterController _characterController;
    private Animator _animator;
    private float _rotationTime = 0;
    private float _jumpSpeed = 0;
    private float _verticalSpeed = 0;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        //-1 ~ 1
        float x = Input.GetAxis("Horizontal");
        //-1 ~ 1 
        float z = Input.GetAxis("Vertical");
        //动画切换
        _animator.SetFloat("speed",Mathf.Abs(z));
        //
        float rotationY = x * rotaSpeed * Time.deltaTime;
        
        _verticalSpeed = gravity + _jumpSpeed;
        
        transform.Rotate(0, rotationY, 0);
        _characterController.Move(transform.forward * moveSpeed * z * Time.deltaTime + transform.up * _verticalSpeed * Time.deltaTime);

        if (_jumpSpeed > 0)
        {
            _jumpSpeed -= Time.deltaTime * jumpBackSpeed;
            if (_jumpSpeed <= 0)
            {
                _jumpSpeed = 0;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jumpSpeed = jumpSpeed;
            _animator.CrossFadeInFixedTime("RunJump",0.1f);
        }
    }
}
