using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    [SerializeField] Vector3 forwardRayOffset = new Vector3(0, 2.5f, 0);
    [SerializeField] float forwardRayLength = 0.8f;
    [SerializeField] LayerMask obstacleLayer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ObstacleCheck()
    { 
        var hitData = new ObstacleHitData();
        var forwardOrigin = transform.position + forwardRayOffset;
        hitData.forwardHitFound =  Physics.Raycast(forwardOrigin, transform.forward, out RaycastHit hitInfo, forwardRayLength, obstacleLayer);
        Debug.DrawRay(forwardOrigin,transform.forward * forwardRayLength,(hitfound) ? Color.red : Color.white);
    }
}
public struct ObstacleHitData
{
    public bool forwardHitFound;
    public RaycastHit forwardHit;

}