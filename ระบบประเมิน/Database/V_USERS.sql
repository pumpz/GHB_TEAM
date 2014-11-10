DELIMITER $$

USE `appraisalasset`$$

DROP VIEW IF EXISTS `v_users`$$

CREATE ALGORITHM=UNDEFINED DEFINER=`sa`@`%` SQL SECURITY DEFINER VIEW `v_users` AS (
SELECT
  `users`.`USER_ID`     AS `USER_ID`,
  `users`.`USER_NAME`   AS `USER_NAME`,
  `users`.`PASSWORD`    AS `PASSWORD`,
  `users`.`ROLE_ID`     AS `ROLE_ID`,
  `users`.`STATUS`      AS `STATUS`,
  `users`.`CITIZEN_ID`  AS `CITIZEN_ID`,
  `users`.`NAME`        AS `NAME`,
  `users`.`EMAIL`       AS `EMAIL`,
  `users`.`PHONE`       AS `PHONE`,
  `users`.`LAST_LOGIN`  AS `LAST_LOGIN`,
  `users`.`USER_LOGIN`  AS `USER_LOGIN`,
  `users`.`DELETE_FLAG` AS `DELETE_FLAG`,
  `users`.`CREATE_DATE` AS `CREATE_DATE`,
  `users`.`UPDATE_DATE` AS `UPDATE_DATE`,
  `users`.`DELETE_DATE` AS `DELETE_DATE`,
  `users`.`CREATE_BY`   AS `CREATE_BY`,
  `users`.`UPDATE_BY`   AS `UPDATE_BY`,
  `users`.`DELETE_BY`   AS `DELETE_BY`,
  `role`.`ROLE_CODE`    AS `ROLE_CODE`,
  `role`.`ROLE_NAME`    AS `ROLE_NAME`
FROM (`users`
   JOIN `role`
     ON ((`users`.`ROLE_ID` = `role`.`ROLE_ID`))))$$

DELIMITER ;