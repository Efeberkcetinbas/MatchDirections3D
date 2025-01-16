using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LifeManager : MonoBehaviour
{
    [Header("DATA")]
    [SerializeField] private GameData gameData;

    [Header("UI Components")]
    [SerializeField] private Image lifeAmountProgress;
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI startLifeText;
    [SerializeField] private TextMeshProUGUI failLifeText;

    [Header("Life Settings")]
    [SerializeField] private int maxLives = 5;
    [SerializeField] private int restoreIntervalMinutes = 15;

    private DateTime lastLifeDecreaseTime;

    private const string LifeAmountKey = "LifeAmount";
    private const string LastLifeDecreaseTimeKey = "LastLifeDecreaseTime";

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnFail, OnFail);
        EventManager.AddHandler(GameEvent.OnLifeFull, OnLifeFull);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnFail, OnFail);
        EventManager.RemoveHandler(GameEvent.OnLifeFull, OnLifeFull);
    }

    private void Start()
    {
        InitializeLifeSystem();
        UpdateLifeUI();
    }

    private void Update()
    {
        if (gameData.lifeTime < maxLives)
        {
            TimeSpan timeUntilNextLife = CalculateTimeUntilNextLife();

            if (timeUntilNextLife.TotalSeconds <= 0)
            {
                RestoreLivesOverTime();
            }

            timerText.text = FormatTimeSpan(timeUntilNextLife);
        }
        else
        {
            timerText.text = "Full";
        }

        UpdateUIValues();
    }

    private void InitializeLifeSystem()
    {
        if (!PlayerPrefs.HasKey(LifeAmountKey))
        {
            gameData.lifeTime = maxLives;
            lastLifeDecreaseTime = DateTime.Now;
            SaveLifeState();
        }
        else
        {
            gameData.lifeTime = PlayerPrefs.GetInt(LifeAmountKey);
            lastLifeDecreaseTime = DateTime.Parse(PlayerPrefs.GetString(LastLifeDecreaseTimeKey, DateTime.Now.ToString()));

            RestoreLivesOverTime();
        }
    }

    private void RestoreLivesOverTime()
    {
        TimeSpan timeElapsed = DateTime.Now - lastLifeDecreaseTime;
        int livesToRestore = Mathf.Min((int)(timeElapsed.TotalMinutes / restoreIntervalMinutes), maxLives - gameData.lifeTime);

        if (livesToRestore > 0)
        {
            gameData.lifeTime += livesToRestore;
            lastLifeDecreaseTime = lastLifeDecreaseTime.AddMinutes(livesToRestore * restoreIntervalMinutes);
            SaveLifeState();
            UpdateLifeUI();
            //EventManager.Broadcast(GameEvent.OnLifeIncrease);
        }
    }

    private void OnFail()
    {
        if (gameData.lifeTime > 0)
        {
            gameData.lifeTime--;
            lastLifeDecreaseTime = DateTime.Now;
            SaveLifeState();
            UpdateLifeUI();

            if (gameData.lifeTime < maxLives)
            {
                //EventManager.Broadcast(GameEvent.OnLifeDecrease);
            }
        }
    }

    private void OnLifeFull()
    {
        gameData.lifeTime = maxLives;
        SaveLifeState();
        UpdateLifeUI();
        //EventManager.Broadcast(GameEvent.OnLifeFullUI);
    }

    private void UpdateLifeUI()
    {
        float progress = (float)gameData.lifeTime / maxLives;
        Debug.LogWarning("PROGRESS LIFE " + progress);
        lifeAmountProgress.DOFillAmount(progress, 0.5f);
        lifeText.text = gameData.lifeTime.ToString();
    }

    private void UpdateUIValues()
    {
        startLifeText.text = gameData.lifeTime.ToString();
        failLifeText.text = gameData.lifeTime.ToString();
    }

    private TimeSpan CalculateTimeUntilNextLife()
    {
        TimeSpan elapsedTime = DateTime.Now - lastLifeDecreaseTime;
        int minutesPassed = (int)elapsedTime.TotalMinutes;
        int secondsPassed = (int)elapsedTime.TotalSeconds % 60;

        int minutesUntilNextLife = restoreIntervalMinutes - minutesPassed;
        return TimeSpan.FromMinutes(minutesUntilNextLife).Subtract(TimeSpan.FromSeconds(secondsPassed));
    }

    private string FormatTimeSpan(TimeSpan timeSpan)
    {
        return timeSpan.ToString(@"mm\:ss");
    }

    private void SaveLifeState()
    {
        PlayerPrefs.SetInt(LifeAmountKey, gameData.lifeTime);
        PlayerPrefs.SetString(LastLifeDecreaseTimeKey, lastLifeDecreaseTime.ToString());
        PlayerPrefs.Save();
    }
}