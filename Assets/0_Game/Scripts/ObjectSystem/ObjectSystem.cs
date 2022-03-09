using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSystem : MonoBehaviour
{
    [SerializeField]
    private float _space;

    [SerializeField]
    private Transform _positionSqawn;

    [SerializeField]
    private GameObject _prefabObstacle;

    private List<Obstacle> _listObstacle;

    private int _numberFirstSqawn;
    // Start is called before the first frame update
    void Start()
    {
        _numberFirstSqawn = 4;
        _listObstacle = new List<Obstacle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FirstSqawn()
    {
        if(_listObstacle.Count > 0)
        {
            for (int i = 0; i < _listObstacle.Count; i++)
            {
                Destroy(_listObstacle[i].gameObject);
            }

            _listObstacle.Clear();
        }

        for(int i = 0; i < _numberFirstSqawn; i ++)
        {
            GameObject obj = Instantiate(_prefabObstacle, _positionSqawn);
            Obstacle obstacle = obj.GetComponent<Obstacle>();

            if(_listObstacle.Count > 0)
            {
                float x = _listObstacle[_listObstacle.Count - 1].transform.localPosition.x + _space;

                obj.transform.localPosition = new Vector2(x, obj.transform.localPosition.y);
            }

            _listObstacle.Add(obstacle);
        }
    }

    public void OnMainLobby()
    {
        FirstSqawn();
    }

    public void OnInGame()
    {

    }

    public void OnEndGame()
    {

    }

    public void OnSqawnNew(Obstacle ob)
    {
        _listObstacle.Remove(ob);

        Destroy(ob);

        GameObject obj = Instantiate(_prefabObstacle, _positionSqawn);

        float x = _listObstacle[_listObstacle.Count - 1].transform.localPosition.x + _space;

        obj.transform.localPosition = new Vector2(x, obj.transform.localPosition.y);

        Obstacle obstacle = obj.GetComponent<Obstacle>();

        _listObstacle.Add(obstacle);
    }
}
