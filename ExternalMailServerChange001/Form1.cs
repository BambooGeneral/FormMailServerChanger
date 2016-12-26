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
        private String ServerIP = "182.48.11.151";

        //文字コード(ここでは、Shift JIS)
        Encoding enc = Encoding.GetEncoding("utf-8");

        //フォームウィンドウの初期化
        public FormMailServerChanger()
        {
            InitializeComponent();
        }

        //スタートボタンを押したとき
        private void buttonStart_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            List<String> lines = new List<string>();

            //行ごとの配列として、テキストファイルの中身をすべて読み込む
            lines = File.ReadAllLines(Thunderbird.Address.ProfileChoicer, enc).ToList();


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
                            //thisAddress.SetProfile("e763dtpl.next");
                            Debug.WriteLine(thisAddress.ProfileFolder);
                            Debug.WriteLine(thisAddress.ProfileSetting);
                            break;
                        }
                    }
                }
            }

            //行ごとの配列として、テキストファイルの中身をすべて読み込む
            lines = File.ReadAllLines(thisAddress.ProfileSetting, enc).ToList();

            //デバッグ用にデスクトップに元ファイルを置く
            //File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\test before.txt", lines.ToArray(), enc);

            //ファイルスキャンとリスト追加
            foreach (var item in lines)
            {
                for (User_prefs i = User_prefs.accoount; i <= User_prefs.smtpserver; i++)
                {
                    if (item.StartsWith(Thunderbird.Get_User_prefs_Line(i)))
                    {
                        //リストに追加
                        thisPrefs.Preflists[i].AddList(item.Split('.')[2]);
                        //社外メールだったら追加
                        if (item.Contains("greenfix.co.jp"))
                        {
                            thisPrefs.Preflists[i].SetOld(item.Split('.')[2]);
                        }
                    }
                }
            }

            //参照用インスタンス
            Thunderbird.PrefsData.Preflist Plist = new Thunderbird.PrefsData.Preflist("a");

            //デバッグ
            for (User_prefs i = User_prefs.accoount; i <= User_prefs.smtpserver; i++)
            {
                Plist = thisPrefs.Preflists[i];
                Debug.WriteLine("LastGreenfix" + Plist.Name.ToUpper() + ":" + Plist.Last);
                Debug.WriteLine("NextGreenfix" + Plist.Name.ToUpper() + ":" + Plist.Next);
                Debug.WriteLine("OldGreenfix" + Plist.Name.ToUpper() + ":" + Plist.Old);
            }

            //新規アカウント用コピー作成
            SortedList<User_prefs, List<string>> copyLines = new SortedList<User_prefs, List<string>>();

            //新規アカウント用コピー作成
            for (User_prefs i = User_prefs.identity; i <= User_prefs.smtpserver; i++)
            {
                Plist = thisPrefs.Preflists[i];
                foreach (var item in lines)
                {
                    if (item.StartsWith(Thunderbird.Get_User_prefs_Line(i) + Plist.Old.ToString()))
                    {
                        if (!copyLines.Keys.Contains(i))
                        {
                            copyLines[i] = new List<string>();
                        }
                        copyLines[i].Add(item.Replace(Plist.Name + Plist.Old.ToString(), Plist.Name + Plist.Next.ToString()));
                    }
                }
            }

            //デバッグ
            foreach (var items in copyLines)
            {
                items.Value.ForEach(delegate (String item)
                {
                    Debug.WriteLine(item);
                });
            }

            //社外メールが無いとき中止
            if (thisPrefs.Preflists[User_prefs.identity].Old == 0 || thisPrefs.Preflists[User_prefs.server].Old == 0 || thisPrefs.Preflists[User_prefs.smtpserver].Old == 0)
            {
                MessageBox.Show("社外メール設定がありません", "MailServerChanger", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //設定編集
            for (User_prefs i = User_prefs.accoount; i <= User_prefs.smtpserver; i++)
            {
                switch (i)
                {
                    case User_prefs.identity:
                        for (int l = 0; l < copyLines[i].Count; l++)
                        {
                            if (copyLines[i][l].Contains("mail.greenfix.co.jp"))
                            {
                                copyLines[i][l] = copyLines[i][l].Replace("mail.greenfix.co.jp", ServerIP);
                            }
                            if (copyLines[i][l].Contains(ServerIP))
                            {
                                copyLines[i][l] = copyLines[i][l].Replace(ServerIP, "mail.greenfix.co.jp");
                            }
                            if (copyLines[i][l].Contains("fullName"))
                            {
                                copyLines[i][l] = copyLines[i][l].Replace("\");", "_New\");");
                            }
                            if (copyLines[i][l].Contains("smtp"))
                            {
                                copyLines[i][l] = copyLines[i][l].Replace("smtp" + thisPrefs.Preflists[User_prefs.smtpserver].Old.ToString(), "smtp" + thisPrefs.Preflists[User_prefs.smtpserver].Next.ToString());
                            }
                        }
                        break;
                    case User_prefs.server:
                        for (int l = 0; l < copyLines[i].Count; l++)
                        {
                            if (copyLines[i][l].Contains("mail.greenfix.co.jp"))
                            {
                                copyLines[i][l] = copyLines[i][l].Replace("mail.greenfix.co.jp", ServerIP);
                            }
                            if (copyLines[i][l].Contains(ServerIP))
                            {
                                copyLines[i][l] = copyLines[i][l].Replace(ServerIP, "mail.greenfix.co.jp");
                            }
                            if (copyLines[i][l].Contains(".leave_on_server"))
                            {
                                copyLines[i][l] = copyLines[i][l].Replace("false", "true");
                            }
                            if (copyLines[i][l].Contains(".num_days_to_leave_on_server"))
                            {
                                copyLines[i][l] = "user_pref(\"mail.server.server"+thisPrefs.Preflists[i].Next.ToString()+".num_days_to_leave_on_server\", 14);";
                            }
                        }
                        break;
                    case User_prefs.smtpserver:
                        for (int l = 0; l < copyLines[i].Count; l++)
                        {
                            if (copyLines[i][l].Contains("mail.greenfix.co.jp"))
                            {
                                copyLines[i][l] = copyLines[i][l].Replace("mail.greenfix.co.jp", ServerIP);
                            }
                            if (copyLines[i][l].Contains(ServerIP))
                            {
                                copyLines[i][l] = copyLines[i][l].Replace(ServerIP, "mail.greenfix.co.jp");
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            //ファイル編集（メモリ内）
            for (User_prefs i = User_prefs.accoount; i <= User_prefs.smtpserver; i++)
            {
                int l;
                switch (i)
                {
                    case User_prefs.accoount:
                        for (l = 0; l < lines.Count; l++)
                        {
                            if (lines[l].StartsWith("user_pref(\"mail.account.lastKey"))
                            {
                                lines[l] = "user_pref(\"mail.account.lastKey\", " + thisPrefs.Preflists[i].Next.ToString() + ");";
                                lines.Insert(l, "user_pref(\"mail.account.account" + thisPrefs.Preflists[i].Next + ".server\", \"server" + thisPrefs.Preflists[User_prefs.server].Next + "\");");
                                lines.Insert(l, "user_pref(\"mail.account.account" + thisPrefs.Preflists[i].Next + ".identities\", \"id" + thisPrefs.Preflists[User_prefs.identity].Next + "\");");
                                break;
                            }
                        }
                        break;
                    case User_prefs.identity:
                        for (l = 0; l < lines.Count; l++)
                        {
                            if (lines[l].StartsWith("user_pref(\"mail.identity.id" + thisPrefs.Preflists[i].Last.ToString() + ".valid"))
                            {
                                for (int m = copyLines[i].Count - 1; m >= 0; m--)
                                {
                                    lines.Insert(l + 1, copyLines[i][m]);
                                }
                                break;
                            }
                        }
                        break;
                    case User_prefs.server:
                        for (l = 0; l < lines.Count; l++)
                        {
                            if (lines[l].StartsWith("user_pref(\"mail.server.server" + thisPrefs.Preflists[i].Last.ToString() + ".userName"))
                            {
                                for (int m = copyLines[i].Count - 1; m >= 0; m--)
                                {
                                    lines.Insert(l + 1, copyLines[i][m]);
                                }
                                break;
                            }
                        }
                        break;
                    case User_prefs.smtpserver:
                        for (l = 0; l < lines.Count; l++)
                        {
                            if (lines[l].StartsWith("user_pref(\"mail.smtpservers"))
                            {
                                lines[l] = "user_pref(\"mail.smtpservers\", " + "\"" + thisPrefs.Preflists[i].GetMembers() + ",smtp" + thisPrefs.Preflists[i].Next.ToString() + "\");";
                                for (int m = copyLines[i].Count - 1; m >= 0; m--)
                                {
                                    lines.Insert(l, copyLines[i][m]);
                                }
                                break;
                            }
                        }
                        break;
                    default:
                        break;
                }

            }

            //デバッグ用にデスクトップに新ファイルを置く
            //File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\test after.txt", lines.ToArray(), enc);
            File.Copy(thisAddress.ProfileSetting, thisAddress.ProfileSetting.Replace(".js", " BackUp .js"));
            File.WriteAllLines(thisAddress.ProfileSetting, lines.ToArray(), enc);

            textBox1.Enabled = true;
        }

        private void ChangeIP(object sender, EventArgs e)
        {
            ServerIP = textBox1.Text;
        }
    }
}
