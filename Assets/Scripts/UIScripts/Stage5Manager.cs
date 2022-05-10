using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

/// Manages external puzzles on stage 5: the debuff mechanic on the boss and the final sequence of the game after the boss is destroyed.
public class Stage5Manager : MonoBehaviour
{
    public string GamePath;
    public bool IsDebuffed;
    private BossCutsceneManager _bossCutsceneManager;
    public bool ReadyToRun;

    /// Called before the first frame.
    /** The start function sets up the game path and clears out puzzle files from previous attempts if they exist. */
    private void Start()
    {
        ReadyToRun = true;
        IsDebuffed = true;
        _bossCutsceneManager = FindObjectOfType<BossCutsceneManager>();

        GamePath = Application.dataPath;
        GamePath += "/../";
        GamePath += "AIS/";

        if (System.IO.File.Exists(GamePath + "debuffed.txt"))
        {
            try
            {
                File.Delete(GamePath + "debuffed.txt");
            }
            catch (IOException)
            {

            }
        }

        if (System.IO.Directory.Exists(GamePath + "AIS") == false)
        {
            Directory.CreateDirectory(GamePath + "AIS");
        }

        if (System.IO.File.Exists(GamePath + "neutralize_0x12_worm.bat") || System.IO.File.Exists(GamePath + "neutralize_0x12_worm_success.txt"))
        {
            try
            {
                File.Delete(GamePath + "neutralize_0x12_worm.bat");
                File.Delete(GamePath + "neutralize_0x12_worm_success.txt");
            }
            catch (IOException)
            {

            }
        }
    }

    /// Called every frame.
    /** The update function removes the player's debuff if the debuffed.txt file is deleted while the player is on the ground. */
    private void Update()
    {
        if (System.IO.File.Exists(GamePath + "debuffed.txt") == false && _bossCutsceneManager.IsDown == true)
        {
            IsDebuffed = false;
        }

        if (System.IO.File.Exists(GamePath + "neutralize_0x12_worm_success.txt") && ReadyToRun == true)
        {
            ReadyToRun = false;
            RunCMD();
        }
    }

    /// Applies player debuff, removes previous 'debuffed.txt' file if it exists and creates a new one.
    public void DebuffPlayer()
    {
        IsDebuffed = true;
        if (System.IO.File.Exists(GamePath + "debuffed.txt"))
        {
            try
            {
                File.Delete(GamePath + "debuffed.txt");
            }
            catch (IOException)
            {

            }
        }
        System.IO.File.WriteAllText(GamePath + "debuffed.txt", "YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! \n YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! \n YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! \n YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! \n YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! \n YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! \n YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED! YOU ARE DEBUFFED!>>debuffed.txt");
        Application.OpenURL("file:///" + GamePath);
    }

    /// Starts the 'StartCMDTimer' coroutine.
    public void RunCMD()
    {
        StartCoroutine(StartCMDTimer());
    }

    /// Deletes the 'neutralize worm'-files if they already exist from previous attempts, then creates them again.
    public void MakeFile()
    {
        if (System.IO.File.Exists(GamePath + "neutralize_0x12_worm.bat") || System.IO.File.Exists(GamePath + "neutralize_0x12_worm_success.txt"))
        {
            try
            {
                File.Delete(GamePath + "neutralize_0x12_worm.bat");
                File.Delete(GamePath + "neutralize_0x12_worm_success.txt");
            }
            catch (IOException)
            {

            }
        }
        System.IO.File.WriteAllText(GamePath + "neutralize_0x12_worm.bat", "echo 0x991ss_ZdadrasUuJJG4456 >>neutralize_0x12_worm_success.txt");
        Application.OpenURL("file:///" + GamePath);
    }

    /// Creates the finishing sequence of the game, opening command prompt windows with different colors and loading the 'The Hub'-scene after 10 seconds.
    IEnumerator StartCMDTimer()
    {
        string _path = Application.dataPath;
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        for (int i = 0; i < 50; i++)
        {
            float _randomNumber = Random.Range(0, 99);
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/K title S.I.A.A TAKING BACK OVER && cd " + _path + " && color " + _randomNumber + " && dir / A:D / B / S && exit";
            process.StartInfo = startInfo;
            process.Start();
            yield return new WaitForFixedUpdate();
        }
        System.Diagnostics.ProcessStartInfo startInfo2 = new System.Diagnostics.ProcessStartInfo();
        System.Diagnostics.Process process2 = new System.Diagnostics.Process();
        startInfo2.FileName = "cmd.exe";
        startInfo2.Arguments = "/C msg * S.I.A.A IS BACK IN CONTROL! I'm taking you back to the hub now.";
        process2.StartInfo = startInfo2;
        process2.Start();

        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("The_Hub");
    }
}
