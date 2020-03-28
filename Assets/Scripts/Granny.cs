using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Granny : MonoBehaviour
{
    [SerializeField] private bool needsHelp;
    [SerializeField] private bool requested;

    [SerializeField] [Range(0, 5)] private int neededMeat;
    [SerializeField] [Range(0, 5)] private int neededFish;

    [SerializeField] private AudioClip requestClip;
    [SerializeField] private AudioClip successClip;

    [SerializeField] private GameObject alertSprite;
    [SerializeField] private GameObject request;
    [SerializeField] private GameObject success;

    [SerializeField] private Transform meatSprite;
    [SerializeField] private Transform fishSprite;
    [SerializeField] private Text meatCounter;
    [SerializeField] private Text fishCounter;

    private AudioSource audioSource;

    private Vector3 originalRequestPosition;

    public void Interact(ref int meat, ref int fish)
    {
        if (!this.requested)
        {
            this.Request();
            return;
        }
        else if (this.needsHelp)
        {
            this.SatisfyRequest(ref meat, ref fish);
        }
    }

    private void Request()
    {
        int numberOfCategories = 0;
        if (neededMeat > 0) { numberOfCategories++; }
        if (neededFish > 0) { numberOfCategories++; }
        float spaceBetweenCategories = 6.0f / ((numberOfCategories * 2) + 1);
        float spaceBetweenCategoryText = 3600.0f / ((numberOfCategories * 2) + 1);

        meatCounter.text = this.neededMeat.ToString();
        fishCounter.text = this.neededFish.ToString();
        
        meatSprite.localPosition = new Vector3((spaceBetweenCategories) - 3.0f, 0.0f, meatSprite.localPosition.z);
        meatCounter.transform.localPosition = new Vector3((2 * spaceBetweenCategoryText) - 1800.0f - 200.0f, 0.0f, meatCounter.transform.localPosition.z);
        fishSprite.localPosition = new Vector3((3 * spaceBetweenCategories) - 3.0f, 0.0f, fishSprite.localPosition.z);
        fishCounter.transform.localPosition = new Vector3((4 * spaceBetweenCategoryText) - 1800.0f - 200.0f, 0.0f, fishCounter.transform.localPosition.z);

        this.request.SetActive(true);
        this.alertSprite.SetActive(false);

        this.requested = true;
    }

    private void SatisfyRequest(ref int meat, ref int fish)
    {
        if (meat < this.neededMeat || fish < this.neededFish)
        {
            this.RemindRequest();
            this.alertSprite.SetActive(false);
            return;
        }

        meat -= this.neededMeat;
        fish -= this.neededFish;

        this.audioSource.clip = this.successClip;
        this.audioSource.Play();

        this.alertSprite.SetActive(false);
        this.request.SetActive(false);
        this.success.SetActive(true);
        this.needsHelp = false;
    }

    private void RemindRequest()
    {
        this.request.SetActive(true);
        StopCoroutine("AnimateRequestReminder");
        this.request.transform.position = this.originalRequestPosition;
        StartCoroutine("AnimateRequestReminder");
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
        this.audioSource.clip = this.requestClip;
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
