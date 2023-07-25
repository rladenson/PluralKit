-- database version 39
-- add trusted privacy option
-- change show_private from boolean to privacy level enum


create table trusted_users
(
    id          serial                   primary key,
    system      serial                   not null references systems (id) on delete cascade,
    uid         bigint                   not null,
    constraint  unique_trusted_relation  unique (system, uid)
);

alter table system_config add default_privacy_shown integer not null default 2;
update system_config set default_privacy_shown = 1 where show_private_info = false;
alter table system_config drop column show_private_info;

-- screeches in can't modify constraints
alter table systems
    drop constraint systems_description_privacy_check,
    add constraint systems_description_privacy_check check (description_privacy between 1 and 3),
    drop constraint systems_front_history_privacy_check,
    add constraint systems_front_history_privacy_check check (front_history_privacy between 1 and 3),
    drop constraint systems_front_privacy_check,
    add constraint systems_front_privacy_check check (front_privacy between 1 and 3),
    drop constraint systems_group_list_privacy_check,
    add constraint systems_group_list_privacy_check check (group_list_privacy between 1 and 3),
    drop constraint systems_member_list_privacy_check,
    add constraint systems_member_list_privacy_check check (member_list_privacy between 1 and 3),
    drop constraint systems_pronoun_privacy_check,
    add constraint systems_pronoun_privacy_check check (pronoun_privacy between 1 and 3),
    drop constraint systems_name_privacy_check,
    add constraint systems_name_privacy_check check (name_privacy between 1 and 3),
    drop constraint systems_avatar_privacy_check,
    add constraint systems_avatar_privacy_check check (avatar_privacy between 1 and 3)
;
alter table members
    drop constraint members_avatar_privacy_check,
    add constraint members_avatar_privacy_check check (avatar_privacy between 1 and 3),
    drop constraint members_birthday_privacy_check,
    add constraint members_birthday_privacy_check check (birthday_privacy between 1 and 3),
    drop constraint members_description_privacy_check,
    add constraint members_description_privacy_check check (description_privacy between 1 and 3),
    drop constraint members_member_privacy_check,
    add constraint members_member_privacy_check check (member_visibility between 1 and 3),
    drop constraint members_metadata_privacy_check,
    add constraint members_metadata_privacy_check check (metadata_privacy between 1 and 3),
    drop constraint members_name_privacy_check,
    add constraint members_name_privacy_check check (name_privacy between 1 and 3),
    drop constraint members_pronoun_privacy_check,
    add constraint members_pronoun_privacy_check check (pronoun_privacy between 1 and 3),
    drop constraint members_proxy_privacy_check,
    add constraint members_proxy_privacy_check check (proxy_privacy between 1 and 3)
;
alter table groups
    drop constraint groups_description_privacy_check,
    add constraint groups_description_privacy_check check (description_privacy between 1 and 3),
    drop constraint groups_icon_privacy_check,
    add constraint groups_icon_privacy_check check (icon_privacy between 1 and 3),
    drop constraint groups_list_privacy_check,
    add constraint groups_list_privacy_check check (list_privacy between 1 and 3),
    drop constraint groups_metadata_privacy_check,
    add constraint groups_metadata_privacy_check check (metadata_privacy between 1 and 3),
    drop constraint groups_name_privacy_check,
    add constraint groups_name_privacy_check check (name_privacy between 1 and 3),
    drop constraint groups_visibility_check,
    add constraint groups_visibility_check check (visibility between 1 and 3)
;

update info set schema_version = 39;