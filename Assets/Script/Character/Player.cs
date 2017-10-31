using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;



public class Player : MonoBehaviour {

    #region Data
    public PlayerData Data = new PlayerData();

    public int Levels { get; set; }

    public int Golds { get; set; }

    public int EXP { get; set; }

    public int HP { get; set; }

    public int ATK { get; set; }

    public int DEF { get; set; }

    public int YellowKeys { get; set; }

    public int BlueKeys { get; set; }

    public int RedKeys { get; set; }

    public string PrefabPath { get { return "Prefabs/Player"; } }

    public Vector2 Direction { get { return direction; } set { direction = value; }}

    private Vector2 direction;

    #endregion

    // 当前主角状态
    public static UIState CurrentState;

    // 每次攻击等待时间
    private WaitForSeconds attackWaitTime;
    
    // 碰撞物体的GameObject标签
    private string hitColliderTag;
   
    // 方向键输入的值得到的一个方向向量
    Vector2 inputVector2;
    
    // 引用动画组件
    Animator anim;
    
    // 引用刚体组件
    Rigidbody2D rigidBody;

    // 引用自身碰撞器
    BoxCollider2D ownCollider;

    // 引用MoveObject，MoveObject可以控制移动对象的移动以及进行碰撞检测
    MoveObject moveObject;

    #region MonoBehaviour
    private void Awake() {
        AddEvent();
        GetInitalData();
    }

    // Use this for initialization
    void Start() {
        // 将当前游戏角色状态设置UI状态
        CurrentState = UIState.Default;
        // 设置每次攻击所需时间为0.3秒
        attackWaitTime = new WaitForSeconds(0.3f);
        //doorOpened = false;
        // 获取动画组件，该动画组件主要用于控制对象的移动面朝方向和移动动画
        anim = GetComponent<Animator>();
        // 获取刚体组件，该刚体组件主要用于控制移动对象的位置移动
        rigidBody = GetComponent<Rigidbody2D>();
        // 获取碰撞器组件，该碰撞器组件主要用于控制移动碰撞和碰撞检测
        ownCollider = GetComponent<BoxCollider2D>();
        // 调用移动对象，这里采用创建一个新的移动对象来进行控制。该移动对象将用于控制移动和进行碰撞检测
        moveObject = new MoveObject(rigidBody, ownCollider, 0.1f);


    }

    // Update is called once per frame
    void Update() {
        // 进行移动判断
        moveObject.Move(GetInputVector(), CurrentState);
        // 控制对象的移动面朝方向
        MoveDirection();
        // 当发生碰撞时，执行相应事件
        ExecuteCollisions(moveObject.hit);
        if (UIManager.instance.CurrentState == UIState.Default) {
            CurrentState = UIState.Default;
        }
    }

    private void OnGUI() {
        GUILayout.TextArea("collider:" + ownCollider.isTrigger);
        GUILayout.TextArea("InputVector" + anim.GetFloat("InputX") + "," + anim.GetFloat("InputY"));
    }

    private void OnDestroy() {
        DeleteEvent();
    }

    #endregion MonoBehaviour


    #region GetData
    /// <summary>
    /// 判断是否主角的属性是否足够
    /// </summary>
    /// <param name="property"></param>
    /// <param name="price"></param>
    /// <returns></returns>
    public bool IsEnough(string property, int price = 0) {
        switch (property) {
            case "HP":
                if (HP - price > 0) {
                    return true;
                } else {
                    return false;
                }
            case "Atk":
                if (ATK - price > 0) {
                    return true;
                } else {
                    return false;
                }
            case "Def":
                if (DEF - price > 0) {
                    return true;
                } else {
                    return false;
                }
            case "Gold":
                if (Golds - price > 0) {
                    return true;
                } else {
                    return false;
                }
            case "Exp":
                if (EXP - price > 0) {
                    return true;
                } else {
                    return false;
                }
            case "Levels":
                if (Levels - price > 0) {
                    return true;
                } else {
                    return false;
                }
            case "YellowKey":
                if (YellowKeys - price > 0) {
                    return true;
                } else {
                    return false;
                }
            case "BlueKey":
                if (BlueKeys - price > 0) {
                    return true;
                } else {
                    return false;
                }
            case "RedKey":
                if (RedKeys - price > 0) {
                    return true;
                } else {
                    return false;
                }
        }
        return false;
    }


    public void GetProps(Props props) {
        switch (props.Property) {
            case "黄钥匙":
                YellowKeys = YellowKeys * props.Multiple + props.Num;
                break;
            case "蓝钥匙":
                BlueKeys = BlueKeys * props.Multiple + props.Num;
                break;
            case "红钥匙":
                RedKeys = RedKeys * props.Multiple + props.Num;
                Debug.Log(props.Multiple);
                break;
            case "生命值":
                HP = HP * props.Multiple + props.Num;
                break;
            case "攻击力":
                ATK = ATK * props.Multiple + props.Num;
                Debug.Log(props.Multiple);
                break;
            case "防御力":
                DEF = DEF * props.Multiple + props.Num;
                break;
            case "金币":
                Golds = Golds * props.Multiple + props.Num;
                break;
            case "经验值":
                EXP = EXP * props.Multiple + props.Num; ;
                break;
            case "等级":
                Levels = Levels + props.Num;
                break;
        }
        Destroy(props.gameObject);
    }


    public void ChangeProperty(string property, int num = 0) {
        switch (property) {
            case "HP":
                HP += num;
                break;
            case "Atk":
                ATK += num;
                break;
            case "Def":
                DEF += num;
                break;
            case "Gold":
                Golds += num;
                break;
            case "Exp":
                EXP += num;
                break;
            case "Levels":
                Levels += num;
                break;
            case "YellowKey":
                YellowKeys += num;
                break;
            case "BlueKey":
                BlueKeys += num;
                break;
            case "RedKey":
                BlueKeys += num;
                break;
        }
    }

    /// <summary>
    /// 获取初始配置数据配置信息
    /// </summary>
    private void GetInitalData() {
        string fileName = "InitialData";
        JsonData MonsterData = ResourcesManager.GetJsonData(fileName, "Player");
                HP = (int)MonsterData[0]["HP"];
                ATK = (int)MonsterData[0]["ATK"];
                DEF = (int)MonsterData[0]["DEF"];
                Golds = (int)MonsterData[0]["Golds"];
                YellowKeys = (int)MonsterData[0]["YellowKeys"];
                BlueKeys = (int)MonsterData[0]["BlueKeys"];
                RedKeys = (int)MonsterData[0]["RedKeys"];
    }

    /// <summary>
    /// 监听方向输入，获取一个方向向量
    /// </summary>
    /// <returns>Vector inputVector2</returns>
    private Vector2 GetInputVector() {
        // 获取水平方向的输入
        inputVector2.x = Input.GetAxisRaw("Horizontal");
        // 获取垂直方向的输入
        inputVector2.y = Input.GetAxisRaw("Vertical");
        // 让水平移动方向先行
        if (inputVector2.x != 0) {
            inputVector2.y = 0;
        }
        // 返回一个方向向量
        return inputVector2;
    }

    /// <summary>
    /// 获取当前面朝主角方向
    /// </summary>
    /// <returns></returns>
    private Vector2 GetDirection() {
        direction.x = anim.GetFloat("InputX");
        direction.y = anim.GetFloat("InputY");
        return direction;
    }


    #endregion GetData


    #region Data Operate
    /// <summary>
    /// 存储数据
    /// </summary>
    private void StoreData() {
        Data.PrefabPath = PrefabPath;
        Debug.Log(PrefabPath);
        Data.DirectionX = GetDirection().x;
        Data.DirectionY = GetDirection().y;
        Data.PositionX = transform.position.x;
        Data.PositionY = transform.position.y;
        Data.PositionZ = transform.position.z;
        Data.Levles = Levels;
        Data.Golds = Golds;
        Data.EXP = EXP;
        Data.HP = HP;
        Data.ATK = ATK;
        Data.DEF = DEF;
        Data.YellowKeys = YellowKeys;
        Data.BlueKeys = BlueKeys;
        Data.RedKeys = RedKeys;
    }

    /// <summary>
    /// 加载数据
    /// </summary>
    private void LoadData() {
        transform.position = new Vector3((float)Data.PositionX, (float)Data.PositionY, (float)Data.PositionZ);
        MoveDirection((float)Data.DirectionX, (float)Data.DirectionY);
        Levels = Data.Levles;
        Golds = Data.Golds;
        EXP = Data.EXP;
        HP = Data.HP;
        ATK = Data.ATK;
        DEF = Data.DEF;
        YellowKeys = Data.YellowKeys;
        BlueKeys = Data.BlueKeys;
        RedKeys = Data.RedKeys;
    }

    /// <summary>
    /// 添加数据到数据列表
    /// </summary>
    private void AddData() {
        DataController.dataContainer.Player.Add(Data);
    }

    /// <summary>
    /// 订阅事件
    /// </summary>
    private void AddEvent() {
        DataController.OnStoreData += StoreData;
        DataController.OnAddData += AddData;
        DataController.OnLoaded += LoadData;
    }

    /// <summary>
    /// 取消订阅
    /// </summary>
    private void DeleteEvent() {
        DataController.OnStoreData -= StoreData;
        DataController.OnAddData -= AddData;
        DataController.OnLoaded -= LoadData;
    }
    #endregion Data Operate



    /// <summary>
    /// 设置移动对象的动画移动方向
    /// </summary>
    private void MoveDirection() {
        if (CurrentState == UIState.Default && UIManager.instance.CurrentState == UIState.Default) {
            if (GetInputVector() != Vector2.zero) {
                // 设置移动方向的x轴值
                anim.SetFloat("InputX", inputVector2.x);
                // 设置移动方向的y轴值
                anim.SetFloat("InputY", inputVector2.y);
            }
            // 如果移动对象正在移动，则开启移动动画，否则关闭
            if (moveObject.IsMoving) {
                // 开启移动动画
                anim.SetBool("IsMoving", true);
            } else {
                // 关闭移动动画
                anim.SetBool("IsMoving", false);
            }
        }
    }


    /// <summary>
    /// 设置移动对象的动画移动方向
    /// </summary>
    private void MoveDirection(float x, float y) {
        // 设置移动方向的x轴值
        GetComponent<Animator>().SetFloat("InputX", x);
        // 设置移动方向的y轴值
        GetComponent<Animator>().SetFloat("InputY", y);
    }




    /// <summary>
    /// 当碰撞发生时，执行相应事件
    /// </summary>
    /// <param name="hit">碰撞返回的一个碰撞体</param>
    private void ExecuteCollisions(RaycastHit2D hit) {
        // 当碰撞检测检测到物体时，每次调用开门事件
        if (moveObject.hit.transform != null) {
            // 根据碰撞物体的标签，执行不同的事件
            switch (moveObject.hit.collider.tag) {
                case "Door":
                    // 正在进行碰撞时执行的事件
                    if (moveObject.collider != null) {
                        // 获取门的脚本组件
                        Door door = moveObject.hit.collider.gameObject.GetComponent<Door>();
                        // 调用开门的方法
                        Open(door);
                    }
                    break;
                case "Monster":
                    // 正在进行碰撞时执行的事件
                    if (moveObject.collider != null) {
                        // 获取敌人脚本组件
                        Monster monster = moveObject.hit.collider.gameObject.GetComponent<Monster>();
                        //  获取当前敌人将对我方造成的总伤害值
                        int damagevalue = monster.GetDemageValue(ATK, DEF);
                        // 如果我方生命值高于敌方对我方造成的总伤害，则开启战斗
                        if (IsEnough("HP", damagevalue)&& damagevalue >= 0) {
                            Debug.Log("开始战斗");
                            // 打开战斗界面
                            UIManager.instance.battle.StartBattle(gameObject, moveObject.hit.collider.gameObject);
                            // 设置当前的状态为战斗状态
                            CurrentState = UIState.Battle;
                            // 开始一段攻击协程
                            StartCoroutine(Attack(monster));
                        }
                    }
                    break;
                case "NPC":
                    if (moveObject.collider != null) {
                        // 设置当前状态为对话状态
                        CurrentState = UIState.Dialogue;
                        // 获取NPC脚本组件
                        NPC npc = moveObject.hit.collider.gameObject.GetComponent<NPC>();
                        // 打开对话界面

                        UIManager.instance.dialogue.StartDialogue(npc.NPCName);
                    }
                    // 在对话没有结束的情况下如果按下空格键，键执行下一段对话
                    if (CurrentState == UIState.Dialogue && Input.GetKeyDown("space")) {
                        // 执行下一段对话
                        UIManager.instance.dialogue.CheckNextSentence();
                    }
                    break;
                case "Props":
                    if (moveObject.collider != null) {
                        // 设置当前状态为获取道具的状态
                        CurrentState = UIState.GetProps;
                        // 获取道具的脚本组件
                        Props props = moveObject.hit.collider.gameObject.GetComponent<Props>() as Props;
                        // 执行获取道具的方法
                        GetProps(props);
                        // 显示UI
                        UIManager.instance.remainder.GetProps(props.Reminder());
                    }
                    break;
                case "Stairs":
                    // 获取楼梯脚本组件
                    Stairs stairs = moveObject.hit.collider.gameObject.GetComponent<Stairs>() as Stairs;
                    // 当游戏角色完成移动到楼梯上时，则调用上下楼的方法
                    if (moveObject.isMoved) {
                        // 进行上去或下来一层楼层
                        LayerManager.instance.DownOrUp(this, stairs.stairState);
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 自身的受伤方法，该方法为一个协同程序，每次在调用期间，都会有一个暂停执行的时间
    /// </summary>
    /// <param name="monster">敌人的引用</param>
    /// <returns></returns>
    private IEnumerator Damage(Monster monster) {
        // 当敌人的生命值大于0时
        if(monster.HP > 0) {
            // 怪物每次攻击造成的伤害，（怪物攻击 - 己方防御）
            HP -= (monster.Atk - DEF);
        }
        // 暂停执行 attackWaitTime时间
        yield return attackWaitTime;
        // 调用战斗画面游戏角色的属性值(生命值)
        UIManager.instance.battle.GetHero(this);
        // 开启攻击方法的协程
        StartCoroutine(Attack(monster));
    }

    private IEnumerator Attack(Monster monster) {
        // 攻击次数。总伤害 / （怪物攻击 - 己方防御）
        if (monster.HP > 0) {
            monster.Damage(DEF);
            yield return attackWaitTime;
            UIManager.instance.battle.GetMonster(monster);
            StartCoroutine(Damage(monster));
        } else {
            CurrentState = UIState.Default;
            moveObject.hit.collider.gameObject.SetActive(false);
            Destroy(moveObject.hit.collider.gameObject);
            UIManager.instance.battle.EndBattle();
        }
    }

    /// <summary>
    /// 开门方法
    /// </summary>
    /// <param name="door">门的引用</param>
    private void Open(Door door) {
        if (!door.IsOpening) {
            switch (door.DoorType) {
                case DoorEnum.黄门:
                    if (YellowKeys > 0) {
                        YellowKeys -= 1;
                        door.Open();
                    }
                    break;
                case DoorEnum.蓝门:
                    if (BlueKeys > 0) {
                        BlueKeys -= 1;
                        door.Open();
                    }
                    break;
                case DoorEnum.红门:
                    if (RedKeys > 0) {
                        RedKeys -= 1;
                        door.Open();
                    }
                    break;
            }
        }
    }


}


