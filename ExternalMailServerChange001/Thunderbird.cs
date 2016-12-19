using System;
using System.Collections.Generic;

namespace ExternalMailServerChange001
{
    public static class Thunderbird
    {

        // enum定義
        public enum User_prefs { accountlastkey, accoount, identity, server, smtpserver };
        // Gender に対する拡張メソッドの定義
        public static string Get_User_prefs(this User_prefs gender)
        {
            string[] names = { "mail.account.lastKey", "mail.account.account", "mail.identity.", "mail.server.server", "mail.smtpserver." };
            return "user_pref(\"" + names[(int)gender];
        }

        public class Address
        {
            //Thunderbirdのユーザープロファイル設定フォルダ
            private static String thunderbirdProfileSettingFolder;

            //Thunderbirdのユーザープロファイル選定ファイル
            private static String thunderbirdProfileChoicer;

            //Thunderbirdのデフォルトユーザープロファイルフォルダ
            private String thunderbirdProfileFolder;

            //Thunderbirdのユーザープロファイル設定ファイル
            private String thunderbirdProfileSetting;

            public static string ThunderbirdProfileSettingFolder { get => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Thunderbird\"; }
            public static string ThunderbirdProfileChoicer { get => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Thunderbird\profiles.ini"; }
            public string ThunderbirdProfileFolder { get => thunderbirdProfileFolder; set => thunderbirdProfileFolder = value; }
            public string ThunderbirdProfileSetting { get => thunderbirdProfileSetting; set => thunderbirdProfileSetting = value; }
        }

        public class PrefsData
        {
            public class preflist
            {
                private List<long> list = new List<Int64>();
                private long old = 0;
                private long last = 0;
                private long next =0;
                public long Last
                {
                    get
                    {
                        list.Sort();
                        last= list[list.Count - 1];
                        return last;
                    }
                    set => last= value;
                }

                public long Old { get => old; set => old = value; }
                public void AddList(String item)
                {
                    list.Add(long.Parse(item.Replace("idsmtpserver", "")));
                }
                public Int64 Getnext() { return next; }
            }
            public preflist accountsGF;
            public preflist identitysGF;
            public preflist serversGF;
            public preflist smtpsGF;
        }
    }
}