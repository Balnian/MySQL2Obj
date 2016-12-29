using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MySQL2ObjWrapper
{
    public class MySQL2Obj:IMySQL2Obj
    {
        private String ConnectionString;
        public MySQL2Obj(string connectionString) //: base(connectionString)
        {
            ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        protected static List<T> ParseReaderToTypeList<T>(System.Data.Common.DbDataReader reader) where T : new()
        {
            Type type = typeof(T);
            List<T> output = new List<T>();
            while (reader.Read())
            {
                // Create instance of the type
                T inst = new T();
                foreach (var propertie in type.GetProperties())
                {
                    //find the value or NULL
                    var value = reader?[propertie.Name];
                    //put it in the instance
                    type.GetRuntimeProperty(propertie.Name).SetValue(inst, value);
                    // add to output
                }
                output.Add(inst);

            }
            reader.Dispose();
            return output;
        }

        public async Task<List<T>> QueryAsync<T>(string SQLQuery, Dictionary<string, object> Params) where T : new()
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = SQLQuery;
                    //Add parameters
                    foreach (var item in Params)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        return ParseReaderToTypeList<T>(reader);
                    }
                }
            }
        }

        public async Task<List<T>> QueryAsync<T>(string SQLQuery) where T : new()
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = SQLQuery;
                    //Add parameters
                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        return ParseReaderToTypeList<T>(reader);
                    }
                }
            }
        }

        public List<T> Query<T>(string SQLQuery, Dictionary<string, object> Params) where T : new()
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = SQLQuery;
                    //Add parameters
                    foreach (var item in Params)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        return ParseReaderToTypeList<T>(reader);
                    }
                }
            }
        }

        public List<T> Query<T>(string SQLQuery) where T : new()
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = SQLQuery;
                    //Add parameters
                    using (var reader = cmd.ExecuteReader())
                    {
                        return ParseReaderToTypeList<T>(reader);
                    }
                }
            }
        }

        public async Task<int> QueryAsync(string SQLQuery, Dictionary<string, object> Params) 
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = SQLQuery;
                    //Add parameters
                    foreach (var item in Params)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> QueryAsync(string SQLQuery)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = SQLQuery;
                    //Add parameters
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public int Query(string SQLQuery, Dictionary<string, object> Params) 
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = SQLQuery;
                    //Add parameters
                    foreach (var item in Params)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int Query(string SQLQuery)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = SQLQuery;
                    return cmd.ExecuteNonQuery();

                }
            }
        }
    }
}
