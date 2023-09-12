using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class XPTracker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentLevelText;
    [SerializeField] TextMeshProUGUI currentXPText;
    [SerializeField] TextMeshProUGUI XPToNextLevelText;

    [SerializeField] UnityEvent<int, int> OnLevelChanged = new UnityEvent<int, int>();


    [SerializeField] BaseXPTranslation XPTranslationType;
    BaseXPTranslation XPTranslation;

    private void Awake()
    {
        XPTranslation = ScriptableObject.Instantiate(XPTranslationType);
    }
    public void AddXP(int amount)
    {
        int previousLevel = XPTranslation.CurrentLevel;
        if (XPTranslation.AddXP(amount))
        {
            OnLevelChanged.Invoke(previousLevel, XPTranslation.CurrentLevel);
        }
        Debug.Log("AddXP");
        RefreshDisplay();
    }
    public void SetLevel(int level)
    {
        int previousLevel = XPTranslation.CurrentLevel;
        XPTranslation.SetLevel(level);

        if(previousLevel != XPTranslation.CurrentLevel)
        {
            OnLevelChanged.Invoke(previousLevel, XPTranslation.CurrentLevel);
        }
        RefreshDisplay();
    }    

    




    // Start is called before the first frame update
    void Start()
    {
        RefreshDisplay();
        OnLevelChanged.Invoke(0, XPTranslation.CurrentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void RefreshDisplay()
    {   
        currentLevelText.text = $"Current Level: {XPTranslation.CurrentLevel}";
        currentXPText.text = $"Current XP: {XPTranslation.CurrentXP}";
        if (!XPTranslation.AtLevelCap)
        {
            XPToNextLevelText.text = $"XP to next level: {XPTranslation.XPRequiredForNextLevel}";
        }else
        {
            XPToNextLevelText.text = $"XP to next level: At Max";
        }
    }
}
