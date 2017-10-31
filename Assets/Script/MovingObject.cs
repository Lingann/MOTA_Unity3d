using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject{
    // 是否开启移动规则，用于外部调用
    //public bool movingRule;
    // 判断移动步数
    private int moveTimer;
    // 判断是否移动完成
    public bool isMoved;
    // 判断是否正在移动
    public bool IsMoving;
    // 限定一个时间基数，用来处理每次移动所需要的时间
    private float moveTime;
    // 保存一个时间基数的倒数
    private float inverseMoveTime;
    // 移动开始位置
    private Vector2 startPosition;
    // 移动目标位置
    private Vector2 targetPosition;
    // 移动对象刚体
    private Rigidbody2D rigidbody2D;
    // 移动对象碰撞器
    private BoxCollider2D ownCollider;
    // 移动对象碰撞检测
    public RaycastHit2D hit;
    // 碰撞物体对象，当碰撞到物体时会返回一个碰撞对象
    public Collider2D collider;
    /// <summary>
    /// 构造函数，引用类型赋值后，赋值后的值指向原来的引用。原来的引用改变后，赋值后的值也同样也会改变
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_rigidbody2D"></param>
    public MoveObject(Rigidbody2D _rigidbody2D, BoxCollider2D _collider, float _moveTime) {
        //movingRule = true;
        moveTimer = 0;
        //transform = _transform;
        rigidbody2D = _rigidbody2D;
        ownCollider = _collider;
        IsMoving = false;
        moveTime = _moveTime;
        inverseMoveTime = 1f / moveTime;
    }


    /// <summary>
    /// 获取目标位置
    /// </summary>
    /// <param name="xDir"></param>
    /// <param name="yDir"></param>
    /// <returns>返回目标位置</returns>
    public Vector2 GetTargetPoition(Vector2 dirVector) {
        // 移动结束位置
        Vector2 endPosition = startPosition + dirVector;
        return endPosition;
    }
    /// <summary>
    /// 移动方法，该移动方法会判断是否到达目标位置
    /// </summary>
    /// <param name="targetPosition"></param>
    private void SmoothMovement(Vector2 targetPosition) {
        //判断当该模的平方是否大于一个趋近于0的数(float.Epsilon)
        if ((rigidbody2D.position - targetPosition) != Vector2.zero) {
            // Vector3.MoveTowards()将一个点从当前位置移动到目标位置,该方法将不断的返回一个点向量，第一个参数是开始位置，第二个是结束为止，第三个是每次移动的最大距离
            Vector2 newPostion = Vector2.MoveTowards(rigidbody2D.position, targetPosition, inverseMoveTime * Time.deltaTime);
            // 将移动对象移动到新位置
            rigidbody2D.MovePosition(newPostion);
        } else {
            moveTimer += 1;
            isMoved = true;
            IsMoving = false;
            startPosition = targetPosition;
        }
    }

    /// <summary>
    /// 碰撞检测，获取一个碰撞检测的开始点和一个目标点。在两点之间发射一条射线，并返回一个碰撞物体
    /// </summary>
    /// <param name="startPoint">碰撞检测射线的原点</param>
    /// <param name="targetPoint">碰撞检测射线的目标点</param>
    /// <returns>Collider2D hit.collider</returns>
    private Collider2D DetectionCollision(Vector2 startPoint,Vector2 targetPoint) {
        // 碰撞检测， 碰撞检测前需要先将自己的碰撞器关掉，否则会检测到自己
        ownCollider.enabled = false;
        // 进行碰撞检测，检测位置是目标移动前的位置 和目标想要到达的目标位置
        hit = Physics2D.Linecast(startPosition, targetPosition);
        // 将自身碰撞器再次打开，否则移动对象不受碰撞器控制
        ownCollider.enabled = true;
        // 如果碰撞检测到有碰撞体，则返回该碰撞体，否则返回null
        if (hit.transform == null ) {
            IsMoving = true;
            return null;
        } else if (hit.collider.isTrigger) {
            IsMoving = true;
            return hit.collider;
        } else {
            // 返回碰撞器的碰撞对象
            return hit.collider;
        }

    }

    /// <summary>
    /// 处理接收的一个二维向量，固定移动判断逻辑
    /// </summary>
    /// <param name="xDir"></param>
    /// <param name="yDir"></param>
    public void Move(Vector2 inputVector,UIState current) {
        collider = null;
        // 如果 移动开启，则进行移动判断
        if (current == UIState.Default) {
            // 每次检测移动时， 设置碰撞体初始值为null
            // 每次移动时，设置isMoved初始值为false
            isMoved = false;
            if (!IsMoving) {
                // 获取移动对象的开始位置
                startPosition = rigidbody2D.position;
                if (inputVector != Vector2.zero) {
                    // 接收外部传来的方向向量
                    Vector2 directionVector = new Vector2((int)inputVector.x, (int)inputVector.y);
                    // 获取目标位置
                    targetPosition = GetTargetPoition(directionVector);
                    // 碰撞检测，获取碰撞体
                    collider = DetectionCollision(startPosition, targetPosition);
                }
            } else {
                // 调用移动方法
                SmoothMovement(targetPosition);
            }
        }
    }
}
