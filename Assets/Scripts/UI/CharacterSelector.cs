using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    int currentIndex = 0;

    void Start()
    {
        // activate only first character on start
        for (int i = 0; i < characters.Length; i++)
            characters[i].SetActive(i == currentIndex);
    }

    void Update()
    {
        // press Tab to switch characters
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            characters[currentIndex].SetActive(false);
            currentIndex = (currentIndex + 1) % characters.Length;
            characters[currentIndex].SetActive(true);
        }
    }
}