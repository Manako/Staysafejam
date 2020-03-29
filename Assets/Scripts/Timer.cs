using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float gameTime;
    [SerializeField] private Text timerText;

    [SerializeField] private GameObject winCon;
    [SerializeField] private Text finalText;
    [SerializeField] private Player player;

    private void Update()
    {
        this.gameTime -= Time.deltaTime;
        timerText.text = ((int)this.gameTime).ToString();

        if (this.gameTime <= 0.0f)
        {
            timerText.gameObject.SetActive(false);
            this.winCon.SetActive(true);
            this.finalText.text = "You helped " + player.granniesHelped + " people and got them food for their next meals, and supplies to resist the times!";
        }
    }
}
