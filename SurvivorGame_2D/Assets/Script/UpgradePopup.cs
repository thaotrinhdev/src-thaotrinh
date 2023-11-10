
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradePopup : MonoBehaviour
{
    public static UpgradePopup Instance;
    [SerializeField] private List<ItemInfoBase> itemInfos;
    [SerializeField] private UpgradeItemPopup upgradeItemViewPrefab;
    [SerializeField] private GameObject container;

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
        DontDestroyOnLoad(gameObject);
    }

    public void SetupPopup()
    {
        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }
        var randomItems = new List<ItemInfoBase>();
        GetRandomItemInfos(randomItems);

        foreach (var item in randomItems)
        {
            var itemView = Instantiate(upgradeItemViewPrefab, container.transform);
            itemView.SetItemData(item);
        }
    }


    private const int TotalRandomItem = 2;
    public void GetRandomItemInfos(List<ItemInfoBase> randomItems)
    {
        if (randomItems.Count >= TotalRandomItem)
        {
            return;
        }
        var totalWeight = itemInfos.Sum(i => i.DropChanceWeight);

        float currentWeight = 0;
        float randomWeight = Random.Range(0, totalWeight);
        for (int i = 0; i < itemInfos.Count; i++)
        {
            currentWeight += itemInfos[i].DropChanceWeight;
            if (randomWeight < currentWeight)
            {
                if (!randomItems.Contains(itemInfos[i]))
                {
                    if (randomItems.Count < TotalRandomItem)
                    {
                        randomItems.Add(itemInfos[i]);
                    }
                    else
                    {
                        return;
                    }
                    if (randomItems.Count < TotalRandomItem)
                    {
                        GetRandomItemInfos(randomItems);
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                    GetRandomItemInfos(randomItems);
                }
                break;
            }
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}