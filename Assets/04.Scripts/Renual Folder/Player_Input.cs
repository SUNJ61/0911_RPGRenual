using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//여기선 인풋 시스템으로 들어온 값만 입력 받음
//받은 입력 값은 move로 보내야함
public class Player_Input : MonoBehaviour
{
    private PlayerMove playerMove; //이동 관리및 플레이어 상태 관리
    private PlayerAni playerAni; //애니메이션 관리

    //아래의 값들 전부 프로퍼티로 전달할 예정
    private Vector3 MovePos;

    private float MoveRotX;
    private float MoveRotY;
    private float RunValue;

    private bool isFire = false;
    public bool IsFire { set { isFire = value; } } //move에서 애니메이션이 끝나면 호출하여 업데이트

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAni = transform.GetChild(0).GetComponent<PlayerAni>();

        //StartCoroutine(DebugLog()); //입력값 체크용
    }
    #region 입력 값 체크용 스타트 코루틴
    IEnumerator DebugLog()
    {
        while (true)
        {
            Debug.Log($"마우스 위아래 값 : {MoveRotX}");
            Debug.Log($"마우스 좌우 값 : {MoveRotY}");
            Debug.Log($"달리기 입력 값 : {RunValue}");
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
        MoveRotX = rot.y; //x축 회전은 마우스 위아래 움직임
        MoveRotY = rot.x; //y축 회전은 마우스 좌우 움직임
    }
    private void OnFire(InputValue State)
    {
        float state = State.Get<float>();
        if (!isFire) //중복 업데이트 방지
        {
            isFire = true;
            //playermove에 isFire를 업데이트하는 프로퍼티 호출 코드 작성 할 것
        }  
    }
    private void OnRun(InputValue Runvalue)
    {
        RunValue = Runvalue.Get<float>();
    }
}
