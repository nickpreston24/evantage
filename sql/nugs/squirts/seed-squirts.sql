drop table if exists squirts;

create table if not exists squirts
(
    id              int NOT NULL AUTO_INCREMENT, -- not very critical, but nice for tracking how many rows and what we've removed.
    name            text,
    url             text,
    featured_image  text,
    thumbnail_image text,

    # PK's
    PRIMARY KEY (id),
    # full-text search
    FULLTEXT (name, url)
);

insert into squirts (name, url)
values ('DD Magpul Grip',
        'https://www.thingiverse.com/search?q=daniel+defense+grip&page=1'),
       ('DD Style Grip Airsoft',
        'https://cults3d.com/en/3d-model/tool/airsoft-aeg-motor-grip-daniel-defense-style-stp-version');

select *
from squirts;