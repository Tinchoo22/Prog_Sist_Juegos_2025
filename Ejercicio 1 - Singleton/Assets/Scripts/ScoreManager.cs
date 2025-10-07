using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton global

    public int score = 0; // Puntaje total acumulado
    private int[] levelScoreThresholds = { 5, 10, 15 }; // Umbrales para avanzar o finalizar
    private int currentLevelReached = 0; // Nivel ya alcanzado

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistente entre escenas
        }
        else
        {
            Destroy(gameObject); // Previene duplicados
        }
    }

    public void AddPoint()
    {
        score += 1; // Suma un punto
        Debug.Log("Puntaje actual: " + score);

        // Avanza solo si no hemos llegado al último umbral aún
        if (currentLevelReached < levelScoreThresholds.Length &&
            score >= levelScoreThresholds[currentLevelReached])
        {
            // Si el puntaje es 15 o más, muestra mensaje final
            if (score >= 15 && currentLevelReached == levelScoreThresholds.Length - 1)
            {
                Debug.Log("¡Completaste los niveles!");
                currentLevelReached++; // Para no repetir el mensaje
            }
            else
            {
                currentLevelReached++;
                LoadNextLevel(); // Cambia a la siguiente escena
            }
        }
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            Debug.Log("No hay más escenas disponibles.");
        }
    }
}