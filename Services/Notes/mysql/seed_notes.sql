drop table if exists notes;
create table if not exists notes
(
    id          INTEGER PRIMARY KEY not null,
    name        text,
    description text
);
