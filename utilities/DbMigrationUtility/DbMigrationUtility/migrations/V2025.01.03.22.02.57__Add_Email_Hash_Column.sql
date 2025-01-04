ALTER TABLE app_user ADD email_hash TEXT;

CREATE INDEX user_email_hash_indx ON app_user(email_hash);

/*
ROLLBACK
DROP INDEX user_email_hash_indx
*/;