using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    [SerializeField] string cameraTargetName = "CameraTarget";
    [SerializeField] TextMeshProUGUI selectedCharacterText; //  ADD THIS

    CameraController cameraController;
    int currentIndex = 0;

    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();

        for (int i = 0; i < characters.Length; i++)
            characters[i].SetActive(i == currentIndex);

        SetCameraTarget(characters[currentIndex]);
        UpdateText(); //  show first character name on start

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == null)
                Debug.LogError($"CharacterSelector: characters[{i}] is NULL!");
            else
                Debug.Log($"characters[{i}] = {characters[i].name}");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            SelectCharacter((currentIndex + 1) % characters.Length);

        for (int i = 0; i < characters.Length && i < 9; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectCharacter(i);
    }

    public void SelectCharacter(int index)
    {
        if (index < 0 || index >= characters.Length) return;

        if (characters[index] == null)
        {
            Debug.LogWarning($"CharacterSelector: slot {index} is empty!");
            return;
        }

        characters[currentIndex].SetActive(false);
        currentIndex = index;
        characters[currentIndex].SetActive(true);
        SetCameraTarget(characters[currentIndex]);
        UpdateText(); // update text every time character switches
    }

    // ADD THIS METHOD
    void UpdateText()
    {
        if (selectedCharacterText != null)
            selectedCharacterText.text = "Selected: " + characters[currentIndex].name;
        else
            Debug.LogWarning("CharacterSelector: selectedCharacterText not assigned!");
    }

    public void ConfirmSelection()
    {
        Debug.Log($"Confirmed: {characters[currentIndex].name}");
        PlayerPrefs.SetInt("SelectedCharacter", currentIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene(2);
    }

    void SetCameraTarget(GameObject character)
    {
        if (cameraController == null) return;

        Transform cameraTarget = character.transform.Find(cameraTargetName);
        Transform target = cameraTarget != null ? cameraTarget : character.transform;
        cameraController.SetFollowTarget(target);

        PlayerController pc = character.GetComponent<PlayerController>();
        if (pc != null)
            pc.SetCamera(cameraController);
    }
}