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

        private Thunderbird.Address thisAddress = new Thunderbird.Address();
        private Thunderbird.PrefsData thisPrefs = new Thunderbird.PrefsData();

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
            String[] lines;

            //行ごとの配列として、テキストファイルの中身をすべて読み込む
            lines = File.ReadAllLines(Thunderbird.Address.ProfileChoicer, enc);

            foreach (var item in lines)
            {
                //Debug.WriteLine(item);
                if (item.StartsWith("P"))
                {
                    if (true)//if (item.Length >= 15)
                    {
                        if (item.StartsWith("Path=Profiles/"))
                        {
                            thisAddress.SetProfile(item.Split('/')[1]);
                            //test
                            thisAddress.SetProfile("e763dtpl.next");
                            Debug.WriteLine(thisAddress.ProfileFolder);
                            Debug.WriteLine(thisAddress.ProfileSetting);
                            break;
                        }
                    }
                }
            }

            //行ごとの配列として、テキストファイルの中身をすべて読み込む
            lines = File.ReadAllLines(thisAddress.ProfileSetting, enc);

            foreach (var item in lines)
            {
                if (item.StartsWith(Thunderbird.Get_User_prefs(Thunderbird.User_prefs.accoount)))
                {
                    //アカウントをリストに追加
                    thisPrefs.accountsGF.AddList(item.Split('.')[2]);
                }
                if (item.StartsWith(Thunderbird.Get_User_prefs(Thunderbird.User_prefs.identity)))
                {
                    //IDをリストに追加
                    thisPrefs.identitysGF.AddList(item.Split('.')[2]);
                    if (item.Contains("@greenfix.co.jp"))
                    {
                        //古い社外メールIDを発見→格納
                        thisPrefs.identitysGF.SetOld(item.Split('.')[2]);
                    }
                }
                if (item.StartsWith(Thunderbird.Get_User_prefs(Thunderbird.User_prefs.server)))
                {
                    //サーバーをリストに追加
                    thisPrefs.serversGF.AddList(item.Split('.')[2]);
                    if (item.Contains("@greenfix.co.jp") && item.Contains("userName"))
                    {
                        //古い社外メールサーバーを発見→格納
                        thisPrefs.serversGF.SetOld(item.Split('.')[2]);
                    }
                }
                if (item.StartsWith(Thunderbird.Get_User_prefs(Thunderbird.User_prefs.smtpserver)))
                {
                    //SMTPサーバーをリストに追加
                    thisPrefs.smtpsGF.AddList(item.Split('.')[2]);
                    if (item.Contains("mail.greenfix.co.jp"))
                    {
                        //古い社外メールSMTPサーバーを発見→格納
                        thisPrefs.smtpsGF.SetOld(item.Split('.')[2]);
                    }
                }
            }

            Debug.WriteLine("LastGreenfixID:" + thisPrefs.identitysGF.Last);
            Debug.WriteLine("LastGreenfixServer:" + thisPrefs.serversGF.Last);
            Debug.WriteLine("LastGreenfixSMTPServer:" + thisPrefs.smtpsGF.Last);
            Debug.WriteLine("OldGreenfixID:" + thisPrefs.identitysGF.Old);
            Debug.WriteLine("OldGreenfixServer:" + thisPrefs.serversGF.Old);
            Debug.WriteLine("OldGreenfixSMTPServer:" + thisPrefs.smtpsGF.Old);

            //インクリメントしてコピー
            List<String> copyLines_ids = new List<string>();
            List<String> copyLines_servers = new List<string>();
            List<String> copyLines_smtpservers = new List<string>();
            foreach (var item in lines)
            {
                foreach (var item in Thunderbird.User_prefs)
                {

                }
                if (item.StartsWith(Thunderbird.Get_User_prefs(Thunderbird.User_prefs.identity) + thisPrefs.Preflists[Thunderbird.User_prefs.identity].Old.ToString()))
                {
                    copyLines_ids.Add(item.Replace("id" + thisPrefs.Preflists[Thunderbird.User_prefs.identity].Old.ToString(), "id" + (thisPrefs.Preflists[Thunderbird.User_prefs.identity].Next).ToString()));
                }
                if (item.StartsWith(Thunderbird.Get_User_prefs(Thunderbird.User_prefs.server) + thisPrefs.Preflists[Thunderbird.User_prefs.server].Old.ToString()))
                {
                    copyLines_ids.Add(item.Replace("server" + thisPrefs.Preflists[Thunderbird.User_prefs.server].Old.ToString(), "server" + (thisPrefs.Preflists[Thunderbird.User_prefs.server].Next).ToString()));
                }
                if (item.StartsWith(Thunderbird.Get_User_prefs(Thunderbird.User_prefs.smtpserver) + thisPrefs.Preflists[Thunderbird.User_prefs.smtpserver].Old.ToString()))
                {
                    copyLines_ids.Add(item.Replace("smtp" + thisPrefs.Preflists[Thunderbird.User_prefs.smtpserver].Old.ToString(), "smtp" + (thisPrefs.Preflists[Thunderbird.User_prefs.smtpserver].Next).ToString()));
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
