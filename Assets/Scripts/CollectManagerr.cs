using UnityEngine;
using TMPro;

public class CollectManagerr : MonoBehaviour
{
    [Header("Prefabs y referencias")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject explosionEffect;

    [Header("UI")]
    [SerializeField] private GameObject coinPanel;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private GameObject winPanel;

    private int brokeCount = 0;
    private int coinCount = 0;
    private int activeCoins = 0;

    private void Start()
    {
        if (coinPanel != null)
            coinPanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("broke"))
        {
            HandleBroke(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("broke"))
        {
            HandleBroke(other.gameObject);
        }
    }

    private void HandleBroke(GameObject brokeObj)
    {
        brokeCount++;

        // 💥 PARTÍCULAS
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, brokeObj.transform.position, Quaternion.identity);
        }

        Destroy(brokeObj);

        if (brokeCount % 5 == 0)
        {
            SpawnCoinNearPlayer();
        }
    }

    private void SpawnCoinNearPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("No se encontró el Player");
            return;
        }

        Vector3 playerPos = player.transform.position;

        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(2f, 5f);

        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

        Vector3 spawnPos = new Vector3(playerPos.x + x, 1f, playerPos.z + z);

        GameObject coin = Instantiate(coinPrefab, spawnPos, Quaternion.Euler(90f, 0f, 0f));

        Coin coinScript = coin.GetComponent<Coin>();
        if (coinScript != null)
        {
            coinScript.SetManager(this);
        }

        activeCoins++;
        ShowCoinPanel();
    }

    private void ShowCoinPanel()
    {
        if (coinPanel != null)
            coinPanel.SetActive(true);

        if (coinText != null)
            coinText.text = "Find the coin!!!";
    }

    private void HideCoinPanel()
    {
        if (coinPanel != null)
            coinPanel.SetActive(false);
    }

    public void OnCoinCollected()
    {
        coinCount++;
        activeCoins--;

        Debug.Log("Monedas: " + coinCount);

        if (activeCoins <= 0)
        {
            HideCoinPanel();
        }

        if (coinCount >= 3)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        Debug.Log("GANASTE 🔥");

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("winPanel NO asignado");
        }

        Time.timeScale = 0f;
    }
}