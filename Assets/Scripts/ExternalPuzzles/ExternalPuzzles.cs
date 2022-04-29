using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ExternalPuzzles : MonoBehaviour
{
    public bool TextFile;
    public bool BatFile;
    public bool BatFileToTxtFile;
    public bool CheckForString;
    private string stringpath;
    private MakeDirectories _makeDirs;

    // Start is called before the first frame update
    void Start()
    {
        /* string _path = Application.dataPath;
         System.Diagnostics.Process process = new System.Diagnostics.Process();
         System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
         //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
         startInfo.FileName = "cmd.exe";
         startInfo.Arguments = "/K cd " + _path;
         startInfo.Arguments = "/K cd ..";
         process.StartInfo = startInfo;
         process.Start();*/
        _makeDirs = Object.FindObjectOfType<MakeDirectories>();
        _makeDirs.MakeDirs();
        _makeDirs.MakeFiles();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Period))
        {
            string _path = Application.dataPath;
            if (TextFile) System.IO.File.WriteAllText("C:/Users/Emil/Desktop/textfile.txt", "This is a textfile made from the game :D");
            if (BatFile)
            {
                System.IO.File.WriteAllText("C:/Users/Emil/Desktop/textfile.bat", "msg * " + _path);
                var folder = Directory.CreateDirectory((_path + "/../") + "/Yeash/rseyhui");
            }
            if (BatFileToTxtFile) System.IO.File.WriteAllText("C:/Users/Emil/Desktop/RunME.bat", "@echo off \n echo This is a test> test.txt \n echo 123>> test.txt \n echo 245.67>> test.txt");
            if (CheckForString)
            {
                string _newPath = _path + "/../";
                _newPath += "yello";
                var folder = Directory.CreateDirectory((_newPath) + "/Yeash/rseyhui");
                _newPath += "/Yeash/rseyhui/";
                stringpath = _newPath;
                System.IO.File.WriteAllText(_newPath + "bat.txt", "Yhduisahdiosahjidhj asiodiosa jdio saiojdijosajiodiojsa dijas oijdio jsaio jdiosa jiod jsaiod jiosaj iod  jsaio djioas jdioasj diosa");
            }
        }
        if (Input.GetKeyDown(KeyCode.L) && CheckForString)
        {
            string lines = System.IO.File.ReadAllText(stringpath + "bat.txt");
            if (lines.Contains("Yhd"))
            {
                //Do something (this works tho)
            }
        }
    }
}
