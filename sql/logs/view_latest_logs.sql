/* logs view */
drop procedure if exists ViewLatestLogs;
DELIMITER ^_^
CREATE PROCEDURE ViewLatestLogs()
BEGIN

    select application_name
         , exception_message
         , exception_text
         , created_at
         , modified_at
    from logs
    order by id desc, created_at desc, modified_at desc;

end ^_^
delimiter ;

# delete from logs where exception_text like '%attempting%processing%'

call ViewLatestLogs();


select exception_message, log_ages.days_old
from (select id, TIMESTAMPDIFF(DAY, created_at, CURDATE()) AS days_old
      from logs 
      order by days_old desc) as log_ages
join logs log on log_ages.id = log.id
where log_ages.days_old >= 1 or log.exception_message is null