using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DirectorSystem : MonoBehaviour
{
    [Header("Hotspot Tracking")]
    [SerializeField] private float hotspotRecordInterval = 2f;
    [SerializeField] private float minimumDistanceForNewHotspot = 3f;
    [SerializeField] private int maxHotspots = 10;

    [Header("Director Timing")]
    [SerializeField] private float searchCommandInterval = 30f;

    private List<GameObject> enemies = new List<GameObject>();
    private List<Vector3> playerHotspots = new List<Vector3>();
    
    private Transform player;
    
    private float hotspotTimer;
    private float searchTimer;

    private void Awake()
    {
        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemies.AddRange(foundEnemies);

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }
    }

    private void Start()
    {
        if (player != null)
            playerHotspots.Add(player.position);
    }

    private void Update()
    {
        if (player == null || enemies.Count == 0)
            return;

        hotspotTimer += Time.deltaTime;
        searchTimer += Time.deltaTime;

        if (hotspotTimer >= hotspotRecordInterval)
        {
            hotspotTimer = 0f;

            if (playerHotspots.Count == 0 ||
                Vector3.Distance(player.position, playerHotspots[playerHotspots.Count - 1]) >= minimumDistanceForNewHotspot)
            {
                if (player.position.y < -50)
                {
                    playerHotspots.Add(player.position + Vector3.up * 100);
                    
                    Debug.Log(player.position + Vector3.up * 100);
                }
                else
                {
                    playerHotspots.Add(player.position);
                    
                    Debug.Log(player.position);
                }
                
                

                if (playerHotspots.Count > maxHotspots)
                    playerHotspots.RemoveAt(0);
            }
        }

        if (searchTimer >= searchCommandInterval && playerHotspots.Count > 0)
        {
            searchTimer = 0f;
            
            Vector3 chosenHotspot = playerHotspots[Random.Range(0, playerHotspots.Count)];
            
            playerHotspots.Remove(chosenHotspot);

            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    continue;
                }

                float distance = Vector3.Distance(enemies[i].transform.position, chosenHotspot);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemies[i];
                }
            }

            if (closestEnemy != null)
            {
                Debug.Log("Attempting to send enemy to :" + chosenHotspot);


                Enemy temp = closestEnemy.GetComponent<Enemy>();
                temp.SearchLocation(chosenHotspot);
            }
        }
    }
}