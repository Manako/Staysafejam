using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float gameTime;
    [SerializeField] private Text timerText;

    private void Update()
    {
        this.gameTime -= Time.deltaTime;
        timerText.text = ((int)this.gameTime).ToString();
    }
}
