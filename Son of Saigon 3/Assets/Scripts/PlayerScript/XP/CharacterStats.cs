using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.HealthSystemCM;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class CharacterStats : MonoBehaviour, IGetHealthSystem
{
    [SerializeField] int BaseStamina_PerLevel = 5;
    [SerializeField] int BaseStamina_Offset = 10;
    [SerializeField] int StaminaConverseToHealth = 10;
    [SerializeField] float StaminaConverseToDamage = 1.25f;
    [SerializeField] TextMeshProUGUI StaminaText;
    [SerializeField] TextMeshProUGUI MaxHealthText;
    [SerializeField] TextMeshProUGUI CurrentHealthText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI DamageText;
    private HealthSystem healthSystem;
    private Animator animator;
    private int currentLevel;
    EnemyNavMesh enemyNavMesh;
    CharacterController characterController;
    ThirdPersonController thirdPersonController;
    ThirdPersonShooterController thirdPersonShooterController;
    GameManager gameManager;
   //Singleton
    private static CharacterStats instance;

   



    // Khai báo các thông số và hành vi của nhân vật ở đây

    // Phương thức public để truy cập thông qua Singleton
    public static CharacterStats Instance
    {
        get
        {
            // Nếu instance chưa được khởi tạo, hãy tìm hoặc tạo mới một đối tượng có script CharacterStats trong Scene
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterStats>();
            }

            return instance;
        }
    }



    public int BaseStamina { get; set; } = 0;

    public int Stamina
    {
        get
        {
            return BaseStamina;
        }
    }
    public float MaxHealthStat
    {
        get
        {
            return Stamina * StaminaConverseToHealth;
        }
    }
    public float CurrentHealthStat
    {
        get
        {
            return healthSystem.GetHealth();
        }
    }
    public int DamageStat
    {
        get
        {
            return (int)(Stamina * StaminaConverseToDamage);
        }
    }


    void Start()
    {
        RefreshUI();
    }
    void Awake()
    {
        if (!isBaseStaminaInitialized)
        {
            InitializeBaseStamina();
            isBaseStaminaInitialized = true;
        }
        healthSystem = new HealthSystem(MaxHealthStat);
        healthSystem.OnDead += HealthSystem_OnDead;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        enemyNavMesh = GetComponent<EnemyNavMesh>();
        gameManager = GetComponent<GameManager>();
        
    }




    public void Damage(int damageAmount)
    {
        /*if (HP <= 0)
       {
           //play death animation
           animator.SetTrigger("Die");
       }
       else
       {
           //play get hit animation
           animator.SetTrigger("Damaged");
       }*/
        healthSystem.Damage(damageAmount);
        Debug.Log("Player Damaged");
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        RefreshUI();
    }

    private bool isBaseStaminaInitialized = false;
    public void OnUpdateLevel(int previousLevel, int currentLevel)
    {
        this.currentLevel = currentLevel; // Cập nhật currentLevel
        InitializeBaseStamina();
        RefreshUI();
        // Cập nhật MaxHealthStat và DamageStat
        
        healthSystem.SetHealthMax(MaxHealthStat,true);
    }
    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        //animator.SetTrigger("Damaged");
        //HealthText.text = $"Max Health: {MaxHealthStat}";
        
    }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        Debug.Log("Game Over");
        StartCoroutine(RestartLevelAfterDelay(3f));
        InputSystem.DisableAllEnabledActions();
    }
    private IEnumerator RestartLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Restart Game");
    }
    void RefreshUI()
    {
        //StaminaText.text = $"Stamina: {Stamina}";
        MaxHealthText.text = $"Max Health: {MaxHealthStat}";
        DamageText.text = $"Damage: {DamageStat}";
        CurrentHealthText.text = $"Current Health: {CurrentHealthStat}";

    }
    private void InitializeBaseStamina()
    {
        BaseStamina = currentLevel * BaseStamina_PerLevel + BaseStamina_Offset;
    }
}

