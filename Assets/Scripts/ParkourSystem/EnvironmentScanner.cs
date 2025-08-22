using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    [SerializeField] Vector3 forwardRayOffset = new Vector3(0, 2.5f, 0);
    [SerializeField] float forwardRayLength = 5f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ObstacleCheck()
    { RaycastHit hitInfo;
        Physics.Raycast(transform.position + forwardRayOffset ,transform.forward,out RaycastHit hitInfo,)
    }
}
