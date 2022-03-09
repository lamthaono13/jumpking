using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Transform _positionSqawn;

    [SerializeField]
    private GameObject _player;

    private bool _canMove;

    private Rigidbody2D ri;

    [SerializeField]
    private float _force;

    [SerializeField]
    private float _maxY;
    private void Start()
    {
        ri = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
            if (Input.GetMouseButtonDown(0))
            {
                AddForce();
            }

            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                    AddForce();
                        break;
                }
            }
    }

    private void AddForce()
    {
        if (_canMove)
        {
            if(gameObject.transform.position.y <= _maxY)
            {
                Vector2 u = new Vector2(0, _force);
                ri.AddForce(u);
            }
        }
    }

    public void OnMainLobby()
    {
        _canMove = false;
        _player.transform.position = _positionSqawn.position;
        _player.transform.rotation = _positionSqawn.rotation;
        ri.velocity = Vector2.zero;
        ri.isKinematic = true;
    }

    public void OnInGame()
    {
        _canMove = true;
        ri.isKinematic = false;
    }

    public void OnEndGame()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pass"))
        {
            LevelManager.Instance.OnPass();
        }

        if (collision.CompareTag("Obstacle"))
        {
            LevelManager.Instance.OnLose();
        }
    }
}
