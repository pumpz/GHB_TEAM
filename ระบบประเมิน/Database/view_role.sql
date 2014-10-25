DELIMITER $$

USE `appraisalasset`$$

DROP VIEW IF EXISTS `v_roles`$$

CREATE  VIEW `v_roles` AS (
SELECT
  `appraisalasset`.`role`.`ROLE_ID`   AS `ROLE_ID`,
  `appraisalasset`.`role`.`ROLE_NAME` AS `ROLE_NAME`
FROM `role`)$$

DELIMITER ;