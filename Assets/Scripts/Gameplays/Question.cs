public enum QuestionType
{
    Tutorial = 0,
    KillEnemy = 1,
    KillBoss = 2
}

[System.Serializable]
public class Question
{
    public QuestionType questionType;
    public int numberEnemy;
    private int numberEnemyKilled;
    public string description;

    public int NumberEnemyKilled { get => numberEnemyKilled; set => numberEnemyKilled = value; }

    public Question()
    {
        questionType = QuestionType.Tutorial;
        numberEnemy = 0;
        NumberEnemyKilled = 0;
        description = string.Empty;
    }
}
