using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//���⼱ ��ǲ �ý������� ���� ���� �Է� ����
//���� �Է� ���� move�� ��������
public class Player_Input : MonoBehaviour
{
    private PlayerMove playerMove; //�̵� ������ �÷��̾� ���� ����
    private PlayerAni playerAni; //�ִϸ��̼� ����

    //�Ʒ��� ���� ���� ������Ƽ�� ������ ����
    private Vector3 MovePos;

    private float MoveRotX;
    private float MoveRotY;
    private float RunValue;

    private bool isFire = false;
    public bool IsFire { set { isFire = value; } } //move���� �ִϸ��̼��� ������ ȣ���Ͽ� ������Ʈ

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAni = transform.GetChild(0).GetComponent<PlayerAni>();

        //StartCoroutine(DebugLog()); //�Է°� üũ��
    }
    #region �Է� �� üũ�� ��ŸƮ �ڷ�ƾ
    IEnumerator DebugLog()
    {
        while (true)
        {
            Debug.Log($"���콺 ���Ʒ� �� : {MoveRotX}");
            Debug.Log($"���콺 �¿� �� : {MoveRotY}");
            Debug.Log($"�޸��� �Է� �� : {RunValue}");
            yield return new WaitForSeconds(2.0f);
        }
    }
    #endregion
    void Update()
    {
        playerMove.MoveDir = MovePos;
        playerAni.MoveDir = MovePos;

        playerMove.RotDirX = MoveRotX;
        playerMove.RotDirY = MoveRotY;

        playerMove.RunValue = RunValue;
        
        playerAni.IsFire = isFire;
    }

    private void OnMove(InputValue Pos)
    {
        Vector2 dir = Pos.Get<Vector2>();
        MovePos = new Vector3(dir.x, 0f, dir.y);
    }
    private void OnLook(InputValue Rot)
    {
        Vector2 rot = Rot.Get<Vector2>();
        MoveRotX = rot.y; //x�� ȸ���� ���콺 ���Ʒ� ������
        MoveRotY = rot.x; //y�� ȸ���� ���콺 �¿� ������
    }
    private void OnFire(InputValue State)
    {
        float state = State.Get<float>();
        if (!isFire) //�ߺ� ������Ʈ ����
        {
            isFire = true;
            //playermove�� isFire�� ������Ʈ�ϴ� ������Ƽ ȣ�� �ڵ� �ۼ� �� ��
        }  
    }
    private void OnRun(InputValue Runvalue)
    {
        RunValue = Runvalue.Get<float>();
    }
}
