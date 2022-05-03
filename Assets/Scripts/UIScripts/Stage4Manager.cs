using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Stage4Manager : MonoBehaviour
{
    public string GamePath;
    public bool ReadyToRun;
    public GameObject FinaleBlock;

    // Start is called before the first frame update
    void Start()
    {
        GamePath = Application.dataPath;
        GamePath += "/../";
        GamePath += "AIS/";
        ReadyToRun = true;
        FinaleBlock.SetActive(true);
        if (System.IO.File.Exists(GamePath + "Crip.bat") || System.IO.File.Exists(GamePath + "CripSuc.txt"))
        {
            try
            {
                File.Delete(GamePath + "Crip.bat");
                File.Delete(GamePath + "CripSuc.txt");
            }
            catch (IOException)
            {

            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (System.IO.File.Exists(GamePath + "CripSuc.txt") && ReadyToRun == true)
        {
            ReadyToRun = false;
            RunCMD();
        }

    }

    public void RunCMD()
    {
        string _path = Application.dataPath;
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        for (int i = 0; i < 50; i++)
        {
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/K title S.I.A.A && cd " + _path + " && color a && dir / A:D / B / S && exit";
            process.StartInfo = startInfo;
            process.Start();
        }
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/C msg * Super! I got a hold of it! Let's get back in the game and catch up to it!";
        process.StartInfo = startInfo;
        process.Start();
        FinaleBlock.SetActive(false);
    }
    public void MakeFile()
    {
        if (System.IO.File.Exists(GamePath + "Crip.bat") || System.IO.File.Exists(GamePath + "CripSuc.txt"))
        {
            try
            {
                File.Delete(GamePath + "Crip.bat");
                File.Delete(GamePath + "CripSuc.txt");
            }
            catch (IOException)
            {

            }
        }
        System.IO.File.WriteAllText(GamePath + "Crip.bat", "echo Sweet >>CripSuc.txt");
        Application.OpenURL("file:///" + GamePath);
    }
}
