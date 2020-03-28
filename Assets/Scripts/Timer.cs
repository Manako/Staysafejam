using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float gameTime;
    [SerializeField] private Text timerText;

    [SerializeField] private GameObject winCon;

    private void Update()
    {
        this.gameTime -= Time.deltaTime;
        timerText.text = ((int)this.gameTime).ToString();

        if (this.gameTime <= 0.0f)
        {
            timerText.gameObject.SetActive(false);
            this.winCon.SetActive(true);
        }
    }
}
