using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public event System.Action OnReachedFinishPoint;
    public static event System.Action OnGuardHasSpottedPlayer;
    public float moveSpeed = 7;
    float storeSpeed;
    public float turnSpeed = 8;
    float inputMagintude;
    public float smoothMoveTime = 0.1f;
    public float lives = 6;
    public float maxLives;
    float freezeTime;
    public float freezeSpeed = 4;
    public ParticleSystem explo;
    public ParticleSystem ice;
    public HealthBar healthBar;
    public TextMeshProUGUI liveText;

    float smoothMoveVelocity;
    float angle;
    float smoothInputMag;
    Vector3 velocity;
    public Transform player;
    public Camera camera;

    Rigidbody rigidbody;
    bool disabled;
    Color originalColor;

    void Start()
    {
        storeSpeed = moveSpeed;
        maxLives = lives;
        rigidbody = GetComponent<Rigidbody>();
        Guard.OnGuardHasSpottedPlayer += Disable;
        var renderer = player.GetComponent<Renderer>();
        originalColor = renderer.material.color;
        UpdateLives(lives);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLives(lives);
        Vector3 inputDirection = Vector3.zero;
        if (!disabled)
        {
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float inputMagintude = inputDirection.magnitude;
            smoothInputMag = Mathf.SmoothDamp(smoothInputMag, inputMagintude, ref smoothMoveVelocity, smoothMoveTime);
            angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagintude);
            velocity = transform.forward * moveSpeed * smoothInputMag;
        if (lives <= 0)
        {
            disabled = true;
            if (OnGuardHasSpottedPlayer != null)
            {
                OnGuardHasSpottedPlayer();
            }
            Destroy(player.gameObject);
        }
        if (lives <= 1)
        {
            var renderer = player.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.red);
        }
        if (moveSpeed < 10 && Time.time >= (freezeTime + 5))
        {
            moveSpeed = storeSpeed;
            var renderer = player.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", originalColor);
        }
        if (transform.position.x >= 30f && transform.position.z >= 0)
        {
            camera.transform.position = new Vector3(40, 40, 25);
        }
        else if (transform.position.x >= 30 && transform.position.z < 0)
        {
            camera.transform.position = new Vector3(40, 40, -25);
        }
        else
        {
            camera.transform.position = new Vector3(0, 40, 4);
        }
        UpdateLives(lives);
    }

    void OnTriggerEnter(Collider hitCollider)
    {
        if (hitCollider.tag == "Finish")
        {
            Disable();
            if (OnReachedFinishPoint != null)
            {
                OnReachedFinishPoint();
            }
        }
        if (hitCollider.tag == ("Enemy"))
        {
            hitCollider.gameObject.SetActive(false);
        }
        if (hitCollider.tag == ("Bullet"))
        {
            Destroy(hitCollider.gameObject);
            lives -= 1;
            explo.Play();
            healthBar.SetHealth((int)lives);
        }
        if (hitCollider.tag == ("Freeze"))
        {
            freezeTime = Time.time;
            Destroy(hitCollider.gameObject);
            moveSpeed = freezeSpeed;
            var renderer = player.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.blue);
            lives -= 0.25f;
            ice.Play();
            healthBar.SetHealth((int)lives);
        }
        if (hitCollider.tag == ("ForceField"))
        {
            freezeTime = (Time.time-2);
            moveSpeed = freezeSpeed;
            var renderer = player.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.blue);
            ice.Play();
            transform.Translate(Vector3.back*2);
        }
        UpdateLives(lives);
    }

    void Disable()
    {
        disabled = true;
    }

    void FixedUpdate()
    {
        rigidbody.MoveRotation(Quaternion.Euler(Vector3.up*angle));
        rigidbody.MovePosition(rigidbody.position + (velocity * Time.deltaTime));
    }

    void OnDestroy()
    {
        Guard.OnGuardHasSpottedPlayer -= Disable;
    }
    void UpdateLives(float lives)
    {
        liveText.text = "Lives: " + lives;
    }
}
