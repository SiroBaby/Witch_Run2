using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    Player player;
    Text distanceText;
    GameObject results;
    Text finalDistanceText;
    GameObject pausePanel;
        Text currentdistance;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        distanceText = GameObject.Find("DistanceText").GetComponent<Text>();
        finalDistanceText = GameObject.Find("FinalDistanceText").GetComponent<Text>();
        currentdistance = GameObject.Find("CurrentDistance").GetComponent<Text>();

        results = GameObject.Find("Results");
        results.SetActive(false);

        pausePanel = GameObject.Find("PausePanel");
        pausePanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance + " m";
        currentdistance.text = distance + "m";

        if (player.isDead)
        {
            results.SetActive(true);
            finalDistanceText.text = distance + "m";
        }


    }

    public void Quit()
    {
        SceneManager.LoadScene("MENUGAME");
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục trò chơi
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Tạm dừng trò chơi
    }
}
