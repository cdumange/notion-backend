CREATE TABLE IF NOT EXISTS users (
  id uuid PRIMARY KEY default(gen_random_uuid()),
  email text NOT NULL,
  creation_date timestamp with time zone NOT NULL default(now()),
  modification_date timestamp with time zone
);