CREATE TABLE app_user
(
    id              UUID PRIMARY KEY,
    account_id      UUID,
    created_date    TIMESTAMPTZ,
    encrypted_email TEXT,
    username        VARCHAR(20) UNIQUE,
    password        TEXT,
    encrypted_name  TEXT,
    encrypted_phone TEXT,
    last_login_date TIMESTAMPTZ,
    last_login_ip   TEXT,
    phone_verified  BOOLEAN,
    email_verified  BOOLEAN,
    email_hash      TEXT
)INHERITS(base_audit_table);

CREATE INDEX au_email_hash_indx ON app_user (email_hash);
CREATE INDEX au_username_hash_indx ON app_user (username);

CREATE INDEX au_user_id ON app_user(account_id)

/*
 ROLLBACK 
 DROP INDEX au_email_hash_indx;
 DROP index au_username_hash_indx;
 DELETE FROM flyway_schema_history WHERE script = 'V4__Create_App_User_Table.sql';
 DROP TABLE app_user CASCADE
 */