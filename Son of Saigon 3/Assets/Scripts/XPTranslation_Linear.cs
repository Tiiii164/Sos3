using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/XP Linear", fileName = "XPTranslation_Linear")]
public class XPTranslation_Linear : BaseXPTranslation
{

    [SerializeField] int OffSet = 100;
    [SerializeField] float Slope = 50;
    [SerializeField] int LevelCap = 20;
    //Ham nay la de tinh cai kinh nghiem theo phep tinh duoi thi cap 1 : 100xp,c2:150,c3:200 
    protected int XPForLevel(int level)
    {
        return Mathf.FloorToInt((Mathf.Min(LevelCap, level) - 1)*Slope+OffSet);
    }

    //AddXP(int amount) được sử dụng để thêm một lượng kinh nghiệm (amount) vào CurrentXP.
    //Sau đó, nó tính toán cấp độ mới (newLevel) dựa trên kinh nghiệm hiện tại.
    //Biến levelledUp sẽ là true nếu newLevel khác CurrentLevel, ngược lại là false.
    //Cuối cùng, phương thức trả về giá trị của levelledUp để biết xem cấp độ đã tăng lên hay chưa.
    public override bool AddXP(int amount)
    {
        CurrentXP += amount;

        //CurrentXP = (CurrentLevel - 1) * Slope + OffSet
        //CurrentLevel = Mathf.FloorToInt((CurrentXP - OffSet)/Slope )+1;
        int newLevel = Mathf.Min(Mathf.FloorToInt((CurrentXP - OffSet) / Slope) + 1,LevelCap);
        
        //bien levelledUP sẽ là true nếu newLevel khác CurrentLevel, và false nếu chúng bằng nhau.
        bool levelledUp = newLevel != CurrentLevel;
        CurrentLevel = newLevel;

        AtLevelCap = CurrentLevel == LevelCap;
        return levelledUp;
        throw new System.NotImplementedException();
    }

    public override void SetLevel(int level)
    {
        CurrentXP = 0;
        CurrentLevel = 1;
        AtLevelCap = false;
        AddXP(XPForLevel(level));
      
    }

    protected override int GetXPRequiredForNextLevel()
    {
        if (AtLevelCap)
        {
            return int.MaxValue;
        }
        return XPForLevel(CurrentLevel + 1) - CurrentXP;
    }

  
}
