using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomList : MonoBehaviour
{
    public Text RoomName;

    public Transform PlayerCount;
    public Button JoinRoomButton;

    // Start is called before the first frame update
    void Start()
    {
        JoinRoomButton.onClick.AddListener(() =>
        {
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }

            PhotonNetwork.JoinRoom(RoomName.text);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(string name, int curPlayer, int maxPlayer)
    {
        RoomName.text = name;
        PlayerCount.GetComponent<RectTransform>().sizeDelta = new Vector2((502 - 50) / (maxPlayer - 1) * (curPlayer - 1), PlayerCount.GetComponent<RectTransform>().sizeDelta.y);
    }
}