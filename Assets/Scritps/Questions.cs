using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Questions : MonoBehaviour
{
    #region Variables

    public Image imageQuestion;
    public Text textQuestion;
    public ToggleGroup toggleGroup;
    public GameObject goNextButton;
    public Image imageHighlight;
    public List<Sprite> spriteList;
    public List<Text> toggleTextList;
    public AudioClip acCorrect;
    public AudioClip acWrong;

    [System.Serializable]
    public class QuestionOptions
    {
        public List<string> options;  // List of options for a single question
    }

    public List<QuestionOptions> predefinedOptions;  // List of predefined options for all questions

    private int _currentQuestionIndex = -1;
    private int _correctAnswersCount = 0;

    private List<string> _optionAnswers;
    private Text _textOptionSelected;
    private Animator _animator;
    private AudioSource _audioSource;

    private string _correctAnswer;
    private string _textForQuestion = "What is this picture of?";

    #endregion

    #region Builtin Methods

    private void Awake()
    {
        if (imageQuestion)
            _animator = imageQuestion.GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartCoroutine(ShowNextQuestion());
    }

    #endregion

    #region Custom Methods

    public void OnNextClick()
    {
        goNextButton.SetActive(false);

        textQuestion.text = _correctAnswer;

        Vector2 rect = imageHighlight.GetComponent<RectTransform>().anchoredPosition;
        Vector2 targetRect = _textOptionSelected.transform.parent.GetComponent<RectTransform>().anchoredPosition;
        rect.x = targetRect.x;
        rect.y = targetRect.y;
        imageHighlight.GetComponent<RectTransform>().anchoredPosition = rect;

        bool isCorrect = CheckCorrectAnswer();

        if (isCorrect)
        {
            imageHighlight.color = new Color32(41, 118, 6, 86);
            _audioSource.clip = acCorrect;
        }
        else
        {
            imageHighlight.color = new Color32(118, 11, 7, 86);
            _audioSource.clip = acWrong;
        }

        _audioSource.Play();

        imageHighlight.gameObject.SetActive(true);

        StartCoroutine(HideQuestion());
    }

    private IEnumerator HideQuestion()
    {
        yield return new WaitForSeconds(2f);

        imageQuestion.gameObject.SetActive(false);
        textQuestion.gameObject.SetActive(false);

        toggleGroup.SetAllTogglesOff(true);
        toggleGroup.gameObject.SetActive(false);

        imageHighlight.gameObject.SetActive(false);

        goNextButton.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(ShowNextQuestion());
    }

    private IEnumerator ShowNextQuestion()
    {
        NextQuestion();
        imageQuestion.gameObject.SetActive(true);
        _animator.SetTrigger("In");

        yield return new WaitForSeconds(0.5f);
        textQuestion.text = _textForQuestion;
        textQuestion.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        toggleGroup.gameObject.SetActive(true);
    }

    private void NextQuestion()
    {
        ++_currentQuestionIndex;

        if (_currentQuestionIndex >= spriteList.Count)
        {
            GameOver();
            return;
        }

        _correctAnswer = spriteList[_currentQuestionIndex].name;

        imageQuestion.sprite = spriteList[_currentQuestionIndex];
        imageQuestion.SetNativeSize();

        SetOptionAnswers();
    }

    private void SetOptionAnswers()
    {
        _optionAnswers = predefinedOptions[_currentQuestionIndex].options;

        for (int i = 0; i < toggleTextList.Count; ++i)
        {
            toggleTextList[i].text = _optionAnswers[i];
        }
    }

    private bool CheckCorrectAnswer()
    {
        if (_currentQuestionIndex > -1)
        {
            if (_textOptionSelected.text.Equals(_correctAnswer))
            {
                ++_correctAnswersCount;
                return true;
            }
        }

        return false;
    }

    public void OnToggleClick(Text t)
    {
        goNextButton.SetActive(true);
        _textOptionSelected = t;
    }

    private void GameOver()
    {
        PlayerPrefs.SetInt("CorrectAnswers", _correctAnswersCount);
        SceneManager.LoadScene("End");
    }

    #endregion
}
