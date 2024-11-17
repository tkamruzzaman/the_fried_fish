using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button exitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() => { SceneManager.LoadScene(1); });
        optionButton.onClick.AddListener(() => { });
        exitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                EditorApplication.ExitPlaymode();
            }
#else
            Application.Quit();
#endif

        });
    }
}
