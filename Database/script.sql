create table "user"
(
    user_id  serial
        constraint user_pkey
            primary key,
    username text    not null,
    password text    not null,
    coins    integer not null,
    elo      integer not null,
    wins     integer not null,
    losses   integer not null
);

alter table "user"
    owner to postgres;

create unique index user_username_uindex
    on "user" (username);

create table card
(
    card_id      serial
        constraint card_pkey
            primary key,
    name         text                  not null,
    damage       integer               not null,
    element_type integer               not null,
    monster_type integer               not null,
    user_id      integer               not null
        constraint card_user_id_fkey
            references "user",
    in_deck      boolean default false not null,
    in_trade     boolean default false,
    level        integer default 0,
    experience   integer default 0
);

alter table card
    owner to postgres;

create table battle
(
    battle_id serial
        constraint battle_pkey
            primary key,
    user_id_1 integer not null
        constraint battle_user_id_1_fkey
            references "user",
    user_id_2 integer not null
        constraint battle_user_id_2_fkey
            references "user",
    completed boolean default false,
    constraint battle_pk
        unique (user_id_1, user_id_2)
);

alter table battle
    owner to postgres;

create table offer
(
    offer_id   serial
        constraint offer_pk
            primary key,
    user_id    integer            not null,
    card_id    integer            not null,
    price      integer default 5  not null,
    element    integer            not null,
    monster    integer            not null,
    min_damage integer default 50 not null
);

alter table offer
    owner to postgres;

create table transaction
(
    transaction_id serial
        constraint transaction_pk
            primary key,
    user_id_1      integer not null,
    user_id_2      integer,
    card_id_1      integer,
    card_id_2      integer,
    coins          integer,
    timestamp      integer
);

alter table transaction
    owner to postgres;

create unique index transaction_transaction_id_uindex
    on transaction (transaction_id);

