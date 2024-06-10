drop table if exists loadouts;

create table if not exists loadouts
(
    id       int NOT NULL AUTO_INCREMENT, -- not very critical, but nice for tracking how many rows and what we've removed.
    name     text,
    url      text,                        # where the user found it, if any.
    build_id int,

    # PK's
    PRIMARY KEY (id),
    # full-text search
    FULLTEXT (name, url)
);

insert into loadouts (name, url)
values ('testzzz',
        'zzz'),
       ('test2zzz',
        'zzz');

select *
from loadouts;