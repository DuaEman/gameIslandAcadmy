using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Import for scene management
using System.Collections;

public class LoadingBarController : MonoBehaviour
{
    public Slider loadingBar; // Reference to the slider UI element
    public GameObject loadingPanel; // Reference to the panel containing the loading bar
    public float loadingTime = 3f; // Total time for the loading process

    private bool isLoading = false;

    void Start()
    {
        // Ensure loading panel is initially inactive
        if (loadingPanel != null)
            loadingPanel.SetActive(false);

        if (loadingBar != null)
            loadingBar.value = 0f;

        // Automatically start loading when the scene starts
        StartLoading();
    }

    public void StartLoading()
    {
        if (!isLoading)
        {
            StartCoroutine(ActivateLoadingBar());
        }
    }

    private IEnumerator ActivateLoadingBar()
    {
        isLoading = true;

        // Activate the loading panel
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            if (loadingBar != null)
                loadingBar.value = Mathf.Clamp01(elapsedTime / loadingTime); // Update the slider value
            yield return null;
        }

        // Finish loading
        if (loadingBar != null)
            loadingBar.value = 1f;

        // Optionally deactivate the loading panel after loading is complete
        yield return new WaitForSeconds(0.5f);
        if (loadingPanel != null)
            loadingPanel.SetActive(false);

        isLoading = false;

        // Load the StartScene
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        // Replace "StartScene" with the name of your next scene
        SceneManager.LoadScene("StartScene");
    }
}