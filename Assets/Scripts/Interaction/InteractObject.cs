using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour {

    /* Выставление новой анимации */
    [SerializeField]
    [Header("Анимация простоя")]
    private AnimationClip _idleAnimation;
    public AnimationClip idleAnimation
    {
        get { return _idleAnimation; }
    }
    [SerializeField]
    [Header("Объект анимации")]
    private GameObject _animateObject;
    public GameObject AnimateObject
    {
        get { return _animateObject; }
    }
    private Animator _animator;
    private AnimatorOverrideController animatorOverrideController;
    /* --------------------------------------- */

    /* Взаимодействие */
    [SerializeField]
    [Header("Интерактивен ли предмет?")]
    private bool _isInteract = false;
    public bool isInteract
    {
        get { return _isInteract; }
    }
    [SerializeField]
    [Header("Показывать ли сообщение о взаимодействии?")]
    private bool _isInteractMessage = false;
    public bool isInteractMessage
    {
        get { return _isInteractMessage; }
    }
    [SerializeField]
    [Header("Зона взаимодействия")]
    private Collider2D TriggerZone;
    private bool _isTriggered = false;
    public bool isTriggered
    {
        get { return _isTriggered; }
    }

    public void Start () {
        InitObject();
    }
    public virtual void InitObject()
    {
        if (_idleAnimation != null)
        {
            _animator = _animateObject.GetComponent<Animator>();

            animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = animatorOverrideController;

            animatorOverrideController["idle"] = _idleAnimation;
        }

        _isTriggered = false;
    }
    private void Update()
    {
        if(_isTriggered)
        {
            if(Input.GetKeyDown(InputManager.KeyBindings["UseKey"]))
            {
                onUse();
            }
        }
    }

    /* СОБЫТИЯ */
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(_isInteract)
        {
            _isTriggered = true;
            onInteract(col.gameObject);

            if(_isInteractMessage)
                MessageManager.ShowInteractMessage(true);
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (_isInteract)
        {
            if (_isInteractMessage)
                MessageManager.ShowInteractMessage(false);

            _isTriggered = false;
        }
    }
    /* ВЗАИМОДЕЙСТВИЕ */
    /* onInteract: Когда что-либо задевает тригер зону
     * obj - объект, который задел
    */
    public virtual void onInteract(GameObject obj)
    {
        return;
    }
    /* onUse: Когда игрок взаимодействует */
    public virtual void onUse()
    {
        return;
    }
    /* ----------------------------------- */
}
