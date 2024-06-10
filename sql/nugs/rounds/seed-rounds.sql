drop table if exists rounds;
create table rounds
(

    id               int      NOT NULL AUTO_INCREMENT, -- not very critical, but nice for tracking how many rows and what we've removed.
    name             varchar(250),

    # Who is responsible?
    application_name varchar(250),                     -- or can be 'Application Id'.
    modified_at      datetime null default now(),
    created_at       datetime null default now(),
    modified_by      varchar(250),
    created_by       varchar(250),

    # PK's
    PRIMARY KEY (id),
    # full-text search
    FULLTEXT (name)
);

insert into rounds (id, name)
values ('foo'),
       ('bar');