using UnityEngine;
using System.Collections.Generic;

public class Test_Audio : MonoBehaviour
{

    bool isRecording = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
        Debug.Log("Premi Space per iniziare la registrazione audio!");
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            
            isRecording = !isRecording;
            if (isRecording) 
            {
                Debug.Log("sto registrando - (Premi Space per interrompere la registrazione)");
                audioSource.clip = Microphone.Start(null, true, 10, 44100);
            }
            else
            {
                Debug.Log("Non sto registrando - Premi Space per iniziare la registrazione");
                Microphone.End(null);
                audioSource.Play();
            }
            
        }
    }
}