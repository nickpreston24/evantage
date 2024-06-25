drop table if exists builds;

create table if not exists builds
(
    id      int NOT NULL AUTO_INCREMENT, -- not very critical, but nice for tracking how many rows and what we've removed.
    name    text,
    user_id int not null,                # where the user found it, if any.

    # PK's
    PRIMARY KEY (id),
    # full-text search
    FULLTEXT (name)
);
