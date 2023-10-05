using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.HealthSystemCM;
using StarterAssets;

public class CharacterStats : MonoBehaviour, IGetHealthSystem
{
    [SerializeField] int BaseStamina_PerLevel = 5;
    [SerializeField] int BaseStamina_Offset = 10;
    [SerializeField] int StaminaConverseToHealth = 10;
    [SerializeField] float StaminaConverseToDamage = 1.25f;
    [SerializeField] TextMeshProUGUI StaminaText;
    [SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] TextMeshProUGUI DamageText;
    private HealthSystem healthSystem;
    private Animator animator;
    private int currentLevel;
    CharacterController characterController;
    ThirdPersonController thirdPersonController;
    ThirdPersonShooterController thirdPersonShooterController;
    // Định nghĩa một biến static để lưu thể hiện Singleton
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
    public int MaxHealthStat
    {
        get
        {
            return Stamina * StaminaConverseToHealth;
        }
    }
    public int DamageStat
    {
        get
        {
            return (int)(Stamina * StaminaConverseToDamage);
        }
    }
    
    void Awake()
    {
        BaseStamina = currentLevel * BaseStamina_PerLevel + BaseStamina_Offset;
        healthSystem = new HealthSystem(MaxHealthStat);
        healthSystem.OnDead += HealthSystem_OnDead;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
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
        
    }
    public void OnUpdateLevel()
    {

        StaminaText.text = $"Stamina: {Stamina}";
        HealthText.text = $"Max Health: {MaxHealthStat}";
        DamageText.text = $"Damage: {DamageStat}";
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
            animator.SetTrigger("Die");
            //transform.parent.position = base.transform.position;
            thirdPersonController.enabled = false;
    }

    
}
// đã làm xong ăn dame sẽ chết nhân vật, cần phải làm cái event để khi mất máu nó cập nhật máu lên thanh máu
