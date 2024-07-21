using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador : MonoBehaviourPunCallbacks
{
    [Header("Informacion")]
    private int id;
    private string nombre;

    public float velocidadMov = 5.0f;
    public float velocidadRot = 200.0f;

    public Animator animator;
    public float x, y;

    public Rigidbody rb;
    public float fuerzaSalto = 8f;
    public bool puedeSaltar;

    [SerializeField] private Player photonJugador;

    public int Id { get { return id; } }

    public string Nombre { get { return nombre; } }



    [PunRPC]
    public void Inicializar(Player jugador)
    {
        id = jugador.ActorNumber;
        nombre = jugador.NickName;
        photonJugador = jugador;
        puedeSaltar = false;
        animator = GetComponent<Animator>();

        ManejadorJuego.instancia.SetJugador(this, id - 1);

        if (!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().gameObject.SetActive(false);
            rb.isKinematic = true;
        }
        //else
            //LookAtCamera.instancia.SetCamara(GetComponentInChildren<Camera>());

    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        transform.Rotate(0, x * Time.deltaTime * velocidadRot, 0);
        transform.Translate(0, 0, y * Time.deltaTime * velocidadMov);
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;
        Mover();

        if (puedeSaltar == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                photonView.RPC("Saltar", RpcTarget.AllBuffered);

            }
            photonView.RPC("tocaSuelo", RpcTarget.AllBuffered);
        }
        else
        {
            photonView.RPC("EstaCayendo", RpcTarget.AllBuffered);
        }

    }

    public void Mover()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);        
    }

    [PunRPC]
    public void EstaCayendo()
    {
        animator.SetBool("tocaSuelo", false);
        animator.SetBool("salte", false);
    }

    [PunRPC]
    public void Saltar()
    {
        animator.SetBool("salte", true);
        rb.AddForce(new Vector3(0, fuerzaSalto, 0), ForceMode.Impulse);
    }

    [PunRPC]
    public void tocaSuelo()
    {
        animator.SetBool("tocaSuelo", true);
    }




}
