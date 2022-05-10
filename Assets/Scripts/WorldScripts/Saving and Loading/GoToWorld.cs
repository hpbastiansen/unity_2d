using UnityEngine;
using UnityEngine.SceneManagement;

/// The GoToWorld class allows a gameobject with a collider trigger to send the player to another scene (Level/World).
public class GoToWorld : MonoBehaviour
{
    private DialogueManager _dialogueManagerScript;
    public bool CanStartDialogue;
    public int AmmoForThisWorld;
    public float HealthForThisWorld;
    private CheckPointManager _checkpointManager;
    private UIManager _myUIManager;



    [Header("Which World")]
    public Dialogue Dialogues;
    public bool Hub;
    public bool World1;
    public bool World2;
    public bool World3;
    public bool Tutorial;

    private string _checkpointCount = "// 0";
    private int _checkpointCountIndex = 0;


    /// Called before the first frame.
    void Start()
    {
        _dialogueManagerScript = GameObject.Find("Dialogue_Manager").GetComponent<DialogueManager>();
        CanStartDialogue = false;
        _checkpointManager = FindObjectOfType<CheckPointManager>();
        _myUIManager = FindObjectOfType<UIManager>();

    }

    /// Called every frame.
    /** Listens for KeyDown events, and teleports the player to that specific checkpoint
        if they're standing at the trigger and has that checkpoint unlocked. */
    void Update()
    {
        if (CanStartDialogue && _myUIManager.UsingMainMenu == false)
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

    /// Called every frame a collider is inside the trigger on the gameobject.
    /** While the player is in the trigger, the dialogue is enabled. */
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _dialogueManagerScript.ShowInteractButton = true;
            CanStartDialogue = true;
        }
    }

    /// Called on a collider exiting the trigger on the gameobject.
    /** If the player exits the trigger, the dialogue is disabled. */
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _dialogueManagerScript.ShowInteractButton = false;
            CanStartDialogue = false;
        }
    }

    /// Called on a collider entering the trigger on the gameobject.
    /** When the player enters this collider, the dialogue is updated with the current scenes the player can go to. */
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

    /// This method makes the player go to the scene their current checkpoint is at.
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
        if (Tutorial)
        {
            SceneManager.LoadScene("TUTORIAL");
        }
        _movePlayer();
    }

    /// This method makes the player go to a specific checkpoint in their current world given the sceneIndex of the stage.
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
        if (Tutorial)
        {
            SceneManager.LoadScene("TUTORIAL");
        }
    }

    /// This method stops any dialogue the player is currently in.
    void _movePlayer()
    {
        _dialogueManagerScript.ShowInteractButton = false;
        _dialogueManagerScript.EndDialogue();
    }
}
