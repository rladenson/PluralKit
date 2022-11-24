-- schema version 32

CREATE TABLE member_custom_fields (
    name            TEXT    NOT NULL,
    id              SERIAL  NOT NULL UNIQUE,
    display_name    TEXT            ,
    position        INT     NOT NULL,
    system          INT     NOT NULL        REFERENCES systems (id) ON DELETE CASCADE,
    type            TEXT    NOT NULL,   
    default         TEXT            ,
                                        
    PRIMARY KEY (id)
);

CREATE TABLE group_custom_fields (
    name            TEXT    NOT NULL,
    id              SERIAL  NOT NULL UNIQUE,
    display_name    TEXT            ,
    position        INT     NOT NULL,
    system          INT     NOT NULL        REFERENCES systems (id) ON DELETE CASCADE,
    type            TEXT    NOT NULL,
    default         TEXT            ,

    PRIMARY KEY (id)
);

CREATE TABLE system_custom_fields (
    name            TEXT    NOT NULL,
    id              SERIAL  NOT NULL UNIQUE,
    display_name    TEXT            ,
    position        INT     NOT NULL,
    system          INT     NOT NULL        REFERENCES systems (id) ON DELETE CASCADE,
    type            TEXT    NOT NULL,
    value           TEXT    NOT NULL,

    PRIMARY KEY (id)
);

CREATE TABLE member_custom_field_values (
    field_id        TEXT    NOT NULL        REFERENCES member_custom_fields (id) ON DELETE CASCADE,
    value_id        TEXT    NOT NULL UNIQUE,
    value           TEXT    NOT NULL,
    member_id       INT     NOT NULL        REFERENCES members (id) ON DELETE CASCADE,

    PRIMARY KEY (value_id)
);

CREATE TABLE group_custom_field_values (
    field_id        TEXT    NOT NULL        REFERENCES group_custom_fields (id) ON DELETE CASCADE,
    value_id        TEXT    NOT NULL UNIQUE,
    value           TEXT    NOT NULL,
    group_id        INT     NOT NULL        REFERENCES groups (id) ON DELETE CASCADE,

    PRIMARY KEY (value_id)
);

update info set schema_version = 32;