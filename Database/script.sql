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
    in_trade     boolean default false
);

alter table card
    owner to postgres;

create table trade
(
    trade_id  serial
        constraint trade_pkey
            primary key,
    user_id_1 integer not null
        constraint trade_user_id_1_fkey
            references "user",
    user_id_2 integer
        constraint trade_user_id_2_fkey
            references "user",
    card_id_1 integer not null
        constraint trade_card_id_1_fkey
            references card,
    card_id_2 integer
        constraint trade_card_id_2_fkey
            references card,
    timestamp timestamp
);

alter table trade
    owner to postgres;

create unique index trade_card_id_1_uindex
    on trade (card_id_1);

create unique index trade_card_id_2_uindex
    on trade (card_id_2);

create unique index trade_user_id_1_uindex
    on trade (user_id_1);

create unique index trade_user_id_2_uindex
    on trade (user_id_2);

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
            references "user"
);

alter table battle
    owner to postgres;

create unique index battle_user_id_1_uindex
    on battle (user_id_1);

create unique index battle_user_id_2_uindex
    on battle (user_id_2);

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

