using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerData _data;
    [Header("Player")]
    public Animator _anim;
    bool podeAtacar = true, podePular = true;
    Rigidbody2D _rb2D;
    float direcao = 1;

    void Start()
    {
        _data = GetComponent<PlayerData>();
        _rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Mover(Input.GetButton("Horizontal"));
        Atacar(Input.GetKeyDown(KeyCode.J));
        Pulo(Input.GetKeyDown(KeyCode.Space));
    }

    private void Pulo(bool pulou)
    {
        if (pulou && podePular)
        {
            _anim.SetBool("pulando", true);
            _anim.SetBool("andando", false);
            podePular = false;
            StartCoroutine(pular());
        }
    }

    IEnumerator pular()
    {
        yield return new WaitForSeconds(0.35f);
        _rb2D.AddForce(Vector2.up * 500);
        yield return new WaitForSeconds(0.4f);
        podePular = true;
    }

    private void Atacar(bool atacou)
    {
        RaycastHit2D hitInfo = ray();
        Debug.DrawLine(transform.position, new Vector2(direcao, 0));
        if (atacou)
            if (podeAtacar)
            {
                _anim.SetBool("ataque", true);
                podeAtacar = false;
                if (hitInfo)
                    if (hitInfo.collider.tag == "inimigo")
                    {
                        Debug.Log("Hit");
                    }
                StartCoroutine(ataque());
            }
            else
                _anim.SetBool("ataque", false);
    }

    public RaycastHit2D ray()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, new Vector2(direcao, 0), 5);
        return ray;
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
        if (collision.transform.tag == "chao")
        {
            _anim.SetBool("caiu", false);
            podePular = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "chao")
        {
            podePular = false;
        }
    }
}
