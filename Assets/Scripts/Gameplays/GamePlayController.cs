using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoSingleton<GamePlayController>
{
    private readonly string YOU_WIN = "YOU WIN !!!";
    private readonly string YOU_ARE_DIE = "YOU ARE DIE !!!";

    [SerializeField] private List<Question> questions = new List<Question>();

    public List<Question> Questions { get => questions; private set => questions = value; }

    private bool isPlayerDie;

    private void Start()
    {
        isPlayerDie = false;
        EventSystem.Instance.Announce(new Message(SystemEventType.SystemEventShowDialogGetStart));
    }

    public void OnEnemyDie()
    {
        foreach (var item in questions)
        {
            if (item.questionType == QuestionType.KillEnemy)
            {
                item.NumberEnemyKilled++;
                EventSystem.Instance.Announce(new Message(SystemEventType.SystemEventUpdateQuestion, item));
                CheckWinGame();
            }
        }
    }
    public void OnBossDie()
    {
        foreach (var item in questions)
        {
            if(item.questionType == QuestionType.KillBoss)
            {
                item.NumberEnemyKilled++;
                EventSystem.Instance.Announce(new Message(SystemEventType.SystemEventUpdateQuestion, item));
                CheckWinGame();
            }
        }
    }

    private void CheckWinGame()
    {
        foreach (var item in questions)
        {
            if (item.NumberEnemyKilled < item.numberEnemy)
                return;
        }

        string message = YOU_WIN;
        EventSystem.Instance.Announce(new Message(SystemEventType.SystemEventShowDialogEndGame, message));
    }

    public void EndGame()
    {
        isPlayerDie = true;

        string message = YOU_ARE_DIE;
        EventSystem.Instance.Announce(new Message(SystemEventType.SystemEventShowDialogEndGame, message));
    }

    public void OnGameRestart()
    {
        SceneManager.LoadScene(GameContans.MAIN_SCENE);
    }
    public void OnQuitGame()
    {
        Application.Quit();
    }
}
