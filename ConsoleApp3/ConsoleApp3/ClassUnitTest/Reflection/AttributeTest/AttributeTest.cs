using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ConsoleApp3.AttributeTest
{
    public class AttributeTest
    {
        public void RunTest()
        {
            var dataParserTest = new Test_DataParser();
            dataParserTest.RunTest();
        }

        public class Test_DataParser
        {
            public void RunTest()
            {
                /// <see cref="BaseDataParserAttribute"/> 를 가지는 모든 Type 들을 가져옴 
                /// 즉 , Parser 들을 가져온다 
                var parsers = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.GetCustomAttribute(typeof(BaseDataParserAttribute)) != null);
                Dictionary<Type, List<object>> dataList = new Dictionary<Type, List<object>>();

                // Parser 순회 
                foreach (var type in parsers)
                {
                    // Public 이며 Static 인 Parse 라는 함수가 있는지 검사 
                    var parsingMethod = type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static);
                    if (parsingMethod == null)
                    {
                        Console.WriteLine($"Parser {type} does not have matching Parse Method");
                        continue;
                    }

                    // Parameter 가 없어야함 
                    if (parsingMethod.GetParameters().Length > 0)
                    {
                        Console.WriteLine($"Parse Method should not have parameter");
                        continue;
                    }

                    var att = type.GetCustomAttribute<BaseDataParserAttribute>();

                    /// 해당 BaseData 타입이 <see cref="BaseData"/> 의 파생 클래스가 맞는지 체크
                    if (att.BaseDataType.IsSubclassOf(typeof(BaseData)) == false)
                    {
                        Console.WriteLine($"Following type {att.BaseDataType} is not derived from {nameof(BaseData)}");
                        continue;
                    }

                    dataList.Add(att.BaseDataType, null);

                    try
                    {
                        // Parameter 없는 Static 함수이기에 다음과 같이 호출 가능 
                        dataList[att.BaseDataType] = (List<object>)parsingMethod.Invoke(null, null);
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine(exp);
                    }

                    Console.WriteLine("--------------------------------------------------------");
                }
            }

            // --------------------------------------------------------- // 

            [AttributeUsage(AttributeTargets.Class)]
            public class BaseDataParserAttribute : System.Attribute
            {
                public System.Type BaseDataType;

                public BaseDataParserAttribute(System.Type baseDataType)
                {
                    this.BaseDataType = baseDataType;
                }
            }

            #region ====:: Table 데이터 구조 정의 ::====

            /// <summary> 모든 Table 데이터들의 공통 베이스 클래스 </summary>
            public abstract class BaseData
            {
                public int ID;
            }

            public class TableData01 : BaseData
            {
                public int ATK;
            }

            public class TableData02 : BaseData
            {
                public int HP;
            }

            #endregion

            #region ====:: Parser 정의 ::====

            [BaseDataParser(typeof(TableData01))]
            public class Parser_TableData01
            {
                public static List<object> Parse()
                {
                    Console.WriteLine("Parse TableData01 !! ");

                    return new List<object>()
                    {
                        new TableData01() { ID = 1, ATK = 10 }
                        ,new TableData01(){ ID = 2, ATK = 20 }
                    };
                }
            }

            [BaseDataParser(typeof(TableData02))]
            public class Parser_TableData02
            {
                public static List<object> Parse()
                {
                    Console.WriteLine("Parse TableData02 !! ");

                    return new List<object>()
                    {
                        new TableData02(){ ID = 1, HP = 10 }
                        , new TableData02() { ID = 2, HP = 20 }
                    };
                }
            }

            #endregion
        }
    }
}
