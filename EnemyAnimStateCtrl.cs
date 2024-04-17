using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimStateCtrl : MonoBehaviour
{
    Animator animator;
    int isWalkingHash, isRunningHash;
    EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash),
            isRunning = animator.GetBool(isRunningHash);

        if (!isWalking && enemyController.enemyPatrol)
            animator.SetBool(isWalkingHash, true);
        else if (isWalking && !enemyController.enemyPatrol)
            animator.SetBool(isWalkingHash, false);

        if (!isRunning && enemyController.enemySpotted)
            animator.SetBool(isRunningHash, true);
        else if (isRunning && !enemyController.enemySpotted)
            animator.SetBool(isRunningHash, false);

        if (enemyController.enemyAttack)
            animator.Play("Fire Walk");
    }

}
