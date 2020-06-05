using Common;
using Demos.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /*
     * 港交所密码规则：
     * Length is 8 characters.
     Must comprise of a mix of alphabets (A-Z and a-z) and digits (0-9)
     Must be changed on first-time logon or first logon after reset from HKEX market operations.
     New password can’t be one of the previous 5 passwords.
     Can’t be changed more than once per day.
     Session will be locked after 3 consecutive invalid passwords
     Expires every 90 days.
     */
    public class AlterPassword
    {
        //CfgManager _cfg = CfgManager.getInstance("StockAdapterHKEXCCCG.exe");
        public static AlterPassword Instance;
        const string _fileName = @"config/password.txt";

        private const short _differentFromPreviousCount = 5;

        public List<(DateTime AlterDate, string Password)> UsedPasswords { get; set; }

        /// <summary>
        /// 最新修改的密码，暂存，修改密码成功操作之后添加到UsedPasswords
        /// </summary>
        private string _latestPassword;
        //private char[] _digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        //private char[] _lowerLetter = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        //private char[] _upperLetter = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        static AlterPassword()
        {
            Instance = new AlterPassword();

        }
        AlterPassword()
        {
            LoadOldPasswords();
        }
        void LoadOldPasswords()
        {
            UsedPasswords = new List<(DateTime AlterDate, string Password)>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);
            var content = TxtFile.ReadTxtFile(path);
            content.ForEach(p =>
            {
                var array = p.Split(',');
                (DateTime AlterDate, string Password) usedPassword = (DateTime.Parse(array[0]), array[1]);
                UsedPasswords.Add(usedPassword);
            });
        }


        /// <summary>
        /// 最多只保存5个密码
        /// </summary>
        public void SaveOldPasswords()
        {
            UsedPasswords.Add((DateTime.Now.Date, _latestPassword));
            UsedPasswords = UsedPasswords.OrderByDescending(p => p.AlterDate).Take(_differentFromPreviousCount).ToList();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);
            List<string> content = new List<string>();
            UsedPasswords.ForEach(p =>
            {
                content.Add($"{p.AlterDate.ToString("yyyy-MM-dd")},{p.Password}");
            });
            TxtFile.SaveTxtFile(filePath, content, FileMode.Create);
        }

        /// <summary>
        /// 校验是否需要改密码，并返回需要更改的密码
        /// 注：首次登陆强制改一次密码。此时本地修改密码记录文件为空。
        /// </summary>
        /// <returns>ture,新密码；false,null</returns>
        public bool CheckShouldAlterPassword(out string newPassword)
        {
            //bool should = false;
            //if (UsedPasswords.Count != 0)
            //{
            //    var latestDate = UsedPasswords.Max(p => p.AlterDate);
            //    var duraton = DateTime.Now.Date - latestDate;
            //    CfgManager cfg = CfgManager.getInstance(null);
            //    if (string.IsNullOrEmpty(cfg.AlterPasswordDays))
            //    {
            //        throw new Exception("CfgManager:Missing value of AlterPasswordDays ");
            //    }
            //    should = duraton.Days >= int.Parse(cfg.AlterPasswordDays);
            //}
            //else
            //{
            //    should = true;
            //}
            //newPassword = should ? GetNewPassword() : default(string);
            //return should;
            newPassword = GetNewPassword();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetNewPassword()
        {
            _latestPassword = CreateAndCheckNewPassword();
            return _latestPassword;
        }

        private string CreateAndCheckNewPassword()
        {
            string newPassword = CreateNewPassword(8);
            if (IsUsed(newPassword))
            {
                CreateAndCheckNewPassword();
            }

            return newPassword;
        }

        /// <summary>
        /// 生成passwordLength位大写、小写、数字组合的密码
        /// </summary>
        /// <param name="passwordLength">密码长度</param>
        /// <returns></returns>
        public string CreateNewPassword(int passwordLength)
        {
            //length=62:10+26+26=62
            //0-9 数字,10-35 小写,35-61 大写
            char[] _characters =
               {
            '0','1','2','3','4','5','6','7','8','9',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
            };

            //rd.Next(minValue, maxValue)==>[minValue, maxValue)
            Random rd = new Random();
            StringBuilder newPassword = new StringBuilder();
            List<int> allkinds = new List<int>();
            for (int i = 0; i < passwordLength; i++)
            {
                allkinds.Add(i);
            }
            List<int> kindSequence = new List<int>();

            //保证满足条件：必须是大写、小写、数字的组合。约定0:大写,1:小写,2:数字，其他随便。
            //随机组合allkinds成员的组合顺序。
            for (int i = 0; i < passwordLength; i++)
            {
                var index = rd.Next(0, allkinds.Count);
                var item = allkinds[index];
                kindSequence.Add(item);
                allkinds.Remove(item);
            }

            kindSequence.ForEach(p =>
            {
                switch (p)
                {
                    case 0://大写
                        newPassword.Append(_characters[rd.Next(35, 61 + 1)]);//大写
                        break;
                    case 1://小写
                        newPassword.Append(_characters[rd.Next(10, 35 + 1)]);//小写
                        break;
                    case 2://数字
                        newPassword.Append(_characters[rd.Next(0, 9 + 1)]);//数字
                        break;
                    default:
                        newPassword.Append(_characters[rd.Next(0, 62)]);
                        break;
                }
            });
            return newPassword.ToString();
        }

        private bool IsUsed(string newPassword)
        {
            return UsedPasswords.Exists(p => p.Password == newPassword);
        }
    }
}
