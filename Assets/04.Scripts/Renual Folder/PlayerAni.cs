using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAni : MonoBehaviour
{
    private Animator animator;
    private PlayerMove playerMove;
    private Player_Input player_Input;

    [SerializeField]private Vector3 _MoveDir = Vector3.zero;
    public Vector3 MoveDir
    {
        set { _MoveDir = value.normalized; } //���� �� �� ���� ���ͷ� ����
    }

    private bool isGrounded;
    public bool IsGrounded
    {
        set { isGrounded = value; }
    }
    private bool isFire;
    public bool IsFire
    {
        set { isFire = value; }
    }
    private bool isRun;
    public bool IsRun
    {
        set { isRun = value; }
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
        playerMove = transform.GetComponentInParent<PlayerMove>();
        player_Input = transform.GetComponentInParent<Player_Input>();
    }
    void Update()
    {
        PlayAni();
    }

    private float nextTime = 0f;

    private void UpdateAni()
    {
        animator.SetFloat("speedX", _MoveDir.x);
        animator.SetFloat("speedY", _MoveDir.z);

        if (_MoveDir == new Vector3(0f, 0f, 1.0f))
            animator.SetBool("isRun", isRun);
        else
            animator.SetBool("isRun", false);
    }

    private void PlayAni()
    {
        if (isGrounded)
        {
            animator.SetBool("isGrounded", true);
            UpdateAni();
            FireAni();
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }
    }

    private void FireAni()
    {
        if (isFire) //������ Ŭ���� ª�� �ð��� ������ �߻��ϸ� ������ �Լ��� �����°� ���� �̸� �ذ��ؾ��� -> Fixed update���� ������Ʈ�� ���� ������ ȣ��
        {
            if (playerMove.state != PlayerMove.PlayerState.ATTACK)
            {
                playerMove.state = PlayerMove.PlayerState.ATTACK;
                animator.SetTrigger("swordAttackTrigger");
            }
            AttackTimeState();
        }
    }

    void AttackTimeState()
    {
        nextTime += Time.deltaTime;
        if (1.5f <= nextTime)
        {
            player_Input.IsFire = false;
            nextTime = 0f;
            playerMove.state = PlayerMove.PlayerState.IDLE;
        }
    }
}
