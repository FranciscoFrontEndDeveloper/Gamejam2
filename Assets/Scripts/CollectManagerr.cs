using UnityEngine;
using TMPro;

public class CollectManagerr : MonoBehaviour
{
    [Header("Prefabs y referencias")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private TMP_Text coinMessage;   // Texto para "Find the coin!!!"
    [SerializeField] private TMP_Text winMessage;    // Texto grande para "¡GANASTE!"

    private int brokeCount = 0;
    private int coinCount = 0;
    private bool waitingForCoin = false;

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
        if (waitingForCoin) return;

        brokeCount++;
        Destroy(brokeObj);

        if (brokeCount % 5 == 0)
        {
            waitingForCoin = true;
            SpawnCoinNearPlayer();
            ShowCoinMessage();
        }
    }

    private void SpawnCoinNearPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No se encontró el objeto con tag Player");
            return;
        }

        Vector3 playerPos = player.transform.position;

        float minDistance = 2f;
        float maxDistance = 5f;
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(minDistance, maxDistance);

        float offsetX = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
        float offsetZ = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

        Vector3 spawnPos = new Vector3(
            playerPos.x + offsetX,
            1f,
            playerPos.z + offsetZ
        );

        Quaternion uprightRotation = Quaternion.Euler(90f, 0f, 0f);
        Instantiate(coinPrefab, spawnPos, uprightRotation);
    }

    private void ShowCoinMessage()
    {
        if (coinMessage != null)
        {
            coinMessage.gameObject.SetActive(true);
            coinMessage.text = "Find the coin!!!";
        }
    }

    private void HideCoinMessage()
    {
        if (coinMessage != null)
            coinMessage.gameObject.SetActive(false);
    }

    public void OnCoinCollected()
    {
        coinCount++;
        waitingForCoin = false;
        HideCoinMessage();

        if (coinCount >= 3)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        Debug.Log("You win!");
        Time.timeScale = 0f;

        if (winMessage != null)
        {
            winMessage.gameObject.SetActive(true);
            winMessage.text = "¡GANASTE!";
            winMessage.fontSize = 100; // tamaño grande
            winMessage.alignment = TextAlignmentOptions.Center;
        }
    }
}