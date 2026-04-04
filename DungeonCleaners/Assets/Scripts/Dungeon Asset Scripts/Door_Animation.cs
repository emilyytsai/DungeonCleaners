using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Door_Animation : MonoBehaviour
{

    [Header("Animation Settings")]
    public Animator doorAnimator;
    public string animationTriggerName = "Open"; // The name of the trigger parameter in your Animator
    public float animationDuration = 1.0f;       // How long to wait before fading

    [Header("Fade Settings")]
    public Image fadeOverlay;                    // Drag your UI Black Image here
    public float fadeDuration = 1.0f;            // How long the fade takes

    [Header("Scene Loading")]
    public int sceneIndexToLoad = 0;             // The scene index to load

    private bool isTransitioning = false;        // Prevents the sequence from triggering twice

    // Use OnTriggerEnter if the door has "Is Trigger" checked. 
    // If you are using 2D, change this to OnTriggerEnter2D(Collider2D other)
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding is exactly the Player and we aren't already transitioning
        if (other.CompareTag("Player") && !isTransitioning)
        {
            StartCoroutine(DoorSequence());
        }
    }

    private IEnumerator DoorSequence()
    {
        isTransitioning = true; // Lock the door so it can't be triggered again

        // 1. Play the door animation
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(animationTriggerName);
        }
        else
        {
            Debug.LogWarning("No Animator assigned to the door!");
        }

        // Wait for the animation to visually finish
        yield return new WaitForSeconds(animationDuration);

        // 2. Fade to black
        if (fadeOverlay != null)
        {
            // Make sure the image is active and set its starting alpha to 0 (transparent)
            fadeOverlay.gameObject.SetActive(true);
            Color fadeColor = fadeOverlay.color;
            fadeColor.a = 0f;
            fadeOverlay.color = fadeColor;

            float timer = 0f;

            // Gradually increase the alpha over time
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeColor.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                fadeOverlay.color = fadeColor;
                yield return null; // Wait for the next frame
            }
        }
        else
        {
            Debug.LogWarning("No Fade Overlay UI Image assigned!");
        }

        // 3. Load the next scene
        SceneManager.LoadScene(sceneIndexToLoad);
    }

}
