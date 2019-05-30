using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Animator m_animator;
    GameObject mos;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
        m_animator.SetBool("check", false);
        mos  = GameObject.Find("mos");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0)){
            m_animator.SetBool("check", true);
          //  mos.SetActive(false);
        }
        AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f && info.IsName("die"))
        {
            mos.SetActive(false);
        }
    }
}
