using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Task_1
{
    internal static class DbContext<T>
    {
        private static string _connectionString = "";
        static DbContext()
        //Статичний конструктор, який одразу під'єднує до бази даних за вказаною адресою
        {
            _connectionString = @"Server=WINDOWS-SFL8F22\SQLEXPRESS02;Database=AstronomicalCatalogue;Trusted_Connection=True;TrustServerCertificate=True;";
        }
        public static void AddData(T obj)
        //Додати дані, використовуючи лише об'єкт
        {
            var tableName = obj.GetType().GetTypeInfo().Name;
            PropertyInfo[] columns = obj.GetType().GetTypeInfo().GetProperties();        
            StringBuilder sqlExpression = new StringBuilder("INSERT INTO ");
            sqlExpression.Append(tableName + " (");
            List<string> cols = new();
            for (int i = 1; i < columns.Length; ++i)
            {
                cols.Add(columns[i].Name); 
            }
            sqlExpression.AppendJoin(", ", cols.ToArray());
            sqlExpression.Append(") VALUES (");
            cols.Clear();
            for (int i = 1; i < columns.Length; ++i)
            {
                var value = columns[i].GetValue(obj);
                if (value.GetType() == typeof(string))
                {
                    cols.Add($"'{value}'");
                }
                else
                {
                    cols.Add(value.ToString());
                }
            }
            sqlExpression.AppendJoin(", ", cols);
            sqlExpression.Append(")");
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression.ToString(), connection);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public static void UpdateData(string setExpression, string whereExpression)
        //Оновлення даних, використовуючи змінну "SET" та умову "WHERE"
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlExpression = $"UPDATE {typeof(T).GetTypeInfo().Name} SET {setExpression} WHERE {whereExpression}";
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
            }
        }
        public static void RemoveData(string whereExpression)
        //Видалення даних, використовуючи умову "WHERE"
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlExpression = $"DELETE FROM {typeof(T).GetTypeInfo().Name} WHERE {whereExpression}";
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
            }
        } 
        public static string GetStringData()
        //Виведення даних у стрічку. Можна буде зробити ще й виведення списку об'єктів конкретного класу в доданок, за потреби.
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlExpression = $"SELECT * FROM {typeof(T).GetTypeInfo().Name}";
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader dataReader = command.ExecuteReader();
                StringBuilder result = new StringBuilder("|| ");
                if (dataReader.HasRows)
                {
                    for (int i = 0; i < dataReader.FieldCount; ++i)
                    {
                        result.Append(dataReader.GetName(i) + " || ");
                    }
                    result.Append("\n");
                    while (dataReader.Read())
                    {
                        result.Append("\n|| ");
                        for (int i = 0; i < dataReader.FieldCount; ++i)
                        {
                            result.Append(dataReader.GetValue(i).ToString() + " || ");
                        }                   
                    }
                }
                return result.ToString();
            }
        }
    }
}
