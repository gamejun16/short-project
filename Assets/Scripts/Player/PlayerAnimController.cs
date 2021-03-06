﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Animations;

public class PlayerAnimController : MonoBehaviour
{
    /*****
     * 
     * Player의 모든 Animation 조작을 전담하는 스크립트
     * 
     * */

    public PlayerController playerController;
    public PlayerStatusManager playerStatusManager;
    public GearPositionController gearPositionController;
   // public UIStatusManager uiStatusManager;

    public Animator animator;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Anim_Hitted()
    {
        animator.SetBool("isHitted", true);

        SoundManager.soundManager.playPlayerSound((int)SoundManager.player.DAMAGED, audioSource);
    }
    void Anim_Hitted_Done()
    {
        animator.SetBool("isHitted", false);
    }


    public void Anim_PowerUP()
    {
        animator.SetBool("isPowerUp", true);
    }
    void Anim_PowerUP_Stay()
    {
        // 레이저발싸
        playerController.LaserSetActive(true);

        // 시간 측정 시작
        StartCoroutine("PowerUP_TimeCheck");
    }
    IEnumerator PowerUP_TimeCheck()
    {
        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime;
            // 지속시간
            if(timer > 5f)
            {
                Anim_PowerUP_Done();
                StopCoroutine("PowerUP_TimeCheck");
            }
            yield return null;
        }
    }
    void Anim_PowerUP_Done()
    {
        // 레이저 종료
        playerController.LaserSetActive(false);
        animator.SetBool("isPowerUp", false);

        // 불필요한 피격 애니메이션 출력을 방지
        animator.SetBool("isHitted", false);

        // Gage 초기화
        UIStatusManager.uiStatusManager.gageManager(true);

        // 신규 기어 장착
        gearPositionController.newGear();
    }
}
