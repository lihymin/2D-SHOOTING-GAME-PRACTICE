using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int enemyScore;
    public float speed;
    public int health;
    public bool isBoss;
    public Sprite[] sprites;
    public float maxShotDelay;
    public float curShotDelay;
    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject itemCoin;
    public GameObject itemPower;
    public GameObject itemBoom;
    public GameObject player;
    public GameManager manager;
    public ObjectManager objectManager;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;    

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (enemyName == "B") {
            anim = GetComponent<Animator>();
        }
    }

    void OnEnable()
    {
        switch (enemyName)
        {
            case "L":
                health = 30;
                break;
            case "M":
                health = 12;
                break;
            case "S":
                health = 5;
                break;
            case "B":
                if (isBoss) {
                    health = 5000;
                    Invoke("Stop", 3);
                }
                isBoss = true;
                break;
        }
    }

    void Update()
    {
        if (enemyName == "B") {
            return;
        }
        Fire();
        Reload();
    }
    public void OnHit(int damage)
    {
        if (health <= 0) {
            return;
        }
        health -= damage;

        if (enemyName == "B") {
            anim.SetTrigger("OnHit");
        }

        else {
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }

        if (health <= 0) {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;
            int random = enemyName == "B" ? 0 : Random.Range(0, 10);
            if (random == 4 || random == 5) {
                Instantiate(itemCoin, transform.position, itemCoin.transform.rotation);
            }
            else if (random == 6 || random == 7) {
                Instantiate(itemPower, transform.position, itemCoin.transform.rotation);
            }
            else if (random == 8 || random == 9) {
                Instantiate(itemBoom, transform.position, itemCoin.transform.rotation);
            }
            gameObject.SetActive(false);
            CancelInvoke("Think");
            CancelInvoke("FireForward");
            CancelInvoke("FireShot");
            CancelInvoke("FireArc");
            CancelInvoke("FireAround");
            transform.rotation = Quaternion.identity;
            manager.CallExplosion(transform.position, enemyName);

            if (enemyName == "B") {
                manager.StageEnd();
            }
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "BorderBullet" && enemyName != "B") {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }

        else if (collider.gameObject.tag == "PlayerBullet") {
            Bullet bullet = collider.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);

            collider.gameObject.SetActive(false);
        }
    }
    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void Fire()
    {
        if (curShotDelay < maxShotDelay) {
            return;
        }

        if (enemyName == "S") {
            GameObject bullet = objectManager.MakeObj("bulletEnemyA");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);
        }

        else if (enemyName == "L") {
            GameObject bulletR = objectManager.MakeObj("bulletEnemyB");
            GameObject bulletL = objectManager.MakeObj("bulletEnemyB");
            bulletR.transform.position = transform.position + Vector3.right * 0.3f;
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);
            rigidR.AddForce(dirVecR.normalized * 10, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 10, ForceMode2D.Impulse);
        }

        curShotDelay = 0;
    }
    
    void Stop()
    {
        if (!gameObject.activeSelf) {
            return;
        }
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }

    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch (patternIndex) 
        {
            case 0:
                FireForward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }
    void FireForward()
    {
        GameObject bulletR = objectManager.MakeObj("bulletBossA");
        GameObject bulletL = objectManager.MakeObj("bulletBossA");
        GameObject bulletRR = objectManager.MakeObj("bulletBossA");
        GameObject bulletLL = objectManager.MakeObj("bulletBossA");
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;
        bulletLL.transform.position = transform.position + Vector3.left * 0.45f;
        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
        rigidR.AddForce(Vector2.down * 8f, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 8f, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 8f, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8f, ForceMode2D.Impulse);
        
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) {
            Invoke("FireForward", 3.5f);
        }
        else {
            Invoke("Think", 3);
        }
    }
    void FireShot()
    {
        for (int index = 0; index < 5; index++) {
            GameObject bullet = objectManager.MakeObj("bulletBossB");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 randomVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += randomVec;
            rigid.AddForce(dirVec.normalized * 8f, ForceMode2D.Impulse);
        }
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) {
            Invoke("FireShot", 3.5f);
        }
        else {
            Invoke("Think", 3);
        }
    }
    void FireArc()
    {
        GameObject bullet = objectManager.MakeObj("bulletBossA");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dirVec = new Vector2(Mathf.Sin(Mathf.PI * 10 * curPatternCount/maxPatternCount[patternIndex]), -1);
        rigid.AddForce(dirVec.normalized * 8f, ForceMode2D.Impulse);
        
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) {
            Invoke("FireArc", 0.15f);
        }
        else {
            Invoke("Think", 3);
        }
    }
    void FireAround()
    {
        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        for (int index = 0; index < roundNum; index++) 
        {
            GameObject bullet = objectManager.MakeObj("bulletBossB");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Sin(Mathf.PI * 2 * (index + 1)/roundNum), Mathf.Cos(Mathf.PI * 2 * (index + 1)/roundNum));
            rigid.AddForce(dirVec.normalized * 6f, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * index/roundNum;
            bullet.transform.Rotate(rotVec);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex]) {
            Invoke("FireAround", 0.9f);
        }
        else {
            Invoke("Think", 3);
        }
    }
}
