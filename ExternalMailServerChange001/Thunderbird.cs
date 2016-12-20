using System;
using System.Collections.Generic;

namespace ExternalMailServerChange001
{
    public static class Thunderbird
    {

        // enum定義
        public enum User_prefs { accoount, identity, server, smtpserver };

        // Gender に対する拡張メソッドの定義
        public static string Get_User_prefs(this User_prefs gender)
        {
            string[] names = { "mail.account.account", "mail.identity.", "mail.server.server", "mail.smtpserver." };
            return "user_pref(\"" + names[(int)gender];
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

                public long Last { get => last; }
                public long Old { get => old; }
                public long Next { get => next; }

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
            }
            public PrefsData()
            {
                Preflists.Add(User_prefs.accoount, new Preflist());
                Preflists.Add(User_prefs.identity, new Preflist());
                Preflists.Add(User_prefs.server, new Preflist());
                Preflists.Add(User_prefs.smtpserver, new Preflist());
            }
        }
    }
}