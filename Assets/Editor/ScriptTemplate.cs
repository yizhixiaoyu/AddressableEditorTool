using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptTemplate
{
    public const string ConfigScript =
         @"
using UnityEngine;

namespace ToolModul
{
    [Config]
    public class 类名Category : ACategory<TestData>
    {
       
    }

    public class 类名 : IConfig
    {
        public string Name { get ; set ; }

        public int Id { get; set; }
       
    }
}

";


    public const string ConfigType=
        @"using System;
using System.Collections.Generic;

namespace ToolModul
{
    public static class ConfigType
    {
		面板
	}
}";
}
