using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using AppraisalSystem.Utility;
using System.Collections;

namespace AppraisalSystem.Models
{
    #region Models
    [Serializable]
    public class AppraisalListsModel
    {
        public int appraisal_assets_id { set; get; }
        public string appraisal_assets_code { set; get; }
        public string alley { set; get; }
        public string road { set; get; }
        public int district_id { set; get; }
        public string district_name { set; get; }
        public string amphur_name { set; get; }
        public string province_name { set; get; }
        public string detailed_location { set; get; }
        public string asset_type { set; get; }
        public string assessment_methods { set; get; }
        public string rights_of_access { set; get; }
        public string paint_the_town { set; get; }
        public int status { set; get; }      
        public DateTime create_date {set; get;}
        public string create_by { set; get; }
        public string type_of_document { set; get; }
        public string no { set; get; }
        public string survey { set; get; }
        public string book_or_page { set; get; }
        public string tc_no { set; get; }
        public string condition_land { set; get; }
        public int courting_the_burden { set; get; }
        public string ownership { set; get; }
        public string rightsholder { set; get; }
        public int rai_area { set; get; }
        public int ngaan_area { set; get; }
        public int wa_area { set; get; }
        public double total_area { set; get; }
        public double appraisal { set; get; }
        public string no_buildings { set; get; }
        public string building_type { set; get; }
        public string floor { set; get; }
        public string condition_building { set; get; }
        public double building_age { set; get; }
        public string structure { set; get; }
        public string pole { set; get; }
        public string roof { set; get; }
        public string materials { set; get; }
        public int ceiling_id { set; get; }
        public string ceiling { set; get; }
        public string exterior_walls { set; get; }
        public string interior_walls { set; get; }
        public string stair { set; get; }
        public string room_1 { set; get; }
        public string room_2 { set; get; }
        public string room_3 { set; get; }
        public double width { set; get; }
        public double high { set; get; }
        public double price_per_meter { set; get; }
        public double depreciation { set; get; }
        public double survey_price { set; get; }
        public double appropriate_price { set; get; }
        public string data_source { set; get; }
        public string phone { set; get; }
        public double size_area { set; get; }
        public int shape_information_id { set; get; }
        public string shape_information { set; get; }
        public string environment { set; get; }
        public string characteristics_assets { set; get; }
        public string characteristics_access { set; get; }
        public string utilities { set; get; }
        public string terms { set; get; }
        public int liquidity_id { set; get; }
        public string liquidity { set; get; }
        public string sequence { set; get; }
        public string latitude { set; get; }
        public string longitude { set; get; }      

    }

    [Serializable]
    public class AppraisalJobModel
    {
        public int appraisal_assets_id { set; get; }
        public string appraisal_assets_code { set; get; }
        public string village { set; get; }
        public string alley { set; get; }
        public string road { set; get; }
        public int district_id { set; get; }
        public int amphur_id { set; get; }
        public int province_id { set; get; }
        public string detailed_location { set; get; }
        public int asset_type_id { set; get; }
        public int assessment_methods_id { set; get; }
        public int rights_of_access_id { set; get; }
        public int paint_the_town_id { set; get; }
        public int status { set; get; }
        public DateTime create_date { set; get; }
        public DateTime update_date { set; get; }
        public DateTime delete_date { set; get; }
        public string create_by { set; get; }
        public string update_by { set; get; }
        public string delete_by { set; get; }
    }

    [Serializable]
    public class AppraisalDetailModel
    {
    }

    [Serializable]
    public class CompareAssetModel
    {
    }

    [Serializable]
    public class LocationAssetModel
    {
    }

    [Serializable]
    public class MapAssetModel
    {
    }

    [Serializable]
    public class UploadPictureAssetModel
    {
    }
    #endregion

    #region Services
    public interface IAppraisalService
    {
        List<AppraisalListsModel> GetAppraisalLists(string appraisalCode, int provinceId, int amphurId, string createBy, bool viewOnly);
        List<AppraisalJobModel> GetAppraisalJob(string appraisalID, string keyword, string createBy);
        Hashtable MngAppraisalJob(AppraisalJobModel appraisalJob, string createBy);
        Boolean DeleteAppraisalJob(int appraisalJobId, string delBy);
    }

    public class AppraisalService : IAppraisalService
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings
                ["ConnectionString"].ConnectionString;
        }

        #region Select
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<AppraisalListsModel> GetAppraisalLists(string appraisalCode, int provinceId, int amphurId, string createBy, bool viewOnly)
        {
            MySqlConnection conn = null;
            List<AppraisalListsModel> result = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.VIEW_APPRAISAL_LIST, conn))
                    {
                        string condition = "";

                        if (ContentHelpers.IsNotnull(appraisalCode))
                        {
                            condition += string.Format(" AND APPRAISAL_ASSETS_CODE LIKE '%{0}%'", appraisalCode);
                        }

                        if (provinceId > 0)
                        {
                            condition += string.Format(" AND PROVINCE_ID = {0}", provinceId);
                        }

                        if (amphurId > 0)
                        {
                            condition += string.Format(" AND AMPHUR_ID = {0}", amphurId);
                        }

                        if (!viewOnly)
                        {
                            condition += string.Format(" AND CREATE_BY = {0}", createBy);
                        }
                        cmd.CommandText += condition;
                        using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (dr.HasRows)
                            {
                                result = new List<AppraisalListsModel>();
                                while (dr.Read())
                                {
                                    AppraisalListsModel AppraisalItem = new AppraisalListsModel();
                                    AppraisalItem.appraisal_assets_id = dr["appraisal_assets_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["appraisal_assets_id"]);
                                    AppraisalItem.appraisal_assets_code = dr["appraisal_assets_code"] == System.DBNull.Value ? "" : Convert.ToString(dr["appraisal_assets_code"]);
                                    AppraisalItem.alley = dr["alley"] == System.DBNull.Value ? "" : Convert.ToString(dr["alley"]);
                                    AppraisalItem.road = dr["road"] == System.DBNull.Value ? "" : Convert.ToString(dr["road"]);
                                    AppraisalItem.district_name = dr["district_name"] == System.DBNull.Value ? "" : Convert.ToString(dr["district_name"]);
                                    AppraisalItem.amphur_name = dr["amphur_name"] == System.DBNull.Value ? "" : Convert.ToString(dr["amphur_name"]);                         
                                    AppraisalItem.province_name = dr["province_name"] == System.DBNull.Value ? "" : Convert.ToString(dr["province_name"]);
                                    AppraisalItem.detailed_location = dr["detailed_location"] == System.DBNull.Value ? "" : Convert.ToString(dr["detailed_location"]);
                                    AppraisalItem.asset_type = dr["asset_type"] == System.DBNull.Value ? "" : Convert.ToString(dr["asset_type"]);
                                    AppraisalItem.assessment_methods = dr["assessment_methods"] == System.DBNull.Value ? "" : Convert.ToString(dr["assessment_methods"]);
                                    AppraisalItem.rights_of_access = dr["rights_of_access"] == System.DBNull.Value ? "" : Convert.ToString(dr["rights_of_access"]);
                                    AppraisalItem.paint_the_town = dr["paint_the_town"] == System.DBNull.Value ? "" : Convert.ToString(dr["paint_the_town"]);
                                    AppraisalItem.status = dr["status"] == System.DBNull.Value ? 0 : Convert.ToInt16(dr["status"]);
                                    AppraisalItem.create_date = Convert.ToDateTime(dr["create_date"]);
                                    AppraisalItem.type_of_document = dr["type_of_document"] == System.DBNull.Value ? "" : Convert.ToString(dr["type_of_document"]);
                                    AppraisalItem.no = dr["no"] == System.DBNull.Value ? "" : Convert.ToString(dr["no"]);
                                    AppraisalItem.survey = dr["survey"] == System.DBNull.Value ? "" : Convert.ToString(dr["survey"]);
                                    AppraisalItem.book_or_page = dr["book_or_page"] == System.DBNull.Value ? "" : Convert.ToString(dr["book_or_page"]);
                                    AppraisalItem.tc_no = dr["tc_no"] == System.DBNull.Value ? "" : Convert.ToString(dr["tc_no"]);
                                    AppraisalItem.condition_land = dr["condition_land"] == System.DBNull.Value ? "" : Convert.ToString(dr["condition_land"]);
                                    AppraisalItem.courting_the_burden = dr["courting_the_burden"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["courting_the_burden"]);
                                    AppraisalItem.ownership = dr["ownership"] == System.DBNull.Value ? "" : Convert.ToString(dr["ownership"]);
                                    AppraisalItem.rightsholder = dr["rightsholder"] == System.DBNull.Value ? "" : Convert.ToString(dr["rightsholder"]);
                                    AppraisalItem.rai_area = dr["rai_area"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["rai_area"]);
                                    AppraisalItem.ngaan_area = dr["ngaan_area"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["ngaan_area"]);
                                    AppraisalItem.wa_area = dr["wa_area"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["wa_area"]);
                                    AppraisalItem.total_area = dr["total_area"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["total_area"]);
                                    AppraisalItem.appraisal = dr["appraisal"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["appraisal"]);
                                    AppraisalItem.no_buildings = dr["no_buildings"] == System.DBNull.Value ? "" : Convert.ToString(dr["no_buildings"]);
                                    AppraisalItem.building_type = dr["building_type"] == System.DBNull.Value ? "" : Convert.ToString(dr["building_type"]);
                                    AppraisalItem.floor = dr["floor"] == System.DBNull.Value ? "" : Convert.ToString(dr["floor"]);
                                    AppraisalItem.condition_building = dr["condition_building"] == System.DBNull.Value ? "" : Convert.ToString(dr["condition_building"]);
                                    AppraisalItem.building_age = dr["building_age"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["building_age"]);
                                    AppraisalItem.structure = dr["structure"] == System.DBNull.Value ? "" : Convert.ToString(dr["structure"]);
                                    AppraisalItem.pole = dr["pole"] == System.DBNull.Value ? "" : Convert.ToString(dr["pole"]);
                                    AppraisalItem.roof = dr["roof"] == System.DBNull.Value ? "" : Convert.ToString(dr["roof"]);
                                    AppraisalItem.materials = dr["materials"] == System.DBNull.Value ? "" : Convert.ToString(dr["materials"]);
                                    AppraisalItem.ceiling_id = dr["ceiling_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["ceiling_id"]);
                                    AppraisalItem.ceiling = dr["ceiling"] == System.DBNull.Value ? "" : Convert.ToString(dr["ceiling"]);
                                    AppraisalItem.exterior_walls = dr["exterior_walls"] == System.DBNull.Value ? "" : Convert.ToString(dr["exterior_walls"]);
                                    AppraisalItem.interior_walls = dr["interior_walls"] == System.DBNull.Value ? "" : Convert.ToString(dr["interior_walls"]);
                                    AppraisalItem.stair = dr["stair"] == System.DBNull.Value ? "" : Convert.ToString(dr["stair"]);
                                    AppraisalItem.room_1 = dr["room_1"] == System.DBNull.Value ? "" : Convert.ToString(dr["room_1"]);
                                    AppraisalItem.room_2 = dr["room_2"] == System.DBNull.Value ? "" : Convert.ToString(dr["room_2"]);
                                    AppraisalItem.room_3 = dr["room_3"] == System.DBNull.Value ? "" : Convert.ToString(dr["room_3"]);
                                    AppraisalItem.width = dr["width"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["width"]);
                                    AppraisalItem.high = dr["high"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["high"]);
                                    AppraisalItem.price_per_meter = dr["price_per_meter"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["price_per_meter"]);
                                    AppraisalItem.depreciation = dr["depreciation"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["depreciation"]);
                                    AppraisalItem.survey_price = dr["survey_price"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["survey_price"]);
                                    AppraisalItem.appropriate_price = dr["appropriate_price"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["appropriate_price"]);
                                    AppraisalItem.data_source = dr["data_source"] == System.DBNull.Value ? "" : Convert.ToString(dr["data_source"]);
                                    AppraisalItem.phone = dr["phone"] == System.DBNull.Value ? "" : Convert.ToString(dr["phone"]);
                                    AppraisalItem.size_area = dr["size_area"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["size_area"]);
                                    AppraisalItem.shape_information_id = dr["shape_information_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["shape_information_id"]);
                                    AppraisalItem.shape_information = dr["shape_information"] == System.DBNull.Value ? "" : Convert.ToString(dr["shape_information"]);
                                    AppraisalItem.environment = dr["environment"] == System.DBNull.Value ? "" : Convert.ToString(dr["environment"]);
                                    AppraisalItem.characteristics_assets = dr["characteristics_assets"] == System.DBNull.Value ? "" : Convert.ToString(dr["characteristics_assets"]);
                                    AppraisalItem.characteristics_access = dr["characteristics_access"] == System.DBNull.Value ? "" : Convert.ToString(dr["characteristics_access"]);
                                    AppraisalItem.utilities = dr["utilities"] == System.DBNull.Value ? "" : Convert.ToString(dr["utilities"]);
                                    AppraisalItem.terms = dr["terms"] == System.DBNull.Value ? "" : Convert.ToString(dr["terms"]);
                                    AppraisalItem.liquidity = dr["liquidity"] == System.DBNull.Value ? "" : Convert.ToString(dr["liquidity"]);
                                    AppraisalItem.sequence = dr["sequence"] == System.DBNull.Value ? "" : Convert.ToString(dr["sequence"]);
                                    AppraisalItem.latitude = dr["latitude"] == System.DBNull.Value ? "" : Convert.ToString(dr["latitude"]);
                                    AppraisalItem.longitude = dr["longitude"] == System.DBNull.Value ? "" : Convert.ToString(dr["longitude"]);                           
                                    result.Add(AppraisalItem);
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

            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<AppraisalJobModel> GetAppraisalJob(string appraisalID, string keyword, string createBy)
        {
            MySqlConnection conn = null;
            List<AppraisalJobModel> result = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.GET_APPRAISAL_JOB_BY_ID, conn))
                    {
                        string condition = "";

                        if (ContentHelpers.IsNotnull(appraisalID))
                        {
                            condition += string.Format(" AND APPRAISAL_ASSETS_ID = {0}", appraisalID);
                        }

                        if (ContentHelpers.IsNotnull(keyword))
                        {
                            condition += string.Format(" AND APPRAISAL_ASSETS_CODE LIKE '%{0}%'", keyword);
                        }

                        if (ContentHelpers.IsNotnull(createBy))
                        {
                            condition += string.Format(" AND CREATE_BY = {0}", createBy);
                        }
                        cmd.CommandText += condition;
                        using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (dr.HasRows)
                            {
                                result = new List<AppraisalJobModel>();
                                while (dr.Read())
                                {
                                    AppraisalJobModel AppraisalJobItem = new AppraisalJobModel();
                                    AppraisalJobItem.appraisal_assets_id = dr["appraisal_assets_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["appraisal_assets_id"]);
                                    AppraisalJobItem.appraisal_assets_code = dr["appraisal_assets_code"] == System.DBNull.Value ? "" : Convert.ToString(dr["appraisal_assets_code"]);
                                    AppraisalJobItem.village = dr["village"] == System.DBNull.Value ? "" : Convert.ToString(dr["village"]);
                                    AppraisalJobItem.alley = dr["alley"] == System.DBNull.Value ? "" : Convert.ToString(dr["alley"]);
                                    AppraisalJobItem.road = dr["road"] == System.DBNull.Value ? "" : Convert.ToString(dr["road"]);
                                    AppraisalJobItem.district_id = dr["district_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["district_id"]);
                                    AppraisalJobItem.amphur_id = dr["amphur_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["amphur_id"]);
                                    AppraisalJobItem.province_id = dr["province_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["province_id"]);
                                    AppraisalJobItem.detailed_location = dr["detailed_location"] == System.DBNull.Value ? "" : Convert.ToString(dr["detailed_location"]);
                                    AppraisalJobItem.asset_type_id = dr["asset_type_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["asset_type_id"]);
                                    AppraisalJobItem.assessment_methods_id = dr["assessment_methods_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["assessment_methods_id"]);
                                    AppraisalJobItem.rights_of_access_id = dr["rights_of_access_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["rights_of_access_id"]);
                                    AppraisalJobItem.paint_the_town_id = dr["paint_the_town_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["paint_the_town_id"]);
                                    AppraisalJobItem.status = dr["status"] == System.DBNull.Value ? 0 : Convert.ToInt16(dr["status"]);
                                    
                                    result.Add(AppraisalJobItem);
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

            return result;
        }
        #endregion

        #region Management
        public Hashtable MngAppraisalJob(AppraisalJobModel appraisalJob, string createBy)
        {
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            Hashtable result = new Hashtable();
            Boolean process = false;
            String msg = "";
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_MNG_APPRAISAL_JOB, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_appraisal_assets_code", MySqlDbType.VarChar).Value = appraisalJob.appraisal_assets_code;
                        cmd.Parameters.Add("p_village", MySqlDbType.VarChar).Value = appraisalJob.village;
                        cmd.Parameters.Add("p_alley", MySqlDbType.VarChar).Value = appraisalJob.alley;
                        cmd.Parameters.Add("p_road", MySqlDbType.VarChar).Value = appraisalJob.road;
                        cmd.Parameters.Add("p_district_id", MySqlDbType.Int32).Value = appraisalJob.district_id;
                        cmd.Parameters.Add("p_amphur_id", MySqlDbType.Int32).Value = appraisalJob.amphur_id;
                        cmd.Parameters.Add("p_province_id", MySqlDbType.Int32).Value = appraisalJob.province_id;
                        cmd.Parameters.Add("p_detailed_location", MySqlDbType.VarChar).Value = appraisalJob.detailed_location;
                        cmd.Parameters.Add("p_asset_type_id", MySqlDbType.Int32).Value = appraisalJob.asset_type_id;
                        cmd.Parameters.Add("p_assessment_mthods_id", MySqlDbType.Int32).Value = appraisalJob.assessment_methods_id;
                        cmd.Parameters.Add("p_rights_of_access_id", MySqlDbType.Int32).Value = appraisalJob.rights_of_access_id;
                        cmd.Parameters.Add("p_paint_the_town_id", MySqlDbType.Int32).Value = appraisalJob.paint_the_town_id;
                        cmd.Parameters.Add("p_mng_by", MySqlDbType.VarChar).Value = createBy;

                        cmd.Parameters.Add(new MySqlParameter("oMessage", MySqlDbType.VarChar)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new MySqlParameter("oAppraisalID", MySqlDbType.Int32)).Direction = ParameterDirection.Output;

                        cmd.ExecuteScalar();
                        //
                        int userId = cmd.Parameters["oAppraisalID"].Value == System.DBNull.Value ? 0 : Convert.ToInt32(cmd.Parameters["oAppraisalID"].Value);
                        if (userId > 0)
                        {
                            tran.Commit();
                            process = true;
                        }
                        msg = Convert.ToString(cmd.Parameters["oMessage"].Value);
                    }
                }
            }
            catch (MySqlException ms)
            {
                throw new Exception("MySqlException: " + ms.Message);
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            result["Status"] = process;
            result["Message"] = msg;
            return result;
        }
        #endregion

        #region Insert
        
        #endregion

        #region Update
        
        #endregion

        #region Delete
        [DataObjectMethod(DataObjectMethodType.Update)]
        public Boolean DeleteAppraisalJob(int appraisalJobId, string delBy)
        {
            if (appraisalJobId <= 0) throw new ArgumentException("Value cannot be null or empty.", "appraisalJobId");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            bool process = false;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_DEL_APPRAISAL_JOB, conn, tran))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_appraisal_job", MySqlDbType.Int32).Value = appraisalJobId;
                        cmd.Parameters.Add("p_delete_by", MySqlDbType.VarChar).Value = delBy;

                        int excute = cmd.ExecuteNonQuery();
                        //
                        if (excute > 0)
                        {
                            tran.Commit();
                            process = true;
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
                tran.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return process;
        }
        #endregion
    }
    #endregion

    #region Validation
    #endregion
}