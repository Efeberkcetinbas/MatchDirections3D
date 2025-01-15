using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
public class PanelManager : MonoBehaviour
{
    [SerializeField] private RectTransform StartPanel,ScenePanel,SuccessPanel,FailPanel;
    [SerializeField] private GameObject vipImage;
    [SerializeField] private GameObject helperPanel;

    [SerializeField] private List<GameObject> SceneUIs=new List<GameObject>();
    [SerializeField] private List<GameObject> SuccessElements=new List<GameObject>();
    [SerializeField] private List<GameObject> FailElements=new List<GameObject>();
    [SerializeField] private List<GameObject> SpecialElements=new List<GameObject>();
    [SerializeField] private Image Fade;
    [SerializeField] private float sceneX,sceneY,oldSceneX,oldSceneY,duration;

    [Header("Game Ending Values")]
    [SerializeField] private TextMeshProUGUI earnedText;
    [SerializeField] private TextMeshProUGUI totalCustomerNumberText;
    public GameData gameData;

    private WaitForSeconds waitForSeconds,waitforSecondsSpecial;

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(.25f);
        waitforSecondsSpecial=new WaitForSeconds(0.05f);
    }





    private void OnEnable() 
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.AddHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);

        
        EventManager.AddHandler(GameEvent.OnVipSummoned,OnVipSummoned);
        EventManager.AddHandler(GameEvent.OnVipLeave,OnVipLeave);

    }


    private void OnDisable() 
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.RemoveHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);

        
        EventManager.RemoveHandler(GameEvent.OnVipSummoned,OnVipSummoned);
        EventManager.RemoveHandler(GameEvent.OnVipLeave,OnVipLeave);

    }

    private void OnVipSummoned()
    {
        vipImage.SetActive(true);
    }

    private void OnVipLeave()
    {
        vipImage.SetActive(false);
    }
    
    
    public void StartGame() 
    {
        StartCoroutine(Blink(Fade.gameObject,Fade));
        gameData.isGameEnd=false;
        StartPanel.gameObject.SetActive(false);
        ScenePanel.gameObject.SetActive(true);
        SetSceneUIPosition(sceneX,sceneY);

        helperPanel.SetActive(true);
        StartCoroutine(SetElementsDotween(SpecialElements,0.25f));
        EventManager.Broadcast(GameEvent.OnGameStart);
        
    }


    
    private void OnRestartLevel()
    {
        FailPanel.gameObject.SetActive(false);
        StartCoroutine(StartRestart());
    }

    private IEnumerator StartRestart()
    {
        yield return waitforSecondsSpecial;
        StartGame();
    }

    

    private void OnNextLevel()
    {
        
        SuccessPanel.gameObject.SetActive(false);
        StartPanel.gameObject.SetActive(true);
        /*SetActivity(SceneUIs,true);
        helperPanel.SetActive(true);
        StartCoroutine(SetElementsDotween(SpecialElements));*/
    }

   

    private IEnumerator Blink(GameObject gameObject,Image image)
    {
        
        gameObject.SetActive(true);
        image.color=new Color(0,0,0,1);
        image.DOFade(0,2f);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        SetSceneUIPosition(sceneX,sceneY);

    }

    private IEnumerator SetElementsDotween(List<GameObject> elements,float localwaitforseconds)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].transform.localScale=Vector3.zero;
        }

        for (int i = 0; i < elements.Count; i++)
        {
            yield return localwaitforseconds;
            elements[i].transform.DOScale(Vector3.one,duration);
        }
    }

    private void SetActivity(List<GameObject> list,bool val)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive(val);
        }
    }

    private void OnSuccess()
    {
        //SetActivity(SceneUIs,false);
        PlayerPrefs.SetInt("CustomerNumber",gameData.totalCustomerNumber);
        helperPanel.SetActive(false);
        SetSceneUIPosition(oldSceneX,oldSceneY);
    }

     private void OnSuccessUI()
    {
        SuccessPanel.gameObject.SetActive(true);
        SetActivity(SceneUIs,false);
        StartCoroutine(SetElementsDotween(SuccessElements,0.1f));
        earnedText.SetText(gameData.earnedAmount.ToString());
        totalCustomerNumberText.SetText(gameData.totalCustomerNumber.ToString());

    }
  

    private void OnFailUI()
    {
        FailPanel.gameObject.SetActive(true);
        helperPanel.SetActive(false);
        SetActivity(SceneUIs,false);
        StartCoroutine(SetElementsDotween(FailElements,0.25f));
    }

    private void SetSceneUIPosition(float valX,float valY)
    {
        ScenePanel.DOAnchorPos(new Vector2(valX,valY),duration);
    }
    
}
