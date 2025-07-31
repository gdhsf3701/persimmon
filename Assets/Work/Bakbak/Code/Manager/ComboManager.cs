using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{

    [SerializeField] 
    private float killtime = 0.5f;

    private int combo = 0;
    private int score = 0;
    private float lastKillTime = 0;

    [SerializeField]
    private TMP_Text Score;
    [SerializeField]
    private TMP_Text Combo;
    public void Kill(Enemy enemy)
    {
        combo++;
        lastKillTime = Time.time;

        score+=enemy.Reward * combo;

        ShowCombo();
        ShowScore();
    }

    private void ShowScore()
    {
        Score.SetText(score.ToString());
    }

    private void ShowCombo()
    {
        Combo.SetText(combo.ToString());
    }

    private void Update()
    {
        if(lastKillTime + killtime > killtime)
        {
            combo = 0;
            ShowCombo();
        }
    }
}
