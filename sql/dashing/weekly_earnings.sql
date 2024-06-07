drop table if exists weekly_earnings;

create table if not exists weekly_earnings
(
    id                      int      NOT NULL AUTO_INCREMENT, -- not very critical, but nice for tracking how many rows and what we've removed.
    phone_app               text,

    daily_earnings_estimate double,
    average_gas_tank_cost   double,
    gas_discount            double,

    days_run_this_week      int,
    fillups_for_the_week    int,

    WeekStart               datetime,
    WeekEnd                 datetime,
    PayDay                  datetime,

    active_time             time,

    # Who is responsible?
    application_name        varchar(250),                     -- or can be 'Application Id'.
    modified_at             datetime null default now(),
    created_at              datetime null default now(),
    modified_by             varchar(250),
    created_by              varchar(250),

    # PK's
    PRIMARY KEY (id),
    # full-text search
    FULLTEXT (phone_app, application_name)
);


insert into weekly_earnings(phone_app,
                            fillups_for_the_week,
                            days_run_this_week)

values ('DoorDash', 4, 6),
       ('DoorDash', 5, 6),
       ('DoorDash', 7, 6),
       ('DoorDash', 6, 5)
;


select phone_app,
       fillups_for_the_week,
       days_run_this_week
from weekly_earnings;

