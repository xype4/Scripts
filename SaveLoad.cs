using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    string SaveFilePath;         
    string ScreenFilePath;
    string loadSavePath;   
    string loadInfoPath;   

    public List<GameObject> EnemySaves = new List<GameObject>();
    private GameObject PlayerBody;
    public GameObject LoadingScreen;
    public GameObject InventoryPanelItem;
    public GameObject InventoryPanelQest;
    public GameObject InventoryPanelEquip;
    public GameObject GameMenu;
    public GameObject PlayerUI;
    public GameObject SkillsMain;

    public Transform PlayerDropItems_;
    public Transform DropItems_;

    public int NumQuickSave = 0;
    public int saveNumber = 0;
    private bool InventoryActive = false;

    private void Awake()
    {
        LoadingScreen.SetActive(true);

        Time.timeScale = 1;
        PlayerBody = gameObject;

        loadSavePath = Application.persistentDataPath + "/SaveInfo.bsa";//Номер быстрого сохранения
        loadInfoPath = Application.persistentDataPath + "/LoadInfo.bsa";//Номер загр сохранения
        SaveFilePath = Application.persistentDataPath + "/Saves";
        ScreenFilePath = Application.persistentDataPath + "/Saves";

        if(!File.Exists(loadInfoPath))                                                   //Если существует файл сохранения
        {
            LoadingScreen.SetActive(false);
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(loadInfoPath, FileMode.Open);
        int save = (int)bf.Deserialize(fs);
        fs.Close();

        if(save == 0)
        {
            LoadingScreen.SetActive(false);
            return;
        }
        else
        {
            BinaryFormatter bf1 = new BinaryFormatter();
            FileStream fs1 = new FileStream(loadSavePath, FileMode.Open);
            int loadsave = (int)bf1.Deserialize(fs1);
            fs.Close();

            NumQuickSave = loadsave;

            StartCoroutine(Loading(save));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("f5"))
        {
            if(NumQuickSave > 4)
                NumQuickSave = 0;

            NumQuickSave++;
            SaveFilePath = Application.persistentDataPath + "/Saves/QuickSave"+NumQuickSave+".GS";
            ScreenFilePath = Application.persistentDataPath + "/Saves/PicQuickSave"+NumQuickSave+".png";
            ScreenCapture.CaptureScreenshot(ScreenFilePath);

            saveNumber = NumQuickSave + 5;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(loadSavePath, FileMode.Create);
            int numsave = NumQuickSave;
            bf.Serialize(fs, numsave);
            fs.Close();  

            BinaryFormatter bf1 = new BinaryFormatter();
            FileStream fs1 = new FileStream(loadInfoPath, FileMode.Create);
            int numsave1 = saveNumber;
            bf1.Serialize(fs1, numsave1);                                              //5-10 сохранения из кнопки 1-5 сохранения из меню
            fs1.Close();  

            SaveGame();
        }
            
        if (Input.GetKeyDown("f6") && NumQuickSave!=0)
        {
            LoadingScreen.SetActive(true);
          
            SceneManager.LoadScene(1);
        }
    }
    
    public void SaveButton (int NumSave)
    {
        SaveFilePath = Application.persistentDataPath + "/Saves/PrivateSave"+NumSave+".GS";
        ScreenFilePath = Application.persistentDataPath + "/Saves/PicPrivateSave"+NumSave+".png";

        BinaryFormatter bf1 = new BinaryFormatter();
        FileStream fs1 = new FileStream(loadInfoPath, FileMode.Create);
        int numsave = NumSave;
        bf1.Serialize(fs1, numsave);
        fs1.Close();  

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(loadSavePath, FileMode.Create);
        int save = NumSave;
        bf.Serialize(fs, save);
        fs.Close(); 

        SaveGame();
        MenuScreen();
    }

    public void LoadButton (int NumSave)
    {
        if(File.Exists(Application.persistentDataPath + "/Saves/PrivateSave"+NumSave+".GS") || File.Exists(Application.persistentDataPath + "/Saves/QuickSave"+ (NumSave - 5)+".GS"))
        {
            LoadingScreen.SetActive(true);

            BinaryFormatter bf1 = new BinaryFormatter();
            FileStream fs1 = new FileStream(loadInfoPath, FileMode.Create);
            int numsave = NumSave;
            bf1.Serialize(fs1, numsave);
            fs1.Close();  

            SceneManager.LoadScene(1);
        }
    }

    public void SaveGame()                                          //Запись переменной в нужный путь
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(SaveFilePath, FileMode.Create);
        Save save = new Save();
        //save.SaveEnemies(EnemySaves);
        save.SavePlayer(PlayerBody, InventoryPanelItem, InventoryPanelQest, InventoryPanelEquip);
        save.SaveDropItems(DropItems_);
        save.SavePlayerDropItems(PlayerDropItems_);
        save.PlayerSkills(PlayerBody, SkillsMain);
        bf.Serialize(fs, save);
        fs.Close();       
    }

    public void MenuScreen()
    {
        GameMenu.SetActive(false);                                             //Выключить Меню, включть ЮИ и выключить инвентарь, если активен
        PlayerUI.SetActive(true);
        if(PlayerUI.transform.GetChild(2).gameObject.activeSelf)
        {
            PlayerUI.transform.GetChild(2).gameObject.SetActive(false);
            InventoryActive = true;
        }
        Time.timeScale = 1;                                                 //Запустить время для работы коротина Screen
        gameObject.GetComponent<ControllerBody>().timeFactor = 0;
        ScreenCapture.CaptureScreenshot(ScreenFilePath);                    //Cделать скриншот в пути
        StartCoroutine(Screen());
    }                                                              

    IEnumerator Screen()
    {
        yield return new WaitForSeconds(0.01f);    
        GameMenu.SetActive(true);                                             
        PlayerUI.SetActive(false);
        gameObject.GetComponent<ControllerBody>().timeFactor = 1;
        if(InventoryActive)
            PlayerUI.transform.GetChild(2).gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void LoadGame()                                           //Загрузка переменной из пути
    {
        if(!File.Exists(SaveFilePath))                                                   //Если существует файл сохранения
            return;
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(SaveFilePath, FileMode.Open);
        Save save = (Save)bf.Deserialize(fs);
        fs.Close();

        GameObject Hand = PlayerBody.transform.GetChild(0).transform.GetChild(1).gameObject;

        PlayerBody.GetComponent<ControllerBody>().LoadData(save.SavePl);
        PlayerBody.transform.GetChild(0).gameObject.GetComponent<ControllerHad>().LoadData(save.SavePl);
        PlayerBody.transform.GetChild(0).transform.GetChild(0).GetComponent<Inventory>().LoadData(save.SavePl);
        PlayerBody.transform.GetChild(0).transform.GetChild(0).GetComponent<ShopVisiable>().LoadData(save.SavePl);
        PlayerBody.transform.GetChild(0).transform.GetChild(2).GetComponent<Player_Sheild>().LoadData(save.SavePl);
        PlayerBody.GetComponent<PlayerStats>().LoadData(save.SavePl);
        gameObject.GetComponent<DropItemManager>().LoadData(save.PlayerDropItem, save.DropItem);

        for(int f = 0; f < SkillsMain.transform.childCount; f ++)
        {
            for(int j = 1; j < SkillsMain.transform.GetChild(f).childCount; j ++)
            {
                SkillsMain.transform.GetChild(f).GetChild(j).GetComponent<Skill>().LoadData(save.plSkills);
            }
        }

        PlayerBody.GetComponent<Skill_Indicator>().LoadData(save.plSkills);
        /*int i = 0;                                                                   //Каждому врагу из листа запускаем метод LoadData и отправляем показатели
        foreach (var enemy in save.EnemiesDataCube)
        {
            EnemySaves[i].GetComponent<Cube_AI>().LoadData(enemy);
            i++;
        }*/
    }                                                                  //Загрузка переменной из пути

    IEnumerator Loading(int save)
    {
        yield return new WaitForSeconds(0.5f);
        if(save < 6)
            SaveFilePath = Application.persistentDataPath + "/Saves/PrivateSave"+ save +".GS";
            ScreenFilePath = Application.persistentDataPath + "/Saves/PicPrivateSave"+save+".png";
        if(save > 5)
        {
            save-= 5;
            SaveFilePath = Application.persistentDataPath + "/Saves/QuickSave"+ save +".GS";
            ScreenFilePath = Application.persistentDataPath + "/Saves/PicQuickSave"+save+".png";
        }
        LoadGame();
        yield return new WaitForSeconds(2f);
        LoadingScreen.SetActive(false);
        Time.timeScale = 1;
    }
}


[System.Serializable]
public class Save
{
    [System.Serializable]
    public struct Arrow
    {
        public int Count;
        public int Id;
        public int InvCell;

        public Arrow (int count, int id, int invCell)
        {
            Count = count;
            Id = id;
            InvCell = invCell;
        }
    }

    [System.Serializable]
    public struct Item_
    {
        public int ID;
        public byte Count;

        public Item_ (int id, byte count)
        {
            ID = id;
            Count = count;
        }
    }

    [System.Serializable]
    public struct Vec3                                     //Структура Vector3
    {
        public float x, y, z;
        public Vec3 (float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }                                                      //Структура Vector3

    [System.Serializable]
    public struct Item_Drop
    {
        public int ID, Count;
        public Vec3 Position;

        public Item_Drop (int id, int count, Vec3 pos)
        {
            ID = id;
            Count = count;
            Position = pos;
        }
    }


    [System.Serializable]
    public struct EnemySaveData                            //Структура(переменная) информации об объекте
    {
        public Vec3 Position;                                                     //Содержит позицию (Vec3)
        public float hp;                                                        //Содержит количество здоровья
        public bool isDie;                                                       //Содержит инфу о смерти

        public EnemySaveData(Vec3 pos, float Hp, bool Die)
        {
            Position = pos;
            hp = Hp;
            isDie = Die;
        }
    }                                                    //Структура(переменная) информации об объекте    
    
    [System.Serializable]
    public struct PlayerSaveData                            //Структура(переменная) информации об игроке
    {
        public Vec3 Position;                                                     //Содержит позицию (Vec3)
        public Vec3 Rotation;
        public List<Item_> InventoryItem;
        public List<Item_> InventoryQuest;
        public List<Item_> InventoryEquip;
        public int Weapon;                         //Номер оружия в иерархии
        public int TypeAttack;           //Тип атаки оружием
        public int Sheild;
        public int LastSheild;
        public float HP,Stamina;
        public int Lvl,Score,EXP;
        public List<byte> EquipQuest;
        public List<byte> EquipEquip;
        public Arrow Arrow_;
        public int Money;
        public int ArmorInform;
        public int ArmorID;
        
        public PlayerSaveData(Vec3 pos, Vec3 rot, List<Item_> inv, List<Item_> que, List<Item_> equ, int weap, int typeAttack, int sheild, int Lsheild, float hp, float stam, int lvl, int score, int exp, List<byte> equipQuest, List<byte> equipEquip, Arrow arr, int money, int armInf, int armID)
        {
            Position = pos;
            Rotation = rot;
            InventoryItem = inv; InventoryQuest = que; InventoryEquip = equ;
            Weapon = weap; Sheild = sheild; LastSheild = Lsheild; TypeAttack = typeAttack;
            HP = hp; Stamina = stam;
            Lvl = lvl; Score = score; EXP = exp;
            EquipQuest = equipQuest; EquipEquip = equipEquip;
            Arrow_ = arr; Money = money;
            ArmorInform = armInf; ArmorID = armID;
        }
    }     

    public List<Item_Drop> DropItem = new List<Item_Drop>();
    public List<Item_Drop> PlayerDropItem = new List<Item_Drop>();

    public void SaveDropItems(Transform dropItems)
    {
        for(int i = 0, j = 0; i< dropItems.transform.childCount; i++, j++)
        {
            if(j == dropItems.GetChild(i).GetComponent<Number>().Num)
            {
                Item digit = dropItems.GetChild(i).GetComponent<Item>();
                GameObject posdigit = dropItems.GetChild(i).gameObject;
                DropItem.Add(new Item_Drop(digit.id, digit.countItem, new Vec3(posdigit.transform.position.x, posdigit.transform.position.y, posdigit.transform.position.z)));
            }
            else
            {
                DropItem.Add(new Item_Drop(0,0, new Vec3(0,0,0)));
                i--;
            }
        }
    }
    public void SavePlayerDropItems(Transform playerDropItems)
    {
        PlayerDropItem.Clear();
        for(int i = 0; i < playerDropItems.transform.childCount; i++)
        {
            Item digit = playerDropItems.GetChild(i).GetComponent<Item>();
            GameObject posdigit = playerDropItems.GetChild(i).gameObject;
            Vec3 pos = new Vec3(posdigit.transform.position.x, posdigit.transform.position.y, posdigit.transform.position.z);
            PlayerDropItem.Add(new Item_Drop(digit.id, digit.countItem, pos));
        }
    }

    public PlayerSaveData SavePl;
    public List<Item_> Inventory_Item = new List<Item_>();
    public List<Item_> Inventory_Quest = new List<Item_>();
    public List<Item_> Inventory_Equip = new List<Item_>();
    
    public void SavePlayer(GameObject player, GameObject InvPanelItems, GameObject InvPanelIQuest, GameObject InvPanelEquip)
    {
        ControllerBody pl = player.GetComponent<ControllerBody>();
        Inventory HandColInventory = player.transform.GetChild(0).transform.GetChild(0).GetComponent<Inventory>();
        GameObject Hand = player.transform.GetChild(0).transform.GetChild(1).gameObject;
        GameObject Hand2 = player.transform.GetChild(0).transform.GetChild(2).gameObject;

        Vec3 pos = new Vec3(player.transform.position.x, player.transform.position.y, player.transform.position.z);  
        Vec3 rot = new Vec3(0 ,player.transform.localEulerAngles.y ,0);

        List<byte> equipQuest = new List<byte>();
        for(byte i = 0; i<InvPanelIQuest.transform.childCount; i++)
        {
            if(InvPanelIQuest.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text == "E")
            {
                equipQuest.Add(i);
            }
        }

        List<byte> equipEquip = new List<byte>();
        for(byte i = 0; i<InvPanelEquip.transform.childCount; i++)
        {
            if(InvPanelEquip.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text == "E")
            {
                equipEquip.Add(i);
            }
        }

        int typeAttack = Hand.GetComponent<Player_Attack>().TypeAtcak;
        int weap = Hand.GetComponent<Player_Attack>().Sword.GetComponent<WeaponAttack>().WeapNumber;   //Номер оружия в иерархии
        Arrow arrow = new Arrow(Hand.GetComponent<Player_Attack>().Arrow.id, Hand.GetComponent<Player_Attack>().Arrow.count, Hand.GetComponent<Player_Attack>().Arrow.InvCell);

        int sheild;
        if(Hand2.GetComponent<Player_Sheild>().Sheild != null)
            sheild = Hand2.GetComponent<Player_Sheild>().Sheild.GetComponent<SheildIDInHand>().Number;
        else 
            sheild = 0;
        int Lsheild;
        if(Hand2.GetComponent<Player_Sheild>().LastSheild != null)
            Lsheild = Hand2.GetComponent<Player_Sheild>().LastSheild.GetComponent<SheildIDInHand>().Number;
        else 
            Lsheild = 0;

        float hp = player.GetComponent<PlayerStats>().HP_Count;
        float stam = player.GetComponent<PlayerStats>().Stamina_Count;
        byte lvl = player.GetComponent<PlayerStats>().Lvl;
        byte score = player.GetComponent<PlayerStats>().Score;
        short exp = player.GetComponent<PlayerStats>().EXP;
                                                                                               
        Inventory_Item.Clear();  
        Inventory_Quest.Clear();   
        Inventory_Equip.Clear();                                                                                              //Инвентарь
        for(int i = 0; i < HandColInventory.cellContainerItem.transform.childCount; i++)
        {
            int id_ = HandColInventory.item[i].id;
            byte count_ = HandColInventory.item[i].countItem;                            //Инвентарь

            Inventory_Item.Add(new Item_(id_, count_));
        }
        for(int i = 0; i < HandColInventory.cellContainerQuest.transform.childCount; i++)
        {
            int id_ = HandColInventory.quest[i].id;
            byte count_ = HandColInventory.quest[i].countItem;                            //Инвентарь

            Inventory_Quest.Add(new Item_(id_, count_));
        }
        for(int i = 0; i < HandColInventory.cellContainerEquip.transform.childCount; i++)
        {
            int id_ = HandColInventory.equip[i].id;
            byte count_ = HandColInventory.equip[i].countItem;                            //Инвентарь

            Inventory_Equip.Add(new Item_(id_, count_));
        }

        int money = player.transform.GetChild(0).transform.GetChild(0).GetComponent<ShopVisiable>().Money;
        int armInf = Hand.GetComponent<Player_Defence>().Inform;
        int armID = Hand.GetComponent<Player_Defence>().ArmID;

        SavePl = new PlayerSaveData(pos, rot, Inventory_Item, Inventory_Quest, Inventory_Equip, weap, typeAttack, sheild, Lsheild, hp, stam, lvl, score, exp, equipQuest, equipEquip, arrow, money, armInf, armID);
    }

    public List<EnemySaveData> EnemiesDataCube = new List<EnemySaveData>();           //Лист обьектов враг для сохранения      
    /*public void SaveEnemies(List<GameObject> enemies)                             //Обновление листа с новыми показателями врага
    {
        foreach (var go in enemies)
        {
            var em = go.GetComponent<Cube_AI>();                                 
            Vec3 pos = new Vec3(go.transform.position.x, go.transform.position.y, go.transform.position.z);
            float Hp = em.HP;
            bool Die = em.die;
            EnemiesDataCube.Add(new EnemySaveData(pos, Hp, Die));                   //Обновление листа с новыми показателями врага
        }
    }*/
    
    [System.Serializable]
    public struct PlayerSkillsData
    {
        public List<float> PlayerStats;
        public List<byte> SkillsInterationWarrior;
        public List<byte> SkillsInterationMagic;
        public List<byte> SkillsInterationOther;
        public List<byte> SkillsInterationCraft;

        public PlayerSkillsData (List<float> stats, List<byte> skillsW, List<byte> skillsM, List<byte> skillsO, List<byte> skillsC)
        {
            PlayerStats = stats;
            SkillsInterationWarrior = skillsW; SkillsInterationMagic = skillsM; SkillsInterationOther = skillsO; SkillsInterationCraft = skillsC;
        }
    } 

    public List<float> stats = new List<float>();
    public List<byte> skillsW = new List<byte>();
    public List<byte> skillsM = new List<byte>();
    public List<byte> skillsO = new List<byte>();
    public List<byte> skillsC = new List<byte>();

    public PlayerSkillsData plSkills;

    public void PlayerSkills(GameObject player, GameObject SkillsMain)
    {
        Skill_Indicator player_Stats = player.GetComponent<Skill_Indicator>();
        /*stats.Add(player_Stats.SwordDefence); stats.Add(player_Stats.SheildDefence); stats.Add(player_Stats.SheildBlockStamina); stats.Add(player_Stats.SwordBlockStamina); stats.Add(player_Stats.StaminaHeavyAttack); stats.Add(player_Stats.StaminaCommonAttack); stats.Add(player_Stats.Damage); stats.Add(player_Stats.SwordDamage); stats.Add(player_Stats.AxeDamage); stats.Add(player_Stats.MaceDamage); stats.Add(player_Stats.BowDamage); stats.Add(player_Stats.ArrowFly); stats.Add(player_Stats.CommonAttackDamage); stats.Add(player_Stats.HeavyAttackDamage); stats.Add(player_Stats.ExpFactor);
        stats.Add(player_Stats.bleendingTime); stats.Add(player_Stats.bleendingChance); stats.Add(player_Stats.bleendingDamage);
        stats.Add(player_Stats.RegenStamina); stats.Add(player_Stats.RegenHeal); stats.Add(player_Stats.StaminaRebound); stats.Add(player_Stats.RunSpeed); stats.Add(player_Stats.EatTime); stats.Add(player_Stats.PotionTime); stats.Add(player_Stats.Oratory); stats.Add(player_Stats.CostFactor); stats.Add(player_Stats.Dispersion); 
        stats.Add(player_Stats.ManaFactor); stats.Add(player_Stats.ManaHeal); stats.Add(player_Stats.ManaAttack); stats.Add(player_Stats.SpellDamage); stats.Add(player_Stats.SpellHeal); stats.Add(player_Stats.FactorEnergyHeal);
        stats.Add(player_Stats.poison); stats.Add(player_Stats.poisonAttack); stats.Add(player_Stats.Range); stats.Add(player_Stats.FactorManaRegen); stats.Add(player_Stats.SpellMagic); stats.Add(player_Stats.ManaMagic);
        stats.Add(player_Stats.AlchLvl); stats.Add(player_Stats.AccessSkill); stats.Add(player_Stats.ArmFactor); stats.Add(player_Stats.LArmFactor); stats.Add(player_Stats.HArmFactor);
        stats.Add(player_Stats.MArmFactor); stats.Add(player_Stats.LStaminaFactor); stats.Add(player_Stats.HStaminaFactor); stats.Add(player_Stats.MStaminaFactor); stats.Add(player_Stats.LightArmorStaminaPLUS);
        stats.Add(player_Stats.HeavyArmorSpeedMINUS); stats.Add(player_Stats.LVL_UP); stats.Add(player_Stats.Herbalism); stats.Add(player_Stats.x2Damage);
*/
        Transform ski11 = SkillsMain.transform.GetChild(0); 
        for(byte i = 1; i< SkillsMain.transform.GetChild(0).childCount; i++)         //c 1 тк 0-линии
        {
            skillsW.Add (ski11.GetChild(i).GetComponent<Skill>().stageActive);
        }

        ski11 = SkillsMain.transform.GetChild(1); 
        for(byte i = 1; i< SkillsMain.transform.GetChild(1).childCount; i++)
        {
            skillsM.Add (ski11.GetChild(i).GetComponent<Skill>().stageActive);
        }

        ski11 = SkillsMain.transform.GetChild(2); 
        for(byte i = 1; i< SkillsMain.transform.GetChild(2).childCount; i++)
        {
            skillsO.Add (ski11.GetChild(i).GetComponent<Skill>().stageActive);
        }

        ski11 = SkillsMain.transform.GetChild(3); 
        for(byte i = 0; i< SkillsMain.transform.GetChild(3).childCount; i++)
        {
            skillsC.Add (ski11.GetChild(i).GetComponent<Skill>().stageActive);
        }

        plSkills = new PlayerSkillsData(stats, skillsW, skillsM, skillsO, skillsC);
    }
}   