using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameEventHandler : MonoBehaviour
{
    [SerializeField] private GameObject scoreCardUI;
    [SerializeField] private ScoreCardOnOff scoreCardOnOff;

    public GameObject golfBall;
    public GameObject spawnPoints;

    public int currentHole = 1;
    public int strokesForCurrentHole = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //set golf ball position to first spawnpoint
        golfBall.transform.position = spawnPoints.transform.GetChild(0).position;
    }
    public void HandleBallLaunched()
    {
        strokesForCurrentHole++;
    }
    public void HandleBallTouchedHole(Collider collider)
    {
        if (collider.gameObject.name == "Hole" + currentHole.ToString())
        {
            Debug.Log("Ball touched hole");
            //Handle UI for scoring
            scoreCardOnOff.ShowScoreCard();
            Transform holeText = scoreCardUI.transform.Find("Scores/Hole" + currentHole.ToString() + "/Hole" + currentHole.ToString());
            TextMeshProUGUI scoreText = holeText.GetComponent<TextMeshProUGUI>();
            scoreText.text = strokesForCurrentHole.ToString();
            scoreCardUI.transform.GetChild(2).gameObject.SetActive(true);
        }
    }
    //Next button has been clicked
    public void NextHole()
    {
        //move ball to next spawn point
        if (currentHole < spawnPoints.transform.childCount)
        {
            currentHole++;
            golfBall.transform.position = spawnPoints.transform.GetChild(currentHole - 1).position;
            scoreCardUI.transform.GetChild(2).gameObject.SetActive(false);
            scoreCardOnOff.HideScoreCard();
            strokesForCurrentHole = 0;
            golfBall.GetComponent<Rigidbody>().linearVelocity = new Vector3(0,0,0);
        } 
        else
        {
            currentHole = 1;
            golfBall.transform.position = spawnPoints.transform.GetChild(currentHole - 1).position;
            scoreCardUI.transform.GetChild(2).gameObject.SetActive(false);
            scoreCardOnOff.HideScoreCard();
            strokesForCurrentHole = 0;
            golfBall.GetComponent<Rigidbody>().linearVelocity = new Vector3(0,0,0);
        }
    }
}
