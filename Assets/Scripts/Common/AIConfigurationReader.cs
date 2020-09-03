using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//给命名空间起别名 常用 或 长
//using Sys = System;
//using GenericNS = System.Collections.Generic;
//给数据类型起别名 常用 或 长
using myint=System.Int32; 
using ConfigDic = System.Collections.Generic.Dictionary
<string, System.Collections.Generic.Dictionary<string, string>>;
namespace AI.FSM
{
    public static class AIConfigurationReader
    {
        //把 配置文件中的信息 加载到 字典中（字典嵌套）
        public static ConfigDic Load(string aiConfigFile)
        {           
            //1> 构造 配置文件的完整路径
            aiConfigFile = Path.Combine(Application.streamingAssetsPath, aiConfigFile);
            //2>
            if (Application.platform != RuntimePlatform.Android)
                aiConfigFile = "file://" + aiConfigFile;
            WWW www = new WWW(aiConfigFile);
            //3>
            while (true)
            {   if (!string.IsNullOrEmpty(www.error))
                    throw new Exception("AI配置文件读取异常");
                if (www.isDone)
                    return BuildDic(www.text);
            }
        }      
        /// 处理所有数据    
        private static ConfigDic BuildDic(string lines)
        {
            ConfigDic dic = new ConfigDic();
            string mainKey = null; //主键
            string subKey = null;  //子键
            string subValue = null;//值
            StringReader reader = new StringReader(lines);
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();//去除空白行
                if (!string.IsNullOrEmpty(line))
                {   //取主键 如 [Idle] 》》Idle
                    if (line.StartsWith("["))
                    {
                        mainKey = line.Substring(1, line.IndexOf("]") - 1);
                        
                        dic.Add(mainKey, new Dictionary<string, string>());
                    }//取子键以及值
                    else
                    {
                        var configValue = line.Split('>');
                        subKey = configValue[0].Trim();
                        subValue = configValue[1].Trim();
                        dic[mainKey].Add(subKey, subValue);
                    }
                }
            }
            return dic;
        }
    }
}


