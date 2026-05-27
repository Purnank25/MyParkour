using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    [SerializeField] string cameraTargetName = "CameraTarget";

    CameraController cameraController;
    int currentIndex = 0;

    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();

        // deactivate all except first
        for (int i = 0; i < characters.Length; i++)
            characters[i].SetActive(i == currentIndex);

        // set camera to first character
        SetCameraTarget(characters[currentIndex]);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchCharacter();
    }

    void SwitchCharacter()
    {
        characters[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % characters.Length;
        characters[currentIndex].SetActive(true);

        SetCameraTarget(characters[currentIndex]);
    }

    void SetCameraTarget(GameObject character)
    {
        // find CameraTarget child
        Transform cameraTarget = character.transform.Find(cameraTargetName);
        Transform target = cameraTarget != null ? cameraTarget : character.transform;
        cameraController.SetFollowTarget(target);

        // update PlayerController camera reference
        PlayerController pc = character.GetComponent<PlayerController>();
        if (pc != null)
            pc.SetCamera(cameraController);
    }
}