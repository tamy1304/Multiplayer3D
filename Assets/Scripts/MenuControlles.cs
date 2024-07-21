using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;

public class MenuControlles : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [Header("Pantallas")]
    [SerializeField] private GameObject menuPrincipal;
    [SerializeField] private GameObject crearRoomPantalla;
    [SerializeField] private GameObject lobbyPantalla;
    [SerializeField] private GameObject lobbyNavegadorPantalla;

    [Header("Menu Principal")]
    [SerializeField] private Button btnCrearRoom;
    [SerializeField] private Button btnEncontrarRoom;

    [Header("Lobby")]
    [SerializeField] private TextMeshProUGUI txtListaJugadores;
    [SerializeField] private TextMeshProUGUI txtInfoRoom;
    [SerializeField] private Button btnIniciarJuego;

    [Header("Lobby Navegador")]
    [SerializeField] private RectTransform roomContenedor;
    [SerializeField] private GameObject roomElementoPrefab;

    [Header("VFX")]
    [SerializeField] private Button btnIniciarVFX;


    private List<GameObject> roomElementos = new List<GameObject>();
    private List<RoomInfo> listaRooms = new List<RoomInfo>();


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        btnCrearRoom.interactable = false;
        btnEncontrarRoom.interactable = false;

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.CurrentRoom.IsVisible = true;
            PhotonNetwork.CurrentRoom.IsOpen = true;
            Debug.Log($"PhotonNetwork.it NickName = {PhotonNetwork.NickName}");

        }
    }

    void SetPantalla(GameObject screen)
    {
        menuPrincipal.SetActive(false);
        crearRoomPantalla.SetActive(false);
        lobbyPantalla.SetActive(false);
        lobbyNavegadorPantalla.SetActive(false);
        screen.SetActive(true);
        if(screen == lobbyNavegadorPantalla)
        {
            ActualizarLobbyNavegador();
        }            
    }

    public void OnNombreJugadorCambia(TMP_InputField inpJugadorNombre)
    {
        PhotonNetwork.NickName = inpJugadorNombre.text;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        btnCrearRoom.interactable = true;
        btnEncontrarRoom.interactable = true;
    }

    public void OnCrearRoomClicked()
    {
        SetPantalla(crearRoomPantalla);
    }

    public void OnEncontrarRoomClicked()
    {
        SetPantalla(lobbyNavegadorPantalla);
    }
    
    public void onCrearRoomBoton(TMP_InputField nombre)
    {
        Debug.Log("onCrearRoomBoton");
        NetworkManager.instancia.CrearRoom(nombre.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom lobby");

        SetPantalla(lobbyPantalla);
        photonView.RPC("ActualizarLobby", RpcTarget.All);

    }

    [PunRPC]
    void ActualizarLobby()
    {
        btnIniciarJuego.interactable = PhotonNetwork.IsMasterClient;
        txtListaJugadores.text = "";
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            txtListaJugadores.text += p.NickName + "\n";
        }
        txtInfoRoom.text = string.Format(@" <b>Nombre Room: </b>{0}{1}", "\n", PhotonNetwork.CurrentRoom.Name);

    }

    private GameObject CrearRoomBoton()
    {
        GameObject obj = Instantiate(roomElementoPrefab, roomContenedor.transform);
        roomElementos.Add(obj);
        return obj;
    }
    


    void ActualizarLobbyNavegador()
    {
        foreach (GameObject b in roomElementos)
        {
            b.SetActive(false);
        }
            
        for (int x = 0; x < listaRooms.Count; x++){
            GameObject boton = x >= roomElementos.Count ? CrearRoomBoton() : roomElementos[x];
            boton.SetActive(true);

            boton.transform.Find("txtNombreRoom").GetComponent<TextMeshProUGUI>().text = listaRooms[x].Name;
            boton.transform.transform.Find("txtCantidadJugadores").GetComponent<TextMeshProUGUI>().text = listaRooms[x].PlayerCount + "/" + listaRooms[x].MaxPlayers;


            Button b1 = boton.GetComponent<Button>();
            string nombre = listaRooms[x].Name;
            b1.onClick.RemoveAllListeners();
            b1.onClick.AddListener(() => { OnUnirseRoomClicked(nombre); });
        }


    }

    public void OnRegresarClicked()
    {
        SetPantalla(menuPrincipal);
    }

    public void OnRefrescarClicked()
    {
        ActualizarLobbyNavegador();
    }
    

    private void OnUnirseRoomClicked(string nombre)
    {
        NetworkManager.instancia.UnirseRoom(nombre);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        listaRooms = roomList;
    }
    public void SalirRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnLanzarJuegoClicked()
    {
        string nombreEscena = "Juego1";
        NetworkManager.instancia.photonView.RPC("CambiarEscena", RpcTarget.All, nombreEscena);
    }





}



