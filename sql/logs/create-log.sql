drop procedure if exists CreateLog;
DELIMITER ^_^
CREATE PROCEDURE CreateLog(
    _table_name varchar(250),
    _exception_message TEXT,
    _exception TEXT,
    _operation varchar(250)
)
BEGIN

    insert into logs (table_name,
                      exception_mesage,
                      exception_text,
                      operation_name)
    values (_table_name,
            _exception_message,
            _exception, _operation);

END ^_^

DELIMITER ;

call CreateLog('airtable.jobs',
               'null reference exception',
               'exception occured at line 405; null reference exception', 'onget()');

select operation_name,
       exception_mesage,
       exception_text,
       payload,
       diff,
       sql_parameters,
       table_name,
       breadcrumb
from logs
order by created_at asc, modified_at asc;


# select * from logs;

# call SearchLogs('zzz', null, null, null);

# delete from logs where id > 0;

