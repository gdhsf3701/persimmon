using System.Collections;
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
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject shower;
    public void Kill(Enemy enemy)
    {
        combo++;
        lastKillTime = Time.time;

        score+=enemy.Reward * combo;

        ShowCombo();
        ShowScore();

        Vector3 Position = Camera.main.WorldToScreenPoint(enemy.transform.position);

        /*DamageShow damage =
            Instantiate(shower, Position, Quaternion.identity, canvas.transform).GetComponent<DamageShow>();
        damage.Show(enemy.Reward);*/
        lastKillTime = Time.time;
    }

    private void ShowScore()
    {
        StartCoroutine(ShowScore(score));
    }

    private IEnumerator ShowScore(int targetScore)
    {
        int currentvalue = targetScore / 2;
        while(currentvalue < targetScore-1)
        {
            currentvalue = (targetScore + currentvalue) / 2;
            Score.SetText(currentvalue.ToString());
            yield return null;
        }
        yield return null;
        Score.SetText(targetScore.ToString());
    }

    private void ShowCombo()
    {
        Combo.SetText(combo.ToString());
    }

    private void Update()
    {
        if(lastKillTime + killtime < Time.time)
        {
            combo = 0;
            ShowCombo();
        }
    }
}
