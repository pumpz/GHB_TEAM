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
using System.ComponentModel.DataAnnotations;

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
        public DateTime create_date { set; get; }
        public string create_by { set; get; }
        public string type_of_document { set; get; }
        public string certificate_of_ownership { set; get; }
        public string parcel_number { set; get; }
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
        public int assets_detail_id { set; get; }

        [Required(ErrorMessage = "กรุณาบันทึกข้อมูลในส่วนงานประเมินให้เรียบร้อยก่อน")]
        public int appraisal_assets_id { set; get; }

        [Required(ErrorMessage = "กรุณาเลือกประเภทของเอกสารสิทธิ์")]
        public int type_of_document_id { set; get; }
        public string certificate_of_ownership { set; get; }
        public string parcel_number { set; get; }
        public string survey { set; get; }
        public string book_or_page { set; get; }
        public string tc_no { set; get; }
        public int condition_land_id { set; get; }
        public int courting_the_burden { set; get; }

        [Required(ErrorMessage = "กรุณาระบุชื่อผู้ถือกรรมสิทธิ์")]
        public string ownership { set; get; }

        [Required(ErrorMessage = "กรุณาระบุชื่อผู้ทรงสิทธิ์")]
        public string rightsholder { set; get; }
        public int province_id { set; get; }
        public int amphur_id { set; get; }
        public int district_id { set; get; }
        public int rai_area { set; get; }
        public int ngaan_area { set; get; }
        public int wa_area { set; get; }
        public double total_area { set; get; }
        public double appraisal { set; get; }
        public int status { set; get; }
        public DateTime create_date { set; get; }
        public DateTime update_date { set; get; }
        public DateTime delete_date { set; get; }
        public string create_by { set; get; }
        public string update_by { set; get; }
        public string delete_by { set; get; }
    }

    [Serializable]
    public class CompareAssetModel
    {
        public int compare_assets_id { set; get; }
        public int appraisal_assets_id { set; get; }
        public double survey_price { set; get; }
        public double appropriate_price { set; get; }
        public string data_source { set; get; }
        public string phone { set; get; }
        public double size_area { set; get; }
        public int shape_information_id { set; get; }
        public int environment_id { set; get; }
        public int characteristics_assets_id { set; get; }
        public int characteristics_access_id { set; get; }
        public int utilities_id { set; get; }
        public int terms_id { set; get; }
        public int liquidity_id { set; get; }
        public int sequence { set; get; }
        public int status { set; get; }
        public DateTime create_date { set; get; }
        public DateTime update_date { set; get; }
        public DateTime delete_date { set; get; }
        public string create_by { set; get; }
        public string update_by { set; get; }
        public string delete_by { set; get; }
    }

    [Serializable]
    public class LocationAssetModel
    {
        public int assets_location_id { set; get; }
        public int appraisal_assets_id { set; get; }
        public string no_buildings { set; get; }
        public int building_type_id { set; get; }
        public int floor { set; get; }
        public int condition_building_id { set; get; }
        public double building_age { set; get; }
        public int structure_id { set; get; }
        public int pole_id { set; get; }
        public int roof_id { set; get; }
        public int materials_id { set; get; }
        public int ceiling_id { set; get; }
        public int exterior_walls_id { set; get; }
        public int interior_walls_id { set; get; }
        public int stair_id { set; get; }
        public string room_1 { set; get; }
        public string room_2 { set; get; }
        public string room_3 { set; get; }
        public double width { set; get; }
        public double high { set; get; }
        public double price_per_meter { set; get; }
        public double depreciation { set; get; }
        public int status { set; get; }
        public DateTime create_date { set; get; }
        public DateTime update_date { set; get; }
        public DateTime delete_date { set; get; }
        public string create_by { set; get; }
        public string update_by { set; get; }
        public string delete_by { set; get; }
    }

    [Serializable]
    public class MapAssetModel
    {
        public int map_assets_id { set; get; }
        public int appraisal_assets_id { set; get; }
        public string latitude { set; get; }
        public string longitude { set; get; }
        public int status { set; get; }
        public DateTime create_date { set; get; }
        public DateTime update_date { set; get; }
        public DateTime delete_date { set; get; }
        public string create_by { set; get; }
        public string update_by { set; get; }
        public string delete_by { set; get; }
    }

    [Serializable]
    public class UploadPictureAssetModel
    {
        public int image_assets_id { set; get; }
        public int appraisal_assets_id { set; get; }
        public int upload_type_id { set; get; }
        public string image_path { set; get; }
        public string file_name { set; get; }
        public string description { set; get; }
        public int sequence { set; get; }
        public string note { set; get; }
        public int status { set; get; }
        public DateTime create_date { set; get; }
        public DateTime update_date { set; get; }
        public DateTime delete_date { set; get; }
        public string create_by { set; get; }
        public string update_by { set; get; }
        public string delete_by { set; get; }
    }

    [Serializable]
    public class CompareDescriptionModel
    {
        public int appraisal_assets_id { set; get; }
        public string note { set; get; }
        public int sequence { set; get; }
        public string create_by { set; get; }
    }
    #endregion

    #region Services
    public interface IAppraisalService
    {
        /// <detail>
        /// GetAppraisalLists
        /// </detail>
        /// <param name="appraisalCode"></param>
        /// <param name="provinceId"></param>
        /// <param name="amphurId"></param>
        /// <param name="createBy"></param>
        /// <param name="viewOnly"></param>
        /// <returns>List<AppraisalListsModel></returns>
        List<AppraisalListsModel> GetAppraisalLists(string appraisalCode, int provinceId, int amphurId, string createBy, bool viewOnly);

        /// <detail>
        /// GetAppraisalJob
        /// </detail>
        /// <param name="appraisalID"></param>
        /// <param name="keyword"></param>
        /// <param name="createBy"></param>
        /// <returns>List<AppraisalJobModel></returns>
        List<AppraisalJobModel> GetAppraisalJob(int appraisalID, string keyword, string createBy);

        /// <detail>
        /// GetAppraisalDetail
        /// </detail>
        /// <param name="appraisalDetailID"></param>
        /// <param name="appraisalID"></param>
        /// <param name="createBy"></param>
        /// <returns>List<AppraisalDetailModel></returns>
        List<AppraisalDetailModel> GetAppraisalDetail(int appraisalDetailID, int appraisalID, string createBy);

        /// <detail>
        /// GetCompareAsset
        /// </detail>
        /// <param name="compareAssetID"></param>
        /// <param name="appraisalID"></param>
        /// <param name="createBy"></param>
        /// <returns>List<CompareAssetModel></returns>
        List<CompareAssetModel> GetCompareAsset(int compareAssetID, int appraisalID, string createBy);

        /// <detail>
        /// GetCompareDescription
        /// </detail>
        /// <param name="sequence"></param>
        /// <param name="appraisalID"></param>
        /// <param name="createBy"></param>
        /// <returns>List<CompareDescriptionModel></returns>
        List<CompareDescriptionModel> GetCompareDescription(int sequence, int appraisalID, string createBy);

        /// <detail>
        /// GetLocationAsset
        /// </detail>
        /// <param name="locationAssetID"></param>
        /// <param name="appraisalID"></param>
        /// <param name="createBy"></param>
        /// <returns>List<LocationAssetModel></returns>
        List<LocationAssetModel> GetLocationAsset(int locationAssetID, int appraisalID, string createBy);

        /// <detail>
        /// GetMapAsset
        /// </detail>
        /// <param name="mapAssetID"></param>
        /// <param name="appraisalID"></param>
        /// <param name="createBy"></param>
        /// <returns>List<MapAssetModel></returns>
        List<MapAssetModel> GetMapAsset(int mapAssetID, int appraisalID, string createBy);

        /// <detail>
        /// GetUploadPictureAsset
        /// </detail>
        /// <param name="mapAssetID"></param>
        /// <param name="appraisalID"></param>
        /// <param name="createBy"></param>
        /// <returns>List<UploadPictureAssetModel></returns>
        List<UploadPictureAssetModel> GetUploadPictureAsset(int mapAssetID, int appraisalID, string createBy);

        /// <management>
        /// MngAppraisalJob
        /// </management>
        /// <param name="appraisalJob"></param>
        /// <param name="mngBy"></param>
        /// <returns>Hashtable["Status", ["Message"]]</returns>
        Hashtable MngAppraisalJob(AppraisalJobModel appraisalJob, string mngBy);

        /// <management>
        /// MngAppraisalDetail
        /// </management>
        /// <param name="appraisalDetail"></param>
        /// <param name="mngBy"></param>
        /// <returns>Boolean</returns>
        Boolean MngAppraisalDetail(AppraisalDetailModel appraisalDetail, string mngBy);

        /// <management>
        /// MngCompareAsset
        /// </management>
        /// <param name="compareAsset"></param>
        /// <param name="mngBy"></param>
        /// <returns>Boolean</returns>
        Boolean MngCompareAsset(CompareAssetModel compareAsset, string mngBy);

        /// <management>
        /// MngCompareDescription
        /// </management>
        /// <param name="compareDesc"></param>
        /// <returns>Boolean</returns>
        Boolean MngCompareDescription(CompareDescriptionModel compareDesc);

        /// <management>
        /// MngLocationAsset
        /// </management>
        /// <param name="locationAsset"></param>
        /// <param name="mngBy"></param>
        /// <returns>Boolean</returns>
        Boolean MngLocationAsset(LocationAssetModel locationAsset, string mngBy);

        /// <management>
        /// MngMapAsset
        /// </management>
        /// <param name="mapAsset"></param>
        /// <param name="mngBy"></param>
        /// <returns>Boolean</returns>
        Boolean MngMapAsset(MapAssetModel mapAsset, string mngBy);

        /// <management>
        /// MngUploadPicture
        /// </management>
        /// <param name="uploadPicture"></param>
        /// <param name="mngBy"></param>
        /// <returns>Boolean</returns>
        Boolean MngUploadPicture(UploadPictureAssetModel uploadPicture, string mngBy);

        /// <delete>
        /// DeleteAppraisalJob
        /// </delete>
        /// <param name="appraisalJobId"></param>
        /// <param name="delBy"></param>
        /// <returns>Boolean</returns>
        Boolean DeleteAppraisalJob(int appraisalJobId, string delBy);

        /// <delete>
        /// DeleteAppraisalDetail
        /// </delete>
        /// <param name="appraisalDetailId"></param>
        /// <param name="delBy"></param>
        /// <returns>Boolean</returns>
        Boolean DeleteAppraisalDetail(int appraisalDetailId, string delBy);

        /// <delete>
        /// DeleteCompareAsset
        /// </delete>
        /// <param name="compareAssetId"></param>
        /// <param name="delBy"></param>
        /// <returns>Boolean</returns>
        Boolean DeleteCompareAsset(int compareAssetId, string delBy);

        /// <delete>
        /// DeleteLocationAsset
        /// </delete>
        /// <param name="locationAssetId"></param>
        /// <param name="delBy"></param>
        /// <returns>Boolean</returns>
        Boolean DeleteLocationAsset(int locationAssetId, string delBy);

        /// <delete>
        /// DeleteMapAsset
        /// </delete>
        /// <param name="mapAssetId"></param>
        /// <param name="delBy"></param>
        /// <returns>Boolean</returns>
        Boolean DeleteMapAsset(int mapAssetId, string delBy);

        /// <delete>
        /// DeleteUploadPicture
        /// </delete>
        /// <param name="uploadPictureId"></param>
        /// <param name="delBy"></param>
        /// <returns>Boolean</returns>
        Boolean DeleteUploadPicture(int uploadPictureId, string delBy);
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
                                    AppraisalItem.certificate_of_ownership = dr["certificate_of_ownership"] == System.DBNull.Value ? "" : Convert.ToString(dr["certificate_of_ownership"]);
                                    AppraisalItem.parcel_number = dr["parcel_number"] == System.DBNull.Value ? "" : Convert.ToString(dr["parcel_number"]);
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
        public List<AppraisalJobModel> GetAppraisalJob(int appraisalID, string keyword, string createBy)
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

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.GET_APPRAISAL_JOB, conn))
                    {
                        string condition = "";

                        if (appraisalID > 0)
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

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<AppraisalDetailModel> GetAppraisalDetail(int appraisalDetailID, int appraisalID, string createBy)
        {
            MySqlConnection conn = null;
            List<AppraisalDetailModel> result = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.GET_APPRAISAL_DETAIL, conn))
                    {
                        string condition = "";

                        if (appraisalDetailID > 0)
                        {
                            condition += string.Format(" AND ASSETS_DETAIL_ID = {0}", appraisalDetailID);
                        }

                        if (appraisalID > 0)
                        {
                            condition += string.Format(" AND APPRAISAL_ASSETS_ID = {0}", appraisalID);
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
                                result = new List<AppraisalDetailModel>();
                                while (dr.Read())
                                {
                                    AppraisalDetailModel AppraisalDetailItem = new AppraisalDetailModel();
                                    AppraisalDetailItem.assets_detail_id = dr["assets_detail_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["assets_detail_id"]);
                                    AppraisalDetailItem.appraisal_assets_id = dr["appraisal_assets_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["appraisal_assets_id"]);
                                    AppraisalDetailItem.type_of_document_id = dr["type_of_document_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["type_of_document_id"]);
                                    AppraisalDetailItem.certificate_of_ownership = dr["certificate_of_ownership"] == System.DBNull.Value ? "" : Convert.ToString(dr["certificate_of_ownership"]);
                                    AppraisalDetailItem.parcel_number = dr["parcel_number"] == System.DBNull.Value ? "" : Convert.ToString(dr["parcel_number"]);
                                    AppraisalDetailItem.survey = dr["survey"] == System.DBNull.Value ? "" : Convert.ToString(dr["survey"]);
                                    AppraisalDetailItem.book_or_page = dr["book_or_page"] == System.DBNull.Value ? "" : Convert.ToString(dr["book_or_page"]);
                                    AppraisalDetailItem.tc_no = dr["tc_no"] == System.DBNull.Value ? "" : Convert.ToString(dr["tc_no"]);
                                    AppraisalDetailItem.condition_land_id = dr["condition_land_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["condition_land_id"]);
                                    AppraisalDetailItem.courting_the_burden = dr["courting_the_burden"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["courting_the_burden"]);
                                    AppraisalDetailItem.ownership = dr["ownership"] == System.DBNull.Value ? "" : Convert.ToString(dr["ownership"]);
                                    AppraisalDetailItem.rightsholder = dr["rightsholder"] == System.DBNull.Value ? "" : Convert.ToString(dr["rightsholder"]);
                                    AppraisalDetailItem.province_id = dr["province_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["province_id"]);
                                    AppraisalDetailItem.amphur_id = dr["amphur_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["amphur_id"]);
                                    AppraisalDetailItem.district_id = dr["district_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["district_id"]);
                                    AppraisalDetailItem.rai_area = dr["rai_area"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["rai_area"]);
                                    AppraisalDetailItem.ngaan_area = dr["ngaan_area"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["ngaan_area"]);
                                    AppraisalDetailItem.wa_area = dr["wa_area"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["wa_area"]);
                                    AppraisalDetailItem.total_area = dr["total_area"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["total_area"]);
                                    AppraisalDetailItem.appraisal = dr["appraisal"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["appraisal"]);
                                    AppraisalDetailItem.status = dr["status"] == System.DBNull.Value ? 0 : Convert.ToInt16(dr["status"]);

                                    result.Add(AppraisalDetailItem);
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
        public List<CompareAssetModel> GetCompareAsset(int compareAssetID, int appraisalID, string createBy)
        {
            MySqlConnection conn = null;
            List<CompareAssetModel> result = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.GET_COMPARE_ASSETS, conn))
                    {
                        string condition = "";

                        if (compareAssetID > 0)
                        {
                            condition += string.Format(" AND COMPARE_ASSETS_ID = {0}", compareAssetID);
                        }

                        if (appraisalID > 0)
                        {
                            condition += string.Format(" AND APPRAISAL_ASSETS_ID = {0}", appraisalID);
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
                                result = new List<CompareAssetModel>();
                                while (dr.Read())
                                {
                                    CompareAssetModel CompareAssetItem = new CompareAssetModel();
                                    CompareAssetItem.compare_assets_id = dr["compare_assets_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["compare_assets_id"]);
                                    CompareAssetItem.appraisal_assets_id = dr["appraisal_assets_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["appraisal_assets_id"]);
                                    CompareAssetItem.survey_price = dr["type_of_document_id"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["survey_price"]);
                                    CompareAssetItem.appropriate_price = dr["appropriate_price"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["appropriate_price"]);
                                    CompareAssetItem.data_source = dr["data_source"] == System.DBNull.Value ? "" : Convert.ToString(dr["data_source"]);
                                    CompareAssetItem.phone = dr["phone"] == System.DBNull.Value ? "" : Convert.ToString(dr["phone"]);
                                    CompareAssetItem.size_area = dr["size_area"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["size_area"]);
                                    CompareAssetItem.shape_information_id = dr["shape_information_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["shape_information_id"]);
                                    CompareAssetItem.environment_id = dr["environment_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["environment_id"]);
                                    CompareAssetItem.characteristics_assets_id = dr["characteristics_assets_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["characteristics_assets_id"]);
                                    CompareAssetItem.characteristics_access_id = dr["characteristics_access_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["characteristics_access_id"]);
                                    CompareAssetItem.utilities_id = dr["utilities_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["utilities_id"]);
                                    CompareAssetItem.terms_id = dr["terms_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["terms_id"]);
                                    CompareAssetItem.liquidity_id = dr["liquidity_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["liquidity_id"]);
                                    CompareAssetItem.sequence = dr["sequence"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["sequence"]);
                                    CompareAssetItem.status = dr["status"] == System.DBNull.Value ? 0 : Convert.ToInt16(dr["status"]);

                                    result.Add(CompareAssetItem);
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
        public List<CompareDescriptionModel> GetCompareDescription(int sequence, int appraisalID, string createBy)
        {
            MySqlConnection conn = null;
            List<CompareDescriptionModel> result = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.GET_COMPARE_ASSETS, conn))
                    {
                        string condition = "";

                        if (sequence > 0)
                        {
                            condition += string.Format(" AND SEQUENCE = {0}", sequence);
                        }

                        if (appraisalID > 0)
                        {
                            condition += string.Format(" AND APPRAISAL_ASSETS_ID = {0}", appraisalID);
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
                                result = new List<CompareDescriptionModel>();
                                while (dr.Read())
                                {
                                    CompareDescriptionModel CompareDescItem = new CompareDescriptionModel();
                                    CompareDescItem.appraisal_assets_id = dr["appraisal_assets_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["appraisal_assets_id"]);
                                    CompareDescItem.note = dr["note"] == System.DBNull.Value ? "" : Convert.ToString(dr["note"]);
                                    CompareDescItem.sequence = dr["sequence"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["sequence"]);

                                    result.Add(CompareDescItem);
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
        public List<LocationAssetModel> GetLocationAsset(int locationAssetID, int appraisalID, string createBy)
        {
            MySqlConnection conn = null;
            List<LocationAssetModel> result = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.GET_LOCATION_ASSETS, conn))
                    {
                        string condition = "";

                        if (locationAssetID > 0)
                        {
                            condition += string.Format(" AND ASSETS_LOCATION_ID = {0}", locationAssetID);
                        }

                        if (appraisalID > 0)
                        {
                            condition += string.Format(" AND APPRAISAL_ASSETS_ID = {0}", appraisalID);
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
                                result = new List<LocationAssetModel>();
                                while (dr.Read())
                                {
                                    LocationAssetModel LocationAssetItem = new LocationAssetModel();
                                    LocationAssetItem.assets_location_id = dr["assets_location_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["assets_location_id"]);
                                    LocationAssetItem.appraisal_assets_id = dr["appraisal_assets_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["appraisal_assets_id"]);
                                    LocationAssetItem.no_buildings = dr["no_buildings"] == System.DBNull.Value ? "" : Convert.ToString(dr["no_buildings"]);
                                    LocationAssetItem.building_type_id = dr["building_type_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["building_type_id"]);
                                    LocationAssetItem.floor = dr["floor"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["floor"]);
                                    LocationAssetItem.condition_building_id = dr["condition_building_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["condition_building_id"]);
                                    LocationAssetItem.pole_id = dr["pole_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["pole_id"]);
                                    LocationAssetItem.roof_id = dr["roof_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["roof_id"]);
                                    LocationAssetItem.materials_id = dr["materials_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["materials_id"]);
                                    LocationAssetItem.ceiling_id = dr["ceiling_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["ceiling_id"]);
                                    LocationAssetItem.exterior_walls_id = dr["exterior_walls_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["exterior_walls_id"]);
                                    LocationAssetItem.interior_walls_id = dr["interior_walls_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["interior_walls_id"]);
                                    LocationAssetItem.stair_id = dr["stair_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["stair_id"]);
                                    LocationAssetItem.room_1 = dr["room_1"] == System.DBNull.Value ? "" : Convert.ToString(dr["room_1"]);
                                    LocationAssetItem.room_2 = dr["room_2"] == System.DBNull.Value ? "" : Convert.ToString(dr["room_2"]);
                                    LocationAssetItem.room_3 = dr["room_3"] == System.DBNull.Value ? "" : Convert.ToString(dr["room_3"]);
                                    LocationAssetItem.width = dr["width"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["width"]);
                                    LocationAssetItem.high = dr["high"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["high"]);
                                    LocationAssetItem.price_per_meter = dr["price_per_meter"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["price_per_meter"]);
                                    LocationAssetItem.depreciation = dr["depreciation"] == System.DBNull.Value ? 0 : Convert.ToDouble(dr["depreciation"]);
                                    LocationAssetItem.status = dr["status"] == System.DBNull.Value ? 0 : Convert.ToInt16(dr["status"]);

                                    result.Add(LocationAssetItem);
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
        public List<MapAssetModel> GetMapAsset(int mapAssetID, int appraisalID, string createBy)
        {
            MySqlConnection conn = null;
            List<MapAssetModel> result = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.GET_MAP_ASSETS, conn))
                    {
                        string condition = "";

                        if (mapAssetID > 0)
                        {
                            condition += string.Format(" AND MAP_ASSETS_ID = {0}", mapAssetID);
                        }

                        if (appraisalID > 0)
                        {
                            condition += string.Format(" AND APPRAISAL_ASSETS_ID = {0}", appraisalID);
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
                                result = new List<MapAssetModel>();
                                while (dr.Read())
                                {
                                    MapAssetModel MapAssetItem = new MapAssetModel();
                                    MapAssetItem.map_assets_id = dr["assets_location_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["assets_location_id"]);
                                    MapAssetItem.appraisal_assets_id = dr["appraisal_assets_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["appraisal_assets_id"]);
                                    MapAssetItem.latitude = dr["latitude"] == System.DBNull.Value ? "" : Convert.ToString(dr["latitude"]);
                                    MapAssetItem.longitude = dr["longitude"] == System.DBNull.Value ? "" : Convert.ToString(dr["longitude"]);
                                    MapAssetItem.status = dr["status"] == System.DBNull.Value ? 0 : Convert.ToInt16(dr["status"]);

                                    result.Add(MapAssetItem);
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
        public List<UploadPictureAssetModel> GetUploadPictureAsset(int uploadAssetID, int appraisalID, string createBy)
        {
            MySqlConnection conn = null;
            List<UploadPictureAssetModel> result = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.GET_IMAGE_ASSETS, conn))
                    {
                        string condition = "";

                        if (uploadAssetID > 0)
                        {
                            condition += string.Format(" AND IMAGE_ASSETS_ID = {0}", uploadAssetID);
                        }

                        if (appraisalID > 0)
                        {
                            condition += string.Format(" AND APPRAISAL_ASSETS_ID = {0}", appraisalID);
                        }

                        if (ContentHelpers.IsNotnull(createBy))
                        {
                            condition += string.Format(" AND CREATE_BY = '{0}'", createBy);
                        }
                        cmd.CommandText += condition;
                        using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (dr.HasRows)
                            {
                                result = new List<UploadPictureAssetModel>();
                                while (dr.Read())
                                {
                                    UploadPictureAssetModel UploadPictureAssetItem = new UploadPictureAssetModel();
                                    UploadPictureAssetItem.image_assets_id = dr["image_assets_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["image_assets_id"]);
                                    UploadPictureAssetItem.appraisal_assets_id = dr["appraisal_assets_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["appraisal_assets_id"]);
                                    UploadPictureAssetItem.upload_type_id = dr["upload_type_id"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["upload_type_id"]);
                                    UploadPictureAssetItem.image_path = dr["image_path"] == System.DBNull.Value ? "" : Convert.ToString(dr["image_path"]);
                                    UploadPictureAssetItem.file_name = dr["file_name"] == System.DBNull.Value ? "" : Convert.ToString(dr["file_name"]);
                                    UploadPictureAssetItem.description = dr["description"] == System.DBNull.Value ? "" : Convert.ToString(dr["description"]);
                                    UploadPictureAssetItem.sequence = dr["sequence"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["sequence"]);
                                    UploadPictureAssetItem.note = dr["note"] == System.DBNull.Value ? "" : Convert.ToString(dr["note"]);
                                    UploadPictureAssetItem.status = dr["status"] == System.DBNull.Value ? 0 : Convert.ToInt16(dr["status"]);

                                    result.Add(UploadPictureAssetItem);
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
        public Hashtable MngAppraisalJob(AppraisalJobModel appraisalJob, string mngBy)
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
                        cmd.Parameters.Add("p_appraisal_asset_code", MySqlDbType.VarChar).Value = appraisalJob.appraisal_assets_code;
                        cmd.Parameters.Add("p_village", MySqlDbType.VarChar).Value = appraisalJob.village;
                        cmd.Parameters.Add("p_alley", MySqlDbType.VarChar).Value = appraisalJob.alley;
                        cmd.Parameters.Add("p_road", MySqlDbType.VarChar).Value = appraisalJob.road;
                        cmd.Parameters.Add("p_district_id", MySqlDbType.Int32).Value = appraisalJob.district_id;
                        cmd.Parameters.Add("p_amphur_id", MySqlDbType.Int32).Value = appraisalJob.amphur_id;
                        cmd.Parameters.Add("p_province_id", MySqlDbType.Int32).Value = appraisalJob.province_id;
                        cmd.Parameters.Add("p_detailed_location", MySqlDbType.Text).Value = appraisalJob.detailed_location;
                        cmd.Parameters.Add("p_asset_type_id", MySqlDbType.Int32).Value = appraisalJob.asset_type_id;
                        cmd.Parameters.Add("p_assessment_mthods_id", MySqlDbType.Int32).Value = appraisalJob.assessment_methods_id;
                        cmd.Parameters.Add("p_rights_of_access_id", MySqlDbType.Int32).Value = appraisalJob.rights_of_access_id;
                        cmd.Parameters.Add("p_paint_the_town_id", MySqlDbType.Int32).Value = appraisalJob.paint_the_town_id;
                        cmd.Parameters.Add("p_mng_by", MySqlDbType.VarChar).Value = mngBy;

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

        public Boolean MngAppraisalDetail(AppraisalDetailModel appraisalDetail, string mngBy)
        {
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            Boolean process = false;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_MNG_APPRAISAL_DETAIL, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_appraisal_asset_id", MySqlDbType.Int32).Value = appraisalDetail.appraisal_assets_id;
                        cmd.Parameters.Add("p_type_of_document_id", MySqlDbType.VarChar).Value = appraisalDetail.type_of_document_id;
                        cmd.Parameters.Add("p_certificate_of_ownership", MySqlDbType.VarChar).Value = appraisalDetail.certificate_of_ownership;
                        cmd.Parameters.Add("p_parcel_number", MySqlDbType.VarChar).Value = appraisalDetail.parcel_number;
                        cmd.Parameters.Add("p_survey", MySqlDbType.VarChar).Value = appraisalDetail.survey;
                        cmd.Parameters.Add("p_book_or_page", MySqlDbType.VarChar).Value = appraisalDetail.book_or_page;
                        cmd.Parameters.Add("p_tc_no", MySqlDbType.VarChar).Value = appraisalDetail.tc_no;
                        cmd.Parameters.Add("p_condition_land_id", MySqlDbType.Int32).Value = appraisalDetail.condition_land_id;
                        cmd.Parameters.Add("p_courting_the_burden", MySqlDbType.Int32).Value = appraisalDetail.courting_the_burden;
                        cmd.Parameters.Add("p_ownership", MySqlDbType.VarChar).Value = appraisalDetail.ownership;
                        cmd.Parameters.Add("p_rightsholder", MySqlDbType.VarChar).Value = appraisalDetail.rightsholder;
                        cmd.Parameters.Add("p_province_id", MySqlDbType.Int32).Value = appraisalDetail.province_id;
                        cmd.Parameters.Add("p_amphur_id", MySqlDbType.Int32).Value = appraisalDetail.amphur_id;
                        cmd.Parameters.Add("p_district_id", MySqlDbType.Int32).Value = appraisalDetail.district_id;
                        cmd.Parameters.Add("p_rai_area", MySqlDbType.Int32).Value = appraisalDetail.rai_area;
                        cmd.Parameters.Add("p_ngaan_area", MySqlDbType.Int32).Value = appraisalDetail.ngaan_area;
                        cmd.Parameters.Add("p_wa_area", MySqlDbType.Int32).Value = appraisalDetail.wa_area;
                        cmd.Parameters.Add("p_total_area", MySqlDbType.Double).Value = appraisalDetail.total_area;
                        cmd.Parameters.Add("p_appraisal", MySqlDbType.Double).Value = appraisalDetail.appraisal;
                        cmd.Parameters.Add("p_mng_by", MySqlDbType.VarChar).Value = mngBy;

                        cmd.Parameters.Add(new MySqlParameter("oAppraisalDetailID", MySqlDbType.Int32)).Direction = ParameterDirection.Output;

                        cmd.ExecuteScalar();
                        //
                        int userId = cmd.Parameters["oAppraisalDetailID"].Value == System.DBNull.Value ? 0 : Convert.ToInt32(cmd.Parameters["oAppraisalDetailID"].Value);
                        if (userId > 0)
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

        public Boolean MngCompareAsset(CompareAssetModel compareAsset, string mngBy)
        {
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            Boolean process = false;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_MNG_COMPARE_ASSET, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_appraisal_asset_id", MySqlDbType.Int32).Value = compareAsset.appraisal_assets_id;
                        cmd.Parameters.Add("p_survey_price", MySqlDbType.Double).Value = compareAsset.survey_price;
                        cmd.Parameters.Add("p_appropriate_price", MySqlDbType.Double).Value = compareAsset.appropriate_price;
                        cmd.Parameters.Add("p_data_source", MySqlDbType.VarChar).Value = compareAsset.data_source;
                        cmd.Parameters.Add("p_phone", MySqlDbType.VarChar).Value = compareAsset.phone;
                        cmd.Parameters.Add("p_size_area", MySqlDbType.Double).Value = compareAsset.size_area;
                        cmd.Parameters.Add("p_shape_infomation_id", MySqlDbType.Int32).Value = compareAsset.shape_information_id;
                        cmd.Parameters.Add("p_environment_id", MySqlDbType.Int32).Value = compareAsset.environment_id;
                        cmd.Parameters.Add("p_characteristics_assets_id", MySqlDbType.Int32).Value = compareAsset.characteristics_assets_id;
                        cmd.Parameters.Add("p_characteristics_access_id", MySqlDbType.Int32).Value = compareAsset.characteristics_access_id;
                        cmd.Parameters.Add("p_utilities_id", MySqlDbType.Int32).Value = compareAsset.utilities_id;
                        cmd.Parameters.Add("p_terms_id", MySqlDbType.Int32).Value = compareAsset.terms_id;
                        cmd.Parameters.Add("p_liquidity_id", MySqlDbType.Int32).Value = compareAsset.liquidity_id;
                        cmd.Parameters.Add("p_sequence", MySqlDbType.Int32).Value = compareAsset.sequence;
                        cmd.Parameters.Add("p_mng_by", MySqlDbType.VarChar).Value = mngBy;

                        cmd.Parameters.Add(new MySqlParameter("oCompareAssetID", MySqlDbType.Int32)).Direction = ParameterDirection.Output;

                        cmd.ExecuteScalar();
                        //
                        int userId = cmd.Parameters["oCompareAssetID"].Value == System.DBNull.Value ? 0 : Convert.ToInt32(cmd.Parameters["oCompareAssetID"].Value);
                        if (userId > 0)
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

        public Boolean MngCompareDescription(CompareDescriptionModel compareDesc)
        {
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            Boolean process = false;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_MNG_OTHER_NOTE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_appraisal_asset_id", MySqlDbType.Int32).Value = compareDesc.appraisal_assets_id;
                        cmd.Parameters.Add("p_note", MySqlDbType.Double).Value = compareDesc.note;
                        cmd.Parameters.Add("p_sequence", MySqlDbType.Double).Value = compareDesc.sequence;

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

        public Boolean MngLocationAsset(LocationAssetModel locationAsset, string mngBy)
        {
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            Boolean process = false;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_MNG_LOCATION_ASSET, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_appraisal_asset_id", MySqlDbType.Int32).Value = locationAsset.appraisal_assets_id;
                        cmd.Parameters.Add("p_no_building", MySqlDbType.VarChar).Value = locationAsset.no_buildings;
                        cmd.Parameters.Add("p_building_type_id", MySqlDbType.Int32).Value = locationAsset.building_type_id;
                        cmd.Parameters.Add("p_floor", MySqlDbType.Int32).Value = locationAsset.floor;
                        cmd.Parameters.Add("p_condition_building_id", MySqlDbType.Int32).Value = locationAsset.condition_building_id;
                        cmd.Parameters.Add("p_building_age", MySqlDbType.Double).Value = locationAsset.building_age;
                        cmd.Parameters.Add("p_structure_id", MySqlDbType.Int32).Value = locationAsset.structure_id;
                        cmd.Parameters.Add("p_pole_id", MySqlDbType.Int32).Value = locationAsset.pole_id;
                        cmd.Parameters.Add("p_roof_id", MySqlDbType.Int32).Value = locationAsset.roof_id;
                        cmd.Parameters.Add("p_materials_id", MySqlDbType.Int32).Value = locationAsset.materials_id;
                        cmd.Parameters.Add("p_ceiling_id", MySqlDbType.Int32).Value = locationAsset.ceiling_id;
                        cmd.Parameters.Add("p_exterior_walls_id", MySqlDbType.Int32).Value = locationAsset.exterior_walls_id;
                        cmd.Parameters.Add("p_interior_walls_id", MySqlDbType.Int32).Value = locationAsset.interior_walls_id;
                        cmd.Parameters.Add("p_stair_id", MySqlDbType.Int32).Value = locationAsset.stair_id;
                        cmd.Parameters.Add("p_room_1", MySqlDbType.VarChar).Value = locationAsset.room_1;
                        cmd.Parameters.Add("p_room_2", MySqlDbType.VarChar).Value = locationAsset.room_2;
                        cmd.Parameters.Add("p_room_3", MySqlDbType.VarChar).Value = locationAsset.room_3;
                        cmd.Parameters.Add("p_width", MySqlDbType.Double).Value = locationAsset.width;
                        cmd.Parameters.Add("p_high", MySqlDbType.Double).Value = locationAsset.high;
                        cmd.Parameters.Add("p_price_per_meter", MySqlDbType.Double).Value = locationAsset.price_per_meter;
                        cmd.Parameters.Add("p_depreciation", MySqlDbType.Double).Value = locationAsset.depreciation;
                        cmd.Parameters.Add("p_mng_by", MySqlDbType.VarChar).Value = mngBy;

                        cmd.Parameters.Add(new MySqlParameter("oAssetsLocationID", MySqlDbType.Int32)).Direction = ParameterDirection.Output;

                        cmd.ExecuteScalar();
                        //
                        int userId = cmd.Parameters["oAssetsLocationID"].Value == System.DBNull.Value ? 0 : Convert.ToInt32(cmd.Parameters["oAssetsLocationID"].Value);
                        if (userId > 0)
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

        public Boolean MngMapAsset(MapAssetModel mapAsset, string mngBy)
        {
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            Boolean process = false;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_MNG_MAP, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_appraisal_asset_id", MySqlDbType.Int32).Value = mapAsset.appraisal_assets_id;
                        cmd.Parameters.Add("p_latitude", MySqlDbType.VarChar).Value = mapAsset.latitude;
                        cmd.Parameters.Add("p_longitude", MySqlDbType.VarChar).Value = mapAsset.longitude;
                        cmd.Parameters.Add("p_mng_by", MySqlDbType.VarChar).Value = mngBy;

                        cmd.Parameters.Add(new MySqlParameter("oMapAssetID", MySqlDbType.Int32)).Direction = ParameterDirection.Output;

                        cmd.ExecuteScalar();
                        //
                        int userId = cmd.Parameters["oMapAssetID"].Value == System.DBNull.Value ? 0 : Convert.ToInt32(cmd.Parameters["oMapAssetID"].Value);
                        if (userId > 0)
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

        public Boolean MngUploadPicture(UploadPictureAssetModel uploadPicture, string mngBy)
        {
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            Boolean process = false;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_MNG_UPLOAD_PICTURE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_appraisal_asset_id", MySqlDbType.Int32).Value = uploadPicture.appraisal_assets_id;
                        cmd.Parameters.Add("p_upload_type_id", MySqlDbType.Int32).Value = uploadPicture.upload_type_id;
                        cmd.Parameters.Add("p_image_path", MySqlDbType.VarChar).Value = uploadPicture.image_path;
                        cmd.Parameters.Add("p_file_name", MySqlDbType.VarChar).Value = uploadPicture.file_name;
                        cmd.Parameters.Add("p_description", MySqlDbType.Text).Value = uploadPicture.description;
                        cmd.Parameters.Add("p_sequence", MySqlDbType.VarChar).Value = uploadPicture.sequence;
                        cmd.Parameters.Add("p_note", MySqlDbType.Text).Value = uploadPicture.note;
                        cmd.Parameters.Add("p_mng_by", MySqlDbType.VarChar).Value = mngBy;

                        cmd.Parameters.Add(new MySqlParameter("oImageAssetID", MySqlDbType.Int32)).Direction = ParameterDirection.Output;

                        cmd.ExecuteScalar();
                        //
                        int userId = cmd.Parameters["oImageAssetID"].Value == System.DBNull.Value ? 0 : Convert.ToInt32(cmd.Parameters["oImageAssetID"].Value);
                        if (userId > 0)
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

        [DataObjectMethod(DataObjectMethodType.Update)]
        public Boolean DeleteAppraisalDetail(int appraisalDetailId, string delBy)
        {
            if (appraisalDetailId <= 0) throw new ArgumentException("Value cannot be null or empty.", "appraisalDetailId");

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

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_DEL_APPRAISAL_DETAIL, conn, tran))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_asset_detail_id", MySqlDbType.Int32).Value = appraisalDetailId;
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

        [DataObjectMethod(DataObjectMethodType.Update)]
        public Boolean DeleteCompareAsset(int compareAssetId, string delBy)
        {
            if (compareAssetId <= 0) throw new ArgumentException("Value cannot be null or empty.", "compareAssetId");

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

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_DEL_COMPARE_ASSET, conn, tran))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_compare_asset_id", MySqlDbType.Int32).Value = compareAssetId;
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

        [DataObjectMethod(DataObjectMethodType.Update)]
        public Boolean DeleteLocationAsset(int locationAssetId, string delBy)
        {
            if (locationAssetId <= 0) throw new ArgumentException("Value cannot be null or empty.", "locationAssetId");

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

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_DEL_LOCATION_ASSET, conn, tran))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_asset_location_id", MySqlDbType.Int32).Value = locationAssetId;
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

        [DataObjectMethod(DataObjectMethodType.Update)]
        public Boolean DeleteMapAsset(int mapAssetId, string delBy)
        {
            if (mapAssetId <= 0) throw new ArgumentException("Value cannot be null or empty.", "mapAssetId");

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

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_DEL_MAP, conn, tran))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_map_asset_id", MySqlDbType.Int32).Value = mapAssetId;
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

        [DataObjectMethod(DataObjectMethodType.Update)]
        public Boolean DeleteUploadPicture(int uploadPictureId, string delBy)
        {
            if (uploadPictureId <= 0) throw new ArgumentException("Value cannot be null or empty.", "uploadPictureId");

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

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_DEL_UPLOAD_PICTURE, conn, tran))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_image_asset_id", MySqlDbType.Int32).Value = uploadPictureId;
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