using UnityEngine;
using UnityEngine.UI;
public class ScoreCardOnOff : MonoBehaviour
{
    [SerializeField] private GameObject scoreCardUI;
    public void ToggleScoreCard()
    {
        scoreCardUI.SetActive(!scoreCardUI.activeInHierarchy);
    }
    public void ShowScoreCard()
    {
        scoreCardUI.SetActive(true);
    }
    public void HideScoreCard()
    {        
        scoreCardUI.SetActive(false);
    }
}
