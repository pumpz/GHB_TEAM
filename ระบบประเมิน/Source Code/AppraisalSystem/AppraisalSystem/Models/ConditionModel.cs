using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace AppraisalSystem.Models
{
    #region Models
    [Serializable]
    public class FilterModel
    {
        public string filter_type_code { set; get; }
        public string filter_type_name { set; get; }
        public string filter_value { set; get; }
        public string filter_text { set; get; }
        public string status { set; get; }
    }

    [Serializable]
    public class ProvinceModel
    {
        public int province_id { set; get; }
        public string province_code { set; get; }
        public string province_name { set; get; }
        public string province_name_eng { set; get; }
        public int geo_id { set; get; }
        public int status { set; get; }   
    }

    [Serializable]
    public class AmphurModel
    {
        public int amphur_id { set; get; }
        public string amphur_code { set; get; }
        public string amphur_name { set; get; }
        public string amphur_name_eng { set; get; }
        public int geo_id { set; get; }
        public int province_id { set; get; }
        public int status { set; get; }	
    }

    [Serializable]
    public class DistrictModel
    {
        public int district_id { set; get; }
        public string district_code { set; get; }
        public string district_name { set; get; }
        public int amphur_id { set; get; }
        public int province_id { set; get; }
        public int geo_id { set; get; }
        public int status { set; get; }
    }

    #endregion

    #region Services
    public interface IConditionService
    {
        /// <detail>
        /// GetFilterLists
        /// </detail>
        /// <returns>List<FilterModel></returns>
        List<FilterModel> GetFilterLists();

        /// <detail>
        /// GetProvinceLists
        /// </detail>
        /// <returns>List<ProvinceModel></returns>
        List<ProvinceModel> GetProvinceLists();

        /// <detail>
        /// GetAmphurLists
        /// </detail>
        /// <returns>List<AmphurModel></returns>
        List<AmphurModel> GetAmphurLists();

        /// <detail>
        /// GetDistrictLists
        /// </detail>
        /// <returns>List<DistrictModel></returns>
        List<DistrictModel> GetDistrictLists();
    }

    public class ConditionService : IConditionService
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings
                ["ConnectionString"].ConnectionString;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<FilterModel> GetFilterLists()
        {
            MySqlConnection conn = null;
            List<FilterModel> filterList = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.VIEW_FILTER, conn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (dr.HasRows)
                            {
                                filterList = new List<FilterModel>();
                                while (dr.Read())
                                {
                                    FilterModel FilterItem = new FilterModel();
                                    FilterItem.filter_type_code = dr["filter_type_code"] == System.DBNull.Value ? "" : Convert.ToString(dr["filter_type_code"]);
                                    FilterItem.filter_type_name = dr["filter_type_name"] == System.DBNull.Value ? "" : Convert.ToString(dr["filter_type_name"]);
                                    FilterItem.filter_value = dr["filter_value"] == System.DBNull.Value ? "" : Convert.ToString(dr["filter_value"]);
                                    FilterItem.filter_text = dr["filter_text"] == System.DBNull.Value ? "" : Convert.ToString(dr["filter_text"]);

                                    filterList.Add(FilterItem);
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException ms)
            {
                throw new Exception("MySqlException: " + ms.Message);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return filterList;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ProvinceModel> GetProvinceLists()
        {
            MySqlConnection conn = null;
            List<ProvinceModel> provinceList = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.VIEW_PROVINCE, conn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (dr.HasRows)
                            {
                                provinceList = new List<ProvinceModel>();
                                while (dr.Read())
                                {
                                    ProvinceModel ProvinceItem = new ProvinceModel();
                                    ProvinceItem.province_id = dr["province_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["province_id"]);
                                    ProvinceItem.province_code = dr["province_code"] == System.DBNull.Value ? "" : Convert.ToString(dr["province_code"]);
                                    ProvinceItem.province_name = dr["province_name"] == System.DBNull.Value ? "" : Convert.ToString(dr["province_name"]);

                                    provinceList.Add(ProvinceItem);
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException ms)
            {
                throw new Exception("MySqlException: " + ms.Message);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return provinceList;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<AmphurModel> GetAmphurLists()
        {
            MySqlConnection conn = null;
            List<AmphurModel> amphurList = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.VIEW_AMPHUR, conn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (dr.HasRows)
                            {
                                amphurList = new List<AmphurModel>();
                                while (dr.Read())
                                {
                                    AmphurModel AmphurItem = new AmphurModel();
                                    AmphurItem.amphur_id = dr["amphur_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["amphur_id"]);
                                    AmphurItem.amphur_code = dr["amphur_code"] == System.DBNull.Value ? "" : Convert.ToString(dr["amphur_code"]);
                                    AmphurItem.amphur_name = dr["amphur_name"] == System.DBNull.Value ? "" : Convert.ToString(dr["amphur_name"]);
                                    AmphurItem.province_id = dr["province_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["province_id"]);

                                    amphurList.Add(AmphurItem);
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException ms)
            {
                throw new Exception("MySqlException: " + ms.Message);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return amphurList;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<DistrictModel> GetDistrictLists()
        {
            MySqlConnection conn = null;
            List<DistrictModel> districtList = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.VIEW_DISTRICT, conn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (dr.HasRows)
                            {
                                districtList = new List<DistrictModel>();
                                while (dr.Read())
                                {
                                    DistrictModel DistrictItem = new DistrictModel();
                                    DistrictItem.district_id = dr["district_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["district_id"]);
                                    DistrictItem.district_code = dr["district_code"] == System.DBNull.Value ? "" : Convert.ToString(dr["district_code"]);
                                    DistrictItem.district_name = dr["district_name"] == System.DBNull.Value ? "" : Convert.ToString(dr["district_name"]);
                                    DistrictItem.amphur_id = dr["amphur_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["amphur_id"]);
                                    DistrictItem.province_id = dr["province_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["province_id"]);

                                    districtList.Add(DistrictItem);
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException ms)
            {
                throw new Exception("MySqlException: " + ms.Message);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return districtList;
        }
    }
    #endregion

    #region Validation
    #endregion
}