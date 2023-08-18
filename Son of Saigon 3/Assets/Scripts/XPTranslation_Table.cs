using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XPTranslationTableEntry
{
    public int Level;
    public int XPRequired;
}

[CreateAssetMenu(menuName = "RPG/XP Table", fileName = "XPTranslation_Table")]
public class XPTranslation_Table : BaseXPTranslation
{
    [SerializeField] List<XPTranslationTableEntry> Table;
    // Hàm này tính toán kinh nghiệm cần thiết cho các cấp độ
   /* public void CalculateXPRequirements()
    {
        if (Table.Count == 0)
        {
            Debug.LogError("XP Translation Table is empty!");
            return;
        }

        // Xóa tất cả các mục hiện có trong bảng
        Table.Clear();

        // Tạo mục cho level 1
        XPTranslationTableEntry level1 = new XPTranslationTableEntry();
        level1.Level = 1;
        level1.XPRequired = 100; // Kinh nghiệm cần thiết cho level 1
        Table.Add(level1);
        Debug.Log("Level: " + level1.Level + ", XP Required: " + level1.XPRequired);
        // Tạo mục cho các cấp độ tiếp theo dựa trên quy tắc "level 2 gấp đôi kinh nghiệm cần để lên cấp của level 1"
        for (int i = 2; i <= Table.Count; i++)
        {
            XPTranslationTableEntry entry = new XPTranslationTableEntry();
            entry.Level = i;
            entry.XPRequired = Table[i - 2].XPRequired * 2; // Kinh nghiệm cần thiết cho level i
            Table.Add(entry);
            Debug.Log("Level: " + level1.Level + ", XP Required: " + level1.XPRequired);
        }
    }*/

    public void Start()
    {
        //CalculateXPRequirements();
    }
    public override bool AddXP(int amount)
    {

        if (AtLevelCap)
        {
            return false;
        }

        CurrentXP += amount;
        for(int i=Table.Count - 1; i >= 0; i--)
        {
            var entry = Table[i];
            //found a matching entry
            if(CurrentXP >= entry.XPRequired)
            {
                //level changed 
                if(entry.Level != CurrentLevel)
                {
                    CurrentLevel = entry.Level;

                    AtLevelCap = Table[^1].Level == CurrentLevel;

                    return true;
                }
                break;
            }
        }
        return false;
    }

    public override void SetLevel(int level)
    {
        CurrentXP = 0;
        CurrentLevel = 1;
        AtLevelCap = false;
        foreach (var entry in Table)
        {

            if(entry.Level == level)
            {
                AddXP(entry.XPRequired);
                return;
            }
        }
        throw new System.ArgumentOutOfRangeException($"Could not find any entry for level {level}");
    }

    protected override int GetXPRequiredForNextLevel()
    {
        if (AtLevelCap)
        {
            return int.MaxValue;
        }
        for (int i = 0; i < Table.Count; i++)
        {
            var entry = Table[i];
            if(entry.Level == CurrentLevel)
            {
                return Table[i +1 ].XPRequired - CurrentXP;
            }
        }
        throw new System.ArgumentOutOfRangeException($"Could not find any entry for level {CurrentLevel}");
    }
}



