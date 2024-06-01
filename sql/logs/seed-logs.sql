drop table if exists logs;
create table logs
(
    id                int      NOT NULL AUTO_INCREMENT, -- not very critical, but nice for tracking how many rows and what we've removed.
    # WHAT happened?
    exception_message text,
    exception_text    text,                             -- stack traces, exception messages, etc.
    sql_parameters    json,                             -- Records what params were passed so we can figure out what went wrong.
    payload           json,                             -- what JSON if any was passed thru the API on the way to our CRUD operation?
    diff              json,                             -- a JsonDiff of what changed in the record (if anything).
    operation_name    varchar(250),                     -- e.g. a CRUD operation, Stored Proc, or a cron, etc. This, plus the app version and name helps us understand what process failed regardless of version.

    # Where did it happen?
    breadcrumb        varchar(250)  default '',
    table_name        varchar(250),
    server_name       varchar(250),
    database_name     varchar(250),

    # Who is responsible?
    application_name  varchar(250),                     -- or can be 'Application Id'.
    modified_at       datetime null default now(),
    created_at        datetime null default now(),
    modified_by       varchar(250),
    created_by        varchar(250),

    # How can we resolve it?
    commit_url        text,                             -- the commit url on Github, Gitlab, bitbucket, etc.  (death to tortoise svn!)
    issue_url         text,                             -- Hopefully not JIRA...but, if we must...

    # Meta/Management
    is_deleted        bit           default null,       -- soft delete
    is_archived       bit           default null,       -- if on, no-touchy (don't delete it)!
    is_enabled        bit           default null,       -- soft hide

    # PK's
    PRIMARY KEY (id),
    # full-text search
    FULLTEXT (created_by, modified_by, exception_text)
);


ALTER TABLE logs
    MODIFY COLUMN modified_at datetime null default now(),
    MODIFY COLUMN created_at datetime null default now()
;


ALTER TABLE logs
    Add column hard_delete_at datetime null # schedules a set time to HARD delete this row.
;

alter table logs
    add column exception_severity varchar(25) null;

# 
# update logs 
# where modified_at = ''
# set modified_at 
# select * from logs;
/*
ALTER TABLE logs
    DROP INDEX idx_log_exceptions;
*/

CREATE INDEX idx_log_exceptions
    ON logs (application_name, created_by, modified_by);

insert into logs ( table_name
                 , database_name
                 , exception_text
                 , breadcrumb
                 , issue_url
                 , created_by
                 , modified_by
                 , created_at
                 , modified_at
                 , is_deleted
                 , is_archived
                 , is_enabled)
values ( 'tpotpapers'
       , 'railway'
       , 'Null Reference Exception ... '
       , 'Home > XYZ > ABC'
       , 'jira.com/blah...'
       , 'Braden Preston'
       , 'Nick Preston'
       , now()
       , now()
       , 0
       , 1
       , 0),
       ( 'tpotpapers'
       , 'railway'
       , 'Argument Missing Exception ... '
       , 'Home > Sandbox > ABC'
       , 'jira.com/blah...'
       , 'Nick Preston'
       , 'Braden Preston'
       , now()
       , now()
       , 1
       , 0
       , 0),
       ( 'tpotpapers'
       , 'railway'
       , 'Argument Missing Exception ... '
       , 'Home > Sandbox > ABC'
       , 'jira.com/blah...'
       , 'Alan Agnew'
       , 'Braden Preston'
       , now()
       , now()
       , 0
       , 0
       , 1)
        ,
       ( 'tpotpapers'
       , 'railway'
       , 'Argument Missing Exception ... '
       , 'Home > Sandbox > ABC'
       , 'jira.com/blah...'
       , 'Ronnie Tanner'
       , 'Braden Preston'
       , now()
       , now()
       , null
       , null
       , null)
;
