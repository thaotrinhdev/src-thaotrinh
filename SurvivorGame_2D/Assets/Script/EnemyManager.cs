using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    [SerializeField] private List<EnemyBase> enemyPrefabs;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float spawnInterval; // Bao lau se spawn 1 lan
    private float spawnTimer; // Da den luc spawn gan nhat cua
    private List<EnemyBase> enemies = new List<EnemyBase>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject); // Sang scence moi se khog xoa no di
    }
    public EnemyBase GetNearestEnemy(Vector3 from)
    {
        if (enemies.Count == 0) return null;
        //var a = enemies.Min(e => (e.transform.position - from).sqrMagnitude);//Tim de gia tri nho nhat
        EnemyBase neareatEnemy = null;
        float minDistance = float.MaxValue;
        foreach (var enemy in enemies)
        {
            var sqrDistance = (from - enemy.transform.position).sqrMagnitude;
            if (minDistance > sqrDistance)
            {
                minDistance = sqrDistance;
                neareatEnemy = enemy;

            }
        }
        return neareatEnemy;
    }
    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0;
            SpawnEnemy(enemyPrefabs.OrderBy(e=>Random.Range(0f,1f)).FirstOrDefault());// Random xuat hien ek dich
        }

    }
    private void SpawnEnemy(EnemyBase enemy)
    {
        var degree = Random.Range(0f, 360f);
        //var rad = Mathf.Deg2Rad * degree; // chuyen ve radian. Deg2Rad la hang so
        var randomVecter = Quaternion.Euler(0, 0, degree) * Vector2.up; // vector trong khoang cach cho phep vecter chay( gan nhat vaf xa nhat)
        var distance = Random.Range(minDistance, maxDistance);
        var clampedvecter = randomVecter.normalized * distance; // vi tri can phao spawn

        var playerPosition = (Vector3)PlayerController.Instance.transform.position; // vi tri nguoi choi
        var spawnPos = playerPosition + clampedvecter;
        var enemyBase = Instantiate(enemy, spawnPos, Quaternion.identity);
        enemies.Add(enemyBase);
    }
    public void RemoveEnemyFromList(EnemyBase enemy)
    {
        enemies.Remove(enemy);
    }
}
