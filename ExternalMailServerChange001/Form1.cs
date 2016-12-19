using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;


namespace ExternalMailServerChange001
{
    public partial class FormMailServerChanger : Form
    {
        private Boolean StartWhistle = false;

        Thunderbird.Address ThisThunderbird;

        //文字コード(ここでは、Shift JIS)
        Encoding enc = Encoding.GetEncoding("shift-jis");

        //フォームウィンドウの初期化
        public FormMailServerChanger()
        {
            InitializeComponent();
        }

        //スタートボタンを押したとき
        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartWhistle = true;
            String[] lines;

            //行ごとの配列として、テキストファイルの中身をすべて読み込む
            lines = File.ReadAllLines(Thunderbird.Address.ThunderbirdProfileChoicer, enc);

            foreach (var item in lines)
            {
                //Debug.WriteLine(item);
                if (item.StartsWith("P"))
                {
                    if (true)//if (item.Length >= 15)
                    {
                        if (item.StartsWith("Path=Profiles/"))
                        {
                            ThisThunderbird.ThunderbirdProfileFolder = Thunderbird.Address.ThunderbirdProfileSettingFolder + @"\Profiles\" + item.Split('/')[1];
                            //test
                            ThisThunderbird.ThunderbirdProfileFolder = Thunderbird.Address.ThunderbirdProfileSettingFolder + @"\Profiles\e763dtpl.next";
                            ThisThunderbird.ThunderbirdProfileSetting = ThisThunderbird.ThunderbirdProfileFolder + @"\prefs.js";
                            Debug.WriteLine(ThisThunderbird.ThunderbirdProfileFolder);
                            Debug.WriteLine(ThisThunderbird.ThunderbirdProfileSetting);
                            break;
                        }
                    }
                }
            }

            //行ごとの配列として、テキストファイルの中身をすべて読み込む
            lines = File.ReadAllLines(ThisThunderbird.ThunderbirdProfileSetting, enc);
            Int32 accountLastkey;
            List<String> servers = new List<string>();
            List<String> ids = new List<string>();
            List<String> smtpServers = new List<string>();
            String OldGreenfixID = null, OldGreenfixServer = null, OldGreenfixSMTPServer = null;
            String LastGreenfixID = null, LastGreenfixServer = null, LastGreenfixSMTPServer = null;
            Thunderbird.PrefsData thisPrefs;
            foreach (var item in lines)
            {
                if (item.StartsWith(Thunderbird.Get_User_prefs(Thunderbird.User_prefs.accountlastkey)))
                {
                    accountLastkey = Int32.Parse(item.Split(',')[1].Split(')')[0].Trim());
                    Debug.WriteLine("accountLastkey:" + accountLastkey);
                    Debug.WriteLine(item);
                }
                if (item.StartsWith(Thunderbird.Get_User_prefs(Thunderbird.User_prefs.accoount)))
                {
                    if (item.Split('\"')[3].StartsWith("id"))
                    {
                        thisPrefs.accountsGF.Last=item.Split('\"')[3]);
                    }
                    else
                    {
                        servers.Add(item.Split('\"')[3]);
                    }

                }
                if (item.StartsWith(Thunderbird.Get_User_prefs(Thunderbird.User_prefs.identity)))
                {
                    if (item.Contains("@greenfix.co.jp"))
                    {
                        OldGreenfixID = item.Split('.')[2];
                    }
                }
                if (item.StartsWith(Thunderbird.Get_User_prefs(Thunderbird.User_prefs.server)))
                {
                    if (item.Contains("@greenfix.co.jp") && item.Contains("userName"))
                    {
                        OldGreenfixServer = item.Split('.')[2];
                    }
                }
                if (item.StartsWith(Thunderbird.Get_User_prefs(Thunderbird.User_prefs.smtpserver)))
                {
                    if (item.Contains("mail.greenfix.co.jp"))
                    {
                        OldGreenfixSMTPServer = item.Split('.')[2];
                    }
                    if (!smtpServers.Contains(item.Split('.')[2]))
                    {
                        smtpServers.Add(item.Split('.')[2]);
                    }
                }
            }
            
                        Debug.WriteLine("LastGreenfixID:" + LastGreenfixID);
            Debug.WriteLine("LastGreenfixServer:" + LastGreenfixServer);
            Debug.WriteLine("LastGreenfixSMTPServer:" + LastGreenfixSMTPServer);
            if (OldGreenfixID != null) Debug.WriteLine("OldGreenfixID:" + OldGreenfixID);
            if (OldGreenfixServer != null) Debug.WriteLine("OldGreenfixServer:" + OldGreenfixServer);
            if (OldGreenfixSMTPServer != null) Debug.WriteLine("OldGreenfixSMTPServer:" + OldGreenfixSMTPServer);

            List<String> copyLines_ids = new List<string>();
            List<String> copyLines_servers = new List<string>();
            List<String> copyLines_smtpservers = new List<string>();
            foreach (var item in lines)
            {
                if (item.StartsWith("user_pref(\"mail.identity." + OldGreenfixID))
                {
                    copyLines_ids.Add(item.Replace(OldGreenfixID, "id" + (Int64.Parse(LastGreenfixID.Replace("id", "")) + 1).ToString()));
                }
                if (item.StartsWith("user_pref(\"mail.server." + OldGreenfixServer))
                {
                    copyLines_ids.Add(item.Replace(OldGreenfixServer, "server" + (Int64.Parse(LastGreenfixServer.Replace("server", "")) + 1).ToString()));
                }
                if (item.StartsWith("user_pref(\"mail.smtpserver." + OldGreenfixSMTPServer))
                {
                    copyLines_ids.Add(item.Replace(OldGreenfixSMTPServer, "smtp" + (Int64.Parse(LastGreenfixSMTPServer.Replace("smtp", "")) + 1).ToString()));
                }
            }
            foreach (var item in copyLines_ids)
            {
                Debug.WriteLine(item);
            }
            foreach (var item in copyLines_servers)
            {
                Debug.WriteLine(item);
            }
            foreach (var item in copyLines_smtpservers)
            {
                Debug.WriteLine(item);
            }


        }

        private void PBDW(object sender, DoWorkEventArgs e)
        {

        }

        private void PBPC(object sender, ProgressChangedEventArgs e)
        {

        }

        private void PBRWC(object sender, RunWorkerCompletedEventArgs e)
        {

        }
    }
}
