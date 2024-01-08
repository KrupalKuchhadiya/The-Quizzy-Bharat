using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NotePadManager : MonoBehaviour
{
    [SerializeField]
    GameObject GamePanel, SelectionPanel, GameOverPanel, WinPanel;
    [SerializeField]
    catogry[] Allcat;


    [SerializeField]
    TextMeshProUGUI QuestionsText;
    [SerializeField]
    TextMeshProUGUI[] OptionText;
    int SelectedField, /*QuestionsNO,*/ SelectedCategory;
    //bool questionunlock;
    bool flag;
    [SerializeField]
    Image Slider;
    [SerializeField]
    float Speed;


    [SerializeField]
    List<cat> cats;

    private void Start()
    {
        var textFile = Resources.Load<TextAsset>("Award And Honour WS");
        

        string textk = textFile.text;
        ////Debug.Log(textk);
        string k = textk;
        
        string[] data = k.Split('=');

        //foreach(string k2 in data)
        //{
        //    //Debug.Log(k2);
        //}
        for (int i = 0; i < data.Length; i++)
        {
            cats[i].Name = data[i];
            //Debug.Log(cats[i].Name);
        }

    }

    private void Update()
    {
        if (flag)
        {
            if (Slider.fillAmount > 0)
            {
                Slider.fillAmount -= Speed * Time.deltaTime;
            }
            else
            {
                GameOverPanel.SetActive(true);
                GamePanel.SetActive(false);
            }
        }

    }


    public void QuestionSet()
    {
        int QuestionsNO = PlayerPrefs.GetInt("level" + SelectedField, 0);
        flag = true;
        Slider.fillAmount = 1;
        if (QuestionsNO < Allcat[SelectedField].AllQuestions.Length)
        {
            for (int j = 0; j < Allcat[SelectedField].AllQuestions.Length; j++)
            {
                QuestionsText.text = Allcat[SelectedField].AllQuestions[QuestionsNO].QuestName;

                //Debug.Log(Allcat[SelectedField].AllQuestions[QuestionsNO].Answers);

                for (int k = 0; k < Allcat[SelectedField].AllQuestions[QuestionsNO].AllOptions.Length; k++)
                {
                    OptionText[k].text = Allcat[SelectedField].AllQuestions[QuestionsNO].AllOptions[k].OptionName;
                }
            }
        }
        else
        {
            SelectedCategory++;
            WinPanel.SetActive(true);
        }
    }
    public void CheckAnswer(TextMeshProUGUI OptText)
    {

        int QuestionsNO = PlayerPrefs.GetInt("level" + SelectedField, 0);
        string ans = Allcat[SelectedField].AllQuestions[QuestionsNO].Answers;
        OptText.text = OptText.text.Replace(" ", "");
        ans = ans.Replace(" ", "");
        if (OptText.text == ans)
        {
            if (QuestionsNO + 1 < Allcat[SelectedField].AllQuestions.Length)
            {

                QuestionsNO++;
                PlayerPrefs.SetInt("level" + SelectedField, QuestionsNO);
                QuestionSet();
            }
            else
            {
                //Debug.Log("Hi");
                SelectionPanel.SetActive(true);
                GamePanel.SetActive(false);
                flag = false;
            }
        }
        else
        {
            Slider.fillAmount = 0;
            //Debug.Log("Answer is wrong");
            GameOverPanel.SetActive(true);
        }
    }
    public void GamePanelOpen(int val)
    {
        GamePanel.SetActive(true);
        WinPanel.SetActive(false);

        SelectedField = val;
        QuestionSet();
    }
    public void GamePanelClose()
    {
        Slider.fillAmount = 1;
        flag = false;
        GamePanel.SetActive(false);
        SelectionPanel.SetActive(true);
        WinPanel.SetActive(false);

    }
    public void gameovrbackbtn()
    {
        GameOverPanel.SetActive(false);
        SelectionPanel.SetActive(true);
        GamePanel.SetActive(false);
        GameOverPanel.SetActive(false);

    }
    public void RetryButton()
    {
        Slider.fillAmount = 1;
        flag = false;
        GamePanel.SetActive(false);
        SelectionPanel.SetActive(true);
        WinPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }
    public void HomeButton()
    {
        SceneManager.LoadScene(0);
    }
}

[System.Serializable]
public class catogry
{
    public string CatName;
    public Questions[] AllQuestions;

}
[System.Serializable]
public class Questions
{
    public string QuestName;
    public Optiopns[] AllOptions;
    public string Answers;
}

[System.Serializable]
public class Optiopns
{
    public string OptionName;

}
[System.Serializable]
public class cat
{
    public string Name;
}
