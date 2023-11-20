using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YamlDotNet.Serialization;

namespace DropOrganize.Utils
{
    public class ConfigHelper
    {
        public static readonly string filePath = "config.yml";

        public static void Test()
        {
            var serializer = new Serializer();
            //var text = serializer.Serialize(new { List = new List<int>() { 1, 2, 3 }, Name = "Hello", Value = "World!" });
            var r = GetRules();
            var text = serializer.Serialize(new
            {
                Rules = r
            });

            MessageBox.Show(text);
        }

        public static IList<Model.RuleItem> GetRules()
        {
            return new List<Model.RuleItem>()
            {
                new Model.RuleItem()
                {
                    Name = "歌曲",
                    ExtName = new List<string> { ".mp3", ".ogg" },
                    Location = @"C:\Users\admin\Desktop\test\音乐"
                },
                new Model.RuleItem()
                {
                    Name = "照片",
                    ExtName = new List<string> { ".png", ".jpg" },
                    Location = @"C:\Users\admin\Desktop\test\图片"
                },
                new Model.RuleItem()
                {
                    Name = "视频",
                    ExtName = new List<string> { ".mp4", ".mkv" },
                    Location = @"C:\Users\admin\Desktop\test\视频"
                },
                new Model.RuleItem()
                {
                    Name = "文件夹类型",
                    ExtName = new List<string> { "#DIR#" },
                    Location = @"C:\Users\admin\Desktop\test\文件夹"
                },
                new Model.RuleItem()
                {
                    Name = "未配置类型",
                    ExtName = new List<string> { "#OTHER#" },
                    Location = @"C:\Users\admin\Desktop\test\其他"
                }

            };
        }
    }
}
