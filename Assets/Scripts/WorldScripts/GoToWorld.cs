using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


///The GoToWorld class allows a gameobject with a collider trigger to send the player to another scene (Level/World).
public class GoToWorld : MonoBehaviour
{

    private DialogueManager _dialogueManagerScript;
    public bool CanStartDialogue;
    public int AmmoForThisWorld;
    public float HealthForThisWorld;
    private PlayerHealth _playerHealth;
    private CheckPointManager _checkpointManager;


    [Header("Which World")]
    public Dialogue Dialogues;
    public bool Hub;
    public bool World1;
    public bool World2;
    public bool World3;

    private string _checkpointCount = "// 0";
    private int _checkpointCountIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        _dialogueManagerScript = GameObject.Find("Dialogue_Manager").GetComponent<DialogueManager>();
        CanStartDialogue = false;
        _checkpointManager = Object.FindObjectOfType<CheckPointManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanStartDialogue)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                GoToLastCheckpoint();
            }
            if (Input.GetKeyDown(KeyCode.Alpha0) && Hub == false)
            {
                GoToSpesificCheckpoint(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) && Hub == false)
            {
                GoToSpesificCheckpoint(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && Hub == false)
            {
                GoToSpesificCheckpoint(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && Hub == false)
            {
                GoToSpesificCheckpoint(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && Hub == false)
            {
                GoToSpesificCheckpoint(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) && Hub == false)
            {
                GoToSpesificCheckpoint(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6) && Hub == false)
            {
                GoToSpesificCheckpoint(6);
            }

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _dialogueManagerScript.ShowInteractButton = true;
            CanStartDialogue = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _dialogueManagerScript.ShowInteractButton = false;
            CanStartDialogue = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && Hub == false)
        {
            if (_checkpointCountIndex < _checkpointManager.W1Scenes.Count - 1)
            {
                _checkpointCountIndex += 1;
                _checkpointCount += " // " + _checkpointCountIndex.ToString();
            }
            Dialogues.SentencesToSpeak[1] = _checkpointCount.ToString();
        }
    }

    private void GoToLastCheckpoint()
    {
        if (World1)
        {
            string _checkpointToLoad = _checkpointManager.W1Scenes[_checkpointManager.W1Scenes.Count - 1];
            SceneManager.LoadScene(_checkpointToLoad);
        }
        if (World2)
        {
            string _checkpointToLoad = _checkpointManager.W1Scenes[_checkpointManager.W1Scenes.Count - 1];
            SceneManager.LoadScene(_checkpointToLoad);
        }
        if (World3)
        {
            string _checkpointToLoad = _checkpointManager.W1Scenes[_checkpointManager.W1Scenes.Count - 1];
            SceneManager.LoadScene(_checkpointToLoad);
        }
        if (Hub)
        {
            SceneManager.LoadScene("The_Hub");
        }
        _movePlayer();
    }

    private void GoToSpesificCheckpoint(int _sceneIndex)
    {
        if (World1 && _checkpointManager.W1Scenes.Count - 1 >= _sceneIndex)
        {
            string _sceneToLoad = _checkpointManager.W1Scenes[_sceneIndex].ToString();
            string _checkpointToLoad = _sceneToLoad;
            SceneManager.LoadScene(_checkpointToLoad);
            _movePlayer();
        }
        if (World2)
        {
            string _sceneToLoad = _checkpointManager.W2Scenes[_sceneIndex].ToString();
            string _checkpointToLoad = _sceneToLoad;
            SceneManager.LoadScene(_checkpointToLoad);
            _movePlayer();
        }
        if (World3)
        {
            string _sceneToLoad = _checkpointManager.W3Scenes[_sceneIndex].ToString();
            string _checkpointToLoad = _sceneToLoad;
            SceneManager.LoadScene(_checkpointToLoad);
            _movePlayer();
        }
        if (Hub)
        {
            SceneManager.LoadScene("The_Hub");
        }
    }

    void _movePlayer()
    {
        _dialogueManagerScript.ShowInteractButton = false;
        _dialogueManagerScript.EndDialogue();
    }
}
