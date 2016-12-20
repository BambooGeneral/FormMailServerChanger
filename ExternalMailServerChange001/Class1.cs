using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
    public static class Thunderbird
    {
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
    }
    public partial class test2
    {

        private Thunderbird.Address thisAddress;

        private bool test()
        {
            String line;
            line = Thunderbird.Address.ProfileChoicer;
            thisAddress.SetProfile("Path=Profiles/sadhjfks.default".Split('/')[1]);
            return true;
        }
    }
}
