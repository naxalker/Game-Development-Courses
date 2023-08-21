using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public float timer;

    [SerializeField] float waitToShowEndScreen = 1f;

    private bool gameActive;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameActive = true;
    }

    void Update()
    {
        if (gameActive)
        {
            timer += Time.deltaTime;
            UIController.Instance.UpdateTimer(timer);
        }
    }

    public void EndLevel()
    {
        gameActive = false;

        StartCoroutine(EndLevelCo());
    }

    IEnumerator EndLevelCo()
    {
        yield return new WaitForSeconds(waitToShowEndScreen);

        float minutes = Mathf.FloorToInt(timer / 60f);
        float seconds = Mathf.FloorToInt(timer % 60);

        UIController.Instance.endTimeText.text = minutes.ToString() + " mins " + seconds.ToString("00") + " secs";
        UIController.Instance.levelEndScreen.SetActive(true);
    }
}
