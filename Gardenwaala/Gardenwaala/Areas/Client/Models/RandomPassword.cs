using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gardenwaala.Areas.Client.Models
{
    public class RandomPassword
    {
        public static string Generate()
        {
            string allowedChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ#$%@*+=";
            Random randNum = new Random();
            char[] passChars = new char[6];
            int allowedCharCnt = allowedChars.Length;
            for (int i = 0; i < 6; i++)
            {
                passChars[i] = allowedChars[(int)(allowedCharCnt * randNum.NextDouble())];
            }
            return new string(passChars);
        }
    }
}