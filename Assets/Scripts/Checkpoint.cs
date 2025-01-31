using System;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool active = false;
    [SerializeField] private bool playerInRange = false;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private ParticleSystem particles2;
    [SerializeField] private GameObject buttonPrompt;
    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;

    private void Start()
    {
        buttonPrompt.SetActive(false);
    }

    public void Activate()
    {
        PlayerScript.Instance.TriggerRest();
        PlayerScript.Instance.starterAssetsInputs.action = false;
        CheckpointUI.Instance.exitButton.onClick.AddListener(Deactivate);
        OnActivated.Invoke();
        buttonPrompt.SetActive(false);
        CheckpointUI.Instance.Show();
    }

    public void Deactivate()
    {
        PlayerScript.Instance.TriggerNotRest();
        PlayerScript.Instance.starterAssetsInputs.action = false;
        CheckpointUI.Instance.exitButton.onClick.RemoveListener(Deactivate);
        OnDeactivated.Invoke();
        CheckpointUI.Instance.Hide();
        buttonPrompt.SetActive(true);
    }

    private void Update()
    {
        if (!playerInRange) return;
        if (PlayerScript.Instance.starterAssetsInputs.action && !active && !PlayerScript.Instance.isSitting)
        {
            Activate();
        }
        else if (PlayerScript.Instance.starterAssetsInputs.action && active && PlayerScript.Instance.isSitting)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            buttonPrompt.SetActive(true);
            particles.Play();
            particles2.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            buttonPrompt.SetActive(false);
            particles.Stop();
            particles2.Stop();
        }
    }
}
