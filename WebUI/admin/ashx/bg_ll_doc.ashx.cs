using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

namespace ZGZY.WebUI.admin.ashx
{
    /// <summary>
    /// bg_ll_doc 的摘要说明
    /// </summary>
    public class bg_ll_doc : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            
        }

        public float hex_to_float(string hex_str)
        {
            MatchCollection matches = Regex.Matches(hex_str, @"[0-9A-Fa-f]{2}");
            byte[] bytes = new byte[matches.Count];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = byte.Parse(matches[i].Value, NumberStyles.AllowHexSpecifier);
            float m = BitConverter.ToSingle(bytes, 0);
            return m;
        }

        public string float_to_hex(float f)
        {
            byte[] floatValues = BitConverter.GetBytes(f);
            string temp = BitConverter.ToString(floatValues).Replace("-", "");
            return temp;

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}