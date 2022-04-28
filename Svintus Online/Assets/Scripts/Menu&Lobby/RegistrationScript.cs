using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class RegistrationScript : MonoBehaviourPunCallbacks
{
    public GameObject mainPanel;
    public InputField inputNickName;
    public InputField inputRoomName;
    public Text errorMessage;

    void Start()
    {
        inputNickName.text = PlayerPrefs.GetString("NickName");
    }

    public void CreateRoom()
    {
        ConnectRoom("Create");
    }

    public void JoinRoom()
    {
        ConnectRoom("Join");
    }

    public void ConnectRoom(string type)
    {
        if (!PhotonNetwork.IsConnected)
        {
            StopAllCoroutines();
            StartCoroutine("ShowErrorMessage");
        }
        else if ((inputNickName.text != String.Empty) && (inputRoomName.text != String.Empty))
        {
            PhotonNetwork.NickName = inputNickName.text;
            PlayerPrefs.SetString("NickName", inputNickName.text);
            if (type == "Join")
                PhotonNetwork.JoinRoom(inputRoomName.text);
            if (type == "Create")
            {
                PhotonNetwork.CreateRoom(inputRoomName.text, new RoomOptions() {MaxPlayers = 8}, null);
            }
        }
    }

    private void PrintMessage()
    {
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("RoomLobby");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Back();
        }
    }

    public void Back()
    {
        errorMessage.color = new Color(errorMessage.color.r, errorMessage.color.g, errorMessage.color.b,0);
        mainPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    IEnumerator ShowErrorMessage()
    {
        for (float f = 1f; f >= 0; f -= 0.02f)
        {
            errorMessage.color = new Color(errorMessage.color.r, errorMessage.color.g, errorMessage.color.b,f);
            yield return new WaitForSeconds(0.05f);
        }
    }
}