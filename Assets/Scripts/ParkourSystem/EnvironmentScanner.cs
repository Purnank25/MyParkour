using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    [SerializeField] Vector3 forwardRayOffset = new Vector3(0, 1.2f, 0);
    [SerializeField] float forwardRayLength = 0.8f;
    [SerializeField] float heightRayLength = 5;
    [SerializeField] LayerMask obstacleLayer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public ObstacleHitData ObstacleCheck()
    { 
        var hitdata = new ObstacleHitData();
        var forwardOrigin = transform.position + forwardRayOffset;
       hitdata.forwardHitFound =  Physics.Raycast(forwardOrigin, transform.forward, out  hitdata.forwardHit, forwardRayLength, obstacleLayer);
         Debug.DrawRay(forwardOrigin,transform.forward * forwardRayLength,(hitdata.forwardHitFound ? Color.red : Color.white));

        if (hitdata.forwardHitFound)
        {
            var heightOrigin = hitdata.forwardHit.point + Vector3.up * heightRayLength;
            hitdata.heightHitFound = Physics.Raycast(heightOrigin, Vector3.down, out hitdata.heightHit, heightRayLength, obstacleLayer );
            Debug.DrawRay(heightOrigin,Vector3.down * heightRayLength, (hitdata.heightHitFound ? Color.red : Color.white));
        }
            return hitdata ;
    }
}

public struct ObstacleHitData
{
    public bool forwardHitFound;
    public bool heightHitFound;
    public RaycastHit forwardHit;
    public RaycastHit heightHit;
}