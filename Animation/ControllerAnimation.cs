using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerAnimation : MonoBehaviour
{
    public Animator Animation_;

    public void AnimatioN(int type)  // 1 - отпрыжка 2- бег 3 - хрдьба 4- отключит бег/хптьбу
    {
        switch (type)
        {
            case 1:
                Animation_.SetTrigger("Rebound");
                break;
            case 2:
                Animation_.SetBool("Run", true);
                break;
            case 3:
                Animation_.SetBool("False", true);
                break;
            case 4:
                Animation_.SetBool("False", false);
                Animation_.SetBool("Run", false);
                break;
        }
    }
}
