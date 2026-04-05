using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentHUD : MonoBehaviour
{
    public static PersistentHUD instance;

    [SerializeField] private string[] hiddenInScenes;

    private Canvas canvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            canvas = GetComponent<Canvas>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        UpdateHUDVisibility(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateHUDVisibility(scene.name);
    }

    private void UpdateHUDVisibility(string sceneName)
    {
        if (canvas == null)
        {
            canvas = GetComponent<Canvas>();
        }

        bool shouldHide = false;

        foreach (string hiddenScene in hiddenInScenes)
        {
            if (sceneName == hiddenScene)
            {
                shouldHide = true;
                break;
            }
        }

        canvas.enabled = !shouldHide;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}