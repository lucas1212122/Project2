using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public int _vida;
    public int _dano;
    Player _playerScript;
    public GameObject _areaDeDano, _player;
    Animator _anim;
    bool podeAtacar = true, podeAndar = true;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _anim = GetComponent<Animator>();
        _playerScript = _player.GetComponent<Player>();
    }

    void Update()
    {
        if (_vida <= 0)
        {
            Morrer();
        }
        seguir();
        ataque();
    }

    //Ataque//
    void ataque()
    {
        if (Vector2.Distance(transform.position, _player.transform.position) <= 2 && podeAtacar)
        {
            _anim.SetTrigger("ataque");
            _anim.SetBool("andando", false);
            podeAtacar = false;
            podeAndar = false;
            darDano();
            StartCoroutine(tempodeAtaque());
        }
    }

    void darDano()
    {
        _areaDeDano.transform.localPosition = GetComponent<SpriteRenderer>().flipX == true 
            ? new Vector2(-1.86f, 1) : new Vector2(0, 1);
        Collider2D[] colider = new Collider2D[3];
        _areaDeDano.GetComponent<BoxCollider2D>().OverlapCollider(new ContactFilter2D(), colider);
        foreach (Collider2D colisao in colider)
        {
            if (colisao != null && colisao.gameObject.CompareTag("Player"))
            {
                _playerScript = colisao.GetComponent<Player>();
                _playerScript.tomarDano(_dano, true);
            }
        }
    }

    IEnumerator tempodeAtaque()
    {
        yield return new WaitForSeconds(1.5f);
        podeAtacar = true;
        podeAndar = true;
    }
    //////////

    //Segir//
    void seguir()
    {

        if (Vector2.Distance(transform.position, _player.transform.position) < 10
&& Vector2.Distance(transform.position, _player.transform.position) > 1.5f)
        {
            GetComponent<SpriteRenderer>().flipX = _player.transform.position.x < transform.position.x
? true : false;
            if (podeAndar)
            {
                transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, 4 * Time.deltaTime);
                _anim.SetBool("andando", true);
            }
            if (Vector2.Distance(transform.position, _player.transform.position) <= 1.5f)
            {
                _anim.SetBool("ataque", false);
                _anim.SetBool("andando", false);
            }
        }

    }
    /////////

    //Vida//
    private void Morrer()
    {
        Destroy(gameObject);
    }

    public void dano(int dano, bool darDano)
    {
        if (darDano)
        {
            _vida -= dano;
            darDano = false;
        }

    }
    ////////
}
