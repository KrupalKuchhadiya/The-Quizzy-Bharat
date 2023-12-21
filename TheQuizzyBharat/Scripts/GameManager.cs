using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static GameManager;
using System.Threading;

public class GameManager : MonoBehaviour
{
    //All GameObject(Panel)
    [SerializeField]
    GameObject GamePanel, SelectionPanel,GameOverPanel,WinPanel,SettingPanel,ADPanel,GetHint1Panel,GetHint2Panel;
    //All CateGory List
    [SerializeField]
    catogry[] AllCategory;
    //Variables For AD Panel

    int Question2;
    int QuestionsNO1;
    //All Question And Answer Needed Variables
    [SerializeField]
    TextMeshProUGUI QuestionsText;
    [SerializeField]
    TextMeshProUGUI[] OptionText;
    //Variables To Find Selected Category
    int SelectedField, SelectedCategory;
    //Variables For Timer
    bool flag;
    [SerializeField]
    Image Slider;
    [SerializeField]
    float Speed;
    //MusicBool ANd SoundBool Setting Variable
    [SerializeField]
    Button MusicBoolBtn, SoundBoolBtn;
    [SerializeField]
    Sprite MusicBoolON, MusicBoolOFF, SoundBoolON, SoundBoolOFF;
    [SerializeField]
    AudioSource MusicBoolSource, SoundBoolSource, RightSource, WrongSource;

    //////////////////////////// Logical Method //////////////////////////////////////

    private void Start()
    {
        var textFile = Resources.Load<TextAsset>("Award And Honour");
        string textk = textFile.text;
        Debug.Log(textk);
        string k = textk.Replace("A", "");
        string k1 = k.Replace("=", "");
        string[] data = k1.Split(':');
        Debug.Log(k);
        Debug.Log(k1);
        Debug.Log(data);

        foreach (string ss in data)
        {
            Debug.Log(ss);
        }








        if (CommonScript.instance.MusicBool)
        {

            MusicBoolBtn.GetComponent<Image>().sprite = MusicBoolON;
            MusicBoolSource.mute = false;

            CommonScript.instance.MusicBool = true;
        }
        else
        {

            MusicBoolBtn.GetComponent<Image>().sprite = MusicBoolOFF;
            MusicBoolSource.mute = true;

            CommonScript.instance.MusicBool = false;
        }


        if (CommonScript.instance.SoundBool)
        {

            SoundBoolBtn.GetComponent<Image>().sprite = SoundBoolON;
            SoundBoolSource.mute = false;
            RightSource.mute = false;
            WrongSource.mute = false;
            CommonScript.instance.SoundBool = true;
        }
        else
        {

            SoundBoolBtn.GetComponent<Image>().sprite = SoundBoolOFF;
            SoundBoolSource.mute = true;
            RightSource.mute = true;
            WrongSource.mute = true;
            CommonScript.instance.SoundBool = false;
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
    public void SoundClickPlay()
    {
        SoundBoolSource.Play();
    }
    //Right Answer SoundBool Play On Click Method
    public void RightSoundBoolPlay()
    {
        RightSource.Play();
    }
    //Wrong Answer SoundBool Play On Click Method
    public void WrongSoundBoolPlay()
    {
        WrongSource.Play();
    }
    //SoundBoolManager Method
    public void SoundBoolManagement()
    {
        SoundClickPlay();
        if (CommonScript.instance.SoundBool)
        {
            SoundBoolBtn.GetComponent<Image>().sprite = SoundBoolOFF;
            SoundBoolSource.mute = true;
            RightSource.mute = true;
            WrongSource.mute = true;
            CommonScript.instance.SoundBool = false;
        }
        else
        {
            SoundBoolBtn.GetComponent<Image>().sprite = SoundBoolON;
            SoundBoolSource.mute = false;
            RightSource.mute = false;
            WrongSource.mute = false;
            CommonScript.instance.SoundBool = true;
        }
    }
    //MusicBoolManager Method
    public void MusicBoolManagement()
    {
        SoundClickPlay();
        if (CommonScript.instance.MusicBool)
        {
            MusicBoolBtn.GetComponent<Image>().sprite = MusicBoolOFF;
            MusicBoolSource.mute = true;
            CommonScript.instance.MusicBool = false;
        }
        else
        {
            MusicBoolBtn.GetComponent<Image>().sprite = MusicBoolON;
            MusicBoolSource.mute = false;
            CommonScript.instance.MusicBool = true;
        }
    }
    //Selection Field Method
    public void SkipLevel(int WatchAds)
    {
        SoundClickPlay();
        if (WatchAds == 1)
        {
            Question2++;
            PlayerPrefs.SetInt("level" + SelectedField, Question2);
            QuestionSet();
            Debug.Log("Level Increase");
            GamePanel.SetActive(true);
            ADPanel.SetActive(false);
            Slider.fillAmount = 1;
            flag = true;
        }
        else
        {
            GamePanel.SetActive(true ) ;
            ADPanel.SetActive(false ) ;
            flag = true;
        }
    }
    public void CheckAnswer(TextMeshProUGUI OptText)
    {

        int QuestionsNO = PlayerPrefs.GetInt("level" + SelectedField, 0);
        Question2 = QuestionsNO;
        string ans = AllCategory[SelectedField].AllQuestions[QuestionsNO].Answers;
        OptText.text = OptText.text.Replace(" ", "");
        ans = ans.Replace(" ", "");
        if (OptText.text == ans)
        {
            if (QuestionsNO + 1 < AllCategory[SelectedField].AllQuestions.Length)
            {
                RightSoundBoolPlay();
                QuestionsNO++;
                PlayerPrefs.SetInt("level" + SelectedField, QuestionsNO);
                QuestionSet();
            }
            else
            {
                WrongSoundBoolPlay();
                Debug.Log("Hi");
                SelectionPanel.SetActive(true);
                GamePanel.SetActive(false);
                flag = false;
            }
        }
        else
        {
            Slider.fillAmount = 0;
            Debug.Log("Answer is wrong");
            GameOverPanel.SetActive(true);
        }
             QuestionsNO1 = Question2;
    }
    public void QuestionSet()
    {
        int QuestionsNO = PlayerPrefs.GetInt("level" + SelectedField, 0);
        flag = true;
        Slider.fillAmount = 1;
        if (QuestionsNO < AllCategory[SelectedField].AllQuestions.Length)
        {
            for (int j = 0; j < AllCategory[SelectedField].AllQuestions.Length; j++)
            {
                QuestionsText.text = AllCategory[SelectedField].AllQuestions[QuestionsNO].QuestName;

                Debug.Log(AllCategory[SelectedField].AllQuestions[QuestionsNO].Answers);

                for (int k = 0; k < AllCategory[SelectedField].AllQuestions[QuestionsNO].AllOptions.Length; k++)
                {
                    OptionText[k].text = AllCategory[SelectedField].AllQuestions[QuestionsNO].AllOptions[k].OptionName;
                }
            }
        }
        else
        {
            SelectedCategory++;
            WinPanel.SetActive(true);
        }
    }

    ////////////////// Local Method ///////////////////////
    public void GamePanelOpen(int val)
    {
        SoundClickPlay();
        GamePanel.SetActive(true);
        WinPanel.SetActive(false);
        SelectedField = val;
        QuestionSet();
    }
    public void GamePanelClose()
    {
        SoundClickPlay();
        Slider.fillAmount = 1;
        flag = false;
        GamePanel.SetActive(false);
        SelectionPanel.SetActive(true);
        WinPanel.SetActive(false );
    }
    public void GameOverHomeButton()
    {
        SoundClickPlay();
        SceneManager.LoadScene(0);
    }
    public void GameOverRetryButton()
    {
        SoundClickPlay();
        GamePanel.SetActive(true ); 
        SelectionPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        WinPanel.SetActive(false) ;
    }
    public void CategoryOverNextButton()
    {
        SoundClickPlay();
        GamePanel.SetActive(false);
        SelectionPanel.SetActive(true);
        GameOverPanel.SetActive(false);
        WinPanel.SetActive(false);
    }
    public void RetryButton()
    {

        SoundClickPlay();
        Slider.fillAmount = 1;
        flag = false;
        GamePanel.SetActive(false);
        SelectionPanel.SetActive(true);
        WinPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }
    public void HomeButton()
    {
        SoundClickPlay();
        SceneManager.LoadScene(0);
    }
    public void PlayToSettingPanel()
    {
        SoundClickPlay();
        GamePanel.SetActive(false) ;
        SettingPanel.SetActive(true );
        flag = false;
    }
    public void PlayToADPanel()
    {
        SoundClickPlay();
        GamePanel.SetActive(false);
        ADPanel.SetActive(true);
        flag = false;
    }
    public void SettingToGamePanel()
    {
        SoundClickPlay();
        SettingPanel.SetActive(false);
        GamePanel.SetActive(true) ;
        flag = true;
    } 
    public void SettingToHome()
    {
        SoundClickPlay();
        SceneManager.LoadScene(0);
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

}
    