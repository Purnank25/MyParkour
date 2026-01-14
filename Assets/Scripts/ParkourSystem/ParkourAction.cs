using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(menuName ="Parkour systems /new parkour Actions")]
public class ParkourAction :ScriptableObject
{
    [SerializeField] string animName;
    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;
    [SerializeField] bool rotateToObstacle;

    [Header("Target Matching")]
    [SerializeField] bool enableTargetMatching = true;
    [SerializeField] AvatarTarget matchBodyPart;
    [SerializeField] float matchStartTime;
    [SerializeField ]float matchTargetTime;
    [SerializeField] Vector3 matchPositionWeight = new Vector3(0,1,0);
    [SerializeField] float postActionDelay;

    public Quaternion TargetRotation { get; set; }
    public Vector3 MatchPos { get; set; }
    public bool CheckIfPossible(ObstacleHitData hitData, Transform player)
    {
       float height= hitData.heightHit.point.y - player.position.y;
        if( height <= minHeight || height >= maxHeight)
        {
            return false;
        }
        if (rotateToObstacle) 
        {
                TargetRotation = Quaternion.LookRotation(-hitData.forwardHit.normal);
        }
        if (enableTargetMatching)
        {
            MatchPos = hitData.heightHit.point;
        }
        return true;
    }
    public string AnimName => animName;
    public bool RotateToObstacle => rotateToObstacle;

    public bool EnableTargetMatching => enableTargetMatching;   
    public AvatarTarget MatchBodyPart => matchBodyPart;
    public float MatchStartTime => matchStartTime;
    public float MatchTargetTime => matchTargetTime;
    public Vector3 MatchPositionWeight => matchPositionWeight;
    public float PostActionDelay => postActionDelay;
}
