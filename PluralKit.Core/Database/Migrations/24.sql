﻿-- schema version 24
-- group name privacy

alter table groups add column name_privacy integer check (name_privacy in (1, 2)) not null default 1;

update info set schema_version = 24;