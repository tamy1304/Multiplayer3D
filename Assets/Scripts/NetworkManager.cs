using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public int maximoJugadores = 10;
    public static NetworkManager instancia;


    private void Awake()
    {
        instancia = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogFormat("Conectados al servidor principal");
        PhotonNetwork.JoinLobby();
    }

    public void CrearRoom(string nombre)
    {
        RoomOptions opciones = new RoomOptions
        {
            MaxPlayers = (byte)maximoJugadores
        };

        PhotonNetwork.CreateRoom(nombre, opciones);
        Debug.Log("onCrearRoomBoton");

    }

    public void UnirseRoom(string nombre)
    {
        PhotonNetwork.JoinRoom(nombre);
    }

    [PunRPC]
    public void CambiarEscena(string escena)
    {
        PhotonNetwork.LoadLevel(escena);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("Menu");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //MenuControlador. .instan.UpdatePlayerInfoText();
        if (PhotonNetwork.IsMasterClient)
        {
            //GameManager. .instance .CheckWinCondition();
        }
    }





}
