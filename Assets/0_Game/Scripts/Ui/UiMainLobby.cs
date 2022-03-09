using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMainLobby : MonoBehaviour
{
    [SerializeField]
    private Button _btnStart;
    // Start is called before the first frame update
    void Start()
    {
        _btnStart.onClick.AddListener(OnClickStart);
    }

    private void OnClickStart()
    {
        LevelManager.Instance.ChangeState(GameState.InGame);
    }

    public void OnMainLobby()
    {

    }

    public void OnIngame()
    {

    }

    public void OnEndGame()
    {

    }
}
