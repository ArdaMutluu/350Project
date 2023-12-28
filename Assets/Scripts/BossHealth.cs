using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Input = UnityEngine.Windows.Input;

public class BossHealth : MonoBehaviour
{
    public Boss boss;
    public Slider hslider;
    void Start()
    {
        boss.health = boss.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (hslider.value != boss.health)
        {
            hslider.value = boss.health;
        }
        
    }
}
