using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerData _data;
    [Header("Player")]
    public Animator _anim;
    [Header("Movimentação")]
    bool podePular = true;
    Rigidbody2D _rb2D;
    float direcao = 1;
    [Header("Ataque")]
    bool podeAtacar = true;
    public GameObject _area;    

    void Start()
    {
        _data = GetComponent<PlayerData>();
        _rb2D = GetComponent<Rigidbody2D>();
        _area = GameObject.Find("arma");
    }

    void Update()
    {
        Mover(Input.GetButton("Horizontal"));
        Atacar(Input.GetKeyDown(KeyCode.J));
        Pulo(Input.GetKeyDown(KeyCode.W));
        if (podePular)
        {
            _anim.SetBool("caiu", false);
        }
    }

    private void Pulo(bool pulou)
    {
        if (pulou && podePular)
        {
            podePular = false;         
            StartCoroutine(pular());
        }
    }

    IEnumerator pular()
    {
        yield return new WaitForSeconds(0.15f);
        _rb2D.AddForce(Vector2.up * 500);
    }

    private void Atacar(bool atacou)
    {
        _area.transform.localPosition = direcao < 0 ? new Vector2(-0.6f, -0.2f) : new Vector2(0,-0.2f);
        if (atacou)
            if (podeAtacar)
            {
                _anim.SetBool("ataque", true);
                podeAtacar = false;
                dano(GameObject.Find("arma"));   
                StartCoroutine(ataque());
            }
            else
                _anim.SetBool("ataque", false);
    }

    void dano(GameObject area)
    {
        Collider2D[] colider = new Collider2D[3];
        area.GetComponent<BoxCollider2D>().OverlapCollider(new ContactFilter2D(), colider);
        foreach (Collider2D colisao in colider)
        {
            if(colisao != null && colisao.gameObject.CompareTag("inimigo"))
            {
                colisao.GetComponent<Inimigo>()._vida -= _data._dano;
            }
        }
    }

    IEnumerator ataque()
    {
        yield return new WaitForSeconds(0.5f);
        _anim.SetBool("ataque", false);
        yield return new WaitForSeconds(_data._velocidadeDeAtaque - 0.5f);
        podeAtacar = true;
    }

    private void Mover(bool mover)
    {
        if (mover)
        {
            _anim.SetBool("andando", Input.GetButton("Horizontal"));
            transform.Translate(Input.GetAxis("Horizontal") * _data._velocidade * Time.deltaTime, 0, 0);
            direcao = Input.GetAxis("Horizontal");
            flip(direcao);
        }
        else
            _anim.SetBool("andando", false);
    }

    private void flip(float direcao)
    {
        if ((direcao > 0 && GetComponent<SpriteRenderer>().flipX == true) ||
            (direcao < 0 && GetComponent<SpriteRenderer>().flipX == false))
        {
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "chao")
        {
            _anim.SetBool("caiu", true);
            _anim.SetBool("pulando", false);
        }        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.transform.tag == "chao")
        podePular = collision.contacts[0].point.y < transform.position.y;
        if (podePular)
        {
            _anim.SetBool("pulando", false);
            _anim.SetBool("caiu", false);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "chao")
        {
            _anim.SetBool("pulando", true);
            _anim.SetBool("andando", false);
            podePular = false;
        }
    }
   
}
