using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthFollow : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform objToFollow;
    RectTransform rectTransform;
    public static HealthFollow Instance;
    private void Awake()
    {
        rectTransform= GetComponent<RectTransform>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        slider.value = 1;
    }
    public void Update()
    {
        if (objToFollow!= null)
        {
            rectTransform.anchoredPosition= objToFollow.localPosition;
        }
    }
    public void UpdateHP(float hpPercent)
    {
        slider.value = hpPercent;

    }
}
