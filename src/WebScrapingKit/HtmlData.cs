﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapingKit
{
    public class HtmlData
    {
        public string Url { get; set; }
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value >= 0)
                {
                    _id = value;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
        public string[] Data { get; set; }
        public HtmlDataGetterResult Result { get; set; }

        public HtmlData(string url, int id, string[] data, HtmlDataGetterResult result)
        {
            Url = url;
            _id = id;
            if(data != null)
            {
                Data = new string[data.Length];
                data.CopyTo(Data, 0);
            }
            Result = result;
        }

        private int _id = -1;
    }
}
