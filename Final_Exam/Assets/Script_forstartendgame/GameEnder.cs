using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnder : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float delayBeforeEnd = 2f;
    [SerializeField] private string endMessage = "You are home!";
    [SerializeField] private bool reloadScene = true;
    [SerializeField] private bool quitGame = false;
    [SerializeField] private string nextSceneName = "";
    
    [Header("UI (Optional)")]
    [SerializeField] private GameObject endGamePanel;
    
    void Start()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false);
        }
    }
    
    // Эту функцию можно вызвать через On Interact()
    public void EndGame()
    {
        Debug.Log($"<color=green>{endMessage}</color>");
        
        // Показать UI если есть
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
        }
        
        // Завершить игру через delay
        Invoke(nameof(FinishGame), delayBeforeEnd);
    }
    
    void FinishGame()
    {
        Time.timeScale = 1f; // Restore time
        
        if (reloadScene)
        {
            // Перезагрузить уровень
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (quitGame)
        {
            // Выйти из игры
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
        else if (!string.IsNullOrEmpty(nextSceneName))
        {
            // Загрузить следующую сцену
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
// ```
//
// ---
//
// ## **Настройка:**
//
// ### **Шаг 1: Добавьте скрипт к зданию**
//
// 1. **Выберите ваше здание** (то же, где Interactable)
// 2. **Add Component → Game Ender**
// 3. **Настройте:**
// ```
//    Delay Before End: 2
//    End Message: "You are home!"
//    Reload Scene: ✓ (или другой вариант)
// ```
//
// ### **Шаг 2: Подключите к событию On Interact**
//
// 1. **Найдите "On Interact ()" в Interactable компоненте**
// 2. **Нажмите кнопку "+"** (добавить событие)
// 3. **Перетащите ваше здание** (то же GameObject) в поле объекта
// 4. **Выберите функцию:**
//    - Dropdown → **GameEnder → EndGame()**
//
// **Визуально:**
// ```
// On Interact ()
// ├─ Runtime ▼
// ├─ [Your Building GameObject] ← Перетащите сюда
// └─ Function: GameEnder → EndGame() ← Выберите это
// ```
//
// ---
//
// ## **Структура в Inspector:**
// ```
// Your Building (GameObject)
// ├─ Interactable (Script)
// │   ├─ Display Name: "You are home"
// │   ├─ Is Enabled: ✓
// │   └─ On Interact ()
// │       └─ GameEnder.EndGame ← Это вызовется!
// │
// └─ GameEnder (Script)
//     ├─ Delay Before End: 2
//     ├─ End Message: "You are home!"
//     └─ Reload Scene: ✓
// ```
//
// ---
//
// ## **Как это работает:**
//
// 1. **Подходишь к зданию** → Видишь "[X] You are home"
// 2. **Нажимаешь X** → `Interactable` вызывает событие `On Interact()`
// 3. **Событие вызывает** → `GameEnder.EndGame()`
// 4. **Через 2 секунды** → Игра заканчивается!
//
// ---
//
// ## **Если хотите UI панель:**
//
// ### **Создайте UI:**
//
// 1. **Canvas → UI → Panel** (назовите "EndGamePanel")
// 2. **Panel → UI → Text - TextMeshPro**
// 3. **Text:** "YOU ARE HOME!" (большой, по центру)
//
// ### **Назначьте в GameEnder:**
// ```
// GameEnder:
// └─ End Game Panel: [Drag EndGamePanel]
// ```
//
// Теперь при окончании игры появится UI!
//
// ---
//
// ## **Варианты окончания:**
//
// ### **Вариант 1: Перезагрузить уровень**
// ```
// Reload Scene: ✓
// Quit Game: ❌
// Next Scene Name: (empty)
// ```
//
// ### **Вариант 2: Следующий уровень**
// ```
// Reload Scene: ❌
// Quit Game: ❌
// Next Scene Name: "Level2"
// ```
//
// ### **Вариант 3: Выйти**
// ```
// Reload Scene: ❌
// Quit Game: ✓