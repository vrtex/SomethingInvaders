using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class EnemyParser : MonoBehaviour
{
    [System.Serializable]
    public enum EnemyType
    {
        Weak,
        Medium,
        Strong
    }


    public class EnemyInfo
    {
        public EnemyType type;
        public int health;
        public int damage;
        public float reloadTime;
        public int scoreGiven;
    }

    private static EnemyParser parser;
    private int difficulty;

    private Dictionary<EnemyType, EnemyInfo> EnemyTemplates;

    void Awake()
    {
        parser = this;

        var diffIno = FindObjectOfType<DifficultyInfo>();
        if(diffIno != null)
        {
            difficulty = FindObjectOfType<DifficultyInfo>().diff;
            Destroy(diffIno.gameObject);
        }

        EnemyTemplates = new Dictionary<EnemyType, EnemyInfo>();


        string info;
        string path = Application.streamingAssetsPath + "/enemies.txt";
        StreamReader streamReader = new StreamReader(path);
        info = streamReader.ReadToEnd();

        foreach(EnemyType type in System.Enum.GetValues(typeof(EnemyType)))
        {
            EnemyInfo nextEnemy = new EnemyInfo
            {
                type = type,
                health = GetInfo(type, "Health", info),
                damage = GetInfo(type, "Dmg", info),
                reloadTime = GetInfo(type, "Reload", info),
                scoreGiven = GetInfo(type, "Score", info)
            };
            if(nextEnemy.health == -1)
                nextEnemy.health = 1;

            if(nextEnemy.damage == -1)
                nextEnemy.damage = 1;

            if(nextEnemy.reloadTime == -1)
                nextEnemy.reloadTime = 1;

            if(nextEnemy.scoreGiven == -1)
                nextEnemy.scoreGiven = 1;

            EnemyTemplates.Add(type, nextEnemy);
        }
    }

    private static int GetInfo(EnemyType type, string value, string source)
    {
        //Regex healthRegex = new Regex("\\[" + type.ToString() + value + "\\]=(\\d+)",
        Regex healthRegex = new Regex("\\[" + type.ToString() + value + "\\]=(.*?), (.*?), (.*?)$",
            RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        Match match;
        match = healthRegex.Match(source);
        if(match.Groups.Count < 4)
            return -1;

        return int.Parse(match.Groups[1 + parser.difficulty].Value);
    }



    public static EnemyInfo GetEnemyInfo(EnemyType type)
    {
        if(!parser.EnemyTemplates.ContainsKey(type))
            return null;
        return parser.EnemyTemplates[type];
    }

}
