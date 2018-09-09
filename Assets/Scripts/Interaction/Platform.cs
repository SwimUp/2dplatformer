using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Platform : MonoBehaviour {

    [SerializeField]
    private Transform[] _wayPoints;
    [SerializeField]
    private float _speed;
    private int _curWaypoint;
    private int _maxWaypoint;

	private void Start () {
        _curWaypoint = 0;
        _maxWaypoint = _wayPoints.Length;
	}

    private void Update () {
        if (transform.position == _wayPoints[_curWaypoint].position)
        {
            _curWaypoint++;

            if (_curWaypoint == _maxWaypoint)
                _curWaypoint = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, _wayPoints[_curWaypoint].position, _speed * Time.deltaTime);
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject touch = col.gameObject;

        if(touch.tag == "Player")
        {
            touch.transform.parent = transform;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        GameObject touch = col.gameObject;

        if (touch.tag == "Player")
        {
            touch.transform.parent = null;
        }
    }
}
