using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationLogic : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void PerformAttack_Signal()
    {
        Debug.Log($"{name} - Performing attack!", this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetFloat("MovementSpeed", 1 - animator.GetFloat("MovementSpeed"));
        }else if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetBool("Attacking", !animator.GetBool("Attacking"));
        }
    }

}
