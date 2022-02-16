using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNotification : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] UIQuestion UIQuestionPrefab;
    [SerializeField] Transform questionContent;

    private List<Question> questions = new List<Question>();
    private Dictionary<QuestionType, UIQuestion> kvpUIQuestions = new Dictionary<QuestionType, UIQuestion>();
    private void Start()
    {
        questions = GamePlayController.Instance.Questions;
        InitUIQuesttion();
    }

    private void InitUIQuesttion()
    {
        foreach (var item in questions)
        {
            UIQuestion UIQuestionClone = Instantiate(UIQuestionPrefab, questionContent);

            string description = string.Format("{0} : {1}/{2}", item.description, item.NumberEnemyKilled, item.numberEnemy);
            UIQuestionClone.SetQuestionTitle(description);
            UIQuestionClone.SetCheckMark(false);
            AddUIQuestions(item, UIQuestionClone);
        }
    }

    private void AddUIQuestions(Question question, UIQuestion uIQuestion)
    {
        if (kvpUIQuestions.ContainsKey(question.questionType))
            kvpUIQuestions[question.questionType] = uIQuestion;
        else
            kvpUIQuestions.Add(question.questionType, uIQuestion);
    }


    private void OnEnable()
    {
        EventSystem.Instance.Subscribe(SystemEventType.SystemEventUpdateQuestion, OnUpdateQuestion);
    }

    private void OnDisable()
    {
        EventSystem.Instance.Unsubscribe(SystemEventType.SystemEventUpdateQuestion, OnUpdateQuestion);
    }

    private void OnUpdateQuestion(Message message)
    {
        Question question = (Question) message.Data;

        if(kvpUIQuestions.TryGetValue (question.questionType, out UIQuestion uIQuestion))
        {
            string description = string.Format("{0} : {1}/{2}", question.description, question.NumberEnemyKilled, question.numberEnemy);
            bool isComplete = question.NumberEnemyKilled >= question.numberEnemy;
            uIQuestion.SetQuestionTitle(description);
            uIQuestion.SetCheckMark(isComplete);
        }
    }
}
