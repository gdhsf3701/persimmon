using Plugins.ScriptFinder.RunTime.Finder;
using System;
using csiimnida.CSILib.SoundManager.RunTime;
using UnityEngine;
using Work.Bakbak.Code.Shape;

public class Player : MonoBehaviour
{
    [SerializeField]
    private ScriptFinderSO finderSO;

    [SerializeField]
    private Animator animator;

    private string currentclip;
    private string newclip;
    public void Hit()
    {
        bool died = finderSO.GetTarget<HeartCounter>().Damage();
        if(died )
        {
            Death();
            SoundManager.Instance.PlaySound("OverBGM");
        }
    }

    private void Death()
    {
        animator.SetBool("IDLE", false);
        animator.SetBool("DIE", true);
    }

    public void Attack(ShapType type)
    {
        //animator.SetBool("IDLE", false);

        switch (type)
        {
            case ShapType.Line:
                newclip = "ATTACK";
                break;
            case ShapType.HLine:
                newclip = "ATTACKHLINE";
                break;
            case ShapType.UnderCheck:
                newclip = "ATTACKDOWNCHECK";
                break;
            case ShapType.UpperCheck:
                newclip = "ATTACKUPPERCHECK";
                break;
            case ShapType.Star:
                newclip = "ATTACKSTAR";
                break;
            case ShapType.Circle:
                newclip = "ATTACKCIRCLE";
                break;
            case ShapType.Eleck:
                newclip = "ATTACKELECK";
                break;
        }
        animator.SetTrigger(newclip) ;
        currentclip = newclip;
    }
}
