DELIMITER $$

USE `appraisalasset`$$

DROP VIEW IF EXISTS `v_result`$$

CREATE ALGORITHM=UNDEFINED DEFINER=`sa`@`%` SQL SECURITY DEFINER VIEW `v_result` AS (
SELECT
  `job`.`APPRAISAL_ASSETS_ID` AS `APPRAISAL_ASSETS_ID`,
  `job`.`APPRAISAL_ASSETS_CODE` AS `APPRAISAL_ASSETS_CODE`,
  `job`.`VILLAGE`               AS `VILLAGE`,
  `job`.`ALLEY`                 AS `ALLEY`,
  `job`.`ROAD`                  AS `ROAD`,
  (SELECT
     `district`.`DISTRICT_NAME`
   FROM `district`
   WHERE (`district`.`DISTRICT_ID` = `job`.`DISTRICT_ID`)) AS `DISTRICT_NAME`,
  (SELECT
     `amphur`.`AMPHUR_NAME`
   FROM `amphur`
   WHERE (`amphur`.`AMPHUR_ID` = `job`.`AMPHUR_ID`)) AS `AMPHUR_NAME`,
  (SELECT
     `province`.`PROVINCE_NAME`
   FROM `province`
   WHERE (`province`.`PROVINCE_ID` = `job`.`PROVINCE_ID`)) AS `PROVINCE_NAME`,
  `job`.`DETAILED_LOCATION`     AS `DETAILED_LOCATION`,
  (SELECT
     `v_filter`.`FILTER_TEXT`
   FROM `v_filter`
   WHERE ((`v_filter`.`FILTER_TYPE_NAME` = 'ASSET_TYPE')
          AND (`v_filter`.`FILTER_VALUE` = `job`.`ASSET_TYPE_ID`))) AS `ASSET_TYPE`,
  (SELECT
     `v_filter`.`FILTER_TEXT`
   FROM `v_filter`
   WHERE ((`v_filter`.`FILTER_TYPE_NAME` = 'ASSESSMENT_METHOD')
          AND (`v_filter`.`FILTER_VALUE` = `job`.`ASSESSMENT_METHODS_ID`))) AS `ASSESSMENT_METHODS`,
  (SELECT
     `v_filter`.`FILTER_TEXT`
   FROM `v_filter`
   WHERE ((`v_filter`.`FILTER_TYPE_NAME` = 'RIGHT_OF_ACCESS')
          AND (`v_filter`.`FILTER_VALUE` = `job`.`RIGHTS_OF_ACCESS_ID`))) AS `RIGHTS_OF_ACCESS`,
  (SELECT
     `v_filter`.`FILTER_TEXT`
   FROM `v_filter`
   WHERE ((`v_filter`.`FILTER_TYPE_NAME` = 'PAINT_THE_TOWN')
          AND (`v_filter`.`FILTER_VALUE` = `job`.`PAINT_THE_TOWN_ID`))) AS `PAINT_THE_TOWN`,
  (`detail`.`TOTAL_AREA` * `detail`.`APPRAISAL`) AS `LAND_VALUE`,
  (((`location`.`WIDTH` * `location`.`HIGH`) * `location`.`PRICE_PER_METER`) - `location`.`DEPRECIATION`) AS `BUILDING_VALUE`
FROM ((`appraisal_assets_job` `job`
  LEFT  JOIN `appraisal_assets_detail` `detail`
      ON ((`job`.`APPRAISAL_ASSETS_ID` = `detail`.`APPRAISAL_ASSETS_ID`)))
 LEFT  JOIN `location_assets` `location`
     ON ((`job`.`APPRAISAL_ASSETS_ID` = `location`.`APPRAISAL_ASSETS_ID`))))$$

DELIMITER ;