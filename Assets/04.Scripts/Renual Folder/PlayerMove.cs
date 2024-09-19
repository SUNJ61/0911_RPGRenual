using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum PlayerState { IDLE, ATTACK, UNDER_ATTACK, DEAD };
    public PlayerState state = PlayerState.IDLE;

    [SerializeField] Transform PlayerTr;
    [SerializeField] CharacterController characterController;
    private PlayerAni playerAni;

    private Vector3 _MoveDir;
    public Vector3 MoveDir
    {
        set { _MoveDir = value; } //적용 할 때 단위 벡터로 설정
    }

    private float _RotDirX;
    public float RotDirX
    {
        set { _RotDirX = value; }
    }
    private float _RotDirY;
    public float RotDirY
    {
        set { _RotDirY = value; }
    }
    private float _RunValue;
    public float RunValue
    {
        set { _RunValue = value; }
    }

    private Vector3 moveVelocity = Vector3.zero;

    [SerializeField] float walkSpeed = 5.0f;
    [SerializeField] float runSpeed = 10.0f;

    private bool IsGrounded;
    private bool IsRun;

    void Awake()
    {
        PlayerTr = transform;
        characterController = GetComponent<CharacterController>();
        playerAni = transform.GetChild(0).GetComponent<PlayerAni>();
    }
    void Update()
    {
        switch (state)
        {
            case PlayerState.IDLE:
                PlayerIdleAndMove();
                break;
            case PlayerState.ATTACK:

                break;
            case PlayerState.UNDER_ATTACK:

                break;
            case PlayerState.DEAD:

                break;
        }
    }

    void PlayerIdleAndMove()
    {
        RunCheck();
        if (characterController.isGrounded)
        {
            if (IsGrounded == false)
            {
                IsGrounded = true;
                playerAni.IsGrounded = IsGrounded;
            }
            //animator.SetBool("isGrounded", true);
            CalcInputMove();
            RaycastHit groundHit;
            if (GroundCheck(out groundHit))
                moveVelocity.y = IsRun ? -runSpeed : -walkSpeed;
            else
                moveVelocity.y = -1f;
            //PlayerAttack();

        }
        else
        {
            if (IsGrounded == false)
            { 
                IsGrounded = true;
                playerAni.IsGrounded = IsGrounded;
            }
            else
                //animator.SetBool("isGrounded", false);

                moveVelocity += Physics.gravity * Time.deltaTime;
        }
        characterController.Move(moveVelocity * Time.deltaTime);
    }

    void CalcInputMove()
    {
        moveVelocity = _MoveDir.normalized * (IsRun ? runSpeed : walkSpeed);
        //animator.SetFloat("speedX", Input.GetAxis("Horizontal"));
        //animator.SetFloat("speedY", Input.GetAxis("Vertical"));
        moveVelocity = transform.TransformDirection(moveVelocity);
        if (0.01f < moveVelocity.sqrMagnitude)
        {
            if (IsRun)
            {
                Quaternion characterRot = Quaternion.LookRotation(moveVelocity);
                characterRot.x = characterRot.z = 0f;
                PlayerTr.rotation = Quaternion.Slerp(PlayerTr.rotation, characterRot, Time.deltaTime * 1.0f);
            }
            else
            {
                PlayerTr.Rotate(Vector3.right * _RotDirX * Time.deltaTime * 10.0f);
                PlayerTr.Rotate(Vector3.up * _RotDirY * Time.deltaTime * 10.0f);
            }
        }
    }

    void RunCheck()
    {
        if (_RunValue > 0 && _MoveDir.normalized == new Vector3(0f, 0f, 1.0f)) //RunValue는 1또는 0만 가져온다.
            IsRun = true;
        else
            IsRun = false;

        playerAni.IsRun = IsRun;
    }

    bool GroundCheck(out RaycastHit hit)
    {
        return Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f);
    }
}
