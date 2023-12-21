using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    //All GameObject(Panels)
    [SerializeField]
    GameObject HomePanel, LoadingPanel, SettingPanel, ExitPanel;
    //All Loading Slider Variables
    [SerializeField]
    Image Slider;
    [SerializeField]
    float Speed;
    bool flag;
    //All Audio Management Variables
    [SerializeField]
    Button MusicBoolBtn, SoundBoolBtn;
    [SerializeField]
    Sprite MusicBoolON, MusicBoolOFF, SoundBoolON, SoundBoolOFF;
    [SerializeField]
    AudioSource MusicBoolSource, SoundBoolSource;

    //Start AudioManagement
    void Start()
    {

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
            CommonScript.instance.SoundBool = true;
        }
        else
        {
            SoundBoolBtn.GetComponent<Image>().sprite = SoundBoolOFF;
            SoundBoolSource.mute = true;
            CommonScript.instance.SoundBool = false;
        }
    }

    //On Click Sound Play Method
    public void SoundButtonClick()
    {
        SoundBoolSource.Play();
    }

    //Sound Management
    public void SoundBoolManagement()
    {
        SoundButtonClick();
        if (CommonScript.instance.SoundBool)
        {
            SoundBoolBtn.GetComponent<Image>().sprite = SoundBoolOFF;
            SoundBoolSource.mute = true;
            CommonScript.instance.SoundBool = false;
        }
        else
        {
            SoundBoolBtn.GetComponent<Image>().sprite = SoundBoolON;
            SoundBoolSource.mute = false;
            CommonScript.instance.SoundBool = true;
        }
    }

    //Music Management
    public void MusicBoolManagement()
    {
        SoundButtonClick();
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

    //Update For Escape Button And Slider
    private void Update()
    {
        if (flag)
        {
            if (Slider.fillAmount < 1)
            {
                Slider.fillAmount += Speed * Time.deltaTime;
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (SettingPanel.activeInHierarchy)
            {
                SettingPanel.SetActive(false);
                HomePanel.SetActive(true);
            }
            else if (ExitPanel.activeInHierarchy)
            {
                ExitPanel.SetActive(false);
                HomePanel.SetActive(true);
            }
            else
            {
                HomePanel.SetActive(false);
                ExitPanel.SetActive(true);
            }
        }

    }

    //Loading Panel Open
    public void LoadingPanelOpen()
    {
        SoundButtonClick();
        HomePanel.SetActive(false);
        LoadingPanel.SetActive(true);  
        flag = true;
    }

    //Setting Panel Open
    public void SettingPanelOpen()
    {
        SoundButtonClick();
        HomePanel.SetActive(false);
        SettingPanel.SetActive(true);
    }

    //Setting Panel Close
    public void SettingPanelClose()
    {
        SoundButtonClick();
        SettingPanel.SetActive(false);
        HomePanel.SetActive(true);
    }

    //Exit Panel Open
    public void ExitPanelOpen()
    {
        SoundButtonClick();
        HomePanel.SetActive(false);
        ExitPanel.SetActive(true);
    }

    //Exit Panel Yes Button
    public void ExitPanelYesBtn()
    {
        SoundButtonClick();
        Application.Quit();
    }

    //Exit Panel No Button
    public void ExitPanelNOBtn()
    {
        SoundButtonClick();
        HomePanel.SetActive(true);
        ExitPanel.SetActive(false);
    }
}
