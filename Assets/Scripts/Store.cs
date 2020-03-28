﻿using UnityEngine;

public enum ProductType {
    Meat,
    Fish
}

public class Store : MonoBehaviour
{
    [SerializeField] private ProductType type;

    private AudioSource audioSource;

    public ProductType Purchase()
    {
        this.audioSource.Play();
        return this.type;
    }

    private void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Player>().SetStore(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponent<Player>().SetStore(null);
    }
}