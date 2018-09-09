using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Player : MonoBehaviour {

    public static Player instance;
    [Header("Скорость движения")]
    [SerializeField]
    private float _speedX;
    private float _moveX = 1.0f;
    private bool _isRun;
    public bool isRun
    {
        get { return _isRun; }
    }
    private float _defaultSpeed;
    [Header("Сила прыжка")]
    [SerializeField]
    private float _speedY;
    [SerializeField]
    [Header("Скорость движения в прыжке")]
    private float _speedX_fly;
    [SerializeField]
    [Header("Скорость спуска по стене")]
    private float _speedY_Climb;
    /* Проверка прыжка
     * true - стоит на земле
     * false - находится в полете
    */
    private bool _isGround;
    public bool isGround
    {
        get { return _isGround; }
    }
    [SerializeField]
    private bool _isClimb;
    public bool isClimb
    {
        get { return _isClimb; }
    }
    private Rigidbody2D _rBody { get; set; }
    public Rigidbody2D rBody {
        get { return _rBody; }
    }
    private Animator _anim;
    public Animator Anim
    {
        get { return _anim; }
    }
    /* Направление взгляда
     * true - смотрит вправо
     * false - смотрит влево
    */
    private bool _isFacingRight = true;
    public bool isFacingRight
    {
        get { return _isFacingRight; }
    }
    /* Крадется ли игрок?
     * true - да
     * false - нет
    */
    private bool _isCrouch = false;
    public bool isCrouch
    {
        get { return _isCrouch; }
    }
    private int _attackmode = 0;
    private bool _doAttack = true;
    private float _lastAttack = 0.0f;
    [Header("Время, в течении которого нужно нажать снова удар")]
    [SerializeField]
    private float _nextAttack = 2.0f;
    [Header("Задержка после всех атак, либо после простоя после первой")]
    [SerializeField]
    private float _cooldownAttack = 2.0f;
    [SerializeField]
    [Header("Замедление движения при атаке")]
    private float[] _slowdownSpeed;
    /* ---------------------------- */
    /* Список всех коллайдеров */
    public Collider2D[] ColliderList;
    /* ---------------------------- */
    private Vector3 _wallPosition;

    private void Start () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        _rBody = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _isGround = true;
        _isCrouch = false;
        _doAttack = true;
        _isClimb = false;
        _isRun = false;
        _defaultSpeed = _speedX;
        _lastAttack = 0.0f;
        _moveX = 0.0f;

        Anim.SetBool("MoveY", isGround);
    }
    private void Update()
    {
        if (Input.GetKey(InputManager.KeyBindings["CrouchKey"]))
        {
            Crouch(true);
        }
        else if(Input.GetKeyUp(InputManager.KeyBindings["CrouchKey"]))
        {
            Crouch(false);
        }

        if (Input.GetKeyUp(InputManager.KeyBindings["RightKey"]) || Input.GetKeyUp(InputManager.KeyBindings["LeftKey"]))
        {
            Move(0f);
        }
        if (Input.GetKey(InputManager.KeyBindings["RightKey"]))
        {
            Move(1f);
        }
        else if (Input.GetKey(InputManager.KeyBindings["LeftKey"]))
        {
            Move(-1f);
        }

        float lAttack = Time.time - _lastAttack;
        if (_attackmode > 0 && lAttack >= _nextAttack)
        {
            _attackmode = 0;
            Anim.SetInteger("Attack", _attackmode);
            _doAttack = false;
            StartCoroutine(CanAttack());
        }
        if (Input.GetKeyDown(InputManager.KeyBindings["AttackKey"]) && _doAttack)
        {
            if (_attackmode == 0)
                Attack(1);
            else
                Attack(2);
        }

    }
    private void Move(float direction)
    {
        if(direction == 0f)
        {
            _isRun = false;
            _moveX = 0.0f;
            Anim.SetFloat("MoveX", Mathf.Abs(_moveX));
            return;
        }

        if (_isClimb)
            return;

        if (direction == _moveX)
            return;

        _isRun = true;
        _moveX = direction;
        Anim.SetFloat("MoveX", Mathf.Abs(_moveX));
    }
    private void FixedUpdate()
    {
        if (isCrouch)
            return;

        float moveY = Jump();

        if (!isGround)
        {
            _rBody.velocity = new Vector2(_moveX * _speedX_fly, moveY);
        }
        else if (_isRun)
        {
            _rBody.velocity = new Vector2(_moveX * _speedX, moveY);
        }

        if (_moveX > 0 && !_isFacingRight)
            Flip();
        else if (_moveX < 0 && _isFacingRight)
            Flip();
    }
    private void Crouch(bool State)
    {
        if (!_isGround)
            return;

        if (State == _isCrouch)
            return;

        _isCrouch = State;
        Anim.SetBool("Crouch", _isCrouch);
        SetColliderData(3, _isCrouch);
    }
    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private float Jump()
    {
        if(_isClimb)
        {
            float fall = _rBody.velocity.y * _speedY_Climb;
            return fall;
        }

        if(!isGround)
            return _rBody.velocity.y;

        if (Input.GetKey(InputManager.KeyBindings["JumpKey"]))
        {
            _isGround = false;
            Anim.SetBool("MoveY", isGround);
            SetColliderData(2, true);
            _speedX = _speedX_fly;
            return _speedY;
        }

        return _rBody.velocity.y;
    }
    private void Attack(int Mode)
    {
        _lastAttack = Time.time;
        _speedX = _slowdownSpeed[Mode - 1];
        _attackmode = Mode;
        _doAttack = false;
        Anim.SetInteger("Attack", _attackmode);
    }
    private void EndAttack()
    {
        if(_attackmode == 1)
            _doAttack = true;

        _speedX = _defaultSpeed;
        Anim.SetInteger("Attack", 0);
    }
    private IEnumerator CanAttack()
    {
        yield return new WaitForSeconds(_cooldownAttack);
        _doAttack = true;
    }
    /* Метод для сколжения по стене
     * @arg1: Data 
     * 0 - cброс
     * 1 - вкл.скольжение
    */
    private void Climb(int Data)
    {
        if(Data == 0)
        {
            _isClimb = false;
            Anim.SetBool("Climb", _isClimb);
            SetColliderData(2, false);
            return;
        }

        if (_isClimb)
            return;

        _isClimb = true;
        SetColliderData(2, true);
        Anim.SetBool("Climb", _isClimb);
        
    }
    /* Метод установки коллайдеров
     * @arg1: AnimState
     * 1 - состояние idle/Run
     * 2 - состояние Climb/Jump
     * 3 - состояние Crouch
     * @arg2: State
     * true - вкл
     * false - выкл
    */
    private void SetColliderData(int AnimState, bool State)
    {
        switch (AnimState)
        {
            case 1:
                {
                    ColliderList[0].enabled = State;
                    break;
                }
            case 2:
                {
                    ColliderList[1].enabled = State;

                    if (State == ColliderList[0].enabled)
                        ColliderList[0].enabled = !State;

                    break;
                }
            case 3:
                {
                    ColliderList[2].enabled = State;
                    ColliderList[0].enabled = !State;
                    break;
                }
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        GameObject touch = col.gameObject;

        if (!isGround)
        {
            if (touch.tag == "Floor")
            {
                _isGround = true;
                Climb(0);
                _speedX = _defaultSpeed;
                Anim.SetBool("MoveY", isGround);
                SetColliderData(2, false);
            }
        }
        if(touch.tag == "Wall")
        {
            _wallPosition = touch.transform.position;
        }
        
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        GameObject touch = col.gameObject;

        if (touch.tag == "Wall")
        {
            if(_isClimb)
                Climb(0);
        }
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        GameObject touch = col.gameObject;

        if (touch.tag == "Wall")
        {
            if (!_isGround)
            {
                Vector3 direction = GetDirection(_wallPosition, transform.position);
                if((direction.x >= 0 && Input.GetKey(InputManager.KeyBindings["RightKey"])) || (direction.x <= 0 && Input.GetKey(InputManager.KeyBindings["LeftKey"])))
                {
                    Climb(1);
                }
                else
                {
                    Climb(0);
                }
            }
        }
    }
    private Vector3 GetDirection(Vector3 target, Vector3 start)
    {
        Vector3 heading = target - start;
        float distance = heading.magnitude;
        return heading / distance;
    }  
}
