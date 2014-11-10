DELIMITER $$

USE `appraisalasset`$$

DROP PROCEDURE IF EXISTS `USP_GET_USERS_LOGIN`$$

CREATE DEFINER=`sa`@`%` PROCEDURE `USP_GET_USERS_LOGIN`(
	IN iUsername VARCHAR(50),
        IN iPassword VARCHAR(50),
        OUT oMessage VARCHAR(100),
	OUT oUserID INT)
BEGIN
	DECLARE isLogIn BOOLEAN;
	SELECT CASE WHEN USER_LOGIN = 1 THEN 'This user is active in the system' ELSE NULL END,
		   CASE WHEN USER_LOGIN = 1 THEN FALSE ELSE TRUE END
		      INTO oMessage, isLogIn
		      FROM users 
		     WHERE (User_Name = TRIM(iUsername) 
			   OR Email = TRIM(iUsername))
			   AND DELETE_FLAG = 0
		     LIMIT 1; -- you better protect yourself from duplicates
	IF(isLogIn) THEN
		SELECT CASE WHEN `STATUS` = 0 THEN 'Inactive account' ELSE NULL END,
		   CASE WHEN `STATUS` = 0 THEN NULL ELSE User_ID END
		      INTO oMessage, oUserID
		      FROM users 
		     WHERE (User_Name = TRIM(iUsername) 
			   OR Email = TRIM(iUsername)) 
		       AND `Password` = iPassword AND DELETE_FLAG = 0
		     LIMIT 1; -- you better protect yourself from duplicates
		IF(oUserID > 0) THEN
			UPDATE USERS
			SET USER_LOGIN = 1,
				LAST_LOGIN = CURRENT_TIMESTAMP
			WHERE User_ID = oUserID AND DELETE_FLAG = 0;
		ELSE
			SET oMessage = IFNULL(oMessage, 'Invalid username and password');
		END IF;
	END IF;
    END$$

DELIMITER ;