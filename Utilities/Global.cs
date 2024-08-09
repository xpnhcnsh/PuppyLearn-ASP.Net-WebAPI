using Microsoft.EntityFrameworkCore;
using PuppyLearn.Models;
using System.Linq.Expressions;
using System.Reflection;

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

        public static async Task<IEnumerable<T>> ApplyFilterAsync<T>(this IQueryable<T> query, Dictionary<string, List<FilterMetadata>> filters, int skip, int take, string sortField, bool sortOrder)
        {
            try
            {
                foreach (var filter in filters)
                {
                    if (filter.Value.All(x => x.Value != null))
                    {
                        query = query.Where(GetWhereExpressions<T>(filter));
                    }
                }
                query = sortOrder ?
                    query.OrderBy(GetSortExpression<T>(sortField)) : query.OrderByDescending(GetSortExpression<T>(sortField));
                return await query.Skip(skip).Take(take).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// return expression like "bookNameCh=='IELTS'", Value:'IELTS', PropName:'bookNameCh',MatchMode:'=='
        /// </summary>
        /// <param name="value"></param>
        /// <param name="PropName"></param>
        /// <param name="MatchMode"></param>
        /// <param name="Constant"></param>
        /// <returns></returns>
        /// <exception cref="InvalidFilterCriteriaException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private static Expression GetFilterExpression(object Value,Expression PropName,string MatchMode)
        {
            try
            {
                var targetType = PropName.Type;
                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    targetType = Nullable.GetUnderlyingType(targetType);
                //"IELTS英语词汇" constExpression
                object V = Value switch
                {
                    "已处理" or "1" => true,
                    "未处理" or "0" => false,
                    _ => Value,
                };
                Value = V;
                ConstantExpression constExpression = Expression.Constant(Convert.ChangeType(Value, targetType!), PropName.Type);
                switch (MatchMode)
                {
                    case "contains":
                        if (Value.GetType() != typeof(string))
                            throw new InvalidFilterCriteriaException();
                        var containsMethodInfo = typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) });
                        return Expression.Call(PropName, containsMethodInfo!, constExpression);
                    case "notContains":
                        if (Value.GetType() != typeof(string))
                            throw new InvalidFilterCriteriaException();
                        var notContainsMethodInfo = typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) });
                        return Expression.Not(Expression.Call(PropName, notContainsMethodInfo!, constExpression));
                    case "startsWith":
                        if (Value.GetType() != typeof(string))
                            throw new InvalidFilterCriteriaException();
                        var startsWithMethodInfo = typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) });
                        return Expression.Call(PropName, startsWithMethodInfo!, constExpression);
                    case "endsWith":
                        if (Value.GetType() != typeof(string))
                            throw new InvalidFilterCriteriaException();
                        var endsWithMethodInfo = typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) });
                        return Expression.Call(PropName, endsWithMethodInfo!, constExpression);
                    case "equals":
                        return Expression.Equal(PropName, constExpression);
                    case "notEquals":
                        return Expression.Not(Expression.Equal(PropName, constExpression));
                    case "dateIs":
                        PropName = Expression.PropertyOrField(PropName, "Date");
                        return Expression.Equal(PropName, constExpression);
                    case "dateIsNot":
                        PropName = Expression.PropertyOrField(PropName, "Date");
                        return Expression.Not(Expression.Equal(PropName, constExpression));
                    case "dateBefore":
                        PropName = Expression.PropertyOrField(PropName, "Date");
                        return Expression.LessThanOrEqual(PropName, constExpression);
                    case "dateAfter":
                        PropName = Expression.PropertyOrField(PropName, "Date");
                        return Expression.GreaterThanOrEqual(PropName, constExpression);
                    default:
                        throw new InvalidOperationException($"Unsupported MatchMode: {MatchMode}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static Expression<Func<T, bool>> GetWhereExpressions<T>(KeyValuePair<string, List<FilterMetadata>> filters)
        {
            try
            {
                //x=>
                var paramter = Expression.Parameter(typeof(T), "x");
                Expression? filterExpression = null;
                if (filters.Value.Any(x => x.Value != null))
                {
                    //这里如果key是"wordName","bookName"或"userEmail"，必须改成"Word.WordName","Word.Book.BookNameCh"和"User.Email";
                    string field = filters.Key switch
                    {
                        "wordName" => "Word.WordName",
                        "bookNameCh" => "Word.Book.BookNameCh",
                        "userEmail" => "User.Email",
                        "reportDate" => "SubmitTime",
                        _ => filters.Key,
                    };
                    //x.Word.WordName
                    Expression propName = paramter;
                    foreach (var memeber in field.Split("."))
                    {
                        propName = Expression.PropertyOrField(propName, memeber);
                    }

                    if (filters.Value.Count == 1 && filters.Value[0].Value != null)
                    {
                        filterExpression = GetFilterExpression(filters.Value[0].Value!, propName, filters.Value[0].MatchMode!);
                    }
                    else if (filters.Value.Count == 2 && filters.Value.All(x => x.Value != null))
                    {
                        var op = filters.Value.Select(x => x.Operator).Distinct().Single();
                        Expression leftExpression = GetFilterExpression(filters.Value[0].Value!, propName, filters.Value[0].MatchMode!);
                        Expression rightExpression = GetFilterExpression(filters.Value[1].Value!, propName, filters.Value[1].MatchMode!);
                        if (op == "and")
                        {
                            filterExpression = Expression.AndAlso(leftExpression, rightExpression);
                        }
                        else if (op == "or")
                        {
                            filterExpression = Expression.OrElse(leftExpression, rightExpression);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Unsupported Operator: {op}");
                        }
                    }
                }
                return Expression.Lambda<Func<T, bool>>(filterExpression!, paramter);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static Expression<Func<T, object>> GetSortExpression<T>(string sortField)
        {
            string field = sortField switch
            {
                "reportDate" => "SubmitTime",
                _=>sortField
            };
            var prop = Expression.Parameter(typeof(T),"x");
            var property = Expression.PropertyOrField(prop, field);
            var res = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), prop);
            return res;
        }
    }

    static class ListExtension
    {
        /// <summary>
        /// list extension method, remove item at index and return the result list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T RemoveThenReturn<T>(this List<T> list, int index)
        {
            T element = list[index];
            list.RemoveAt(index);
            return element;
        }
    }
}
