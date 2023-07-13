using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    //Non-persistent Singleton for easy access for Enemy script
    public static Player Instance { get; private set; }

    [SerializeField] private GameObject resultsTab;
    [SerializeField] private GameObject nextButton;
    private TMP_Text resultText;

    [SerializeField] private GameObject[] hearts;
    private int health;
    private Vector2 originalPosition;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        originalPosition = transform.position;

        health = hearts.Length;

        resultText = resultsTab.GetComponentInChildren<TMP_Text>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Win Condition
        if (collision.gameObject.CompareTag("Goal"))
        {
            ShowResults();
        }

        if (collision.gameObject.CompareTag("Death Zone"))
        {
            transform.position = originalPosition;
            DamagePlayer();
        }

        //If the player collides with the something that can damage me
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Trap"))
        {
            DamagePlayer();   
        }
    }

    private void DamagePlayer()
    {
        health--;
        hearts[health].SetActive(false);
        if (health <= 0)
        {
            Destroy(gameObject);
            ShowResults();
        }
    }

    private void ShowResults()
    {
        if (resultText == null) return;

        if (health > 0)
        {
            resultText.text = "YOU WON";
            nextButton.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            resultText.text = "YOU LOST";
        }

        resultsTab.SetActive(true);
    }
}
