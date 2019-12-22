using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //モーションを切り替える
        if(Input.GetAxis("Vertical") > 0)
        {
            animator.SetInteger("Vertical", 1);
        }
        else
        {
            animator.SetInteger("Vertical", 0);
        }
    }
}
