using PuppyLearn.Models;

namespace PuppyLearn.Utilities
{
    public static class Global
    {
        
        public static string GetAccountTypeStr(int index)
        {
            if (index == 1)
            {
                return Roles.normalUser;
            }
            else if (index == 2) {
                return Roles.vip;
            }
            else if (index == 3)
            {
                return Roles.teacher;
            }
            else if (index == 4)
            {
                return Roles.admin;
            }
            else
            {
                return Roles.superAdmin;
            }
        }

        public static string GetCatalog(string bookNameCh) 
        {
            if (bookNameCh == "初中英语词汇")
            {
                return "中考";
            }
            else if (bookNameCh == "高中英语词汇")
            {
                return "高考";
            }
            else if (bookNameCh == "考研英语词汇")
            {
                return "考研";
            }
            else if (bookNameCh.Contains("初中")) 
            {
                return "初中";
            }
            else if (bookNameCh.Contains("小学"))
            {
                return "小学";
            }
            else if(bookNameCh.Contains("高中"))
            {
                return "高中";
            }
            else if (bookNameCh.Contains("GRE") || bookNameCh.Contains("SAT") || bookNameCh.Contains("TOEFL")|| bookNameCh.Contains("GMAT")|| bookNameCh.Contains("IELTS"))
            {
                return "留学/移民";
            }
            else if(bookNameCh.Contains("四级") || bookNameCh.Contains("专八") || bookNameCh.Contains("六级"))
            {
                return "大学";
            }
            else
            {
                return "商务";
            }
        }

        public static string GetBookNameCh(string bookName) 
        {
            var BookNameBookNameChMaping = new Dictionary<string, string>()
            {
                #region 小学
                { "RJXiaoXue3_1", "人教版小学英语-三年级上册"},
                { "RJXiaoXue3_2", "人教版小学英语-三年级下册"},
                { "RJXiaoXue4_1", "人教版小学英语-四年级上册"},
                { "RJXiaoXue4_2", "人教版小学英语-四年级下册"},
                { "RJXiaoXue5_1", "人教版小学英语-五年级上册"},
                { "RJXiaoXue5_2", "人教版小学英语-五年级下册"},
                { "RJXiaoXue6_1", "人教版小学英语-六年级上册"},
                { "RJXiaoXue6_2", "人教版小学英语-六年级下册"},
                #endregion
                #region 初中
                { "RJChuZhong7_1", "人教版初中英语-七年级上册"},
                { "RJChuZhong7_2", "人教版初中英语-七年级下册"},
                { "RJChuZhong8_1", "人教版初中英语-八年级上册"},
                { "RJChuZhong8_2", "人教版初中英语-八年级下册"},
                { "RJChuZhong9", "人教版初中英语-九年级全册"},
                { "WYSChuZhong7_1", "外研社版初中英语-七年级上册"},
                { "WYSChuZhong7_2", "外研社版初中英语-七年级下册"},
                { "WYSChuZhong8_1", "外研社版初中英语-八年级上册"},
                { "WYSChuZhong8_2", "外研社版初中英语-八年级下册"},
                { "WYSChuZhong9_1", "外研社版初中英语-九年级上册"},
                { "WYSChuZhong9_2", "外研社版初中英语-九年级下册"},
                #endregion
                #region 高中
                {"RJGaoZhong_6","人教版高中英语-选修六" },
                {"RJGaoZhong_5","人教版高中英语-必修五" },
                {"BeiShiGaoZhong_8","北师大版高中英语-选修八" },
                {"RJGaoZhong_9","人教版高中英语-选修九" },
                {"RJGaoZhong_4","人教版高中英语-必修四" },
                {"BeiShiGaoZhong_9","北师大版高中英语-选修九" },
                {"BeiShiGaoZhong_6","北师大版高中英语-选修六" },
                {"BeiShiGaoZhong_7","北师大版高中英语-选修七" },
                {"RJGaoZhong_10","人教版高中英语-选修十" },
                {"RJGaoZhong_7","人教版高中英语-选修七" },
                {"BeiShiGaoZhong_10","北师大版高中英语-选修十" },
                {"RJGaoZhong_11","人教版高中英语-选修十一" },
                {"BeiShiGaoZhong_1","北师大版高中英语-必修一" },
                {"BeiShiGaoZhong_4","北师大版高中英语-必修四" },
                {"RJGaoZhong_3","人教版高中英语-必修三" },
                {"BeiShiGaoZhong_3","北师大版高中英语-必修三" },
                {"RJGaoZhong_1","人教版高中英语-必修一" },
                {"BeiShiGaoZhong_2","北师大版高中英语-必修二" },
                {"BeiShiGaoZhong_5","北师大版高中英语-必修五" },
                {"RJGaoZhong_8","人教版高中英语-选修八" },
                {"RJGaoZhong_2","人教版高中英语-必修二" },
                {"BeiShiGaoZhong_11","北师大版高中英语-选修十一" },
                #endregion
                #region 中考
                {"ChuZhong","初中英语词汇" },
                #endregion
                #region 高考
                {"GaoKao","高中英语词汇" },
                #endregion
                #region 考研
                {"KaoYan","考研英语词汇" },
                #endregion
                #region 留学/移民
                {"GRE","GRE词汇" },
                {"SAT","SAT词汇" },
                {"TOEFL","TOEFL词汇" },
                {"GMAT","GMAT词汇" },
                {"IELTS","IELTS词汇" },
                #endregion
                #region 大学
                {"CET4","四级英语词汇" },
                {"Level8","专八核心词汇" },
                {"CET6","六级词汇" },
                {"Level4","四级核心词汇" },
                #endregion
                #region 商务
                {"BEC","商务英语词汇" }
                #endregion
            };
            BookNameBookNameChMaping.TryGetValue(bookName, out string res);
            return res;
        }

        /// <summary>
        /// call this in case a logging on user doesn't has its own userSettings.
        /// </summary>
        /// <returns></returns>
        public static UserSettings GetDefaultUserSettings()
        {
            return new UserSettings
            {
                audioAutoPlay = true,
                wordsPerRound = 20
            };
        }

        enum wordStatus
        {
            TotalNew = 0,
            Status1 = 1,
            Status2 = 2,
            Status3 = 3
        } 
    }

    static class ListExtension
    {
        public static T RemoveThenReturn<T>(this List<T> list, int index)
        {
            T element = list[index];
            list.RemoveAt(index);
            return element;
        }
    }
}
