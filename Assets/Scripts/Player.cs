using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;

    private Camera cam;
    private Rigidbody2D rbody;
    private AudioSource audioSource;
    private Animator animator;

    private Vector2 currentDirection;
    [SerializeField] private GameObject inventory;
    [SerializeField] private int carriedMeat;
    [SerializeField] private int carriedFish;
    [SerializeField] private int carriedPharm;
    [SerializeField] private int carriedVegetables;
    private int totalItems;
    private Granny granny;
    private Store store;
    
    [SerializeField] private Text meatCounter;
    [SerializeField] private Text fishCounter;
    [SerializeField] private Text pharmCounter;
    [SerializeField] private Text vegetableCounter;

    [SerializeField] private InputField pieceOfPaper;

    [SerializeField] private GameObject winCon;

    public void SetGranny(Granny granny)
    {
        this.granny = granny;
    }

    public void SetStore(Store store)
    {
        this.store = store;
    }

    private void Start()
    {
        this.cam = Camera.main;
        this.rbody = this.GetComponent<Rigidbody2D>();
        this.audioSource = this.GetComponent<AudioSource>();
        this.animator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        this.cam.transform.position = 
            new Vector3(this.transform.position.x, 
                        this.transform.position.y, 
                        this.cam.transform.position.z);

        if (this.pieceOfPaper.isFocused) { return; }
        if (this.winCon.activeSelf) { return; }

        this.currentDirection = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) {
            this.currentDirection += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A)) {
            this.currentDirection += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S)) {
            this.currentDirection += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D)) {
            this.currentDirection += Vector2.right;
        }
        this.currentDirection.Normalize();
        this.currentDirection *= this.speed;

        if (this.currentDirection != Vector2.zero)
        {
            this.animator.SetBool("IsWalking", true);
        }
        else
        {
            this.animator.SetBool("IsWalking", false);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            this.granny?.Interact(ref this.carriedMeat, ref this.carriedFish, ref this.carriedPharm, ref this.carriedVegetables);
            if (this.store == null) goto CannotPurchase;
            if (this.totalItems >= 5)
            {
                this.audioSource.Play();
                goto CannotPurchase;
            }
            ProductType type = this.store.Purchase();
            switch (type)
            {
                case ProductType.Meat:
                    this.carriedMeat++;
                    break;
                case ProductType.Fish:
                    this.carriedFish++;
                    break;
                case ProductType.Pharm:
                    this.carriedPharm++;
                    break;
                case ProductType.Vegetables:
                    this.carriedVegetables++;
                    break;
                default:
                    Debug.LogWarning("Store carries product type that player does not support.");
                    break;
            }
            
            StopCoroutine("DisplayInventory");
            StartCoroutine("DisplayInventory");
        }
        CannotPurchase:;
        this.totalItems = this.carriedMeat + this.carriedFish;
        this.meatCounter.text = this.carriedMeat.ToString();
        this.fishCounter.text = this.carriedFish.ToString();
        this.pharmCounter.text = this.carriedPharm.ToString();
        this.vegetableCounter.text = this.carriedVegetables.ToString();
    }

    IEnumerator DisplayInventory()
    {
        float timeAccumulator = 0.0f;
        this.inventory.SetActive(true);
        while (timeAccumulator < 2.0f)
        {
            timeAccumulator += Time.deltaTime;
            yield return null;
        }
        this.inventory.SetActive(false);
    }

    private void FixedUpdate()
    {
        this.rbody.MovePosition(this.rbody.position + this.currentDirection);
    }
}
