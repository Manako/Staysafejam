using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Granny : MonoBehaviour
{
    [SerializeField] private bool needsHelp;
    [SerializeField] private bool requested;

    [SerializeField] [Range(0, 5)] private int neededMeat;
    [SerializeField] [Range(0, 5)] private int neededFish;
    [SerializeField] [Range(0, 5)] private int neededPharm;
    [SerializeField] [Range(0, 5)] private int neededVegetables;

    [SerializeField] private AudioClip attentionClip;
    [SerializeField] private AudioClip requestClip;
    [SerializeField] private AudioClip successClip;

    [SerializeField] private GameObject alertSprite;
    [SerializeField] private GameObject request;
    [SerializeField] private GameObject success;

    [SerializeField] private Transform meatSprite;
    [SerializeField] private Transform fishSprite;
    [SerializeField] private Transform pharmSprite;
    [SerializeField] private Transform vegetableSprite;
    [SerializeField] private Text meatCounter;
    [SerializeField] private Text fishCounter;
    [SerializeField] private Text pharmCounter;
    [SerializeField] private Text vegetableCounter;

    private AudioSource audioSource;

    private Vector3 originalRequestPosition;

    public int Interact(ref int meat, ref int fish, ref int pharm, ref int vegetables)
    {
        if (!this.requested)
        {
            this.Request();
        }
        else if (this.needsHelp)
        {
            return this.SatisfyRequest(ref meat, ref fish, ref pharm, ref vegetables);
        }

        return 0;

    }

    private void Request()
    {
        meatCounter.text = this.neededMeat.ToString();
        fishCounter.text = this.neededFish.ToString();
        pharmCounter.text = this.neededPharm.ToString();
        vegetableCounter.text = this.neededVegetables.ToString();
  
        this.request.SetActive(true);
        this.alertSprite.SetActive(false);

        this.audioSource.clip = this.requestClip;
        this.audioSource.Play();

        this.requested = true;
    }

    private int SatisfyRequest(ref int meat, ref int fish, ref int pharm, ref int vegetables)
    {
        if (meat < this.neededMeat || fish < this.neededFish || pharm < this.neededPharm || vegetables < this.neededVegetables)
        {
            this.RemindRequest();
            this.alertSprite.SetActive(false);
            return 0;
        }

        meat -= this.neededMeat;
        fish -= this.neededFish;
        pharm -= this.neededPharm;
        vegetables -= this.neededVegetables;

        this.audioSource.clip = this.successClip;
        this.audioSource.Play();

        this.alertSprite.SetActive(false);
        this.request.SetActive(false);
        this.success.SetActive(true);
        this.needsHelp = false;

        return 1;
    }

    private void RemindRequest()
    {
        this.request.SetActive(true);
        StopCoroutine("AnimateRequestReminder");
        this.request.transform.position = this.originalRequestPosition;
        StartCoroutine("AnimateRequestReminder");
        this.audioSource.clip = this.requestClip;
        this.audioSource.Play();
        return;
    }

    IEnumerator AnimateRequestReminder()
    {
        float timeAccumulator = 0.0f;
        Vector3 originalPosition = this.request.transform.position;

        while (timeAccumulator < Mathf.PI / 4)
        {
            this.request.transform.position = originalPosition + new Vector3(0.0f, (0.8f - timeAccumulator) * Mathf.Sin(40 * timeAccumulator), 0.0f);
            timeAccumulator += Time.deltaTime;
            yield return null;
        }
    }

    private void Start() {
        this.audioSource = this.GetComponent<AudioSource>();

        this.originalRequestPosition = this.request.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!this.needsHelp) { return; }

        this.alertSprite.SetActive(true);
        this.audioSource.clip = this.attentionClip;
        this.audioSource.Play();

        other.GetComponent<Player>().SetGranny(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponent<Player>().SetGranny(null);
        this.request.SetActive(false);
        this.success.SetActive(false);

        if (this.needsHelp)
        {
            this.alertSprite.SetActive(true);
        }
    }
}
