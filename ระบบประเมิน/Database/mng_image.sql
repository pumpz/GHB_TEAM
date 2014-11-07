DELIMITER $$

USE `appraisalasset`$$

DROP PROCEDURE IF EXISTS `USP_MNG_UPLOAD_PICTURE`$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `USP_MNG_UPLOAD_PICTURE`(
	IN p_appraisal_asset_id INT, 
        IN p_upload_type_id INT,
        IN p_image_path VARCHAR(250),
        IN p_file_name VARCHAR(100),
        IN p_description TEXT,
        IN p_sequence INT,
        IN p_note TEXT,
        IN p_mng_by VARCHAR(20),
        OUT oImageAssetID INT
    )
BEGIN
	SELECT `IMAGE_ASSETS_ID`
		INTO oImageAssetID
	      FROM IMAGE_ASSETS
	      WHERE `APPRAISAL_ASSETS_ID` = p_appraisal_asset_id AND 
	       UPLOAD_TYPE_ID = p_upload_type_id AND `SEQUENCE` = p_sequence AND `status` = 1
	     LIMIT 1; -- you better protect yourself from duplicates
	IF(ISNULL(oImageAssetID)) THEN
		INSERT INTO IMAGE_ASSETS
		(
			APPRAISAL_ASSETS_ID, 
			UPLOAD_TYPE_ID, 
			IMAGE_PATH, 
			FILE_NAME, 
			DESCRIPTION,
			SEQUENCE,
			NOTE,
			CREATE_BY
		)
		VALUES 
		( 
			p_appraisal_asset_id, 
			p_upload_type_id, 
			p_image_path, 
			p_file_name, 
			p_description,
			p_sequence,
			p_note,
			p_mng_by
		); 
		SET oImageAssetID = LAST_INSERT_ID();
	ELSE
		UPDATE IMAGE_ASSETS
		SET IMAGE_PATH = p_image_path, 
			FILE_NAME = p_file_name,
			DESCRIPTION = p_description,
			NOTE = p_note,
			UPDATE_DATE = CURRENT_TIMESTAMP,
			UPDATE_BY = p_mng_by
		WHERE APPRAISAL_ASSETS_ID = p_appraisal_asset_id 
			AND UPLOAD_TYPE_ID = p_upload_type_id 
			AND SEQUENCE = p_sequence;
	END IF;
    END$$

DELIMITER ;