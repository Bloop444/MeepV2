using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class XP_Manager : MonoBehaviour
{
    public int xp;
    public int[] xpToNextLevel = new int[7]
    {
        25000,
        50000,
        100000,
        150000,
        250000,
        500000,
        1500000
    };

    public Texture[] rankImages = new Texture[8];

    public TMP_Text rankDisplayText;
    public TMP_Text xpDisplayText;
    public TMP_Text nextRankDisplayText;

    public string xpToNextLevelStr;
    public string curRankStr;

    public Slider xpSlider;

    public RawImage rankIcon;

    public enum Rank
    {
        Bum,
        Cook,
        Navigator,
        Swordmans,
        Sailor,
        FirstMate,
        SeaCaptain,
        Admiral
    }

    public Rank curRank;

    public Rank nextRank;

    public float xpIncreasePerSecond = 1f;

    private float xpIncreaseTimer = 0f;

    private void Start()
    {
        SetAndGetCurrentStats();
    }

    private void Update()
    {
        xpIncreaseTimer += Time.deltaTime;

        if (xpIncreaseTimer >= 1f)
        {
            xp += Mathf.FloorToInt(xpIncreasePerSecond);
            xpIncreaseTimer = 0f;
        }

        
        if (xp >= xpToNextLevel[(int)curRank])
        {
            RankUp();
        }

        GetNextRank();
        UpdateRankImage();
    }

    private void LateUpdate()
    {
        switch (curRank)
        {
            case Rank.Bum: xpToNextLevelStr = xpToNextLevel[0].ToString(); break;
            case Rank.Cook: xpToNextLevelStr = xpToNextLevel[1].ToString(); break;
            case Rank.Navigator: xpToNextLevelStr = xpToNextLevel[2].ToString(); break;
            case Rank.Swordmans: xpToNextLevelStr = xpToNextLevel[3].ToString(); break;
            case Rank.Sailor: xpToNextLevelStr = xpToNextLevel[4].ToString(); break;
            case Rank.FirstMate: xpToNextLevelStr = xpToNextLevel[5].ToString(); break;
            case Rank.SeaCaptain: xpToNextLevelStr = xpToNextLevel[6].ToString(); break;
            case Rank.Admiral: xpToNextLevelStr = xpToNextLevel[6].ToString(); break;
        }

        xpDisplayText.text = xp + "/" + xpToNextLevelStr + " XP";
        rankDisplayText.text = " " + curRank.ToString().ToUpper();
        nextRankDisplayText.text = "NEXT RANK: " + nextRank.ToString().ToUpper();

        curRankStr = curRank.ToString();

        xpSlider.value = xp;
        xpSlider.maxValue = GetXpToNextLevel();
        xpSlider.minValue = 0;

        PlayerPrefs.SetInt("XP", xp);
        PlayerPrefs.SetString("Rank", curRankStr);
        PlayerPrefs.Save();
    }

    public int GetXpToNextLevel()
    {
        switch (curRank)
        {
            case Rank.Bum: return xpToNextLevel[0];
            case Rank.Cook: return xpToNextLevel[1];
            case Rank.Navigator: return xpToNextLevel[2];
            case Rank.Swordmans: return xpToNextLevel[3];
            case Rank.Sailor: return xpToNextLevel[4];
            case Rank.FirstMate: return xpToNextLevel[5];
            case Rank.SeaCaptain: return xpToNextLevel[6];
            case Rank.Admiral: return xpToNextLevel[6];
            default: return 99;
        }
    }

    public void SetAndGetCurrentStats()
    {
        if (!PlayerPrefs.HasKey("XP"))
        {
            xp = 0;
        }
        else
        {
            xp = PlayerPrefs.GetInt("XP");
        }

        if (!PlayerPrefs.HasKey("Rank"))
        {
            curRank = Rank.Bum;
        }
        else
        {
            curRankStr = PlayerPrefs.GetString("Rank");
            switch (curRankStr)
            {
                case "Bum": curRank = Rank.Bum; break;
                case "Cook": curRank = Rank.Cook; break;
                case "Navigator": curRank = Rank.Navigator; break;
                case "Swordmans": curRank = Rank.Swordmans; break;
                case "Sailor": curRank = Rank.Sailor; break;
                case "FirstMate": curRank = Rank.FirstMate; break;
                case "SeaCaptain": curRank = Rank.SeaCaptain; break;
                case "Admiral": curRank = Rank.Admiral; break;
            }
        }
    }

    public string GetNextRank()
    {
        switch (curRank)
        {
            case Rank.Bum: nextRank = Rank.Cook; break;
            case Rank.Cook: nextRank = Rank.Navigator; break;
            case Rank.Navigator: nextRank = Rank.Swordmans; break;
            case Rank.Swordmans: nextRank = Rank.Sailor; break;
            case Rank.Sailor: nextRank = Rank.FirstMate; break;
            case Rank.FirstMate: nextRank = Rank.SeaCaptain; break;
            case Rank.SeaCaptain: nextRank = Rank.Admiral; break;
            case Rank.Admiral: break;
        }
        return nextRank.ToString();
    }

    public void RankUp()
    {
        if (curRank < Rank.Admiral)
        {
            curRank++;
        }
        else
        {
            curRank = Rank.Admiral; 
        }
    }

    public void UpdateRankImage()
    {
        switch (curRank)
        {
            case Rank.Bum: rankIcon.texture = rankImages[0]; break;
            case Rank.Cook: rankIcon.texture = rankImages[1]; break;
            case Rank.Navigator: rankIcon.texture = rankImages[2]; break;
            case Rank.Swordmans: rankIcon.texture = rankImages[3]; break;
            case Rank.Sailor: rankIcon.texture = rankImages[4]; break;
            case Rank.FirstMate: rankIcon.texture = rankImages[5]; break;
            case Rank.SeaCaptain: rankIcon.texture = rankImages[6]; break;
            case Rank.Admiral: rankIcon.texture = rankImages[7]; break;
        }
    }

    public void AddXp(int value)
    {
        xp += value;
    }

    public void TakeXp(int value)
    {
        xp -= value;
    }

    public void ResetXP()
    {
        PlayerPrefs.DeleteKey("XP");
        xp = 0;
    }
}
