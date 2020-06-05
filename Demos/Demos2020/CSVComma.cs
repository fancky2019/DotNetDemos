using Demos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Demos.Demos2020
{
    public class CSVComma
    {
        public void Test()
        {
            //TxtFileHelper.SaveTxtFile("datas/1.txt", new List<string>() { "1", "2" });

            DealData();
            var line = "NWD_C,8,O,\"NEW WORLD DEVELOPMENT CO., LTD\",N,,HKD,10.00,0.01,100,2,,,00017.HK,C,A";
        }

        private void DealData()
        {
            var path = @"C:\Users\Administrator\Desktop\StockOption.csv";
            var datas = TxtFileHelper.ReadTxtFile(path);
            List<List<string>> dataList = new List<List<string>>();
            datas.ForEach(p =>
            {
                //if(p.Contains("NWD"))
                //{

                //}
              
                var contentList = Split(p);
                dataList.Add(contentList);
            });

            List<List<string>> errorList = new List<List<string>>();
            dataList.ForEach(p =>
            {
                if (p.Count != 16)
                {
                    errorList.Add(p);
                }
            });
        }

        private List<string> Split(string line, char separator = ',')
        {
            return IsContentContainComma(line) ? DealCommaLine(line, separator) : line.Split(separator).ToList();
        }
        private bool IsContentContainComma(string line)
        {
            var pattern = ".*\\\".*.\\\".*";
            return Regex.IsMatch(line, pattern);
        }

        private List<string> DealCommaLine(string line, char separator = ',')
        {
            var quotationMark = "\"";
            var leftQuotationMarkIndex = line.IndexOf(quotationMark); ;
            var rightQuotationMarkIndex = line.IndexOf(quotationMark, leftQuotationMarkIndex + 1);
            var quotationMarkStr = line.Substring(leftQuotationMarkIndex, rightQuotationMarkIndex - leftQuotationMarkIndex + 1);
            var replaceStr = "!@#Comma123";
            var replacedLine = line.Replace(quotationMarkStr, replaceStr);
            var replaceList = replacedLine.Split(separator).ToList();
            var replaceStrIndex = replaceList.IndexOf(replaceStr);
            var removeQuotationMarkStr = quotationMarkStr.Trim('\"');
            replaceList[replaceStrIndex] = removeQuotationMarkStr;
            return replaceList;
        }
    }
}
