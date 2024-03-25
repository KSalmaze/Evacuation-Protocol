using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Inventário")]
    [SerializeField] private GameObject inventoryHolder;
    [SerializeField] private int inventorySize;
    [SerializeField] private List<Item> inventory;
    [SerializeField] private List<Image> holders;
    [SerializeField] private GameObject itemOnGroundPrefab;
    
    [Header("Timer")] 
    [SerializeField] private float totalTime;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private bool timerIsPaused;
    [SerializeField] private float remainingTime;
    
    [Header("Interações")]
    [SerializeField] private GameObject textHolder;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject documentHolder;
    [SerializeField] private TMP_Text documentText;
    
    private bool inputDetected = false;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        remainingTime = totalTime * 60;
        StartCoroutine(TimerCoroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryHolder.SetActive(!inventoryHolder.activeSelf);
        }
    }
    
    // Inventário
    public bool AddItem(Item item)
    {
        if (inventory.Count < inventorySize)
        {
            inventory.Add(item);
            holders[inventory.Count - 1].sprite = item.sprite;
            if (item.isFeminine)
            {
                ShowText("Você pega uma " + item.itemName);
            }
            else
            {
                ShowText("Você pega um " + item.itemName);
            }
            return true;
        }
        else
        {
            if (item.isFeminine)
            {
                ShowText("Você ve uma " + item.itemName + " mas não tem espaço para carrega-la");
            }
            else
            {
                ShowText("Você ve um " + item.itemName + " mas não tem espaço para carrega-lo");
            }

            return false;
        }
    }

    public void DropItem(int slot)
    {
        if (inventory[slot] != null)
        {
            Debug.Log("Soltando o item");
            GameObject itemA = Instantiate(itemOnGroundPrefab,transform);
            ItemInteraction itemInfos = itemA.GetComponent<ItemInteraction>();
            itemInfos.item = inventory[slot];
            itemInfos.isOnGround = true;
            inventory.Remove(inventory[slot]);
            holders[slot].sprite = null;
        }
    }
    
    // Timer
    IEnumerator TimerCoroutine()
    {
        while (remainingTime > 0f)
        {
            // Verifica se o timer está pausado
            while (timerIsPaused)
            {
                yield return null; // Aguarda até que o timer não esteja pausado
            }

            // Decrementa o tempo restante
            remainingTime -= Time.deltaTime;
            timerText.text = FormatTimer(remainingTime);

            // Aguarda até o próximo frame
            yield return null;
        }

        // O tempo acabou
        TimeisOver();
    }

    private string FormatTimer(float time)
    {
        int timeInInt = (int)time;
        int minutes = timeInInt / 60;
        int seconds = timeInInt % 60;

        string formatedTime = minutes + ":" + seconds;

        if (seconds < 10)
        {
            formatedTime += "0";
        }

        if (minutes < 10)
        {
            formatedTime = "0" + formatedTime;
        }

        return formatedTime;
    }
    
    private void SoftPause()
    {
        timerIsPaused = !timerIsPaused;
    }

    private void TimeisOver()
    {
        Debug.Log("O tempo acabou");
    }
    
    // Interações
    public void ShowText(string newtext)
    {
        text.text = newtext;
        textHolder.SetActive(true);
        StartCoroutine(WaitForInputCoroutine(textHolder));
    }

    public void ShowDocument(string newText)
    {
        documentText.text = newText;
        documentHolder.SetActive(true);
        StartCoroutine(WaitForInputCoroutine(documentHolder));
    }

    IEnumerator WaitForInputCoroutine(GameObject theGameObject)
    {
        SoftPause();
        Debug.Log("Aguardando entrada do jogador...");

        // Loop até que um input seja detectado
        while (!inputDetected)
        {
            // Espera pelo próximo frame
            yield return null;

            // Verifica se algum input foi detectado
            if (Input.anyKeyDown)
            {
                // Define a flag indicando que um input foi detectado
                theGameObject.SetActive(false);
                inputDetected = true;
            }
        }

        // Resetar a flag para permitir que o processo seja executado novamente se necessário
        inputDetected = false;

        // Agora que um input foi detectado, continue com o resto da função
        Debug.Log("Input do jogador detectado!");
        SoftPause();
    }
}
