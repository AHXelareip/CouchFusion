using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFPlayerAnimController : MonoBehaviour
{
    public System.Action OnEndJump;

    public void EndJump()
    {
        if (OnEndJump != null)
        {
            OnEndJump();
        }
    }
}
