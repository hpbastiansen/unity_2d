using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

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

    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        Door1Animator.SetBool("IsOpen", Door1Open);
        ThreeADoorAnimator.SetBool("IsOpen", GeneratorCanBeStarted);
        ThreeBDoorAnimator.SetBool("IsOpen", GeneratorCanBeStarted);
        ThreeCDoorAnimator.SetBool("IsOpen", GeneratorCanBeStarted);
        FinalDoorAnimator.SetBool("IsOpen", IsLastDoorOpen);

        if (TargetLight1.IsOn == true && TargetLight2.IsOn == true && TargetLight3.IsOn == true)
        {
            IsLastDoorOpen = true;
        }
        else
        {
            IsLastDoorOpen = false;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenDoor1();
        }
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

        if (GeneratorCanBeStarted)
        {
            AfterOpenDoorDiaTrigger.SetActive(false);
        }
        else
        {
            AfterOpenDoorDiaTrigger.SetActive(true);
        }
        if (System.IO.File.Exists(GamePath + "WormPosition/AWR/" + "Position.txt") && System.IO.File.Exists(GamePath + "WormPosition/AQO/" + "Position.txt") && System.IO.File.Exists(GamePath + "WormPosition/RTP/" + "Position.txt") && System.IO.File.Exists(GamePath + "WormPosition/TRIQ/" + "Position.txt")
         && System.IO.File.Exists(GamePath + "WormPosition/RTYD/" + "Position.txt") && System.IO.File.Exists(GamePath + "WormPosition/ARIQ/" + "Position.txt") && System.IO.File.Exists(GamePath + "WormPosition/SIAA/" + "Position.txt") && System.IO.File.Exists(GamePath + "WormPosition/WQSY/" + "Position.txt")
          && System.IO.File.Exists(GamePath + "WormPosition/PPOO/" + "Position.txt") && System.IO.File.Exists(GamePath + "WormPosition/IAFAR/" + "Position.txt"))
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
    public void OpenDoor1()
    {
        Door1Open = true;
        Door1Animator.SetBool("IsOpen", Door1Open);
        ScreenShakeController.Instance.StartShake(1f, .05f, false);
        Door1SoundSource.Play();
        OpenDoorDiaTrigger.IsDone = true;
        GeneratorCanBeStarted = false;
    }
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
                }
                catch (IOException)
                {

                }
            }
            else
            {
                Application.OpenURL("file:///" + GamePath);
                Directory.CreateDirectory((GamePath + "Generator/"));
            }
            _startedOnGenerator = true;
        }
    }

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
                    System.IO.File.WriteAllText(GamePath + "WormPosition/" + "Triangulator.bat", "echo Node.sub.position successfully located>Position.txt");
                    Directory.CreateDirectory((GamePath + "WormPosition/AWR"));
                    Directory.CreateDirectory((GamePath + "WormPosition/AQO"));
                    Directory.CreateDirectory((GamePath + "WormPosition/RTP"));
                    Directory.CreateDirectory((GamePath + "WormPosition/TRIQ"));
                    Directory.CreateDirectory((GamePath + "WormPosition/RTYD"));
                    Directory.CreateDirectory((GamePath + "WormPosition/ARIQ"));
                    Directory.CreateDirectory((GamePath + "WormPosition/SIAA"));
                    Directory.CreateDirectory((GamePath + "WormPosition/WQSY"));
                    Directory.CreateDirectory((GamePath + "WormPosition/PPOO"));
                    Directory.CreateDirectory((GamePath + "WormPosition/IAFAR"));
                }
                catch (IOException)
                {

                }

            }
            else
            {
                Application.OpenURL("file:///" + GamePath);
                Directory.CreateDirectory((GamePath + "WormPosition/"));
                System.IO.File.WriteAllText(GamePath + "WormPosition/" + "Triangulator.bat", "echo Node.sub.position successfully located>Position.txt");
                Directory.CreateDirectory((GamePath + "WormPosition/AWR"));
                Directory.CreateDirectory((GamePath + "WormPosition/AQO"));
                Directory.CreateDirectory((GamePath + "WormPosition/RTP"));
                Directory.CreateDirectory((GamePath + "WormPosition/TRIQ"));
                Directory.CreateDirectory((GamePath + "WormPosition/RTYD"));
                Directory.CreateDirectory((GamePath + "WormPosition/ARIQ"));
                Directory.CreateDirectory((GamePath + "WormPosition/SIAA"));
                Directory.CreateDirectory((GamePath + "WormPosition/WQSY"));
                Directory.CreateDirectory((GamePath + "WormPosition/PPOO"));
                Directory.CreateDirectory((GamePath + "WormPosition/IAFAR"));
            }
        }
        else
        {
            System.IO.File.WriteAllText(GamePath + "WormPosition/" + "Triangulator.bat", "echo Node.sub.position successfully located>Position.txt");
            Application.OpenURL("file:///" + GamePath);
        }

        FinaleStarted = true;
    }
}