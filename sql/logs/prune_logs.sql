select *
from logs
where is_deleted || hard_delete_at <> null && logs.is_archived != true;

drop procedure if exists MarkForDeletion;
DELIMITER ^_^
CREATE PROCEDURE MarkForDeletion(_id int)
BEGIN
    update logs
    set hard_delete_at = now()
    where id = _id && logs.is_archived != true; # prevent archived logs from being marked for deletion
    select hard_delete_at, id from logs where id = _id && logs.is_archived != true;
END ^_^

DELIMITER ;

call MarkForDeletion(1);



drop procedure if exists PruneDoomedLogs;
DELIMITER ^_^
CREATE PROCEDURE PruneDoomedLogs()
BEGIN
    #     select *
    delete
    from logs
    where hard_delete_at is not null && logs.is_archived != true # never delete archived logs.
      and date(now()) >= date(hard_delete_at);
END ^_^

DELIMITER ;

call PruneDoomedLogs();


select *
from logs;
