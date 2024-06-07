drop table if exists parts;
create table if not exists parts
(
    id           int NOT NULL AUTO_INCREMENT, -- not very critical, but nice for tracking how many rows and what we've removed.
    name         text,
    cost         float,
    manufacturer text,
    kind         text,
    url          text,
    imageurl     text,

    # PK's
    PRIMARY KEY (id),
    # full-text search
    FULLTEXT (name, manufacturer, kind)
);

insert into parts (id, name, cost)
VALUES (1, 'DD 300 BLK PDW', 2399.99),
       (2, 'DD 556 NATO MK18', 1900.00),
       (3, 'MCMR-15 BCM 300 BLK Upper', 850.00)
;

update parts
set manufacturer = 'Bravo Company Manufacturing'
  , kind         = 'ar-15'
where id = 3;

update parts
set manufacturer = 'Daniel Defense'
  , kind         = 'ar-15'
where name like 'DD%';

update parts
set url      = 'https://danieldefense.com/ddm4-pdw-sbr.html'
  , kind     = 'ar-15'
  , imageurl = 'https://danieldefense.com/media/catalog/product/cache/57787b00c9d0d809fcf82071201be77f/p/d/pdw_sbr_fde0144.png'
where name like '%PDW%';

select *
from parts;

-- cleanup
/*
delete
from parts
where cost = 0
   or id == null
   or manufacturer = null
   or kind = null
;

 */