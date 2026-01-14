using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ParkourController : MonoBehaviour
 
{
    [SerializeField] List<ParkourAction> parkourActions;
    bool inAction;
   EnvironmentScanner environmentScanner;
    Animator animator;
    PlayerController playerController;
    private void Awake() 
    {
        environmentScanner = GetComponent<EnvironmentScanner>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }
    private void Update() 
    {
      if(Input.GetButton("Jump")&& ! inAction)
        {
            var hitData = environmentScanner.ObstacleCheck(); ;
            if(hitData.forwardHitFound)
            {
                foreach(var action in parkourActions)
                {
                   if(action.CheckIfPossible(hitData, transform))
                    {
                        StartCoroutine(DoParkourAction(action));
                        break;
                    }
                }
                
            }
        }
    }
    IEnumerator DoParkourAction(ParkourAction action)
    {
        inAction = true;
        playerController.SetControl(false);
        animator.CrossFade(action.AnimName, 0.2f);
        yield return null;
        var animState = animator.GetNextAnimatorStateInfo(0);

        if (!animState.IsName(action.AnimName) == false)
            Debug.Log("parkkour animattion is wrong");

       yield return new WaitForSeconds(animState.length);

        float timer = 0f;
        while ( timer < animState.length)
        {
            timer += Time.deltaTime;
            // rotate player toward obstacle
            if (action.RotateToObstacle)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, action.TargetRotation, playerController.RotationSpeed * Time.deltaTime);
            }
            if (action.EnableTargetMatching)
            {
                MatchTarget(action);
            }
           
        }
        yield return new WaitForSeconds(action.PostActionDelay);
        playerController.SetControl(true);
        inAction = false;
    }
    
    void MatchTarget(ParkourAction action)
    {
        var animState = animator.GetCurrentAnimatorStateInfo(0);
        if (animator.isMatchingTarget || animState.IsTag("Transition")) return;
        if (animator.IsInTransition(0)) return;

        animator.MatchTarget(
            action.MatchPos,
            transform.rotation,
            action.MatchBodyPart,
            new MatchTargetWeightMask(action.MatchPositionWeight, 0),
            action.MatchStartTime,
            action.MatchTargetTime
        );
    }
    /*
    void MatchTarget(ParkourAction action)
    {
        float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (normalizedTime < action.MatchStartTime ||
            normalizedTime > action.MatchTargetTime)
            return;

        if (animator.isMatchingTarget)
            return;

        animator.MatchTarget(
            action.MatchPos,
            transform.rotation,
            action.MatchBodyPart,
            new MatchTargetWeightMask(action.MatchPositionWeight, 0),
            action.MatchStartTime,
            action.MatchTargetTime
        );
    }
  */
}
