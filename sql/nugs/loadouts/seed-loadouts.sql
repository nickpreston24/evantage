drop table if exists loadouts;

create table if not exists loadouts
(
    id               int NOT NULL AUTO_INCREMENT, -- not very critical, but nice for tracking how many rows and what we've removed.
    name             text,
    url              text,                        # where the user found it, if any.
    primary_arm_id   int,
    secondary_arm_id int,

    # PK's
    PRIMARY KEY (id),
    # full-text search
    FULLTEXT (name, url)
);

## Joins

drop table if exists builds_x_parts;

create table if not exists builds_x_parts
(
    part_id  int not null,
    build_id int not null,
    user_id  text,

    # PK's
    PRIMARY KEY (part_id, build_id)
);


insert into loadouts (name, url)
values ('loadout1 zzz',
        'zzz'),
       ('loadout2 zzz',
        'zzz');



select *
from loadouts;