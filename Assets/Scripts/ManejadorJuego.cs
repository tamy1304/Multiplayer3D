using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;

public class ManejadorJuego : MonoBehaviourPun
{
    [Header("Jugadores")]
    [SerializeField] private string jugadorPrefab;
    [SerializeField] private Controlador[] jugadores;
    [SerializeField] private Transform[] posiciones;
    [SerializeField] private int jugadoresVivos;

    public void SetJugador(Controlador jugador, int posicion)
    {
        jugadores[posicion] = jugador;

    }
    private int jugadoresEnJuego;
    public static ManejadorJuego instancia;


    private void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        jugadores = new Controlador[PhotonNetwork.PlayerList.Length];
        jugadoresVivos = jugadores.Length;
        photonView.RPC("JugadorEnJuego", RpcTarget.AllBuffered);

    }

    [PunRPC]
    void JugadorEnJuego()
    {
        jugadoresEnJuego++;
        if (PhotonNetwork.IsMasterClient &&
            jugadoresEnJuego == PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC("ColocarJugador", RpcTarget.All);
        }


    }

    [PunRPC]
    void ColocarJugador()
    {
        GameObject jugadorObj =
            PhotonNetwork.Instantiate(jugadorPrefab,
            posiciones[Random.Range(0, posiciones.Length)].position,
            Quaternion.identity);
        jugadorObj.GetComponent<Controlador>().photonView.RPC("Inicializar", RpcTarget.All, PhotonNetwork.LocalPlayer);


    }


}

