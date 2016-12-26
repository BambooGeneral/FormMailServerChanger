using System;
using System.Collections.Generic;

namespace ExternalMailServerChange001
{
    // enum定義
    public enum User_prefs { accoount, identity, server, smtpserver };
    public static class Thunderbird
    {

        // 拡張メソッドの定義
        public static string Get_User_prefs_Line(this User_prefs item)
        {
            string[] names = { "mail.account.account", "mail.identity.id", "mail.server.server", "mail.smtpserver.smtp" };
            return "user_pref(\"" + names[(int)item];
        }
        public static string Get_User_prefs_name(this User_prefs item)
        {
            string[] names = { "account", "id", "server", "smtp" };
            return names[(int)item];
        }

        public class Address
        {

            //Thunderbirdのデフォルトユーザープロファイルフォルダ
            private String profileFolder = "";

            //Thunderbirdのユーザープロファイル設定ファイル
            private String profileSetting = "";

            public static string ProfileSettingFolder { get => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Thunderbird\"; }
            public static string ProfileChoicer { get => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Thunderbird\profiles.ini"; }
            public string ProfileFolder { get => profileFolder; }
            public string ProfileSetting { get => profileSetting; }
            public void SetProfile(String item)
            {
                profileFolder = ProfileSettingFolder + @"\Profiles\" + item;
                profileSetting = ProfileFolder + @"\prefs.js";
            }
        }

        public class PrefsData
        {
            public SortedList<User_prefs, Preflist> Preflists = new SortedList<User_prefs, Preflist>();
            public class Preflist
            {
                private List<long> list = new List<Int64>();
                private long old = 0;
                private long last = 0;
                private long next = 0;
                private String name = "";

                public long Last { get => last; }
                public long Old { get => old; }
                public long Next { get => next; }
                public String Name { get => name; }

                public void SetOld(String item)
                {
                    if (item.GetTypeCode() == TypeCode.String)
                    {
                        old = long.Parse(trimer(item.ToString()));
                    }
                    else
                    {
                        Exception Exp = new Exception("Need String!");
                        throw Exp;
                    }
                }
                public void AddList(String item)
                {
                    long value = long.Parse(trimer(item));
                    if (!list.Contains(value))
                    {
                        list.Add(value);
                    }
                    list.Sort();
                    last = list[list.Count - 1];
                    next = last + 1;
                }
                private String trimer(String item)
                {
                    return item.Trim("identitysmtpserveraccount".ToCharArray());
                }
                public Int64 Getnext() { return Next; }
                public Preflist(String item)
                {
                    name = item;
                }
                public String GetMembers()
                {
                    String ans = null;
                    list.ForEach(delegate (Int64 item)
                    {
                        if (ans != null)
                        {
                            ans += "," + name + item.ToString();
                        }
                        else
                        {
                            ans = name + item.ToString();
                        }
                    });
                    return ans;
                }
            }
            public PrefsData()
            {
                for (User_prefs i = User_prefs.accoount; i <= User_prefs.smtpserver; i++)
                {
                    Preflists.Add(i, new Preflist(Get_User_prefs_name(i)));
                }
            }
        }
    }
}
