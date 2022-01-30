using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider coreHealthSlider;
    public TMP_Text coreHealthText;
    public Slider healthSlider;
    public TMP_Text healthText;
    public Slider xpSlider;
    public TMP_Text xpText;
    public TMP_Text lvlText;
    public TMP_Text woodText;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
