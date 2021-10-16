using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//タスクが完了する前に同じタスクを繰り返すと意図しない事態が起きやすいので注意

public class GaussBullet : MonoBehaviour
{
    Rigidbody _rb;
    Vector3 velocity;
    Collider _collider;
    [SerializeField] float cannonPower = 5.0f;
    [SerializeField] float effectiveRange = 300.0f;
    [SerializeField] float dissolveTime = 1.0f;
    const float MinReflectAngle = 0.0f;
    [SerializeField] float _maxReflectAngle = 160.0f;

    ObjectPool _objectPool;
    [Header("オブジェクトプールに設定するもの")]
    [SerializeField] GameObject _explodeEffect;
    [SerializeField] int _startExplodeEffectNum;

    Vector3 startPosi = Vector3.zero;
    Quaternion forwardDire;

    bool willDeactive = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _objectPool = GetComponent<ObjectPool>();
        Reset();
    }

    // Start is called before the first frame update
    void Start()
    {
        _objectPool.CreatePool(_explodeEffect, _startExplodeEffectNum);
    }

    private void OnEnable()
    {
        //startPosi = transform.position;
        forwardDire = transform.rotation;
        if (_rb == null) _rb = GetComponent<Rigidbody>();
        Reset();
        MoveFoward(transform.forward, cannonPower);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = _rb.velocity;
        FadeGaussBullet(startPosi, effectiveRange);
    }

    private void Reset()
    {
        willDeactive = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _collider.enabled = true;
    }

    /// <summary>
    /// 発射位置からの距離で消えるようにしている
    /// </summary>
    /// <param name="startBulletPosi"></param>
    /// <param name="maxEffectiveRange"></param>
    void FadeGaussBullet(Vector3 startBulletPosi, float maxEffectiveRange)
    {
        if (!willDeactive && Vector3.Distance(transform.position, startBulletPosi) > maxEffectiveRange)
        {
            willDeactive = true;
            DOVirtual.DelayedCall(dissolveTime, () =>
            {
                gameObject.SetActive(false);
            }, false);
        }
    }

    public void MoveFoward(Vector3 moveVector, float speed)
    {
        _rb.velocity = moveVector * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("hit!");
        var collisionEnemyDeath = collision.gameObject.GetComponent<EnemyDeath>();
        _objectPool.GetObject(_explodeEffect, transform.position, Quaternion.identity); //爆発エフェクト生成

        if (collisionEnemyDeath != null)
        {
            collisionEnemyDeath.OnDead();
            if (collisionEnemyDeath.Penetratable)
            {
                transform.rotation = forwardDire; //貫通させるため角度を変えずにそのままの勢いで進ませる
                MoveFoward(velocity, 1.0f);
                return; //貫通するならば反射の計算処理を行う必要がないのでここでreturn
            }
        }

        Vector3 reflect = Vector3.Reflect(velocity, collision.contacts[0].normal);
        float reflectAngle = Vector3.Angle(velocity, reflect);
        Debug.Log("reflectAngle : " + reflectAngle);

        if (reflectAngle < _maxReflectAngle)
        {
            forwardDire = transform.rotation;
            reflect = reflect.normalized;
            MoveFoward(reflect, cannonPower);
        }
        else
        {
            _collider.enabled = false;
            MoveFoward(Vector3.zero, 0);
            //dissolveTime変数の時間経過後オブジェクトを非アクティブにする
            DOVirtual.DelayedCall(dissolveTime, () =>
            {
                gameObject.SetActive(false);
            }, false);
        }

        //if (collision.transform.CompareTag("Ground"))
        //{
        //    _collider.enabled = false;
        //    MoveFoward(0);
        //    //dissolveTime変数の時間経過後オブジェクトを非アクティブにする
        //    DOVirtual.DelayedCall(dissolveTime, () =>
        //    {
        //        gameObject.SetActive(false);
        //    });
        //}


    }
}
