using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float resetDelay = 1.5f; // Retraso antes de reiniciar el nivel
    [SerializeField] private AudioSource musicSource; // Fuente de audio para reproducir sonidos
    [SerializeField] private AudioSource sfxSource; // Fuente de audio para reproducir sonidos
    [SerializeField] private AudioClip menuMusic; // Clip de audio para el sonido de reinicio
    [SerializeField] private AudioClip gameMusic; // Clip de audio para el sonido de reinicio
    [SerializeField] private AudioClip failLevelSound;
    [SerializeField] private AudioClip completedLevelSound;
    public static GameManager Instance;
    void Awake()
    {
        if (Instance == null) {
            Instance = this; // Esto quiere decir que se guardara el GameManager
            DontDestroyOnLoad(gameObject); // Mantener este GameManager a través de las escenas
        }  
        else Destroy(Instance); // Si ya existe, destruir este GameManager para evitar duplicados
    }


    public void LoadNextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex; // Obtener el índice del nivel actual
        int nextLevel = currentLevel + 1; // Incrementar el índice para cargar el siguiente nivel
        if (nextLevel < SceneManager.sceneCountInBuildSettings) // Verificar que el siguiente nivel existe
        {
            if(currentLevel > PlayerPrefs.GetInt("HigherLevel")) PlayerPrefs.SetInt("HigherLevel", currentLevel);

            SceneManager.LoadScene(nextLevel); // Cargar el siguiente nivel
        }
        else
        {
            SceneManager.LoadScene(0); // Si no hay más niveles, reiniciar al primer nivel
            musicSource.clip = menuMusic;
            musicSource.Play();
        }
        PlaySound(completedLevelSound, 0.3f);
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex); // Cargar el nivel especificado por el índice
        musicSource.clip = gameMusic;
        musicSource.Play();
    }

    public void ResetLevel()
    {
        StartCoroutine(nameof(ResetLevelDelayed)); // Iniciar la corrutina para reiniciar el nivel después de un retraso
        PlaySound(failLevelSound, 0.5f);
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        sfxSource.PlayOneShot(clip, volume); // Reproducir el clip de audio proporcionado
    }

    private IEnumerator ResetLevelDelayed()
    {
        yield return new WaitForSeconds(resetDelay); // Esperar 1 segundo antes de reiniciar el nivel
        int currentLevel = SceneManager.GetActiveScene().buildIndex; // Obtener el índice del nivel actual
        SceneManager.LoadScene(currentLevel); // Reiniciar el nivel actual
    }
}
