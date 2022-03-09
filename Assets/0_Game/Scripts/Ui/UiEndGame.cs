using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiEndGame : MonoBehaviour
{
    [SerializeField]
    private Button _btnRestart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _btnRestart.onClick.AddListener(OnClickBtnRestart);
    }

    private void OnClickBtnRestart()
    {
        LevelManager.Instance.OnReMain();
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
