// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParkourGameManager.cs" company="Exit Games GmbH">
//   Part of: Asteroid demo
// </copyright>
// <summary>
//  Game Manager for the Asteroid Demo
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class ParkourGameManager : MonoBehaviourPunCallbacks
{
    public static ParkourGameManager Instance = null;

    public Text InfoText;
    public Camera MainCamera;
    public GameObject PlayerPrefab;
    public GameObject[] Spawnpoints;

    #region UNITY

    void Awake()
    {
        Instance = this;
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        if(pool != null && this.PlayerPrefab != null)
        {
            pool.ResourceCache.Add("Player", PlayerPrefab);
        }
    }

    public override void OnEnable()
    {
        Debug.Log("Enalbed!!!!!");
        base.OnEnable();

        CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
    }

    public void Start()
    {
        //Hashtable props = new Hashtable
        //{
        //};
        //PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        //StartGame();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
    }

    #endregion

    #region COROUTINES

    private IEnumerator EndOfGame(string winner, int score)
    {
        float timer = 5.0f;

        while (timer > 0.0f)
        {
            //InfoText.text = string.Format("Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, timer.ToString("n2"));

            yield return new WaitForEndOfFrame();

            timer -= Time.deltaTime;
        }

        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region PUN CALLBACKS

    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
         
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CheckEndOfGame();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
           
        
    }

    #endregion

        
    // called by OnCountdownTimerIsExpired() when the timer ended
    private void StartGame()
    {
        Debug.Log("StartGame!");

        // on rejoin, we have to figure out if the spaceship exists or not
        // if this is a rejoin (the ship is already network instantiated and will be setup via event) we don't need to call PN.Instantiate


        //float angularStart = (360.0f / PhotonNetwork.CurrentRoom.PlayerCount) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
        //float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
        //float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
        //Vector3 position = new Vector3(x, 0.0f, z);
        
        int index = PhotonNetwork.LocalPlayer.GetPlayerNumber(); Debug.Log(index);
        index = index < 0 ? 0 : index; 
        
        
        Vector3 position = Spawnpoints[index].transform.localPosition;
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        Debug.Log(position);
        PhotonNetwork.Instantiate("Player", position, rotation, 0);

    }

    private bool CheckAllPlayerLoadedLevel()
    {
       
        return true;
    }

    private void CheckEndOfGame()
    {
           
    }

    private void OnCountdownTimerIsExpired()
    {
        Debug.Log("---count down---");
        MainCamera.gameObject.SetActive(false);
        StartGame();
    }
}
