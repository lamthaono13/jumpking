using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.CanMoveObject())
        {
            float deltaX = -_speed * Time.deltaTime;
            transform.position += new Vector3(deltaX, 0);
        }

    }

    private void SetUp()
    {
        float y = Random.Range(-5, 5);
        transform.localPosition = new Vector3(transform.localPosition.x, y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CheckObstacle"))
        {
            LevelManager.Instance.ObjectSystem.OnSqawnNew(this);
        }
    }

}
