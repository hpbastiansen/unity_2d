using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

/// This script handles the external puzzles in stage 3. 
public class Puzzle1 : MonoBehaviour
{
    public bool Door1Open;
    public Animator Door1Animator;
    public Animator ThreeADoorAnimator;
    public Animator ThreeBDoorAnimator;
    public Animator ThreeCDoorAnimator;
    public Animator FinalDoorAnimator;
    public AudioSource Door1SoundSource;
    public string GamePath;
    public bool GotGeneratorKey;
    public bool GeneratorCanBeStarted;
    public DialogueTrigger OpenDoorDiaTrigger;
    public GameObject AfterOpenDoorDiaTrigger;
    public DialogueTrigger GeneratorDiaTrigger;
    public DialogueTrigger OverrideKeyDiaTrigger;
    public DialogueTrigger FinaleDiaTrigger;
    public GameObject PostFinaleDia;
    public GameObject PreFinaleDia;
    public float GeneratorPowerValue;
    public int AccessOverrideCode;
    private bool _startedOnGenerator;
    public bool IsLastDoorOpen;
    public TargetLights TargetLight1;
    public TargetLights TargetLight2;
    public TargetLights TargetLight3;
    public bool IsBossHackDone;
    public bool FinaleStarted;

    /// Called before the first frame.
    /** In the Start method we set the Game Path and puzzle variables, and clean up any files remaining from previous attempts. */
    void Start()
    {
        Door1Animator.SetBool("IsOpen", Door1Open);
        GamePath = Application.dataPath;
        GamePath += "/../";
        GamePath += "AIS/";
        if (System.IO.File.Exists(GamePath + "RunMe.bat") || System.IO.File.Exists(GamePath + "Success.txt"))
        {
            File.Delete(GamePath + "RunMe.bat");
            File.Delete(GamePath + "Success.txt");
        }
        GeneratorCanBeStarted = true;
        GeneratorPowerValue = 0;
        AccessOverrideCode = Random.Range(134, 999);
        OverrideKeyDiaTrigger.Dialogues.SentencesToSpeak.Add("The Override Key Is: " + AccessOverrideCode.ToString());
        Debug.Log(AccessOverrideCode);
        _startedOnGenerator = false;
        IsBossHackDone = false;
        FinaleStarted = false;
        if (Directory.Exists(GamePath + "WormPosition/"))
        {
            try { Directory.Delete(GamePath + "WormPosition/", true); }
            catch (IOException) { }
        }
        if (Directory.Exists(GamePath + "Generator/"))
        {
            try { Directory.Delete(GamePath + "Generator/", true); }
            catch (IOException) { }
        }
        if (Directory.Exists(GamePath + "Generator/"))
        {
            try
            {
                Directory.Delete(GamePath + "Generator/", true);
            }
            catch (IOException)
            {

            }
        }
    }

    /// Called every frame.
    /** In the Update method we check for the completion of every puzzle in the stage. */
    void Update()
    {
        Door1Animator.SetBool("IsOpen", Door1Open);
        ThreeADoorAnimator.SetBool("IsOpen", GeneratorCanBeStarted);
        ThreeBDoorAnimator.SetBool("IsOpen", GeneratorCanBeStarted);
        ThreeCDoorAnimator.SetBool("IsOpen", GeneratorCanBeStarted);
        FinalDoorAnimator.SetBool("IsOpen", IsLastDoorOpen);

        // If all three lights of the final door puzzle is on, the door opens.
        if (TargetLight1.IsOn == true && TargetLight2.IsOn == true && TargetLight3.IsOn == true)
        {
            IsLastDoorOpen = true;
        }
        else
        {
            IsLastDoorOpen = false;
        }

        // If the 'Success.txt' file contains the word 'Open' we open the first door.
        if (System.IO.File.Exists(GamePath + "Success.txt"))
        {
            string lines = System.IO.File.ReadAllText(GamePath + "Success.txt");
            if (lines.Contains("Open"))
            {
                Door1Open = true;
            }
            else
            {
                Door1Open = false;
            }
        }
        else
        {
            Door1Open = false;
        }

        // If the 'GeneratorOverride.txt' file contains the AccessOverrideCode and a power value, we turn the generator on.
        if (System.IO.File.Exists(GamePath + "Generator/" + "GeneratorOverride.txt"))
        {
            string[] lines1 = System.IO.File.ReadAllLines(GamePath + "Generator/" + "GeneratorOverride.txt");
            if (1 < lines1.Length)
            {
                lines1[0] = Regex.Replace(lines1[0], "[^0-9]", "");
                GeneratorPowerValue = float.Parse(lines1[0]);
                if (lines1[1] == AccessOverrideCode.ToString())
                {
                    if (Door1Open == true)
                    {
                        GeneratorCanBeStarted = true;
                    }
                }
                else
                {
                    GeneratorCanBeStarted = false;
                }
            }
            else
            {
                GeneratorCanBeStarted = false;
            }
        }
        else
        {
            GeneratorPowerValue = 0;
        }

        // If the generator puzzle is done, we deactivate the Open Door dialogue trigger.
        if (GeneratorCanBeStarted)
        {
            AfterOpenDoorDiaTrigger.SetActive(false);
        }
        else
        {
            AfterOpenDoorDiaTrigger.SetActive(true);
        }

        // If all 'Position.txt' text files are in place, we set the final objective as completed.
        if (System.IO.File.Exists(GamePath + "WormPosition/POS1/" + "Position.txt") && System.IO.File.Exists(GamePath + "WormPosition/POS2/" + "Position.txt") && System.IO.File.Exists(GamePath + "WormPosition/POS3/" + "Position.txt"))
        {
            IsBossHackDone = true;
            PostFinaleDia.SetActive(true);
            PreFinaleDia.SetActive(false);
        }
        else
        {
            IsBossHackDone = false;
            PostFinaleDia.SetActive(false);
            PreFinaleDia.SetActive(true);
        }
    }

    /// This method opens the door at the start of the stage.
    public void OpenDoor1()
    {
        Door1Open = true;
        Door1Animator.SetBool("IsOpen", Door1Open);
        ScreenShakeController.Instance.StartShake(1f, .05f, false);
        Door1SoundSource.Play();
        OpenDoorDiaTrigger.IsDone = true;
        GeneratorCanBeStarted = false;
    }

    /// This method creates the 'RunMe.bat' file to help solve the first door puzzle.
    public void OpenFileExplorerP1()
    {
        if (System.IO.File.Exists(GamePath + "RunMe.bat"))
        {
            try
            {
                File.Delete(GamePath + "RunMe.bat");
                File.Delete(GamePath + "Success.txt");
            }
            catch (IOException)
            {

            }
        }
        System.IO.File.WriteAllText(GamePath + "RunMe.bat", "echo AudioSource: >Success.txt\necho   OutputAudioMixerGroup: {fileID: 0} >>Success.txt \n  SpatializePostEffects: 0 >>Success.txt  \necho   m_Name: 1 (1) >>Success.txt \necho   m_TagString: Untagged >>Success.txt \necho   m_Icon: {fileID: 0} >>Success.txt \necho   m_NavMeshLayer: 0 >>Success.txt \necho   m_StaticEditorFlags: 0 >>Success.txt \necho   m_IsActive: 1 >>Success.txt \necho --- !u!4 &6308544320427265022 >>Success.txt \necho Transform: >>Success.txt \necho   m_ObjectHideFlags: 0 >>Success.txt \necho   m_CorrespondingSourceObject: {fileID: 0} >>Success.txt \necho   m_PrefabInstance: {fileID: 0} >>Success.txt \necho   m_PrefabAsset: {fileID: 0} >>Success.txt \necho   m_GameObject: {fileID: 6308544320427265009} >>Success.txt \necho   m_LocalRotation: {x: 0, y: 0, z: 0, w: 1} >>Success.txt \necho   m_LocalPosition: {x: 0, y: -1.5, z: 74.375} >>Success.txt \necho   m_LocalScale: {x: 1, y: 1, z: 1} >>Success.txt \necho   m_Children: [] >>Success.txt \necho   m_Father: {fileID: 6308544321378050844} >>Success.txt \necho   m_RootOrder: 1 >>Success.txt \necho   m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0} >>Success.txt \necho --- !u!212 &6308544320427265023 >>Success.txt \necho SpriteRenderer: >>Success.txt \necho   m_ObjectHideFlags: 0 >>Success.txt \necho   m_CorrespondingSourceObject: {fileID: 0} >>Success.txt \necho   m_PrefabInstance: {fileID: 0} >>Success.txt \necho   m_PrefabAsset: {fileID: 0} >>Success.txt \necho   m_GameObject: {fileID: 6308544320427265009} >>Success.txt \necho   m_Enabled: 1 >>Success.txt \necho   m_CastShadows: 0 >>Success.txt \necho   m_ReceiveShadows: 0 >>Success.txt \necho   m_DynamicOccludee: 1 >>Success.txt \necho   m_MotionVectors: 1 >>Success.txt \necho   m_LightProbeUsage: 1 >>Success.txt \necho   m_ReflectionProbeUsage: 1 >>Success.txt \necho   m_RayTracingMode: 0 >>Success.txt \necho   m_RayTraceProcedural: 0 >>Success.txt \necho   m_RenderingLayerMask: 1 >>Success.txt \necho   m_RendererPriority: 0 >>Success.txt \necho   m_Materials: >>Success.txt \necho   - {fileID: 2100000, guid: a97c105638bdf8b4a8650670310a4cd3, type: 2} >>Success.txt \necho   m_StaticBatchInfo: >>Success.txt \necho     firstSubMesh: 0 >>Success.txt \necho     subMeshCount: 0 >>Success.txt \necho   m_StaticBatchRoot: {fileID: 0} >>Success.txt \necho   m_ProbeAnchor: {fileID: 0} >>Success.txt \necho   m_LightProbeVolumeOverride: {fileID: 0} >>Success.txt \necho   m_ScaleInLightmap: 1 >>Success.txt \necho   m_ReceiveGI: 1 >>Success.txt \necho   m_PreserveUVs: 0 >>Success.txt \necho   m_IgnoreNormalsForChartDetection: 0 >>Success.txt \necho   m_ImportantGI: 0 >>Success.txt \necho   m_StitchLightmapSeams: 1 >>Success.txt \necho   m_State: Closed >>Success.txt \necho   m_MinimumChartSize: 4 >>Success.txt \necho   m_AutoUVMaxDistance: 0.5 >>Success.txt \necho   m_AutoUVMaxAngle: 89 >>Success.txt \necho   m_LightmapParameters: {fileID: 0} >>Success.txt \necho   m_SortingLayerID: 193805239 >>Success.txt \necho   m_SortingLayer: 4 >>Success.txt \necho   m_SortingOrder: 0 >>Success.txt \necho --- !u!61 &6308544320427265020 >>Success.txt \n");
        Application.OpenURL("file:///" + GamePath);
    }

    /// This method starts the generator puzzle by creating and filling the 'Generator' folder.
    public void StartGeneratorSetup()
    {
        if (_startedOnGenerator == false)
        {
            if (Directory.Exists(GamePath + "Generator/"))
            {
                try
                {
                    Application.OpenURL("file:///" + GamePath);
                    Directory.Delete(GamePath + "Generator/", true);
                    Directory.CreateDirectory((GamePath + "Generator/"));
                    System.IO.File.WriteAllText(GamePath + "Generator/" + "GeneratorOverride.txt", "0% \n xxx");

                }
                catch (IOException)
                {

                }
            }
            else
            {
                Application.OpenURL("file:///" + GamePath);
                Directory.CreateDirectory((GamePath + "Generator/"));
                System.IO.File.WriteAllText(GamePath + "Generator/" + "GeneratorOverride.txt", "0% \n xxx");
            }
            _startedOnGenerator = true;
        }
    }

    /// This method starts the final section of stage 3 by creating and filling the 'Position' folders, and starting the enemy spawner.
    public void StartFinaleSetup()
    {
        if (FinaleStarted == false)
        {
            if (Directory.Exists(GamePath + "WormPosition/"))
            {
                try
                {
                    Application.OpenURL("file:///" + GamePath);
                    Directory.Delete(GamePath + "WormPosition/", true);
                    Directory.CreateDirectory((GamePath + "WormPosition/"));
                    Directory.CreateDirectory((GamePath + "WormPosition/POS1"));
                    Directory.CreateDirectory((GamePath + "WormPosition/POS2"));
                    Directory.CreateDirectory((GamePath + "WormPosition/POS3"));
                    System.IO.File.WriteAllText(GamePath + "WormPosition/POS1/" + "Triangulator.bat", "echo Node.sub.position successfully located>Position.txt");
                    System.IO.File.WriteAllText(GamePath + "WormPosition/POS2/" + "Triangulator.bat", "echo Node.sub.position successfully located>Position.txt");
                    System.IO.File.WriteAllText(GamePath + "WormPosition/POS3/" + "Triangulator.bat", "echo Node.sub.position successfully located>Position.txt");

                }
                catch (IOException)
                {

                }

            }
            else
            {
                Application.OpenURL("file:///" + GamePath);
                Directory.CreateDirectory((GamePath + "WormPosition/"));
                Directory.CreateDirectory((GamePath + "WormPosition/POS1"));
                Directory.CreateDirectory((GamePath + "WormPosition/POS2"));
                Directory.CreateDirectory((GamePath + "WormPosition/POS3"));
                System.IO.File.WriteAllText(GamePath + "WormPosition/POS1/" + "Triangulator.bat", "echo Node.sub.position successfully located>Position.txt");
                System.IO.File.WriteAllText(GamePath + "WormPosition/POS2/" + "Triangulator.bat", "echo Node.sub.position successfully located>Position.txt");
                System.IO.File.WriteAllText(GamePath + "WormPosition/POS3/" + "Triangulator.bat", "echo Node.sub.position successfully located>Position.txt");
            }
        }
        else
        {
            System.IO.File.WriteAllText(GamePath + "WormPosition/POS1/" + "Triangulator.bat", "echo Node.sub.position successfully located>Position.txt");
            System.IO.File.WriteAllText(GamePath + "WormPosition/POS2/" + "Triangulator.bat", "echo Node.sub.position successfully located>Position.txt");
            System.IO.File.WriteAllText(GamePath + "WormPosition/POS3/" + "Triangulator.bat", "echo Node.sub.position successfully located>Position.txt");
            Application.OpenURL("file:///" + GamePath);
        }

        FinaleStarted = true;
        GameObject.Find("Wave Controller").GetComponent<WaveController>().Trigger();
    }
}