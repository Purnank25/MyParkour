using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Assign prefabs in SAME ORDER as CharacterSelector")]
    [SerializeField] GameObject[] characterPrefabs;

    [Header("Where the player spawns")]
    [SerializeField] Transform spawnPoint;

    void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

        Debug.Log($"GameManager: Loading character index {selectedIndex}");

        if (characterPrefabs.Length == 0)
        {
            Debug.LogError("GameManager: No prefabs assigned!");
            return;
        }

        if (selectedIndex < 0 || selectedIndex >= characterPrefabs.Length)
        {
            Debug.LogWarning($"GameManager: Index {selectedIndex} out of range. Defaulting to 0.");
            selectedIndex = 0;
        }

        if (characterPrefabs[selectedIndex] == null)
        {
            Debug.LogError($"GameManager: Prefab at index {selectedIndex} is null!");
            return;
        }

        // Spawn the selected character
        Vector3 pos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        Quaternion rot = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

        GameObject player = Instantiate(characterPrefabs[selectedIndex], pos, rot);
        Debug.Log($"GameManager: Spawned {player.name}");

        // Hook up camera
        CameraController cam = Camera.main != null
            ? Camera.main.GetComponent<CameraController>()
            : null;

        if (cam != null)
        {
            Transform camTarget = player.transform.Find("CameraTarget");
            cam.SetFollowTarget(camTarget != null ? camTarget : player.transform);

            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null)
                pc.SetCamera(cam);
        }
    }
}