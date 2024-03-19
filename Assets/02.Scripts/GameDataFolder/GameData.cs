using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DataInfo
{
    [System.Serializable]
    public class GameData
    {
        public int bullet = 10;
        public int Hp = 100;
        public int damage = 10;
        public int KillCount = 0;
        public float speed = 3.0f;
        public List<Item> equipItem = new List<Item>();
    }
    [System.Serializable]
    public class Item
    {
        public enum ItemType {HP = 1, BULLET, SPEED, DAMGE}
        public enum ItemCalc { VALUE, PERSENT}
        public ItemType itemtype;
        public ItemCalc itemCalc;

        public string name;
        public string desc;
        public float value; 
    }
}
